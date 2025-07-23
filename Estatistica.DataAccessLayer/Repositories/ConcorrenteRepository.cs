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
    public class ConcorrenteRepository : IConcorrenteRepository
    {
        private readonly ApplicationDbContext context;

        public ConcorrenteRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<IEnumerable<Concorrente>> GetConcorrentes()
        {
            return await context.Concorrentes.ToListAsync();
        }

        public async Task<IEnumerable<Concorrente>> GetConcorrentesByCondition(Expression<Func<Concorrente, bool>> expression)
        {
            return await context.Concorrentes.Where(expression).OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Concorrente> AddConcorrente(Concorrente concorrente)
        {
            await context.Concorrentes.AddAsync(concorrente);
            await context.SaveChangesAsync();
            return concorrente;
        }

        public async Task<bool> UpdateConcorrenteNome(Concorrente concorrente)
        {
            var concorrenteDb = await context.Concorrentes.FindAsync(concorrente.Id);
            if (concorrenteDb is null)
                return false;
            concorrenteDb.Nome = concorrente.Nome;     
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<Concorrente?> GetConcorrenteById(int id)
        {
            return await context.Concorrentes
                .FirstOrDefaultAsync(x => x.Id == id);                
        }

        public async Task<IEnumerable<Concorrente>> GetConcorrentesByConditionNoTracking(Expression<Func<Concorrente, bool>> expression)
        {
            return await context.Concorrentes.AsNoTracking().Where(expression).OrderBy(x => x.Id).ToListAsync();
        }
    }
}
