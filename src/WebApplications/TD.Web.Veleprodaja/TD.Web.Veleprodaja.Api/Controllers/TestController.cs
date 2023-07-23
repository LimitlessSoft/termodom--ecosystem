using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

namespace TD.Web.Veleprodaja.Api.Controllers
{
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("/test")]
        public Response<string> Get()
        {
            return new Response<string>("hi");
        }
    }
}
