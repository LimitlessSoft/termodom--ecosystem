using System.ComponentModel.DataAnnotations;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class MagacinController : Controller
	{
		private ILogger<MagacinController> _logger { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="logger"></param>
		public MagacinController(ILogger<MagacinController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Vraca dictionary magacina u bazi za izabranu godinu.
		/// Key je magacinID, value je objekat magacina.
		/// </summary>
		/// <param name="godinaBaze"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("/komercijalno/magacin/dictionary")]
		public Task<IActionResult> Dictionary([Required] [FromQuery] int godinaBaze)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					using (
						FbConnection con = new FbConnection(
							DB.Settings.ConnectionStringKomercijalno[
								DB.Settings.MainMagacinKomercijalno,
								godinaBaze
							]
						)
					)
					{
						con.Open();
						return Json(MagacinManager.Dictionary(con));
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
