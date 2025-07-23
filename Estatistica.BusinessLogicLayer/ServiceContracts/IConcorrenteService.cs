using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.DataAccessLayer.Entities;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IConcorrenteService
    {
        Task<int> AddConcorrente(string nome);
        Task<ConcorrenteGetResponse?> GetConcorrenteById(int id);
        Task<IEnumerable<Concorrente>> GetConcorrentes(OrderByEnum orderBy);
        Task<ConcorrenteGetResponse?> UpdateConcorrenteNome(int id, string nome);
    }
}