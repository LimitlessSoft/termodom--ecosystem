using Microsoft.AspNetCore.Mvc;

namespace TD.Office.InterneOtpremnice.Api.Controllers;

public class PingController : ControllerBase
{
    [HttpGet]
    [Route("/ping")]
    public IActionResult Ping() => Ok("Pong");
}
