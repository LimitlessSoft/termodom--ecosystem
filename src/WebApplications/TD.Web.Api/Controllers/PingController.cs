using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class PingController : Controller
    {
        public PingController()
        {

        }

        [HttpGet]
        [Route("/ping")]
        public Response<string> Get()
        {
            return new Response<string>("Pinged GET!");
        }
    }
}
