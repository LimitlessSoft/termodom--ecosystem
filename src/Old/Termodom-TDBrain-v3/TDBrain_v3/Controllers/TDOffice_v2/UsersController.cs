using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
	public class UsersController : Controller
	{
		[HttpGet]
		[Tags("/TDOffice_v2/Users")]
		[Route("/TDOffice_v2/Users/Dictionary")]
		public Task<IActionResult> Dictionary()
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					return Json(UserManager.Dictionary());
				}
				catch (Exception ex)
				{
					return StatusCode(500);
				}
			});
		}
	}
}
