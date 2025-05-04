using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Webshop
{
	/// <summary>
	/// Koristi se za upravljanje proizvodima
	/// </summary>
	[ApiController]
	[RequireBearer]
	public class ProizvodController : Controller
	{
		private object _insertLock { get; set; } = new object();

		/// <summary>
		/// Azurira podatke proizvoda
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Webshop/Proizvod/Update")]
		[SwaggerOperation(Tags = new[] { "/Webshop/Proizvod" })]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Task<IActionResult> Update([Required] Models.Webshop.Proizvod o)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					using (
						MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop)
					)
					{
						con.Open();
						using (
							MySqlCommand cmd = new MySqlCommand(
								@"UPDATE PROIZVOD SET
SLIKA = @SLIKA,
PODGRUPAID = @PODGRUPAID,
AKTIVAN = @AKTIVAN,
PDV = @PDV,
DISPLAY_INDEX = @DISPLAY_INDEX,
KRATAK_OPIS = @KRATAK_OPIS,
POSETA = @POSETA,
RASPRODAJA = @RASPRODAJA,
KEYWORDS = @KEYWORDS,
NABAVNA_CENA = @NABAVNA_CENA,
PRODAJNA_CENA = @PRODAJNA_CENA,
REL = @REL,
DETALJAN_OPIS = @DETALJAN_OPIS,
KLASIFIKACIJA = @KLASIFIKACIJA,
TRANSPORTNO_PAKOVANJE = @TRANSPORTNO_PAKOVANJE,
TRANSPORTNO_PAKOVANJE_JM = @TRANSPORTNO_PAKOVANJE_JM,
KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU = @KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU,
ISTAKNUTI_PROIZVODI = @ISTAKNUTI_PROIZVODI,
UPOZORENJE_ZALIHA_MALIH_STOVARISTA = @UPOZORENJE_ZALIHA_MALIH_STOVARISTA,
CENOVNA_GRUPA_ID = @CENOVNA_GRUPA_ID,
POVEZANI_PROIZVODI = @POVEZANI_PROIZVODI,
AKTUELNI_RABAT = @AKTUELNI_RABAT,
SUBGROUPS = @SUBGROUPS,
PARENT = @PARENT,
BREND_ID = @BREND_ID
WHERE ROBAID = @ROBAID",
								con
							)
						)
						{
							cmd.Parameters.Add("@ROBAID", MySqlDbType.Int32);
							cmd.Parameters.Add("@SLIKA", MySqlDbType.VarChar);
							cmd.Parameters.Add("@PODGRUPAID", MySqlDbType.Int32);
							cmd.Parameters.Add("@AKTIVAN", MySqlDbType.Int16);
							cmd.Parameters.Add("@PDV", MySqlDbType.Double);
							cmd.Parameters.Add("@DISPLAY_INDEX", MySqlDbType.Int32);
							cmd.Parameters.Add("@KRATAK_OPIS", MySqlDbType.VarChar);
							cmd.Parameters.Add("@POSETA", MySqlDbType.Int32);
							cmd.Parameters.Add("@RASPRODAJA", MySqlDbType.Int16);
							cmd.Parameters.Add("@KEYWORDS", MySqlDbType.VarChar);
							cmd.Parameters.Add("@NABAVNA_CENA", MySqlDbType.Double);
							cmd.Parameters.Add("@PRODAJNA_CENA", MySqlDbType.Double);
							cmd.Parameters.Add("@REL", MySqlDbType.VarChar);
							cmd.Parameters.Add("@DETALJAN_OPIS", MySqlDbType.VarChar);
							cmd.Parameters.Add("@KLASIFIKACIJA", MySqlDbType.Int16);
							cmd.Parameters.Add("@TRANSPORTNO_PAKOVANJE", MySqlDbType.Double);
							cmd.Parameters.Add("@TRANSPORTNO_PAKOVANJE_JM", MySqlDbType.VarChar);
							cmd.Parameters.Add(
								"@KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU",
								MySqlDbType.Int16
							);
							cmd.Parameters.Add("@ISTAKNUTI_PROIZVODI", MySqlDbType.VarChar);
							cmd.Parameters.Add(
								"@UPOZORENJE_ZALIHA_MALIH_STOVARISTA",
								MySqlDbType.Int16
							);
							cmd.Parameters.Add("@CENOVNA_GRUPA_ID", MySqlDbType.Int16);
							cmd.Parameters.Add("@POVEZANI_PROIZVODI", MySqlDbType.VarChar);
							cmd.Parameters.Add("@AKTUELNI_RABAT", MySqlDbType.Double);
							cmd.Parameters.Add("@SUBGROUPS", MySqlDbType.VarChar);
							cmd.Parameters.Add("@PARENT", MySqlDbType.Int32);
							cmd.Parameters.Add("@BREND_ID", MySqlDbType.Int32);

							cmd.Parameters["@ROBAID"].Value = o.RobaID;
							cmd.Parameters["@SLIKA"].Value = o.Slika;
							cmd.Parameters["@PODGRUPAID"].Value = o.PodgrupaID;
							cmd.Parameters["@AKTIVAN"].Value = o.Aktivan;
							cmd.Parameters["@PDV"].Value = o.PDV;
							cmd.Parameters["@DISPLAY_INDEX"].Value = o.DisplayIndex;
							cmd.Parameters["@KRATAK_OPIS"].Value = o.KratakOpis;
							cmd.Parameters["@POSETA"].Value = o.Poseta;
							cmd.Parameters["@RASPRODAJA"].Value = o.Rasprodaja;
							cmd.Parameters["@KEYWORDS"].Value = o.Keywords;
							cmd.Parameters["@NABAVNA_CENA"].Value = o.NabavnaCena;
							cmd.Parameters["@PRODAJNA_CENA"].Value = o.ProdajnaCena;
							cmd.Parameters["@REL"].Value = o.Rel;
							cmd.Parameters["@DETALJAN_OPIS"].Value = o.DetaljanOpis;
							cmd.Parameters["@KLASIFIKACIJA"].Value = o.Klasifikacija;
							cmd.Parameters["@TRANSPORTNO_PAKOVANJE"].Value = o.TransportnoPakovanje;
							cmd.Parameters["@TRANSPORTNO_PAKOVANJE_JM"].Value =
								o.TransportnoPakovanjeJM;
							cmd.Parameters["@KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU"].Value =
								o.KupovinaSamoUTransportnomPakovanju;
							cmd.Parameters["@ISTAKNUTI_PROIZVODI"].Value = o.IstaknutiProizvodi;
							cmd.Parameters["@UPOZORENJE_ZALIHA_MALIH_STOVARISTA"].Value =
								o.UpozorenjeZalihaMalihStovarista;
							cmd.Parameters["@CENOVNA_GRUPA_ID"].Value = o.CenovnaGrupaID;
							cmd.Parameters["@POVEZANI_PROIZVODI"].Value = o.PovezaniProizvodi;
							cmd.Parameters["@AKTUELNI_RABAT"].Value = o.AktuelniRabat;
							cmd.Parameters["@SUBGROUPS"].Value = JsonConvert.SerializeObject(
								o.Podgrupe
							);
							cmd.Parameters["@PARENT"].Value = o.Parent;
							cmd.Parameters["@BREND_ID"].Value = o.BrendID;

							cmd.ExecuteNonQuery();

							return StatusCode(200);
						}
					}
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Azurira ID proizvoda
		/// </summary>
		/// <param name="oldID"></param>
		/// <param name="newID"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Webshop/Proizvod/ID/Set")]
		[SwaggerOperation(Tags = new[] { "/Webshop/Proizvod" })]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Task<IActionResult> IDSet(int oldID, int newID)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					using (
						MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop)
					)
					{
						con.Open();
						using (
							MySqlCommand cmd = new MySqlCommand(
								"UPDATE PROIZVOD SET ROBAID = @ID WHERE ROBAID = @OLD",
								con
							)
						)
						{
							cmd.Parameters.AddWithValue("@ID", newID);
							cmd.Parameters.AddWithValue("@OLD", oldID);
							cmd.ExecuteNonQuery();
						}
					}
					using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
					{
						con.Open();
						using (
							MySqlCommand cmd = new MySqlCommand(
								"UPDATE ROBA SET ROBAID = @NEWID WHERE ROBAID = @OLDID",
								con
							)
						)
						{
							cmd.Parameters.AddWithValue("@NEWID", newID);
							cmd.Parameters.AddWithValue("@OLDID", oldID);
							cmd.ExecuteNonQuery();
						}
					}
					return StatusCode(200);
				}
				catch (Exception)
				{
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Kreira novi proizvod u bazi
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Webshop/Proizvod/Insert")]
		[SwaggerOperation(Tags = new[] { "/Webshop/Proizvod" })]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public Task<IActionResult> Insert([Required] DTO.Webshop.ProizvodInsertDTO o)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					lock (_insertLock)
					{
						using (
							MySqlConnection con = new MySqlConnection(
								Program.ConnectionStringWebshop
							)
						)
						{
							con.Open();
							using (
								MySqlCommand cmd = new MySqlCommand(
									@"INSERT INTO PROIZVOD
(ROBAID, SLIKA, PODGRUPAID, AKTIVAN, PDV, DISPLAY_INDEX, KRATAK_OPIS, POSETA,
RASPRODAJA, KEYWORDS, NABAVNA_CENA, PRODAJNA_CENA, REL, DETALJAN_OPIS,
KLASIFIKACIJA, TRANSPORTNO_PAKOVANJE, TRANSPORTNO_PAKOVANJE_JM,
KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU, ISTAKNUTI_PROIZVODI,
UPOZORENJE_ZALIHA_MALIH_STOVARISTA, CENOVNA_GRUPA_ID, POVEZANI_PROIZVODI,
AKTUELNI_RABAT, SUBGROUPS, PARENT, BREND_ID)
VALUES
(@ROBAID, @SLIKA, @PODGRUPAID, @AKTIVAN, @PDV, @DISPLAY_INDEX, @KRATAK_OPIS,
@POSETA, @RASPRODAJA, @KEYWORDS, @NABAVNA_CENA, @PRODAJNA_CENA, @REL, @DETALJAN_OPIS,
@KLASIFIKACIJA, @TRANSPORTNO_PAKOVANJE, @TRANSPORTNO_PAKOVANJE_JM,
@KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU, @ISTAKNUTI_PROIZVODI,
@UPOZORENJE_ZALIHA_MALIH_STOVARISTA, @CENOVNA_GRUPA_ID, @POVEZANI_PROIZVODI,
@AKTUELNI_RABAT, @SUBGROUPS, @PARENT, @BREND_ID)",
									con
								)
							)
							{
								cmd.Parameters.Add("@ROBAID", MySqlDbType.Int32);
								cmd.Parameters.Add("@SLIKA", MySqlDbType.VarChar);
								cmd.Parameters.Add("@PODGRUPAID", MySqlDbType.Int32);
								cmd.Parameters.Add("@AKTIVAN", MySqlDbType.Int16);
								cmd.Parameters.Add("@PDV", MySqlDbType.Double);
								cmd.Parameters.Add("@DISPLAY_INDEX", MySqlDbType.Int32);
								cmd.Parameters.Add("@KRATAK_OPIS", MySqlDbType.VarChar);
								cmd.Parameters.Add("@POSETA", MySqlDbType.Int32);
								cmd.Parameters.Add("@RASPRODAJA", MySqlDbType.Int16);
								cmd.Parameters.Add("@KEYWORDS", MySqlDbType.VarChar);
								cmd.Parameters.Add("@NABAVNA_CENA", MySqlDbType.Double);
								cmd.Parameters.Add("@PRODAJNA_CENA", MySqlDbType.Double);
								cmd.Parameters.Add("@REL", MySqlDbType.VarChar);
								cmd.Parameters.Add("@DETALJAN_OPIS", MySqlDbType.VarChar);
								cmd.Parameters.Add("@KLASIFIKACIJA", MySqlDbType.Int16);
								cmd.Parameters.Add("@TRANSPORTNO_PAKOVANJE", MySqlDbType.Double);
								cmd.Parameters.Add(
									"@TRANSPORTNO_PAKOVANJE_JM",
									MySqlDbType.VarChar
								);
								cmd.Parameters.Add(
									"@KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU",
									MySqlDbType.Int16
								);
								cmd.Parameters.Add("@ISTAKNUTI_PROIZVODI", MySqlDbType.VarChar);
								cmd.Parameters.Add(
									"@UPOZORENJE_ZALIHA_MALIH_STOVARISTA",
									MySqlDbType.Int16
								);
								cmd.Parameters.Add("@CENOVNA_GRUPA_ID", MySqlDbType.Int16);
								cmd.Parameters.Add("@POVEZANI_PROIZVODI", MySqlDbType.VarChar);
								cmd.Parameters.Add("@AKTUELNI_RABAT", MySqlDbType.Double);
								cmd.Parameters.Add("@SUBGROUPS", MySqlDbType.VarChar);
								cmd.Parameters.Add("@PARENT", MySqlDbType.Int32);
								cmd.Parameters.Add("@BREND_ID", MySqlDbType.Int32);

								cmd.Parameters["@ROBAID"].Value = o.RobaID;
								cmd.Parameters["@SLIKA"].Value = o.Slika;
								cmd.Parameters["@PODGRUPAID"].Value = o.PodgrupaID;
								cmd.Parameters["@AKTIVAN"].Value = o.Aktivan;
								cmd.Parameters["@PDV"].Value = o.PDV;
								cmd.Parameters["@DISPLAY_INDEX"].Value = o.DisplayIndex;
								cmd.Parameters["@KRATAK_OPIS"].Value = o.KratakOpis;
								cmd.Parameters["@POSETA"].Value = o.Poseta;
								cmd.Parameters["@RASPRODAJA"].Value = o.Rasprodaja;
								cmd.Parameters["@KEYWORDS"].Value = o.Keywords;
								cmd.Parameters["@NABAVNA_CENA"].Value = o.NabavnaCena;
								cmd.Parameters["@PRODAJNA_CENA"].Value = o.ProdajnaCena;
								cmd.Parameters["@REL"].Value = o.Rel;
								cmd.Parameters["@DETALJAN_OPIS"].Value = o.DetaljanOpis;
								cmd.Parameters["@KLASIFIKACIJA"].Value = o.Klasifikacija;
								cmd.Parameters["@TRANSPORTNO_PAKOVANJE"].Value =
									o.TransportnoPakovanje;
								cmd.Parameters["@TRANSPORTNO_PAKOVANJE_JM"].Value =
									o.TransportnoPakovanjeJM;
								cmd.Parameters["@KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU"].Value =
									o.KupovinaSamoUTransportnomPakovanju;
								cmd.Parameters["@ISTAKNUTI_PROIZVODI"].Value = o.IstaknutiProizvodi;
								cmd.Parameters["@UPOZORENJE_ZALIHA_MALIH_STOVARISTA"].Value =
									o.UpozorenjeZalihaMalihStovarista;
								cmd.Parameters["@CENOVNA_GRUPA_ID"].Value = o.CenovnaGrupaID;
								cmd.Parameters["@POVEZANI_PROIZVODI"].Value = o.PovezaniProizvodi;
								cmd.Parameters["@AKTUELNI_RABAT"].Value = o.AktuelniRabat;
								cmd.Parameters["@SUBGROUPS"].Value = JsonConvert.SerializeObject(
									o.Podgrupe
								);
								cmd.Parameters["@PARENT"].Value = o.Parent;
								cmd.Parameters["@BREND_ID"].Value = o.BrendID;

								cmd.ExecuteNonQuery();

								return StatusCode(201);
							}
						}
					}
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Vraca informacije o proizvodu. Proslediti id ili rel
		/// </summary>
		/// <param name="id"></param>
		/// <param name="rel"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("/Webshop/Proizvod/Get")]
		[SwaggerOperation(Tags = new[] { "/Webshop/Proizvod" })]
		[Consumes(MediaTypeNames.Text.Plain)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public Task<IActionResult> Get(int id, string rel)
		{
			return Task.Run<IActionResult>(() =>
			{
				if (
					id <= 0 && string.IsNullOrWhiteSpace(rel)
					|| id > 0 && !string.IsNullOrWhiteSpace(rel)
				)
					return StatusCode(400);
				try
				{
					using (
						MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop)
					)
					{
						con.Open();
						string cmdText =
							@"SELECT ROBAID, SLIKA, PODGRUPAID, AKTIVAN, PDV, DISPLAY_INDEX, KRATAK_OPIS,
POSETA, RASPRODAJA, KEYWORDS, NABAVNA_CENA, PRODAJNA_CENA, REL, DETALJAN_OPIS, KLASIFIKACIJA,
TRANSPORTNO_PAKOVANJE, TRANSPORTNO_PAKOVANJE_JM, KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU, ISTAKNUTI_PROIZVODI,
UPOZORENJE_ZALIHA_MALIH_STOVARISTA, CENOVNA_GRUPA_ID, POVEZANI_PROIZVODI, AKTUELNI_RABAT, SUBGROUPS, PARENT,
BREND_ID FROM PROIZVOD WHERE ";

						if (id > 0)
							cmdText += "ROBAID = @ID";
						else
							cmdText += "REL = @REL";

						using (MySqlCommand cmd = new MySqlCommand(cmdText, con))
						{
							if (id > 0)
								cmd.Parameters.AddWithValue("@ID", id);
							else
								cmd.Parameters.AddWithValue("@REL", rel);

							using (MySqlDataReader dr = cmd.ExecuteReader())
								if (dr.Read())
									return StatusCode(
										200,
										new Models.Webshop.Proizvod()
										{
											RobaID = Convert.ToInt32(dr["ROBAID"]),
											Slika = dr["SLIKA"].ToString(),
											PodgrupaID = Convert.ToInt32(dr["PODGRUPAID"]),
											Aktivan = Convert.ToInt16(dr["AKTIVAN"]),
											PDV = Convert.ToDouble(dr["PDV"]),
											DisplayIndex = Convert.ToInt32(dr["DISPLAY_INDEX"]),
											KratakOpis = dr["KRATAK_OPIS"].ToString(),
											Poseta = Convert.ToInt32(dr["POSETA"]),
											Rasprodaja = Convert.ToInt16(dr["RASPRODAJA"]),
											Keywords =
												dr["KEYWORDS"] is DBNull
													? null
													: dr["KEYWORDS"].ToString(),
											NabavnaCena = Convert.ToDouble(dr["NABAVNA_CENA"]),
											ProdajnaCena = Convert.ToDouble(dr["PRODAJNA_CENA"]),
											Rel = dr["REL"].ToString(),
											DetaljanOpis =
												dr["DETALJAN_OPIS"] is DBNull
													? null
													: dr["DETALJAN_OPIS"].ToString(),
											Klasifikacija = Convert.ToInt16(dr["KLASIFIKACIJA"]),
											TransportnoPakovanje =
												dr["TRANSPORTNO_PAKOVANJE"] is DBNull
													? 0
													: Convert.ToDouble(dr["TRANSPORTNO_PAKOVANJE"]),
											TransportnoPakovanjeJM =
												dr["TRANSPORTNO_PAKOVANJE_JM"] is DBNull
													? null
													: dr["TRANSPORTNO_PAKOVANJE_JM"].ToString(),
											KupovinaSamoUTransportnomPakovanju = Convert.ToInt16(
												dr["KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU"]
											),
											IstaknutiProizvodi =
												dr["ISTAKNUTI_PROIZVODI"] is DBNull
													? null
													: dr["ISTAKNUTI_PROIZVODI"].ToString(),
											UpozorenjeZalihaMalihStovarista = Convert.ToInt16(
												dr["UPOZORENJE_ZALIHA_MALIH_STOVARISTA"]
											),
											CenovnaGrupaID = Convert.ToInt16(
												dr["CENOVNA_GRUPA_ID"]
											),
											PovezaniProizvodi =
												dr["POVEZANI_PROIZVODI"] is DBNull
													? null
													: dr["POVEZANI_PROIZVODI"].ToString(),
											AktuelniRabat = Convert.ToDouble(dr["AKTUELNI_RABAT"]),
											Podgrupe =
												dr["SUBGROUPS"] is DBNull
													? new List<int>()
													: JsonConvert.DeserializeObject<List<int>>(
														dr["SUBGROUPS"].ToString()
													),
											Parent = Convert.ToInt32(dr["PARENT"]),
											BrendID = Convert.ToInt32(dr["BREND_ID"])
										}
									);
						}
					}

					return StatusCode(404);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		/// <summary>
		/// Vraca listu proizvoda
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("/Webshop/Proizvod/List")]
		[SwaggerOperation(Tags = new[] { "/Webshop/Proizvod" })]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Task<IActionResult> List()
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					List<Models.Webshop.Proizvod> list = new List<Models.Webshop.Proizvod>();
					using (
						MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop)
					)
					{
						con.Open();
						using (
							MySqlCommand cmd = new MySqlCommand(
								@"SELECT ROBAID, SLIKA, PODGRUPAID, AKTIVAN, PDV, DISPLAY_INDEX, KRATAK_OPIS,
                                POSETA, RASPRODAJA, KEYWORDS, NABAVNA_CENA, PRODAJNA_CENA, REL, DETALJAN_OPIS, KLASIFIKACIJA,
                                TRANSPORTNO_PAKOVANJE, TRANSPORTNO_PAKOVANJE_JM, KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU, ISTAKNUTI_PROIZVODI,
                                UPOZORENJE_ZALIHA_MALIH_STOVARISTA, CENOVNA_GRUPA_ID, POVEZANI_PROIZVODI, AKTUELNI_RABAT, SUBGROUPS, PARENT,
                                BREND_ID FROM PROIZVOD",
								con
							)
						)
						using (MySqlDataReader dr = cmd.ExecuteReader())
							while (dr.Read())
								list.Add(
									new Models.Webshop.Proizvod()
									{
										RobaID = Convert.ToInt32(dr["ROBAID"]),
										Slika = dr["SLIKA"].ToString(),
										PodgrupaID = Convert.ToInt32(dr["PODGRUPAID"]),
										Aktivan = Convert.ToInt16(dr["AKTIVAN"]),
										PDV = Convert.ToDouble(dr["PDV"]),
										DisplayIndex = Convert.ToInt32(dr["DISPLAY_INDEX"]),
										KratakOpis = dr["KRATAK_OPIS"].ToString(),
										Poseta = Convert.ToInt32(dr["POSETA"]),
										Rasprodaja = Convert.ToInt16(dr["RASPRODAJA"]),
										Keywords =
											dr["KEYWORDS"] is DBNull
												? null
												: dr["KEYWORDS"].ToString(),
										NabavnaCena = Convert.ToDouble(dr["NABAVNA_CENA"]),
										ProdajnaCena = Convert.ToDouble(dr["PRODAJNA_CENA"]),
										Rel = dr["REL"].ToString(),
										DetaljanOpis =
											dr["DETALJAN_OPIS"] is DBNull
												? null
												: dr["DETALJAN_OPIS"].ToString(),
										Klasifikacija = Convert.ToInt16(dr["KLASIFIKACIJA"]),
										TransportnoPakovanje =
											dr["TRANSPORTNO_PAKOVANJE"] is DBNull
												? 0
												: Convert.ToDouble(dr["TRANSPORTNO_PAKOVANJE"]),
										TransportnoPakovanjeJM =
											dr["TRANSPORTNO_PAKOVANJE_JM"] is DBNull
												? null
												: dr["TRANSPORTNO_PAKOVANJE_JM"].ToString(),
										KupovinaSamoUTransportnomPakovanju = Convert.ToInt16(
											dr["KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU"]
										),
										IstaknutiProizvodi =
											dr["ISTAKNUTI_PROIZVODI"] is DBNull
												? null
												: dr["ISTAKNUTI_PROIZVODI"].ToString(),
										UpozorenjeZalihaMalihStovarista = Convert.ToInt16(
											dr["UPOZORENJE_ZALIHA_MALIH_STOVARISTA"]
										),
										CenovnaGrupaID = Convert.ToInt16(dr["CENOVNA_GRUPA_ID"]),
										PovezaniProizvodi =
											dr["POVEZANI_PROIZVODI"] is DBNull
												? null
												: dr["POVEZANI_PROIZVODI"].ToString(),
										AktuelniRabat = Convert.ToDouble(dr["AKTUELNI_RABAT"]),
										Podgrupe =
											dr["SUBGROUPS"] is DBNull
												? new List<int>()
												: JsonConvert.DeserializeObject<List<int>>(
													dr["SUBGROUPS"].ToString()
												),
										Parent = Convert.ToInt32(dr["PARENT"]),
										BrendID = Convert.ToInt32(dr["BREND_ID"])
									}
								);
					}
					return StatusCode(200, list);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}
	}
}
