using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class ParametriController(IParametarManager parametarManager) : ControllerBase
{
    [HttpPut]
    [Route("/parametri")]
    public IActionResult Update([FromBody] UpdateParametarRequest request)
    {
        parametarManager.Update(request);
        return Ok();
    }
}