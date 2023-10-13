using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IMCPartnerCenovnikItemManager : IBaseManager
    {
        Response Delete(IdRequest request);
        ListResponse<MCpartnerCenovnikItemEntityGetDto> GetMultiple(MCPartnerCenovnikItemGetRequest request);
        Response<MCPartnerCenovnikItemEntity> Save(SaveMCPartnerCenovnikItemRequest request);
    }
}
