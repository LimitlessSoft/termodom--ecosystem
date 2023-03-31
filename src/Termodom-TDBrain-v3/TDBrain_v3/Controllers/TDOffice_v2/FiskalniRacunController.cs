using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;
using TDBrain_v3.RequestBodies.TDOffice;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
    [ApiController]
    public class FiskalniRacunController : Controller
    {
        private readonly ILogger<FiskalniRacunController> _logger;
        public FiskalniRacunController(ILogger<FiskalniRacunController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Vraca dictionary fiskalnih racuna iz baze
        /// </summary>
        /// <param name="odDatuma">Parametar od datuma mora biti u formatu dd-MM-yyyy. Moze biti null, inclusive</param>
        /// <param name="doDatuma">Parametar do datuma mora biti u formatu dd-MM-yyyy. Moze biti null, inclusive</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/TDOffice/FiskalniRacun")]
        [Route("/TDOffice/FiskalniRacun/Dictionary")]
        public Task<IActionResult> Dictionary([FromQuery] string odDatuma, [FromQuery] string doDatuma)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> whereParameters = new List<string>();

                    if (!string.IsNullOrWhiteSpace(odDatuma) && odDatuma.Length != 10)
                        return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
                    else if (!string.IsNullOrWhiteSpace(odDatuma))
                        whereParameters.Add($"SDCTIME_SERVER_TIME_ZONE >= '{odDatuma}'");

                    if (!string.IsNullOrWhiteSpace(doDatuma) && doDatuma.Length != 10)
                        return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
                    else if (!string.IsNullOrWhiteSpace(doDatuma))
                        whereParameters.Add($"SDCTIME_SERVER_TIME_ZONE <= '{doDatuma}'");

                    return Json(FiskalniRacunManager.Dictionary(whereParameters));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
