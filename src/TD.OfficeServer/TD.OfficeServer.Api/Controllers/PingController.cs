using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;

namespace TD.OfficeServer.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public LSCoreResponse<string> Ping() => new LSCoreResponse<string>("Pong!");
    }
}