using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.ModuleHelper;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ModuleHelp;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
public class ModuleHelpsController(IModuleHelperManager moduleHelperManager) : ControllerBase
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
