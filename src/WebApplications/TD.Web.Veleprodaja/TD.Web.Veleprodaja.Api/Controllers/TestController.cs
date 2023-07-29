using LS.MinIO.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Api.Controllers
{
    [ApiController]
    public class TestController : Controller
    {
        private readonly IMinIOManager _minIOManager;
        public TestController(VeleprodajaDbContext dbContext, IMinIOManager minIOManager)
        {
            _minIOManager = minIOManager;
        }

        [HttpGet]
        [Route("/test")]
        public async Task<Response<string>> Get()
        {
            System.IO.File.WriteAllText("hello.txt", "Hello test asd");

            using (var fs = new FileStream("hello.txt", FileMode.OpenOrCreate))
            {
                await _minIOManager.UploadAsync("test-bucket", "test-object.txt", fs, "text/plain");
            }
            return new Response<string>("hi");
        }
    }
}
