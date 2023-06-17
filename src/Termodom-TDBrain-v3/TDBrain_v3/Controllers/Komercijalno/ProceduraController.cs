using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.DB.Komercijalno;
using TDBrain_v3.RequestBodies.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    [ApiController]
    public class ProceduraController : Controller
    {
        private ILogger<ProceduraController> _logger { get; set; }

        public ProceduraController(ILogger<ProceduraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/asd")]
        public IActionResult sad()
        {
            return Json(DateTime.Now);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Procedura")]
        [Route("/Komercijalno/Procedura/ProdajnaCenaNaDan")]
        [Consumes("application/json")]
        public Task<IActionResult> Collection([FromBody] ProceduraProdajnaCenaNaDanRequestBody request)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[request.BazaId, request.GodinaBaze]))
                    {
                        con.Open();
                        return Json(ProcedureManager.ProdajnaCenaNaDan(con, request.MagacinId, request.RobaId, request.Datum));
                    }
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
