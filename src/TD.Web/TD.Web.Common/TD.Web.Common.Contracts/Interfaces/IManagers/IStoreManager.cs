using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IStoreManager
{
	List<StoreDto> GetMultiple(GetMultipleStoresRequest request);
}
