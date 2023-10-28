using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

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
        public Response Ping()
        {
            return new Response();
        }
    }
}
