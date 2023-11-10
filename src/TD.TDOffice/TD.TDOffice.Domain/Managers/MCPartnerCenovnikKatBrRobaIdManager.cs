using TD.TDOffice.Contracts.IManagers;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Repository;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;

namespace TD.TDOffice.Domain.Managers
{
    public class MCPartnerCenovnikKatBrRobaIdManager : LSCoreBaseManager<MCPartnerCenovnikKatBrRobaIdManager, MCPartnerCenovnikKatBrRobaIdEntity>, IMCPartnerCenovnikKatBrRobaIdManager
    {
        public MCPartnerCenovnikKatBrRobaIdManager(ILogger<MCPartnerCenovnikKatBrRobaIdManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<MCPartnerCenovnikKatBrRobaIdEntity> Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request)
        {
            return base.Save(request);
        }

        public LSCoreListResponse<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<MCPartnerCenovnikKatBrRobaIdEntity>();

            response.Payload = Queryable()
                .Where(x =>
                    (request.DobavljacPPID == null || x.DobavljacPPID == request.DobavljacPPID.Value))
                .ToList();

            return response;
        }
    }
}
