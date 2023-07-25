using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Api.Controllers
{
    [ApiController]
    public class TestController : Controller
    {
        public TestController(VeleprodajaDbContext dbContext)
        {

        }

        [HttpGet]
        [Route("/test")]
        public Response<string> Get()
        {
            return new Response<string>("hi");
        }
    }
}
