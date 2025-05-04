using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
public class StoresController(IStoreManager storeManager) : ControllerBase
{
	[HttpGet]
	[LSCoreAuth]
	[Route("/stores")]
	public async Task<IActionResult> GetMultiple() => Ok(await storeManager.GetMultiple());
}
