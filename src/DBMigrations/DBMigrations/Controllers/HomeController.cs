using Microsoft.AspNetCore.Mvc;

namespace DBMigrations.Controllers
{
    public class HomeController : Controller
    {
        [Route("~/")]
        public IActionResult Index()
        {
            return Json("hi");
        }
    }
}
