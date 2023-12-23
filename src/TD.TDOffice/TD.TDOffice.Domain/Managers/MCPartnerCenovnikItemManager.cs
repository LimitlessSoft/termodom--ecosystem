using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class MCPartnerCenovnikItemManager : LSCoreBaseManager<MCPartnerCenovnikItemManager, MCPartnerCenovnikItemEntity>, IMCPartnerCenovnikItemManager
    {
        public MCPartnerCenovnikItemManager(ILogger<MCPartnerCenovnikItemManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse Delete(LSCoreIdRequest request)
        {
            var response = new LSCoreResponse();

            var entityResponse = First(x => x.Id == request.Id);
            response.Merge(entityResponse);
            if(response.NotOk)
                return response;

            base.HardDelete(entityResponse.Payload);
            return response;
        }

        public LSCoreListResponse<MCpartnerCenovnikItemEntityGetDto> GetMultiple(MCPartnerCenovnikItemGetRequest request)
        {
            var response = new LSCoreListResponse<MCpartnerCenovnikItemEntityGetDto>();

            var qResponse = Queryable(x => x.IsActive &&
                (!request.PPID.HasValue || x.PPID == request.PPID.Value) &&
                (!request.VaziOdDana.HasValue || x.VaziOdDana.Date == request.VaziOdDana.Value.Date));
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList().ToListDto();
            return response;
        }

        public LSCoreResponse<MCPartnerCenovnikItemEntity> Save(SaveMCPartnerCenovnikItemRequest request)
        {
            return base.Save(request);
        }
    }
}
