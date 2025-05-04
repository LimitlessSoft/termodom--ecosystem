using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;
using TDBrain_v3.RequestBodies.TDOffice;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
	[ApiController]
	public class FiskalniRacunController : Controller
	{
		private readonly ILogger<FiskalniRacunController> _logger;

		public FiskalniRacunController(ILogger<FiskalniRacunController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Insertuje fiskalni racun u bazu.
		/// Ukoliko fiskalni racun vec postoji, zaobilazi ga.
		/// Vraca listu invoiceNumber-a insertovanih racuna.
		/// </summary>
		/// <param name="fiskalniRacuni"></param>
		/// <returns></returns>
		[HttpPost]
		[Tags("/TDOffice/FiskalniRacun")]
		[Route("/TDOffice/FiskalniRacun/Insert")]
		public Task<IActionResult> Insert([FromBody] FiskalniRacun[] fiskalniRacuni)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					List<string> addedInvoices = new List<string>();
					using (
						FbConnection con = new FbConnection(
							DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()
						)
					)
					{
						con.Open();
						var postojeciFiskalniRacuni = FiskalniRacunManager.Dictionary();

						foreach (var fr in fiskalniRacuni)
							if (!postojeciFiskalniRacuni.ContainsKey(fr.InvoiceNumber))
							{
								FiskalniRacunManager.Insert(con, fr);
								addedInvoices.Add(fr.InvoiceNumber);
							}
					}
					return StatusCode(201, addedInvoices);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
					Debug.Log(ex.Message);
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Vraca dictionary fiskalnih racuna iz baze
		/// </summary>
		/// <param name="odDatuma">Parametar od datuma mora biti u formatu dd-MM-yyyy. Moze biti null, inclusive</param>
		/// <param name="doDatuma">Parametar do datuma mora biti u formatu dd-MM-yyyy. Moze biti null, inclusive</param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/TDOffice/FiskalniRacun")]
		[Route("/TDOffice/FiskalniRacun/Dictionary")]
		public Task<IActionResult> Dictionary(
			[FromQuery] string? odDatuma,
			[FromQuery] string? doDatuma
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					List<string> whereParameters = new List<string>();

					if (!string.IsNullOrWhiteSpace(odDatuma) && odDatuma.Length != 10)
						return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
					else if (!string.IsNullOrWhiteSpace(odDatuma))
						whereParameters.Add($"SDCTIME_SERVER_TIME_ZONE >= '{odDatuma}'");

					if (!string.IsNullOrWhiteSpace(doDatuma) && doDatuma.Length != 10)
						return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
					else if (!string.IsNullOrWhiteSpace(doDatuma))
						whereParameters.Add($"SDCTIME_SERVER_TIME_ZONE <= '{doDatuma}'");

					return Json(FiskalniRacunManager.Dictionary(whereParameters));
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
					Debug.Log(ex.Message);
					return StatusCode(500);
				}
			});
		}
	}
}
