using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class ConcorrenteFilialTempRepository : IConcorrenteFilialTempRepository
    {
        private readonly ApplicationDbContext context;

        public ConcorrenteFilialTempRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<bool> AddCocorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps)
        {
            await context.ConcorrenteFilialTemps.AddRangeAsync(concorrenteFilialTemps);
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task DeleteConcorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps)
        {
            context.ConcorrenteFilialTemps.RemoveRange(concorrenteFilialTemps);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConcorrenteFilialTemp>> GetConcorrenteFilialTempByCondition(Expression<Func<ConcorrenteFilialTemp, bool>> expression)
        {
            return await context.ConcorrenteFilialTemps.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<ConcorrenteFilialTemp>> GetConcorrenteFilialTempByPlanilhaId(int planilhaId)
        {
            return await context.ConcorrenteFilialTemps.AsNoTracking().Where(x => x.Planilha != null && x.Planilha.Id == planilhaId).ToListAsync();
        }

        public async Task UpdateConcorrenteFilialTempRange(int planilhaId, List<ConcorrenteFilialTemp> concorrenteFilialTemps)
        {
            foreach (var item in concorrenteFilialTemps)
            {
                var db = await context.ConcorrenteFilialTemps.Include(x => x.Planilha)
                    .FirstOrDefaultAsync(x => x.Planilha != null && x.Planilha.Id == planilhaId && x.Cnpj == item.Cnpj && x.Incluido == false);
                if (db != null)
                    db.Incluido = item.Incluido;
                await context.SaveChangesAsync();
            }
        }
    }
}
