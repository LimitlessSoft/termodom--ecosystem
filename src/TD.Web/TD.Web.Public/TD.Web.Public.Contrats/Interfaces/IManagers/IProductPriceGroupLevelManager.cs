using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Requests.ProductPriceGroupLevels;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IProductPriceGroupLevelManager : ILSCoreBaseManager
    {
        public LSCoreResponse<int> GetUserLevel(GetUserLevelRequest request);
    }
}
