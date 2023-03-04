using Microsoft.AspNetCore.Mvc;

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
