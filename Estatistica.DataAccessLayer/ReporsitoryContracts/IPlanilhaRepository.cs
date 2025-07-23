using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IPlanilhaRepository
    {
        Task<IEnumerable<Planilha>> GetPlanilhasByStatus(int status);
        Task<Planilha?> GetPlanilhaById(int id);
        Task<IEnumerable<Planilha>> GetPlanilhasByCondition(Expression<Func<Planilha, bool>> condition);
        Task<IEnumerable<Planilha>> GetPlanilhasByConditionNoTracking(Expression<Func<Planilha, bool>> condition);
        Task<Planilha> AddPlanilha(Planilha planilha);
        Task<Planilha> UpdatePlanilhaStatus(Planilha planilha);
    }
}
