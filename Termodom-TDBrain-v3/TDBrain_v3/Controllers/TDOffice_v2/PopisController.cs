using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class PopisController : Controller
    {
        private ILogger<PopisController> _logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public PopisController(ILogger<PopisController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Vraca objekat dokumenta popisa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/TDOffice_v2/Popis")]
        [Route("/TDOffice_v2/Popis/Get")]
        public Task<IActionResult> Get(
            [FromQuery][Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(DB.TDOffice_v2.DokumentPopis.Get(id));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca dictionary dokumenata popisa
        /// </summary>
        /// <param name="magacinID">Opcioni parametar za filtriranje po magacinID-u</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/TDOffice_v2/Popis")]
        [Route("/TDOffice_v2/Popis/Dictionary")]
        public Task<IActionResult> Dictionary(
            [FromQuery] int? magacinID)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> whereParameters = new List<string>();

                    if (magacinID != null)
                        whereParameters.Add($"MAGACINID = {magacinID}");

                    return Json(DB.TDOffice_v2.DokumentPopis.Dictionary(whereParameters.Count > 0 ? string.Join(" AND ", whereParameters) : null));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
