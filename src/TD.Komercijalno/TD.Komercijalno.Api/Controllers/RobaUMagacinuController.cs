using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class RobaUMagacinuController : Controller
    {
        public RobaUMagacinuController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
