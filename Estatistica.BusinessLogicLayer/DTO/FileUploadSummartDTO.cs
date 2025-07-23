using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class FileUploadSummartDTO
    {
        public int TotalFilesUploaded { get; set; }
        public string? TotalSizeUploaded { get; set; }
        public List<string>? FilePaths { get; set; }
        public List<string>? NotUploadedFiles { get; set; }

    }
}
