using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class StoreManager : LSCoreBaseManager<StoreManager, StoreEntity>, IStoreManager
    {
        public StoreManager(ILogger<StoreManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreSortedListResponse<StoreDto> GetMultiple(GetMultipleStoresRequest request) =>
            new LSCoreSortedListResponse<StoreDto>(Queryable().ToSortedListResponse(request, StoresSortColumnCodes.StoresSortRules)
                .Payload.ToDtoList<StoreDto, StoreEntity>());
    }
}
