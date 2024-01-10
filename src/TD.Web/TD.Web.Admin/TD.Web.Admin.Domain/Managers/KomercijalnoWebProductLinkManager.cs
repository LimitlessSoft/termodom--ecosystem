using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class KomercijalnoWebProductLinkManager : LSCoreBaseManager<KomercijalnoWebProductLinkManager, KomercijalnoWebProductLinkEntity>, IKomercijalnoWebProductLinkManager
    {
        public KomercijalnoWebProductLinkManager(ILogger<KomercijalnoWebProductLinkManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<KomercijalnoWebProductLinksGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<KomercijalnoWebProductLinksGetDto>();

            var rQuery = Queryable();
            response.Merge(rQuery);
            if(response.NotOk)
                return response;

            response.Payload = rQuery.Payload!.ToDtoList<KomercijalnoWebProductLinksGetDto, KomercijalnoWebProductLinkEntity>();

            return response;
        }
    }
}
