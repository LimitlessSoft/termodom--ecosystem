using Microsoft.AspNetCore.Mvc;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public IActionResult Ping() =>
            Ok("Pong");
    }
}
