using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Common.Domain.Managers;

public class StoreManager(IStoreRepository repository) : IStoreManager
{
	public List<StoreDto> GetMultiple(GetMultipleStoresRequest request) =>
		repository
			.GetMultiple()
			.SortQuery(request, StoresSortColumnCodes.StoresSortRules)
			.ToMappedList<StoreEntity, StoreDto>();
}
