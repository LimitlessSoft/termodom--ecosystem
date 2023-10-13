using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class MCPartnerCenovnikItemManager : BaseManager<MCPartnerCenovnikItemManager, MCPartnerCenovnikItemEntity>, IMCPartnerCenovnikItemManager
    {
        public MCPartnerCenovnikItemManager(ILogger<MCPartnerCenovnikItemManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response Delete(IdRequest request)
        {
            var response = new Response();

            var entityResponse = First(x => x.Id == request.Id);
            response.Merge(entityResponse);
            if(response.NotOk)
                return response;

            base.HardDelete(entityResponse.Payload);
            return response;
        }

        public ListResponse<MCpartnerCenovnikItemEntityGetDto> GetMultiple(MCPartnerCenovnikItemGetRequest request)
        {
            return new ListResponse<MCpartnerCenovnikItemEntityGetDto>(Queryable()
                .Where(x =>
                    (!request.PPID.HasValue || x.PPID == request.PPID.Value) &&
                    (!request.VaziOdDana.HasValue || x.VaziOdDana.Date == request.VaziOdDana.Value.Date))
                .ToList()
                .ToListDto());
        }

        public Response<MCPartnerCenovnikItemEntity> Save(SaveMCPartnerCenovnikItemRequest request)
        {
            return base.Save(request);
        }
    }
}
