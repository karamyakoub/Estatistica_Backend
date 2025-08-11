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
    public class ConcorrenteFilialRepository : IConcorrenteFilialRepository
    {
        private readonly ApplicationDbContext context;

        public ConcorrenteFilialRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<ConcorrenteFilial> AddConcorrenteFilial(string cnpj, Concorrente concorrente)
        {
            var concorrenteFilial = new ConcorrenteFilial
            {
                Cnpj = cnpj,
                Concorrente = concorrente
            };

            await context.ConcorrenteFilials.AddAsync(concorrenteFilial);
            await context.SaveChangesAsync();
            return concorrenteFilial;
        }

        public async Task<IEnumerable<ConcorrenteFilial>> GetConccorenteFiliaisByConcorrente(Concorrente concorrente)
        {
            return await context.ConcorrenteFilials
                .Where(cf => cf.Concorrente.Id == concorrente.Id).ToListAsync();
        }

        public async Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliais()
        {
            return await context.ConcorrenteFilials.ToListAsync();
        }

        public async Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliais(Expression<Func<ConcorrenteFilial, bool>> expression)
        {
            return await context.ConcorrenteFilials
                .Where(expression).ToListAsync();
        }

        public async Task<ConcorrenteFilial?> UpdateConcorrenteFilial(string cnpj, Concorrente concorrente)
        {
            var concorrenteFilialDb = await context.ConcorrenteFilials
                                .FirstOrDefaultAsync(cf => cf.Cnpj == cnpj);
            if (concorrenteFilialDb is null)
                return null;
            concorrenteFilialDb.Concorrente = concorrente;
            await context.SaveChangesAsync();
            return concorrenteFilialDb;
        }

        public async Task<ConcorrenteFilial?> GetConcorrenteFilialByCnpj(string cnpj)
        {
            return await context.ConcorrenteFilials.Include(x => x.Concorrente)
                .FirstOrDefaultAsync(cf => cf.Cnpj == cnpj);
        }

        public async Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliaisNoTracking(Expression<Func<ConcorrenteFilial, bool>> expression)
        {
            return await context.ConcorrenteFilials
                .Include(x => x.Concorrente)
                .AsNoTracking()
                .Where(expression).ToListAsync();
        }
    }
}
