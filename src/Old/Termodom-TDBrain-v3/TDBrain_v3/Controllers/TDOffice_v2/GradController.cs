using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class GradController : Controller
	{
		/// <summary>
		/// Vraca objekat grada sa datim ID-em
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Tags("/TDOffice_v2/Grad")]
		[Route("/TDOffice_v2/Grad/Get")]
		public Task<IActionResult> Get([FromQuery] [Required] int id)
		{
			return Task.Run<IActionResult>(() =>
			{
				Termodom.Data.Entities.TDOffice_v2.Grad? grad = GradManager.Get(id);

				if (grad == null)
					return StatusCode(204);

				return Json(grad);
			});
		}

		/// <summary>
		/// Vraca dictionary objekata grad
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Tags("/TDOffice_v2/Grad")]
		[Route("/TDOffice_v2/Grad/Dictionary")]
		public Task<IActionResult> Dictionary()
		{
			return Task.Run<IActionResult>(() =>
			{
				return Json(GradManager.Dictionary());
			});
		}
	}
}
