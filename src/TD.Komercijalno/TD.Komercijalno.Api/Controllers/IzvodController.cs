using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Izvodi;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class IzvodController(IIzvodManager izvodManager) : ControllerBase
{
    [HttpGet]
    [Route("/izvodi")]
    public IActionResult GetMultiple([FromQuery] IzvodGetMultipleRequest request) =>
        Ok(izvodManager.GetMultiple(request));

}
