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
    public class ConcorrenteFilialPendenteRepository : IConcorrenteFilialPendenteRepository
    {
        private readonly ApplicationDbContext context;

        public ConcorrenteFilialPendenteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddConcorrenteFilialPendente(ConcorrenteFilialPendente concorrenteFilialPendente)
        {
            await context.ConcorrenteFilialPendentes.AddAsync(concorrenteFilialPendente);
            await context.SaveChangesAsync();
        }

        public async Task AddConcorrenteFilialPendenteRange(List<ConcorrenteFilialPendente> concorrenteFilialPendenteList)
        {
            await context.ConcorrenteFilialPendentes.AddRangeAsync(concorrenteFilialPendenteList);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConcorrenteFilialPendente>> GetConcorrenteFilialPendentes(Expression<Func<ConcorrenteFilialPendente, bool>> expression)
        {
            return await context.ConcorrenteFilialPendentes.Include(x => x.Planilha).Include(x => x.Concorrente).Where(expression)
                .ToListAsync();
        }

        

        public async Task UpdateConcorrenteFilialPendente(string cnpj, Concorrente concorrente)
        {
            var filiaisPendentes = await context.ConcorrenteFilialPendentes
                .Include(x => x.Concorrente)
                .Where(x => x.Cnpj == cnpj && x.Concorrente == null)
                .ToListAsync();
            foreach(var filialPendente in filiaisPendentes)
            {
                filialPendente.Concorrente = concorrente;
            }
            await context.SaveChangesAsync();
        }
    }
}
