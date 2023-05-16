using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        [Route("~/")]
        public string Index()
        {
            return "hi";
        }
    }
}
