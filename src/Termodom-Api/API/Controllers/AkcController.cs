using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RequireBearer]
    [ApiController]
    public class AkcController : Controller
    {

        private readonly ILogger<AkcController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AkcController(ILogger<AkcController> logger)
        {
            _logger = logger;
        }

        public class InsertRequestBody
        {
            public string Action { get;set; }
            public int Sender { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/akc/insert")]
        [SwaggerOperation(Tags = new[] { "api/Akc"})]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public Task<IActionResult> Insert([FromBody] InsertRequestBody requestBody)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    Models.Akc.Insert(requestBody.Action, requestBody.Sender);
                    return StatusCode(201);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/akc/list")]
        [SwaggerOperation(Tags = new[] { "api/Akc"})]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(Models.Akc.List());
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/akc/delete/{id}")]
        [SwaggerOperation(Tags = new[] { "api/Akc" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Delete([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    Models.Akc.Delete(id);
                    return StatusCode(200);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

    }
}
