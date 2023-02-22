using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class MagacinController : Controller
    {
        private ILogger<MagacinController> _logger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public MagacinController(ILogger<MagacinController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Vraca dictionary magacina u bazi za izabranu godinu.
        /// Key je magacinID, value je objekat magacina.
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/komercijalno/magacin/dictionary")]
        public Task<IActionResult> Dictionary([Required][FromQuery] int godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(DB.Komercijalno.MagacinManager.Collection(godina).ToDictionary(x => x.ID));
                }
                catch (Exceptions.PathToMainDatabaseNotFoundException ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(400, $"Putanja do baze magacina {ex.MagacinID} za godinu {ex.Godina} nije definisana!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca listu magacina u bazi za izabranu godinu
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/komercijalno/magacin/list")]
        [Obsolete]
        public Task<IActionResult> List([Required][FromQuery] int godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(DB.Komercijalno.MagacinManager.Collection(godina).ToList());
                }
                catch(Exceptions.PathToMainDatabaseNotFoundException ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(400, $"Putanja do baze magacina {ex.MagacinID} za godinu {ex.Godina} nije definisana!");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}