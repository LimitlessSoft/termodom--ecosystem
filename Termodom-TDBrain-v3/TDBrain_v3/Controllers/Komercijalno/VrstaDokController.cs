using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class VrstaDokController : Controller
    {
        [HttpGet]
        [Tags("/Komercijalno/VrstaDok")]
        [Route("/Komercijalno/VrstaDok/Get")]
        public Task<IActionResult> Get([FromQuery][Required] int vrDok, [FromQuery] int? godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    VrstaDok vrstaDok = DB.Komercijalno.VrstaDokManager.Get(vrDok, godinaBaze);

                    if (vrstaDok == null)
                        return StatusCode(204);

                    return Json(vrstaDok);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/VrstaDok")]
        [Route("/Komercijalno/VrstaDok/Dictionary")]
        public Task<IActionResult> Dictionary([FromQuery] int? godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(DB.Komercijalno.VrstaDokManager.Dictionary(godinaBaze));
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
