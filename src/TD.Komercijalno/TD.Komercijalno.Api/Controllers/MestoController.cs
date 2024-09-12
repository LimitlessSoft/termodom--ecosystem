using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class MestoController(IMestoManager mestoManager) : ControllerBase
{
    [HttpGet]
    [Route("/mesta")]
    public IActionResult GetMultiple() => Ok(mestoManager.GetMultiple());
}
