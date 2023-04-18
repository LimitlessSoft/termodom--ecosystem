using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
    [ApiController]
    public class FiskalniRacunTaxItemController : Controller
    {
        private readonly ILogger<FiskalniRacunTaxItemController> _logger;
        public FiskalniRacunTaxItemController(ILogger<FiskalniRacunTaxItemController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Tags("/TDOffice/FiskalniRacunTaxItem")]
        [Route("/TDOffice/FiskalniRacunTaxItem/Insert")]
        public Task<IActionResult> Insert([FromBody] FiskalniRacunTaxItem[] taxItems)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
                    {
                        con.Open();

                        foreach(var taxItem in taxItems)
                            FiskalniRacunTaxItemManager.Insert(con, taxItem);
                    }

                    return StatusCode(201);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca dictionary FiskalniRacunTaxItem iz baze
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Tags("/TDOffice/FiskalniRacunTaxItem")]
        [Route("/TDOffice/FiskalniRacunTaxItem/Dictionary")]

        public Task<IActionResult> Dictionary([FromQuery] string[] invoiceNumber)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> whereParameters = new List<string>
                    {
                        $"INVOICE_NUMBER IN ({string.Join(", ", invoiceNumber.Select(x => $"'{x}'"))})"
                    };
                    return Json(FiskalniRacunTaxItemManager.Dictionary(whereParameters));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }

            });
        }

        
    }
}
