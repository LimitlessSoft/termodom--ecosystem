using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers;

public class MCPartnerCenovnikKatBrRobaIdManager(
	ILogger<MCPartnerCenovnikKatBrRobaIdManager> logger,
	TDOfficeDbContext dbContext
)
	: LSCoreManagerBase<MCPartnerCenovnikKatBrRobaIdManager, MCPartnerCenovnikKatBrRobaIdEntity>(
		logger,
		dbContext
	),
		IMCPartnerCenovnikKatBrRobaIdManager
{
	public MCPartnerCenovnikKatBrRobaIdEntity Save(
		MCPartnerCenovnikKatBrRobaIdSaveRequest request
	) => Save(request);

	public List<MCPartnerCenovnikKatBrRobaIdEntity> GetMultiple(
		MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request
	) =>
		Queryable()
			.Where(x =>
				!request.DobavljacPPID.HasValue || x.DobavljacPPID == request.DobavljacPPID.Value
			)
			.ToList();
}
