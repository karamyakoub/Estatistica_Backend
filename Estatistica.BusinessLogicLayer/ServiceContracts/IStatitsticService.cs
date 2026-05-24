using Estatistica.BusinessLogicLayer.DTO;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IStatitsticService
    {
        Task<IEnumerable<SearchProductPriceResponse>> SearchProductPrice(string dateTime, List<int> idConcorrente, string textSearch);
        Task<bool> UpdateNfiCorrectionCount(string id, int correctionCount);
        Task<IEnumerable<StatistcDTO>> GetStatisticResult(List<int>? concorrenteId, DateTime? dtEmissaoIni, DateTime? dtEmissaoFin, decimal? desconto, string? fabricante);
        Task<List<StatistcDTO>> GetStatisticResultByNfcKey(string key, decimal discount = 0);
    }
}
