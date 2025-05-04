using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class PFRController : Controller
	{
		private readonly ILogger<PFRController> _logger;

		/// <summary>
		///
		/// </summary>
		/// <param name="logger"></param>
		public PFRController(ILogger<PFRController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Vraca dictionary PFR-a
		/// </summary>
		/// <param name="bazaId"></param>
		/// <param name="godinaBaze"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/PFR")]
		[Route("/Komercijalno/PFR/Dictionary")]
		public Task<IActionResult> Dictionary(
			[FromQuery] [Required] int bazaId,
			[FromQuery] int? godinaBaze
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					return Json(PFRManager.Dictionary(bazaId, godinaBaze));
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
					_logger.LogError(ex.Message);
					return StatusCode(500);
				}
			});
		}
	}
}
