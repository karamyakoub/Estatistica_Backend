using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IProdutoRepository
    {
        Task<Produto> AddProduto(Produto produto);
        Task<Produto> GetProductByCodigo(string codigoProduto);
        Task<Produto?> GetProductByCodigoBarra(string codigoBarra);
        Task<bool> AddProdutoRange(IEnumerable<Produto> produtoList);
        Task<Produto> UpdateProduto(Produto produto);
        Task<bool> UpdateProdutoRange(IEnumerable<Produto> produtoList);
        Task<IEnumerable<Produto>> GetProdutosByCondition(Expression<Func<Produto, bool>> condition);
        Task<IEnumerable<Produto>> GetProdutosByConditionNoTracking(Expression<Func<Produto, bool>> condition);
    }
}
