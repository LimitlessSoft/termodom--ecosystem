using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.Statistics;
using TD.Web.Admin.Contracts.Requests.Statistics;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IStatisticsManager
    {
        LSCoreResponse<ProductsStatisticsDto> GetProductsStatistics(ProductsStatisticsRequest request);
        LSCoreResponse<SearchPhrasesStatisticsDto> GetSearchPhrasesStatistics(SearchPhrasesStatisticsRequest request);
    }
}