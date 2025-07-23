using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class ConcorrenteProdutoRepository : IConcorrenteProdutoRepository
    {
        private readonly ApplicationDbContext context;

        public ConcorrenteProdutoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<ConcorrenteProduto> AddConcorrenteProduto(ConcorrenteProduto concorrenteProduto)
        {
            await context.ConcorrenteProdutos.AddAsync(concorrenteProduto);
            await context.SaveChangesAsync();
            return concorrenteProduto;
        }

        public async Task<bool> AddConcorrenteProdutoRange(List<ConcorrenteProduto> concorrenteProduto)
        {
            await context.ConcorrenteProdutos.AddRangeAsync(concorrenteProduto);
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByConcorrente(int concorrenteId)
        {
            return await context.ConcorrenteProdutos.Where(x => x.Concorrente.Id == concorrenteId)
                                              .OrderBy(x => x.Concorrente.Id)
                                              .ThenBy(x => x.CodigoProdutoConcorrente)
                                              .ToListAsync();
        }

        public async Task<ConcorrenteProduto?> GetConcorrenteProdutosByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente)
        {
            return await context.ConcorrenteProdutos
                                              .FirstOrDefaultAsync(x => x.Concorrente.Id == concorrenteId &&
                                                                        x.CodigoProdutoConcorrente == codigoProdutoConcorrente);
        }

        public async Task<IEnumerable<ConcorrenteProduto>> GetConcorrenteProdutosByCondition(Expression<Func<ConcorrenteProduto, bool>> expression)
        {
            return await context.ConcorrenteProdutos.Where(expression)
                                              .OrderBy(x => x.Concorrente.Id)
                                              .ThenBy(x => x.CodigoProdutoConcorrente)
                                              .ToListAsync();
        }

        public async Task<ConcorrenteProduto?> GetConcorrenteProdutosByProdutoId(string produtoId)
        {
            return await context.ConcorrenteProdutos
                                              .FirstOrDefaultAsync(x => x.Id == produtoId);
        }

        public async Task<bool> LinkConcorrenteProduto(string produtoId, Produto produto)
        {
            var concorrenteProduto = await context.ConcorrenteProdutos.FindAsync(produtoId);
            if (concorrenteProduto is null)
                return false;
            concorrenteProduto.Produto = produto;
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> LinkConcorrenteProdutoByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente, Produto produto)
        {
            var concorrenteProduto = await context.ConcorrenteProdutos.FirstOrDefaultAsync(x => x.Concorrente.Id == concorrenteId && x.CodigoProdutoConcorrente == codigoProdutoConcorrente);
            if (concorrenteProduto is null)
                return false;
            concorrenteProduto.Produto = produto;
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> UnlinkConcorrenteProduto(string produtoId)
        {
            var concorrenteProduto = await context.ConcorrenteProdutos.FindAsync(produtoId);
            if (concorrenteProduto is null)
                return false;
            concorrenteProduto.Produto = null;
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> UnlinkConcorrenteProdutoByConcorrenteAndCodigo(int concorrenteId, string codigoProdutoConcorrente)
        {
            var concorrenteProduto = await context.ConcorrenteProdutos.FirstOrDefaultAsync(x => x.Concorrente.Id == concorrenteId && x.CodigoProdutoConcorrente == codigoProdutoConcorrente);
            if (concorrenteProduto is null)
                return false;
            concorrenteProduto.Produto = null;
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
