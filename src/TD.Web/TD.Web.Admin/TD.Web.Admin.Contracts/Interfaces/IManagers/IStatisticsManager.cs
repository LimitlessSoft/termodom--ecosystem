using TD.Web.Admin.Contracts.Dtos.Statistics;
using TD.Web.Admin.Contracts.Requests.Statistics;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IStatisticsManager
{
	ProductsStatisticsDto GetProductsStatistics(ProductsStatisticsRequest request);
	SearchPhrasesStatisticsDto GetSearchPhrasesStatistics(SearchPhrasesStatisticsRequest request);
}
