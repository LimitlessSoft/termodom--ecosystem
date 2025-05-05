using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.DtoMappings.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers;

public class MCPartnerCenovnikItemManager(
	ILogger<MCPartnerCenovnikItemManager> logger,
	TDOfficeDbContext dbContext
)
	: LSCoreManagerBase<MCPartnerCenovnikItemManager, MCPartnerCenovnikItemEntity>(
		logger,
		dbContext
	),
		IMCPartnerCenovnikItemManager
{
	public void Delete(LSCoreIdRequest request) => HardDelete(request.Id);

	public List<MCpartnerCenovnikItemEntityGetDto> GetMultiple(
		MCPartnerCenovnikItemGetRequest request
	) =>
		Queryable()
			.Where(x =>
				(!request.PPID.HasValue || x.PPID == request.PPID.Value)
				&& (
					!request.VaziOdDana.HasValue
					|| x.VaziOdDana.Date == request.VaziOdDana.Value.Date
				)
			)
			.ToList()
			.ToListDto();

	public MCPartnerCenovnikItemEntity Save(SaveMCPartnerCenovnikItemRequest request) =>
		Save(request);
}
