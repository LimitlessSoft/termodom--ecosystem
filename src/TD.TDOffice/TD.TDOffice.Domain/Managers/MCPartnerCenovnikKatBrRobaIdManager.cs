using TD.Core.Domain.Managers;
using TD.TDOffice.Contracts.IManagers;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Repository;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Domain.Managers
{
    public class MCPartnerCenovnikKatBrRobaIdManager : BaseManager<MCPartnerCenovnikKatBrRobaIdManager, MCPartnerCenovnikKatBrRobaIdEntity>, IMCPartnerCenovnikKatBrRobaIdManager
    {
        public MCPartnerCenovnikKatBrRobaIdManager(ILogger<MCPartnerCenovnikKatBrRobaIdManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<MCPartnerCenovnikKatBrRobaIdEntity> Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request)
        {
            var response = new Response<int>();
            return base.Save(request);
        }

        public ListResponse<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request)
        {
            var response = new ListResponse<MCPartnerCenovnikKatBrRobaIdEntity>();

            response.Payload = Queryable()
                .Where(x =>
                    (request.DobavljacPPID == null || x.DobavljacPPID == request.DobavljacPPID.Value))
                .ToList();

            return response;
        }
    }
}
