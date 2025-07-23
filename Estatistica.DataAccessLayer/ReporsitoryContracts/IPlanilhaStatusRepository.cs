using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IPlanilhaStatusRepository
    {        
        Task<IEnumerable<PlanilhaStatus>> GetPlanilhaStatusByPlanilhaId(int id);
        Task<IEnumerable<PlanilhaStatus>> GetPlanilhaStatusByPlanilhaIdNoTracking(int id);
        Task<PlanilhaStatus> AddPlanilhaStatus(PlanilhaStatus planilhaStatus);
    }
}
