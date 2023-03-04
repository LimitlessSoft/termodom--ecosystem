using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;

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
