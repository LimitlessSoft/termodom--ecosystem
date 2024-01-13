using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
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

        public LSCoreResponse<KomercijalnoWebProductLinksGetDto> Put(KomercijalnoWebProductLinksSaveRequest request)
        {
            var response = new LSCoreResponse<KomercijalnoWebProductLinksGetDto>();

            var saveResponse = Save(request);
            response.Merge(saveResponse);
            if (response.NotOk)
                return response;

            response.Payload = new KomercijalnoWebProductLinksGetDto();
            response.Payload.InjectFrom(saveResponse.Payload!);
            return response;
        }

    }
}
