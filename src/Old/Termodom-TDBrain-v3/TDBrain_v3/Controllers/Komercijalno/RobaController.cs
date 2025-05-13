using System.ComponentModel.DataAnnotations;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class RobaController : Controller
	{
		private class NabavnaCenaDTO
		{
			public int RobaID { get; set; }
			public double NabavnaCenaBezPDV { get; set; }
		}

		private ILogger<RobaController> _logger { get; set; }

		#region Properties
		private static TimeSpan _dokumentiNabavkeUpdateInterval { get; set; } =
			TimeSpan.FromSeconds(30);
		private static DateTime? _dokumentiNabavkeLastUpdate { get; set; } = null;
		private static Task<List<DB.Komercijalno.DokumentManager>>? _dokumentiNabavke { get; set; }

		private static TimeSpan _stavkeNabavkeUpdateInterval { get; set; } =
			TimeSpan.FromSeconds(30);
		private static DateTime? _stavkeNabavkeLastUpdate { get; set; } = null;
		private static Task<Termodom.Data.Entities.Komercijalno.StavkaDictionary>? _stavkeNabavke { get; set; }

		private static TimeSpan _robaUpdateInterval { get; set; } = TimeSpan.FromSeconds(30);
		private static DateTime? _robaLastUpdate { get; set; } = null;
		private static Task<List<Termodom.Data.Entities.Komercijalno.Roba>>? _roba { get; set; }

		private static TimeSpan _nabavneCeneBufferUpdateInterval { get; set; } =
			TimeSpan.FromSeconds(30);
		private static DateTime? _nabavneCeneBufferLastUpdate { get; set; } = null;
		private static List<NabavnaCenaDTO> _nabavneCeneBuffer { get; set; } =
			new List<NabavnaCenaDTO>();
		#endregion

		/// <summary>
		/// Controller constructor
		/// </summary>
		/// <param name="logger"></param>
		public RobaController(ILogger<RobaController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Vraca objekat robe po zadatom ID-u
		/// </summary>
		/// <param name="robaID"></param>
		/// <param name="godina">Godina baze nad kojom se vrsi select. Ukoliko nije prosledjeno, akcija ce biti izvrsena nad bazom trenutne godine.</param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/Roba")]
		[Route("/Komercijalno/Roba/Get")]
		public Task<IActionResult> Get(
			[FromQuery] [Required] int robaID,
			[FromQuery] int? godinaBaze
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					if (godinaBaze == null)
						return Json(DB.Komercijalno.RobaManager.Get(robaID));
					else
						return Json(DB.Komercijalno.RobaManager.Get(robaID, (int)godinaBaze));
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, ex.ToString());
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Vraca dictionary informacija o robi.
		/// Key = ID, Value = objekat robe.
		/// </summary>
		/// <param name="godina"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/Roba")]
		[Route("/Komercijalno/Roba/Dictionary")]
		public Task<IActionResult> Dictionary([FromQuery] int? godina)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					if (godina == null)
						godina = DateTime.Now.Year;

					var col = DB.Komercijalno.RobaManager.Collection((int)godina);

					return Json(col);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, ex.ToString());
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Vraca realnu nabavnu cenu.
		/// Ukoliko se prosledi robaID, vraca samo za tu robu,
		/// u suprotnom vraca za svu robu iz magacina 50
		/// </summary>
		/// <param name="robaID"></param>
		/// <param name="datum">Format: dd-MM-yyyy</param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/Komercijalno/Roba")]
		[Route("/Komercijalno/Roba/GetNabavnaCena")]
		public Task<IActionResult> GetNabavnaCena(
			[FromQuery] [Required] string datum,
			[FromQuery] int[]? robaID
		)
		{
			return Task.Run<IActionResult>(async () =>
			{
				try
				{
					if (string.IsNullOrWhiteSpace(datum) || datum.Length != 10)
						return StatusCode(400, "Parametar 'datum' mora imati format 'dd-MM-yyyy'!");

					if (
						_nabavneCeneBufferLastUpdate != null
						&& Math.Abs(
							(
								DateTime.Now - ((DateTime)_nabavneCeneBufferLastUpdate)
							).TotalMilliseconds
						) < _nabavneCeneBufferUpdateInterval.TotalMilliseconds
					)
						return Json(_nabavneCeneBuffer);

					if (
						_dokumentiNabavkeLastUpdate == null
						|| _dokumentiNabavke == null
						|| Math.Abs(
							(
								DateTime.Now - ((DateTime)_dokumentiNabavkeLastUpdate)
							).TotalMilliseconds
						) > _dokumentiNabavkeUpdateInterval.TotalMilliseconds
					)
						_dokumentiNabavke = Task.Run(() =>
						{
							return DB.Komercijalno.DokumentManager.List(
								DateTime.Now.Year,
								new[] { DB.Settings.MainMagacinKomercijalno },
								new List<string>() { "MAGACINID = 50", "VRDOK IN (0, 1, 2, 36)" }
							);
						});

					if (
						_stavkeNabavkeLastUpdate == null
						|| _stavkeNabavke == null
						|| Math.Abs(
							(DateTime.Now - ((DateTime)_stavkeNabavkeLastUpdate)).TotalMilliseconds
						) > _stavkeNabavkeUpdateInterval.TotalMilliseconds
					)
						_stavkeNabavke = Task.Run(() =>
						{
							using (
								FbConnection con = new FbConnection(
									DB.Settings.ConnectionStringKomercijalno[
										DB.Settings.MainMagacinKomercijalno,
										DateTime.Now.Year
									]
								)
							)
							{
								con.Open();
								return DB.Komercijalno.StavkaManager.Dictionary(
									con,
									new List<string>()
									{
										"MAGACINID = 50",
										"VRDOK IN (0, 1, 2, 36)"
									}
								);
							}
						});

					if (
						_robaLastUpdate == null
						|| _roba == null
						|| Math.Abs((DateTime.Now - ((DateTime)_robaLastUpdate)).TotalMilliseconds)
							> _robaUpdateInterval.TotalMilliseconds
					)
						_roba = Task.Run(() =>
						{
							return DB
								.Komercijalno.RobaManager.Collection(DateTime.Now.Year)
								.Values.ToList();
						});

					string[] dParts = datum.Split('-');
					DateTime dat = new DateTime(
						Convert.ToInt32(dParts[2]),
						Convert.ToInt32(dParts[1]),
						Convert.ToInt32(dParts[0])
					);

					List<NabavnaCenaDTO> list = new List<NabavnaCenaDTO>();

					List<Termodom.Data.Entities.Komercijalno.Roba> rob = await _roba;
					if (robaID != null && robaID.Length > 0)
						rob.RemoveAll(x => !robaID.Contains(x.ID));

					Parallel.ForEach(
						rob,
						r =>
						{
							List<Termodom.Data.Entities.Komercijalno.Stavka> stavkeNabavke =
								_stavkeNabavke.Result.Values.Where(x => x.RobaID == r.ID).ToList();
							List<DB.Komercijalno.DokumentManager> doks =
								new List<DB.Komercijalno.DokumentManager>();

							foreach (Termodom.Data.Entities.Komercijalno.Stavka s in stavkeNabavke)
							{
								DB.Komercijalno.DokumentManager? d =
									_dokumentiNabavke.Result.FirstOrDefault(x =>
										x.VrDok == s.VrDok && x.BrDok == s.BrDok
									);
								if (d != null)
									doks.Add(d);
							}

							List<DB.Komercijalno.DokumentManager> dokumentiNabavke = doks;

							DB.Komercijalno.DokumentManager? dokument36 =
								dokumentiNabavke.FirstOrDefault(x =>
									x.VrDok == 36 && dat >= x.Datum && dat <= x.DatRoka
								);

							if (dokument36 != null)
							{
								list.Add(
									new NabavnaCenaDTO()
									{
										RobaID = r.ID,
										NabavnaCenaBezPDV = stavkeNabavke
											.First(x =>
												x.VrDok == dokument36.VrDok
												&& x.BrDok == dokument36.BrDok
											)
											.NabavnaCena
									}
								);
								return;
							}

							List<DB.Komercijalno.DokumentManager> dokumentiKojiDolazeUObzir =
								new List<DB.Komercijalno.DokumentManager>(dokumentiNabavke);
							dokumentiKojiDolazeUObzir.RemoveAll(x =>
								x.Datum > dat || !new int[] { 0, 1, 2, 3 }.Contains(x.VrDok)
							);
							dokumentiKojiDolazeUObzir.Sort((y, x) => x.Datum.CompareTo(y.Datum));

							DB.Komercijalno.DokumentManager? vazeciDokumentNabavke =
								dokumentiKojiDolazeUObzir.FirstOrDefault();

							if (vazeciDokumentNabavke == null)
							{
								list.Add(
									new NabavnaCenaDTO() { RobaID = r.ID, NabavnaCenaBezPDV = 0 }
								);
								return;
							}
							else
							{
								list.Add(
									new NabavnaCenaDTO()
									{
										RobaID = r.ID,
										NabavnaCenaBezPDV = stavkeNabavke
											.First(x =>
												x.VrDok == vazeciDokumentNabavke.VrDok
												&& x.BrDok == vazeciDokumentNabavke.BrDok
											)
											.NabavnaCena
									}
								);
								return;
							}
						}
					);

					return Json(list);
				}
				catch (Exception ex)
				{
					return StatusCode(500, ex.ToString());
				}
			});
		}

		/// <summary>
		/// Insertuje robu u baze svih magacina
		/// </summary>
		/// <param name="katBr"></param>
		/// <param name="katBrPro"></param>
		/// <param name="naziv"></param>
		/// <param name="vrsta"></param>
		/// <param name="grupaID"></param>
		/// <param name="podgrupaID"></param>
		/// <param name="proizvodjacID"></param>
		/// <param name="jm"></param>
		/// <param name="tarifaID"></param>
		/// <param name="trPakJM"></param>
		/// <param name="trPakKolicina"></param>
		/// <returns>Novi ROBAID</returns>
		[HttpPost]
		[Tags("/Komercijalno/Roba")]
		[Route("/Komercijalno/Roba/Insert")]
		public Task<IActionResult> Insert(
			[FromForm] [Required] string katBr,
			[FromForm] [Required] string katBrPro,
			[FromForm] [Required] string naziv,
			[FromForm] [Required] int vrsta,
			[FromForm] [Required] string grupaID,
			[FromForm] [Required] int podgrupaID,
			[FromForm] [Required] string proizvodjacID,
			[FromForm] [Required] string jm,
			[FromForm] [Required] string tarifaID,
			[FromForm] string? trPakJM,
			[FromForm] double? trPakKolicina
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					int noviRobaID = -1;
					foreach (
						string conS in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(
							DateTime.Now.Year
						)
					)
					{
						using (FbConnection con = new FbConnection(conS))
						{
							con.Open();
							noviRobaID = DB.Komercijalno.RobaManager.Insert(
								con,
								katBr,
								katBrPro,
								naziv,
								vrsta,
								grupaID,
								podgrupaID,
								proizvodjacID,
								jm,
								tarifaID,
								trPakJM,
								trPakKolicina
							);
							int[] magacini = DB.Settings.ConnectionStringKomercijalno.GetMagacini(
								DateTime.Now.Year,
								conS
							);

							// Sasa rekao da ovo sada uklanjamo 14.09.2023
							//foreach (int magacin in magacini)
							//    DB.Komercijalno.RobaUMagacinuManager.Insert(con, magacin, noviRobaID);
						}
					}
					return StatusCode(201, noviRobaID);
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Azurira podatke robe u bazi
		/// </summary>
		/// <param name="robaID"></param>
		/// <param name="katBr"></param>
		/// <param name="katBrPro"></param>
		/// <param name="naziv"></param>
		/// <param name="vrsta"></param>
		/// <param name="grupaID"></param>
		/// <param name="podgrupaID"></param>
		/// <param name="proizvodjacID"></param>
		/// <param name="jm"></param>
		/// <param name="tarifaID"></param>
		/// <param name="trPakJM"></param>
		/// <param name="trPakKolicina"></param>
		/// <returns></returns>
		[HttpPost]
		[Tags("/Komercijalno/Roba")]
		[Route("/Komercijalno/Roba/Update")]
		public Task<IActionResult> Update(
			[FromForm] [Required] int robaID,
			[FromForm] [Required] string katBr,
			[FromForm] [Required] string katBrPro,
			[FromForm] [Required] string naziv,
			[FromForm] [Required] int vrsta,
			[FromForm] [Required] string grupaID,
			[FromForm] [Required] int podgrupaID,
			[FromForm] [Required] string proizvodjacID,
			[FromForm] [Required] string jm,
			[FromForm] [Required] string tarifaID,
			[FromForm] string? trPakJM,
			[FromForm] double? trPakKolicina
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				foreach (
					string conS in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(
						DateTime.Now.Year
					)
				)
				{
					using (FbConnection con = new FbConnection(conS))
					{
						con.Open();
						DB.Komercijalno.RobaManager.Update(
							con,
							robaID,
							katBr,
							katBrPro,
							naziv,
							vrsta,
							grupaID,
							podgrupaID,
							proizvodjacID,
							jm,
							tarifaID,
							trPakJM,
							trPakKolicina
						);
						int[] magacini = DB.Settings.ConnectionStringKomercijalno.GetMagacini(
							DateTime.Now.Year,
							conS
						);
					}
				}
				return StatusCode(200);
			});
		}
	}
}
