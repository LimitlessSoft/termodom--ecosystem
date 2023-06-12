using Microsoft.AspNetCore.Mvc;

namespace TD.Webshop.Api.Controllers
{
    [ApiController]
    public class TestController
    {
        [HttpGet("~/")]
        public string Hello()
        {
            return "hello";
        }
    }
}
