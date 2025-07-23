using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IPlanilhaService
    {
        Task<int?> AddPlanilha(string nomePlanilha, string caminho);
        Task<PlanilhaGetResponse> UpdatePlanilhaStatus(int planilhaId, PlanilhaStatusEnum planilhaStatus, string obs);
        Task<PageObject<PlanilhaGetResponse>> GetPlanilhas(int pageSize, int pageCount, OrderByEnum orderBy);
        Task<IEnumerable<PlanilhaStatusGetResponse>> GetPlanilhaStatus(int id);
        Task<PlanilhaStatusGetResponse> AddPlanilhaStatus(int idPlanilha, PlanilhaStatusEnum situacao, string obs);
        Task<IEnumerable<ConcorrenteFilialTempGetResponse>> GetConcorrenteFilialTempByPlanilhaId(int planilhaId);
        Task<bool> AddCocorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps);
        Task DeleteConcorrenteFilialTempRange(int planilhaId);
        Task UpdateConcorrenteFilialTempRange(int planilhaId, List<ConcorrenteFilialTempUpdateRequest> concorrenteFilialTemps);
        Task<IEnumerable<Planilha>> GetPlanilhasForProcessing();
        Task CheckAndUpdatePlanilhaStatus();
        Task<int> IncludePlanilhaHeader(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista);
        Task<int> IncludePlanilhaProducts(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista);
        Task<int> IncludePlanilhaItems(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista);
    }
}
