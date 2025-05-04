using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TDBrain_v3.Controllers
{
	/// <summary>
	///
	/// </summary>
	[ApiExplorerSettings(IgnoreApi = true)]
	[ApiController]
	public class HomeController : Controller
	{
		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("/")]
		public IActionResult Index()
		{
			return Json("Zdravo! 1");
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("/info")]
		public IActionResult Info()
		{
			return Json(Settings.ToDTO());
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("/log")]
		public Task<IActionResult> Log()
		{
			return Task.Run<IActionResult>(() =>
			{
				return Content(
					"Log se cita odozdo na gore!"
						+ Environment.NewLine
						+ Environment.NewLine
						+ string.Join(Environment.NewLine, Debug.Get())
				);
			});
		}
	}
}
