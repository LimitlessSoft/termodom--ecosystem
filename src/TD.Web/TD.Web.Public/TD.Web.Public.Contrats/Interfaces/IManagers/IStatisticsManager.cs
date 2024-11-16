using TD.Web.Public.Contracts.Requests.Statistics;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IStatisticsManager
{
    Task LogAsync(ProductViewCountRequest request);
    Task LogAsync(ProductSearchKeywordRequest request);
    void Log(ProductSearchKeywordRequest request);
}
