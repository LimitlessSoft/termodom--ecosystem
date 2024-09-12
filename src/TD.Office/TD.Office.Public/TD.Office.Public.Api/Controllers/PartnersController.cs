using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
public class PartnersController(ITDKomercijalnoApiManager komercijalnoApiManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/partners")]
    public IActionResult GetPartners([FromQuery] PartneriGetMultipleRequest request) =>
        Ok(komercijalnoApiManager.GetPartnersAsync(request));
}