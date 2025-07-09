using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Dtos.ModuleHelp;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Contracts.Requests.ModuleHelp;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
public class ModuleHelpsController(IModuleHelpManager moduleHelperManager) : ControllerBase
{
	[HttpGet]
	[Route("/module-helps")]
	public ModuleHelpDto GetModuleHelps([FromQuery] GetModuleHelpRequest request) =>
		moduleHelperManager.GetModuleHelps(request);

	[HttpPut]
	[Route("/module-helps")]
	public IActionResult PutModuleHelps([FromBody] PutModuleHelpRequest request)
	{
		moduleHelperManager.PutModuleHelps(request);
		return Ok();
	}
}
