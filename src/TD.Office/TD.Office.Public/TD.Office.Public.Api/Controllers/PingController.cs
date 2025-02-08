using Microsoft.AspNetCore.Mvc;
using TD.Common.Vault;
using TD.Office.Public.Contracts.Dtos.Vault;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
public class PingController() : ControllerBase
{
    [HttpGet]
    [Route("/ping")]
    public IActionResult Ping() =>
        Ok("Pong");
}