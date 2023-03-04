using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class TekuciRacunController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/TekuciRacun")]
        [Route("/Komercijalno/TekuciRacun/List")]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return Json(TekuciRacunManager.List());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
