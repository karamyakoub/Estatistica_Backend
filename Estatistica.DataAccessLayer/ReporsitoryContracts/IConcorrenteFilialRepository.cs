using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IConcorrenteFilialRepository
    {
        Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliais(Expression<Func<ConcorrenteFilial,bool>> expression);
        Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliaisNoTracking(Expression<Func<ConcorrenteFilial,bool>> expression);
        Task<IEnumerable<ConcorrenteFilial>> GetConcorrentesFiliais();
        Task<ConcorrenteFilial> AddConcorrenteFilial(string cnpj,Concorrente concorrente);
        Task<ConcorrenteFilial?> UpdateConcorrenteFilial(string cnpj,Concorrente concorrente);
        Task<IEnumerable<ConcorrenteFilial>> GetConccorenteFiliaisByConcorrente(Concorrente concorrente);
        Task<ConcorrenteFilial?> GetConcorrenteFilialByCnpj(string cnpj);
    }
}
