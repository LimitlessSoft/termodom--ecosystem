using System.ComponentModel.DataAnnotations;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;
using TDBrain_v3.RequestBodies.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class TekuciRacunController : Controller
	{
		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/TekuciRacun")]
		[Route("/Komercijalno/TekuciRacun/List")]
		public Task<IActionResult> List(
			[FromQuery] [Required] int bazaId,
			[FromQuery] [Required] int godinaBaze
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					using (
						FbConnection con = new FbConnection(
							DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]
						)
					)
					{
						con.Open();
						return Json(TekuciRacunManager.List(con));
					}
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Tags("/Komercijalno/TekuciRacun")]
		[Route("/Komercijalno/TekuciRacun/Insert")]
		[Consumes("application/json")]
		public Task<IActionResult> Insert([FromBody] TekuciRacunInsertRequestBody request)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					TekuciRacunManager.Insert(request);
					return StatusCode(201);
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					return StatusCode(500);
				}
			});
		}
	}
}
