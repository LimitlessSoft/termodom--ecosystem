using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Cities;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class CityManager : LSCoreBaseManager<CityManager, CityEntity>, ICityManager
    {
        public CityManager(ILogger<CityManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<CityDto> GetMultiple(GetMultipleCitiesRequest request)
        {
            var response = new LSCoreListResponse<CityDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            return new LSCoreListResponse<CityDto>(
                qResponse.Payload!
                    .SortQuery(request, CitiesSortColumnCodes.CitiesSortRules)
                    .ToDtoList<CityDto, CityEntity>());
        }
    }
}
