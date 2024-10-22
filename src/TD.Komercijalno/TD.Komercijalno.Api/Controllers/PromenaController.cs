using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Promene;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class PromenaController(IPromenaManager promenaManager) : ControllerBase
{
    [HttpGet]
    [Route("/promene")]
    public IActionResult GetMultiple([FromQuery] PromenaGetMultipleRequest request) =>
        Ok(promenaManager.GetMultiple(request));
}
