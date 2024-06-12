using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class StoreManager (ILogger<StoreManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<StoreManager, StoreEntity>(logger, dbContext), IStoreManager
{
    // TODO: Implement sorting by request, StoresSortColumnCodes.StoresSortRules
    public List<StoreDto> GetMultiple(GetMultipleStoresRequest request) =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<StoreEntity, StoreDto>();
}