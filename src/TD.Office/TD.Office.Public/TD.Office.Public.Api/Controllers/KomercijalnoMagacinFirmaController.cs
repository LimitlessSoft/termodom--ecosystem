using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
public class KomercijalnoMagacinFirmaController(IKomercijalnoMagacinFirmaManager manager)
	: ControllerBase
{
	[HttpGet("/komercijalno-magacin-firma/{magacinId}")]
	public IActionResult Get([FromRoute] int magacinId) => Ok(manager.Get(magacinId));
}
