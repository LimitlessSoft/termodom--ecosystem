using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;
using TD.OfficeServer.Contracts.IManagers;

namespace TD.OfficeServer.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        public PingController()
        {
        }

        [HttpGet]
        [Route("/ping")]
        public LSCoreResponse<string> Ping() => new LSCoreResponse<string>("Pong!");
    }
}