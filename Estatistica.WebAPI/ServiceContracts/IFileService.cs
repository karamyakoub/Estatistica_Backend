using Estatistica.BusinessLogicLayer.DTO;

namespace Estatistica.WebAPI.ServiceContracts
{
    public interface IFileService
    {
        Task<FileUploadSummartDTO> UploadPlanilha(Stream fileStream, string contentType, List<string> allowedExtensions, string uploadDirectory);
    }
}