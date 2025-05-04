using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.Web
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class StavkaController : Controller
	{
		/// <summary>
		/// Vraca listu stavki za datu porudzbinu
		/// </summary>
		/// <param name="porudzbinaid"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Webshop/Stavka")]
		[Route("/webshop/stavka/list")]
		public Task<IActionResult> List([FromQuery] [Required] int porudzbinaid)
		{
			return Task.Run<IActionResult>(async () =>
			{
				try
				{
					var resp = await TDWebAPI.GetAsync(
						$"/webshop/stavka/list?porudzbinaid={porudzbinaid}"
					);
					return Content(await resp.Content.ReadAsStringAsync());
				}
				catch (Exception ex)
				{
					ex.Log();
					return StatusCode(500);
				}
			});
		}
	}
}
