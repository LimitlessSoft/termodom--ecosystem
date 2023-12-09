using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class StoreManager : LSCoreBaseManager<StoreManager, StoreEntity>, IStoreManager
    {
        public StoreManager(ILogger<StoreManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<StoreDto> GetMultiple() =>
            new LSCoreListResponse<StoreDto>(Queryable().ToDtoList<StoreDto, StoreEntity>());
    }
}
