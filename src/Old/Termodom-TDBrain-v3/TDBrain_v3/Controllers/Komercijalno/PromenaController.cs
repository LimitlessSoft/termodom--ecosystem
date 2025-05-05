using System.ComponentModel.DataAnnotations;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class PromenaController : Controller
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="godina"></param>
		/// <param name="konto"></param>
		/// <param name="vrstaNal"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/komercijalno/promena")]
		[Route("/komercijalno/promena/list")]
		public Task<IActionResult> List(
			[Required] [FromQuery] int godina,
			[FromQuery] string[]? konto,
			[FromQuery] int[]? vrstaNal
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				List<string> whereParametrs = new List<string>();

				if (konto != null && konto.Length > 0)
					whereParametrs.Add($"KONTO IN ({string.Join(", ", konto)})");

				if (vrstaNal != null && vrstaNal.Length > 0)
					whereParametrs.Add($"VRSTANAL IN ({string.Join(", ", vrstaNal)})");

				return Json(
					DB.Komercijalno.PromenaManager.Collection(godina, whereParametrs).ToDictionary()
				);
			});
		}
	}
}
