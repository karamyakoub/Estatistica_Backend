using Estatistica.BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IConcorrenteProdutoService
    {
        Task<IEnumerable<PageObject<ConcorrenteProdutoSearchDto>>> SearchConcorrenteProdutos(int? idConcorrente, string descricaoProduto, int pageSize, int pageNumber);

    }
}
