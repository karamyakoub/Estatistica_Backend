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
    public class NfcRespository : INfcRespository
    {
        private readonly ApplicationDbContext context;

        public NfcRespository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Nfc> AddNfc(Nfc nfc)
        {
            await context.Nfcs.AddAsync(nfc);
            await context.SaveChangesAsync();
            return nfc;
        }

        public async Task<bool> AddNfcRange(List<Nfc> nfcList)
        {
            await context.Nfcs.AddRangeAsync(nfcList);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Nfc>> GetNfcsByCondition(Expression<Func<Nfc, bool>> condition)
        {
            return await context.Nfcs
                .Where(condition)
                .ToListAsync();
        }

        public async Task<IEnumerable<Nfc>> GetNfcsByConditionNoTracking(Expression<Func<Nfc, bool>> condition)
        {
            return await context.Nfcs
                .Where(condition)
                .Include(x => x.ConcorrenteCnpj)
                .ThenInclude(X => X.Concorrente)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
