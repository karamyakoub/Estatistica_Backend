using Estatistica.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Estatistica.DataAccessLayer.Repositories
{
    public interface IConcorrenteProdutoRepository
    {
        Task<ConcorrenteProduto> AddConcorrenteProduto(ConcorrenteProduto concorrenteProduto);
        Task<bool> AddConcorrenteProdutoRange(List<ConcorrenteProduto> concorrenteProduto);
        Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByConcorrente(int concorrenteId);
        Task<ConcorrenteProduto?> GetConcorrenteProdutosByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente);
        Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByCondition(Expression<Func<ConcorrenteProduto, bool>> expression);
        Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByConditionNoTracking(Expression<Func<ConcorrenteProduto, bool>> expression);
        Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByConditionNoTrackingWithoutConcorrente(Expression<Func<ConcorrenteProduto, bool>> expression);
        Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByConditionNoTrackingSeaarch(Expression<Func<ConcorrenteProduto, bool>> expression);
        Task<ConcorrenteProduto?> GetConcorrenteProdutosByProdutoId(string produtoId);
        Task<bool> LinkConcorrenteProduto(string produtoId, Produto produto);
        Task<bool> LinkConcorrenteProdutoByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente, Produto produto);
        Task<bool> UnlinkConcorrenteProduto(string produtoId);
        Task<bool> UnlinkConcorrenteProdutoByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente);
    }
}