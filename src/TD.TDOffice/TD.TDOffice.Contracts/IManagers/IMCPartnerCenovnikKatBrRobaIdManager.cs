using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Contracts.IManagers;

public interface IMCPartnerCenovnikKatBrRobaIdManager
{
    MCPartnerCenovnikKatBrRobaIdEntity Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request);
    List<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request);
}