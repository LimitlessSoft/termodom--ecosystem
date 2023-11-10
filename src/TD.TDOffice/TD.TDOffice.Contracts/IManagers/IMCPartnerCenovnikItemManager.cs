using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IMCPartnerCenovnikItemManager : ILSCoreBaseManager
    {
        LSCoreResponse Delete(LSCoreIdRequest request);
        LSCoreListResponse<MCpartnerCenovnikItemEntityGetDto> GetMultiple(MCPartnerCenovnikItemGetRequest request);
        LSCoreResponse<MCPartnerCenovnikItemEntity> Save(SaveMCPartnerCenovnikItemRequest request);
    }
}
