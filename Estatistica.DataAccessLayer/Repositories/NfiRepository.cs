using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class NfiRepository : INfiRespository
    {
        private readonly ApplicationDbContext context;

        public NfiRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Nfi> AddNfi(Nfi nfi)
        {
            await context.Nfis.AddAsync(nfi);
            await context.SaveChangesAsync();
            return nfi;
        }

        public async Task<bool> AddNfiRange(List<Nfi> nfiList)
        {
            await context.Nfis.AddRangeAsync(nfiList);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Nfi>> GetNfisByCondition(Expression<Func<Nfi, bool>> condition)
        {
            return await context.Nfis.Include(x => x.Nfc).Where(condition)                                
                .ToListAsync();
        }

        public async Task<IEnumerable<Nfi>> GetNfisByConditionNoTracking(Expression<Func<Nfi, bool>> condition)
        {
            return await context.Nfis.Include(x => x.Nfc).AsNoTracking().Where(condition)
                .ToListAsync();
        }
    }
}
