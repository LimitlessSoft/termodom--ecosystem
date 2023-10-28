using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public Response Ping()
        {
            return new Response();
        }
    }
}
