using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Managers.TDOffice_v2;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class FirmaController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/TDOffice/Firma")]
        [Route("/TDOffice/Firma/Get")]
        public Task<IActionResult> Get([FromQuery][Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    Firma firma = FirmaManager.Get(id);

                    if (firma == null)
                        return StatusCode(204);

                    return Json(firma);
                }
                catch(Exception ex)
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
        [Tags("/TDOffice/Firma")]
        [Route("/TDOffice/Firma/Dictionary")]
        public Task<IActionResult> Dictionary()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(FirmaManager.Dictionary());
                }
                catch(Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
