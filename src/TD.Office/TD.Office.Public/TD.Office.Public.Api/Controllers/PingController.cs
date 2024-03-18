using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public LSCoreResponse Ping()
        {
            return new LSCoreResponse();
        }
    }
}
