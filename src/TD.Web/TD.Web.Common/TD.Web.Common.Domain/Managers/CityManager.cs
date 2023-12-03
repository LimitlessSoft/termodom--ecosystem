using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class CityManager : LSCoreBaseManager<CityManager, CityEntity>, ICityManager
    {
        public CityManager(ILogger<CityManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<CityDto> GetMultiple() =>
            new LSCoreListResponse<CityDto>(Queryable().ToDtoList<CityDto, CityEntity>());
    }
}
