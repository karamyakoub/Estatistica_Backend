using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.WebAPI.ServiceContracts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Estatistica.WebAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IPlanilhaService planilhaService;

        public FileService(IPlanilhaService planilhaService)
        {
            this.planilhaService=planilhaService;
        }
        public async Task<FileUploadSummartDTO> UploadPlanilha(Stream fileStream, string? contentType, List<string> allowedExtensions, string uploadDirectory)
        {
            var fileCount = 0;
            long totalSizeInBytes = 0;

            var boundry = getBoundary(MediaTypeHeaderValue.Parse(contentType));

            var multipartReader = new MultipartReader(boundry, fileStream);

            var section = await multipartReader.ReadNextSectionAsync();
            var filePaths = new List<string>();
            var notUploadedFiles = new List<string>();

            while (section is not null)
            {
                var fileSection = section.AsFileSection();
                if (fileSection is not null)
                {
                    var result = await saveFile(fileSection, filePaths, notUploadedFiles, allowedExtensions, uploadDirectory);
                    if (result > 0)
                    {
                        totalSizeInBytes += result;
                        fileCount++;
                    }
                }
                section = await multipartReader.ReadNextSectionAsync();
            }

            return new FileUploadSummartDTO
            {
                TotalFilesUploaded = fileCount,
                TotalSizeUploaded = totalSizeInBytes > 0 ? $"{totalSizeInBytes / 1024} KB" : "0 KB",
                FilePaths = filePaths,
                NotUploadedFiles = notUploadedFiles
            };

        }

        private async Task<long> saveFile(FileMultipartSection fileSection, List<string> filePaths, List<string> notUploadedFiles, List<string> allowedExtensions, string uploadDirectory)
        {
            var extension = Path.GetExtension(fileSection.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                notUploadedFiles.Add(fileSection.FileName);
                return 0;
            }

            Directory.CreateDirectory(uploadDirectory);
            var filePath = Path.Combine(uploadDirectory, fileSection.FileName);

            if (File.Exists(filePath))
                return 0;

            await using var stram = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
            await fileSection.FileStream!.CopyToAsync(stram);
            filePaths.Add(filePath);
            var id = await planilhaService.AddPlanilha(fileSection.FileName, filePath);
            if(id.HasValue)
                await planilhaService.AddPlanilhaStatus(id.Value, BusinessLogicLayer.Enums.PlanilhaStatusEnum.AguardandoInclusao, "Planilha aguardando a leitura inicial.");
            return fileSection.FileStream.Length;
        }

        private string getBoundary(MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            var boundry = HeaderUtilities.RemoveQuotes(mediaTypeHeaderValue.Boundary).Value;
            if (string.IsNullOrEmpty(boundry))
                throw new InvalidDataException("Missing content-type boundary.");
            return boundry;
        }
    }
}
