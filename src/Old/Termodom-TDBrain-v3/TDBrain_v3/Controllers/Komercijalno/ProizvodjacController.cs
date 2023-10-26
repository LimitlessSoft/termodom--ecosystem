using Microsoft.AspNetCore.Mvc;
using System.Net;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    [ApiController]
    public class ProizvodjacController : Controller
    {
        private readonly ILogger<ProizvodjacController> _logger;

        public ProizvodjacController(ILogger<ProizvodjacController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Tags("/Komercijalno/Proizvodjac")]
        [Route("/Komercijalno/Proizvodjac/Dictionary")]
        public Task<IActionResult> Dictionary()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return StatusCode(501);
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
