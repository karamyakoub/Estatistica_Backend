using Estatistica.BusinessLogicLayer.DTO;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IStatitsticService
    {
        Task<IEnumerable<SearchProductPriceResponse>> SearchProductPrice(string dateTime, List<int> idConcorrente, string textSearch);
    }
}
