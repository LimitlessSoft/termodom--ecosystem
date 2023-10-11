using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IMCPartnerCenovnikKatBrRobaIdManager : IBaseManager
    {
        Response<MCPartnerCenovnikKatBrRobaIdEntity> Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request);
        ListResponse<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request);
    }
}
