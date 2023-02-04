using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class KorisnikController : Controller
    {
        /// <summary>
        /// Vraca dictionary korisnika.
        /// Key je id korisnika, value je objekat korisnika
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/tdoffice_v2/korisnik/dictionary")]
        public Task<IActionResult> Dictionary()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(DB.TDOffice_v2.Korisnik.Collection().ToDictionary(x => x.ID));
            });
        }
    }
}
