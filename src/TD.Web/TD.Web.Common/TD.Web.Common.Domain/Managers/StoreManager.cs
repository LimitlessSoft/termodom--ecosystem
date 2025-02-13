using LSCore.Domain.Extensions;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Common.Domain.Managers;

public class StoreManager (IStoreRepository repository)
    : IStoreManager
{
    public List<StoreDto> GetMultiple(GetMultipleStoresRequest request) =>
        repository.GetMultiple()
            .ToDtoList<StoreEntity, StoreDto>();
}