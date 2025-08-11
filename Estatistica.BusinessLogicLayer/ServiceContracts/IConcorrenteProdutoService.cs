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
        Task<IEnumerable<ConcorrenteProdutoSearchDto>> SearchConcorrenteProdutos(string? idsConcorrente, string descricaoProduto, string fabricante);        
        Task<bool> LinkProduct(string CodigoProdutoConcorrente, string idProduto);
        Task<bool> UnLinkProduct(string CodigoProdutoConcorrente);
    }
}
