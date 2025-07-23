using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IConcorrenteFilialTempRepository
    {
        Task<IEnumerable<ConcorrenteFilialTemp>> GetConcorrenteFilialTempByPlanilhaId(int planilhaId);
        Task<IEnumerable<ConcorrenteFilialTemp>> GetConcorrenteFilialTempByCondition(Expression<Func<ConcorrenteFilialTemp, bool>> expression);
        Task<IEnumerable<ConcorrenteFilialTemp>> GetConcorrenteFilialTempByConditionNoTracking(Expression<Func<ConcorrenteFilialTemp, bool>> expression);
        Task<bool> AddCocorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps);
        Task DeleteConcorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps);
        Task UpdateConcorrenteFilialTempRange(int planilhaId, List<ConcorrenteFilialTemp> concorrenteFilialTemps);
    }
}
