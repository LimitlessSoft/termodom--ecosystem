using LSCore.Framework.Attributes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
public class StoresController (IStoreManager storeManager)
    : ControllerBase
{
    [HttpGet]
    [LSCoreAuthorize]
    [Route("/stores")]
    public async Task<IActionResult> GetMultiple() => Ok(await storeManager.GetMultiple());
}