using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Contracts.IManagers;

public interface IMCPartnerCenovnikKatBrRobaIdManager
{
	MCPartnerCenovnikKatBrRobaIdEntity Save(MCPartnerCenovnikKatBrRobaIdSaveRequest request);
	List<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(
		MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request
	);
}
