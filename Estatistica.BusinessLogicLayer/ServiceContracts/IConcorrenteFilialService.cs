using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{

    public interface IConcorrenteFilialService
    {
        Task<IEnumerable<ConcorrenteFilialGetResponse>> GetConcorrentesFiliais();
        Task<ConcorrenteFilial?> GetConcorrentesFilialByCnpj(string cnpj);
        Task<ConcorrenteFilialGetResponse> AddConcorrenteFilial(string cnpj, int idConcorrente);
        Task<ConcorrenteFilialGetResponse> UpdateConcorrenteFilial(string cnpj, int idConcorrente);
        Task<IEnumerable<ConcorrenteFilialGetResponse>> GetConcorrenteFiliaisByConcorrente(int concorrente);
        Task AddConcorrenteFilialPendenteRange(int planilhaId,List<ConcorrenteFilialPendente> concorrenteFilialPendenteList);
        Task UpdateConcorrenteFilialPendente(string cnpj, int idConcorrente);
        Task<IEnumerable<ConcorrenteFilialPendenteGetResponse>> GetConcorrenteFilialPendentesAgrupado();        
    }
}
