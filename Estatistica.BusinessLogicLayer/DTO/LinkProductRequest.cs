using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class LinkProductRequest
    {
        public string? ConcorrenteProdutoId { get; set; }
        public string? InternalProductCode { get; set; }
    }
}
