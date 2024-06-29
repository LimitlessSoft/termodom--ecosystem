using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using LSCore.Contracts.Requests;

namespace TD.TDOffice.Contracts.IManagers;

public interface IMCPartnerCenovnikItemManager
{
    void Delete(LSCoreIdRequest request);
    List<MCpartnerCenovnikItemEntityGetDto> GetMultiple(MCPartnerCenovnikItemGetRequest request);
    MCPartnerCenovnikItemEntity Save(SaveMCPartnerCenovnikItemRequest request);
}