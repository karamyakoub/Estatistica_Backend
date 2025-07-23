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
using static Mysqlx.Expect.Open.Types;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class PlanilhaRepository : IPlanilhaRepository
    {
        private readonly ApplicationDbContext context;

        public PlanilhaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Planilha> AddPlanilha(Planilha planilha)
        {
            await context.Planilhas.AddAsync(planilha);
            await context.SaveChangesAsync();
            return planilha;
        }



        public async Task<Planilha?> GetPlanilhaById(int id)
        {
            return await context.Planilhas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Planilha>> GetPlanilhasByCondition(Expression<Func<Planilha, bool>> condition)
        {
            return await context.Planilhas.Where(condition).ToListAsync();
        }

        public async Task<IEnumerable<Planilha>> GetPlanilhasByConditionNoTracking(Expression<Func<Planilha, bool>> condition)
        {
            return await context.Planilhas.AsNoTracking().Where(condition).ToListAsync();
        }

        public async Task<IEnumerable<Planilha>> GetPlanilhasByStatus(int status)
        {
            return await context.Planilhas.Where(x => x.Status == status).ToListAsync();
        }

        public async Task<Planilha> UpdatePlanilhaStatus(Planilha planilha)
        {
            var planilhaToUpdate = context.Planilhas.FirstOrDefault(x => x.Id == planilha.Id);
            if (planilhaToUpdate is null)
                throw new ArgumentException("Planilha not found");
            
            planilhaToUpdate.Status = planilha.Status;
            context.Planilhas.Update(planilhaToUpdate);
            await context.SaveChangesAsync();
            return planilhaToUpdate;
        }
    }
}
