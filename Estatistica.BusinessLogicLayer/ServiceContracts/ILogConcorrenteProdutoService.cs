using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface ILogConcorrenteProdutoService
    {
        Task<bool> Add(LogConcorrenteProduto log);
        Task<IEnumerable<LogConcorrenteProdutoGetResponse>> GetbyFilter(DateTime startDate,DateTime endDate,string filterText);
    }
}
