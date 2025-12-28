using Estatistica.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IConcorrenteRepository
    {
        Task<Concorrente> AddConcorrente(Concorrente concorrente);
        Task<IEnumerable<Concorrente>> GetConcorrentes(bool? includeFiliais = null);
        Task<IEnumerable<Concorrente>> GetConcorrentesByCondition(Expression<Func<Concorrente, bool>> expression);
        Task<IEnumerable<Concorrente>> GetConcorrentesByConditionNoTracking(Expression<Func<Concorrente, bool>> expression);
        Task<bool> UpdateConcorrenteNome(Concorrente concorrente);
        Task<Concorrente?> GetConcorrenteById(int id);
    }
}