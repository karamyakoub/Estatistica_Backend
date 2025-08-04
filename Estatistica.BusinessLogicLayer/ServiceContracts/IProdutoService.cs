using Estatistica.BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IProdutoService
    {
        Task<bool> CheckProductExists(string codigoProduto);
        Task AddProdutoRange(IEnumerable<Estatistica.BusinessLogicLayer.Data.Produto> produtos);
        Task UpdateProdutoRange(IEnumerable<Estatistica.BusinessLogicLayer.Data.Produto> produtos);
        Task<IEnumerable<ProductSuggestionResponse>> GetProdutosSuggestion(string descricao);
        
    }
}
