using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        public PingController()
        {

        }

        [HttpGet]
        [Route("/ping")]
        public LSCoreResponse Ping()
        {
            return new LSCoreResponse();
        }
    }
}
