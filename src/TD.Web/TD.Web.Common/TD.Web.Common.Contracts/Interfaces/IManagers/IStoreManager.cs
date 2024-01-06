using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IStoreManager : ILSCoreBaseManager
    {
        LSCoreListResponse<StoreDto> GetMultiple(GetMultipleStoresRequest request);
    }
}
