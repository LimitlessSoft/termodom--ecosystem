using TD.Web.Public.Contracts.Requests.Statistics;
using LSCore.Contracts.IManagers;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IStatisticsManager : ILSCoreBaseManager
    {
        Task LogAsync(ProductViewCountRequest request);
    }
}