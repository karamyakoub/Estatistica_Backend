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
    public class LogConcorrenteProdutoRepository : ILogConcorrenteProdutoRepository
    {
        private readonly ApplicationDbContext context;

        public LogConcorrenteProdutoRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<LogConcorrenteProduto> Add(LogConcorrenteProduto log)
        {
            await context.LogConcorrenteProdutos.AddAsync(log);
            await context.SaveChangesAsync();
            return log;
        }

        public async Task<IEnumerable<LogConcorrenteProduto>> GetByCondition(Expression<Func<LogConcorrenteProduto, bool>> condition)
        {
            return await context.LogConcorrenteProdutos
                .Include(x => x.Concorrente)
                .Include(x => x.CodigoProdutoConcorrente)
                .Include(x => x.CodigoProdutoAnt)
                .Include(x => x.CodigoProdutoAtual)
                .Where(condition).ToListAsync();
        }
    }
}
