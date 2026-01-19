using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.TipKorisnika;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.TipKorisnikaRead)]
public class TipKorisnikaController(ITipKorisnikaManager tipKorisnikaManager) : ControllerBase
{
	[HttpGet]
	[Route("/tip-korisnika")]
	public IActionResult GetMultiple() => Ok(tipKorisnikaManager.GetMultiple());

	[HttpPut]
	[Route("/tip-korisnika")]
	[Permissions(Permission.TipKorisnikaWrite)]
	public IActionResult Save([FromBody] SaveTipKorisnikaRequest request)
	{
		tipKorisnikaManager.Save(request);
		return Ok();
	}

	[HttpDelete]
	[Route("/tip-korisnika/{id}")]
	[Permissions(Permission.TipKorisnikaWrite)]
	public IActionResult Delete([FromRoute] long id)
	{
		tipKorisnikaManager.Delete(id);
		return Ok();
	}
}
