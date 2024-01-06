using TD.TDOffice.Contracts.IManagers;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Repository;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using LSCore.Contracts.Extensions;

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

            var qResponse = Queryable(x => x.IsActive &&
                (!request.DobavljacPPID.HasValue || x.DobavljacPPID == request.DobavljacPPID.Value));
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList();
            return response;
        }
    }
}
