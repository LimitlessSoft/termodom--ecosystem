using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class GlobalAlertManager : LSCoreBaseManager<GlobalAlertManager, GlobalAlertEntity>, IGlobalAlertManager
    {
        public GlobalAlertManager(ILogger<GlobalAlertManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<GlobalAlertDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if(response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .Where(x => x.IsActive &&
                    x.Application == request.Application)
                .ToDtoList<GlobalAlertDto, GlobalAlertEntity>();
            return response;
        }
    }
}
