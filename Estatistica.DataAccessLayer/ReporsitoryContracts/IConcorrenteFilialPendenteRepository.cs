using Estatistica.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IConcorrenteFilialPendenteRepository
    {
        Task AddConcorrenteFilialPendente(ConcorrenteFilialPendente concorrenteFilialPendente);
        Task AddConcorrenteFilialPendenteRange(List<ConcorrenteFilialPendente> concorrenteFilialPendenteList);
        Task UpdateConcorrenteFilialPendente(string cnpj,Concorrente concorrente);
        Task<IEnumerable<ConcorrenteFilialPendente>> GetConcorrenteFilialPendentes(Expression<Func<ConcorrenteFilialPendente, bool>> expression);
        Task<IEnumerable<ConcorrenteFilialPendente>> GetConcorrenteFilialPendentesNoTracking(Expression<Func<ConcorrenteFilialPendente, bool>> expression);

    }
}
