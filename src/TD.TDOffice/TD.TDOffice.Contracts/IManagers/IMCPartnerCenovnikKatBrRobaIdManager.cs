using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IMCPartnerCenovnikKatBrRobaIdManager : ILSCoreBaseManager
    {
        LSCoreResponse<MCPartnerCenovnikKatBrRobaIdEntity> Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request);
        LSCoreListResponse<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request);
    }
}
