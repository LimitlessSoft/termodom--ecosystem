using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Cities;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class CityManager (ILogger<CityManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<CityManager, CityEntity>(logger, dbContext), ICityManager
{
    public List<CityDto> GetMultiple(GetMultipleCitiesRequest request) =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<CityEntity, CityDto>();
    // TODO: Implement sorting by request with CitiesColumnCodes.CitiesSortRules
}