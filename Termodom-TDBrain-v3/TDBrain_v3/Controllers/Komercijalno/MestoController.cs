using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class MestoController : Controller
    {
        /// <summary>
        /// Vraca dictionary mesta
        /// </summary>
        /// <returns></returns>
        /// <param name="godinaBaze">Godina baze. Ukoliko se ne prosledi vratice mesta iz baze trenutne godine.</param>
        [HttpGet]
        [Tags("/Komercijalno/Mesto")]
        [Route("/Komercijalno/Mesto/Dictionary")]
        public Task<IActionResult> Dictionary([FromQuery] int? godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(DB.Komercijalno.Mesta.Dictionary(godinaBaze));
            });
        }
    }
}
