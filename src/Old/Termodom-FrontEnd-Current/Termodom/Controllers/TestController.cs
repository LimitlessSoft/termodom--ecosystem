using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Termodom.Models;

namespace Termodom.Controllers
{
    public class TestController : Controller
    {
        [Route("/Test")]
        public IActionResult Index()
        {
            Log.WriteAsync(new LogovanjeGreske()
            {
                Date = DateTime.UtcNow.ToString("dd-MM HH:mm:ss"),
                Messages = "Testiraaaam",
            });
            return StatusCode(200);
        }
    }
}
