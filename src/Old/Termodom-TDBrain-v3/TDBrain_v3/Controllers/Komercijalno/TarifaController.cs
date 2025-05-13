using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class TarifaController : Controller
	{
		private ILogger<TarifaController> _logger { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="logger"></param>
		public TarifaController(ILogger<TarifaController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="godinaBaze"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/Tarifa")]
		[Route("/Komercijalno/Tarifa/Dictionary")]
		public Task<IActionResult> Dictionary([FromQuery] int? godinaBaze)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					return Json(TarifeManager.Dictionary(godinaBaze));
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					_logger.LogError(ex, ex.ToString());
					return StatusCode(500);
				}
			});
		}
	}
}
