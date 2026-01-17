using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.TipOdsustva;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.TipOdsustvaRead)]
public class TipOdsustvaController(ITipOdsustvaManager tipOdsustvaManager) : ControllerBase
{
	[HttpGet]
	[Route("/tip-odsustva")]
	public IActionResult GetMultiple() => Ok(tipOdsustvaManager.GetMultiple());

	[HttpPut]
	[Route("/tip-odsustva")]
	[Permissions(Permission.TipOdsustvaWrite)]
	public IActionResult Save([FromBody] SaveTipOdsustvaRequest request)
	{
		tipOdsustvaManager.Save(request);
		return Ok();
	}

	[HttpDelete]
	[Route("/tip-odsustva/{id}")]
	[Permissions(Permission.TipOdsustvaWrite)]
	public IActionResult Delete([FromRoute] long id)
	{
		tipOdsustvaManager.Delete(id);
		return Ok();
	}
}
