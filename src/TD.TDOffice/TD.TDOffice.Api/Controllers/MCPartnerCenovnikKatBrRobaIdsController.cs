using Microsoft.AspNetCore.Mvc;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Helpers.MCPartnerCenovnikKatBrRobaIdsHelpers;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Api.Controllers;

public class MCPartnerCenovnikKatBrRobaIdsController(
	IMCPartnerCenovnikKatBrRobaIdManager mcPartnerCenovnikKatBrRobaIdManager
) : ControllerBase
{
	[HttpGet]
	[Route("/mc-partner-cenovnik-kat-br-roba-ids")]
	public List<MCPartnerCenovnikKatBrRobaIdGetDto> GetMultiple(
		[FromQuery] MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request
	) => mcPartnerCenovnikKatBrRobaIdManager.GetMultiple(request).ToListDto();

	[HttpPut]
	[Route("/mc-partner-cenovnik-kat-br-roba-ids")]
	public MCPartnerCenovnikKatBrRobaIdEntity Save(
		[FromBody] MCPartnerCenovnikKatBrRobaIdSaveRequest request
	) => mcPartnerCenovnikKatBrRobaIdManager.Save(request);
}
