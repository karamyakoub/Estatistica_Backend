using Estatistica.BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Estatistica.BusinessLogicLayer.Data.ProdutoConsulta;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IProdutoService
    {
        Task<bool> CheckProductExists(string codigoProduto);
        Task AddProdutoRange(IEnumerable<Content> produtos);
        Task UpdateProdutoRange(IEnumerable<Content> produtos);
        Task<bool> UpdateProdutoPrice(string codigoProduto,decimal price,decimal custo);
        Task<IEnumerable<ProductSuggestionResponse>> GetProdutosSuggestion(string descricao);
        
    }
}
