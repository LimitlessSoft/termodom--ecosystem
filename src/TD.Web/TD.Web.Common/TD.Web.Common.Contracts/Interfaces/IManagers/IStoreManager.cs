using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IStoreManager : ILSCoreBaseManager
    {
        LSCoreSortedListResponse<StoreDto> GetMultiple(GetMultipleStoresRequest request);
    }
}
