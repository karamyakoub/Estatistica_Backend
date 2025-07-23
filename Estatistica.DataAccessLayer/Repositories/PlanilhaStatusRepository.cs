using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class PlanilhaStatusRepository : IPlanilhaStatusRepository
    {
        private readonly ApplicationDbContext context;

        public PlanilhaStatusRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<PlanilhaStatus> AddPlanilhaStatus(PlanilhaStatus planilhaStatus)
        {
            await context.PlanilhaStatuses.AddAsync(planilhaStatus);
            await context.SaveChangesAsync();
            return planilhaStatus;
        }

        public async Task<IEnumerable<PlanilhaStatus>> GetPlanilhaStatusByPlanilhaId(int id)
        {
            return await context.PlanilhaStatuses
                .Where(x => x.Planhila.Id == id)
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlanilhaStatus>> GetPlanilhaStatusByPlanilhaIdNoTracking(int id)
        {
            return await context.PlanilhaStatuses
                .AsNoTracking()
                .Where(x => x.Planhila.Id == id)
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }
    }
}
