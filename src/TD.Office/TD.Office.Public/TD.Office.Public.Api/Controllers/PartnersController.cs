using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access, Permission.PartneriRead)]
public class PartnersController(ITDKomercijalnoApiManager komercijalnoApiManager) : ControllerBase
{
    [HttpGet]
    [Route("/partners")]
    public async Task<IActionResult> GetPartners([FromQuery] PartneriGetMultipleRequest request) =>
        Ok(await komercijalnoApiManager.GetPartnersAsync(request));
}
