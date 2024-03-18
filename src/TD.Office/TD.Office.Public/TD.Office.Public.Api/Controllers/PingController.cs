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
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://89.216.100.204:47810").Result;
            return new LSCoreResponse();
        }
    }
}
