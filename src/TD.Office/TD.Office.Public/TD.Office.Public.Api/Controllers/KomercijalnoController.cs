using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class KomercijalnoController(ITDKomercijalnoApiManager komercijalnoApiManager)
	: ControllerBase
{
	[HttpGet]
	[Route("/komercijalno-vr-dok")]
	public async Task<IActionResult> GetMultipleVrDok() =>
		Ok(await komercijalnoApiManager.GetMultipleVrDokAsync());

	[HttpGet]
	[Route("/komercijalno-nacini-placanja")]
	public async Task<IActionResult> GetMultipleNaciniPlacanja() =>
		Ok(await komercijalnoApiManager.GetMultipleNaciniPlacanjaAsync());

	[HttpGet]
	[Route("/komercijalno-roba")]
	public async Task<IActionResult> GetMultipleRoba([FromQuery] RobaGetMultipleRequest request) =>
		Ok(await komercijalnoApiManager.GetMultipleRobaAsync(request));
}
