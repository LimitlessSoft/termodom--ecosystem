using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
	public class Stavka
	{
		public int StavkaID { get; set; }
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public int MagacinID { get; set; }
		public int RobaID { get; set; }
		public int? Vrsta { get; set; }
		public string Naziv { get; set; }
		public double? NabCenSaPor { get; set; }
		public double? FakturnaCena { get; set; }
		public double? NabCenaBT { get; set; }
		public double? Troskovi { get; set; }
		public double NabavnaCena { get; set; }
		public double ProdCenaBP { get; set; }
		public double? Korekcija { get; set; }
		public double ProdajnaCena { get; set; }
		public double DeviznaCena { get; set; }
		public double? DevProdCena { get; set; }
		public double Kolicina { get; set; }
		public double NivKol { get; set; }
		public string TarifaID { get; set; }
		public int? ImaPorez { get; set; }
		public double Porez { get; set; }
		public double Rabat { get; set; }
		public double Marza { get; set; }
		public double? Taksa { get; set; }
		public double? Akciza { get; set; }
		public double ProsNab { get; set; }
		public double PreCena { get; set; }
		public double PreNab { get; set; }
		public double ProsProd { get; set; }
		public string MTID { get; set; }
		public char PT { get; set; }
		public string Zvezdica { get; set; }
		public double? TrenStanje { get; set; }
		public double PorezUlaz { get; set; }
		public DateTime? SDatum { get; set; }
		public double? DevNabCena { get; set; }
		public double PorezIz { get; set; }
		public double? X4 { get; set; }
		public double? Y4 { get; set; }
		public double? Z4 { get; set; }
		public double? CenaPoAJM { get; set; }
		public int? KGID { get; set; }
		public double SAkciza { get; set; }

		private static object _insertLock = new object();

		static Stavka()
		{
			//_buffer.StartUpdatingLoopAsync(TimeSpan.FromMilliseconds(200));
		}

		/// <summary>
		/// Azurira podatke stavke unutar baze. Osnova: STAVKAID
		/// </summary>
		public void Update(int godina)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				Update(con);
			}
		}

		/// <summary>
		/// Azurira podatke stavke unutar baze. Osnova: STAVKAID
		/// </summary>
		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE STAVKA SET
                                                        VRSTA = @VRSTA,
                                                        NAZIV = @NAZIV,
                                                        NABCENSAPOR = @NABCENSAPOR,
                                                        FAKTURNACENA = @FAKTURNACENA,
                                                        NABCENABT = @NABCENABT,
                                                        TROSKOVI = @TROSKOVI,
                                                        NABAVNACENA = @NABAVNACENA,
                                                        PRODCENABP = @PRODCENABP,
                                                        KOREKCIJA = @KOREKCIJA,
                                                        PRODAJNACENA = @PRODAJNACENA,
                                                        DEVIZNACENA = @DEVIZNACENA,
                                                        KOLICINA = @KOLICINA,
                                                        NIVKOL = @NIVKOL,
                                                        TARIFAID = @TARIFAID,
                                                        IMAPOREZ = @IMAPOREZ,
                                                        POREZ = @POREZ,
                                                        RABAT = @RABAT,
                                                        MARZA = @MARZA,
                                                        TAKSA = @TAKSA,
                                                        AKCIZA = @AKCIZA,
                                                        PROSNAB = @PROSNAB,
                                                        PRECENA = @PRECENA,
                                                        PRENAB = @PRENAB,
                                                        PROSPROD = @PROSPROD,
                                                        MTID = @MTID,
                                                        PT = @PT,
                                                        ZVEZDICA = @ZVEZDICA,
                                                        TREN_STANJE = @TREN_STANJE,
                                                        POREZ_ULAZ = @POREZ_ULAZ,
                                                        SDATUM = @SDATUM,
                                                        DEVNABCENA = @DEVNABCENA,
                                                        POREZ_IZ = @POREZ_IZ,
                                                        X4 = @X4,
                                                        Y4 = @Y4,
                                                        Z4 = @Z4,
                                                        CENAPOAJM = @CENAPOAJM,
                                                        KGID = @KGID,
                                                        SAKCIZA = @SAKCIZA
                                                        WHERE STAVKAID = @SID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRSTA", Vrsta);
				cmd.Parameters.AddWithValue("@NAZIV", Naziv);
				cmd.Parameters.AddWithValue("@NABCENSAPOR", NabCenSaPor);
				cmd.Parameters.AddWithValue("@FAKTURNACENA", FakturnaCena);
				cmd.Parameters.AddWithValue("@NABCENABT", NabCenaBT);
				cmd.Parameters.AddWithValue("@TROSKOVI", Troskovi);
				cmd.Parameters.AddWithValue("@NABAVNACENA", NabavnaCena);
				cmd.Parameters.AddWithValue("@PRODCENABP", ProdCenaBP);
				cmd.Parameters.AddWithValue("@KOREKCIJA", Korekcija);
				cmd.Parameters.AddWithValue("@PRODAJNACENA", ProdajnaCena);
				cmd.Parameters.AddWithValue("@DEVIZNACENA", DeviznaCena);
				cmd.Parameters.AddWithValue("@KOLICINA", Kolicina);
				cmd.Parameters.AddWithValue("@NIVKOL", NivKol);
				cmd.Parameters.AddWithValue("@TARIFAID", TarifaID);
				cmd.Parameters.AddWithValue("@IMAPOREZ", ImaPorez);
				cmd.Parameters.AddWithValue("@POREZ", Porez);
				cmd.Parameters.AddWithValue("@RABAT", Rabat);
				cmd.Parameters.AddWithValue("@MARZA", Marza);
				cmd.Parameters.AddWithValue("@TAKSA", Taksa);
				cmd.Parameters.AddWithValue("@AKCIZA", Akciza);
				cmd.Parameters.AddWithValue("@PROSNAB", ProsNab);
				cmd.Parameters.AddWithValue("@PRECENA", PreCena);
				cmd.Parameters.AddWithValue("@PRENAB", PreNab);
				cmd.Parameters.AddWithValue("@PROSPROD", ProsProd);
				cmd.Parameters.AddWithValue("@MTID", MTID);
				cmd.Parameters.AddWithValue("@PT", PT);
				cmd.Parameters.AddWithValue("@ZVEZDICA", Zvezdica);
				cmd.Parameters.AddWithValue("@TREN_STANJE", TrenStanje);
				cmd.Parameters.AddWithValue("@POREZ_ULAZ", PorezUlaz);
				cmd.Parameters.AddWithValue("@SDATUM", SDatum);
				cmd.Parameters.AddWithValue("@DEVNABCENA", DevNabCena);
				cmd.Parameters.AddWithValue("@POREZ_IZ", PorezIz);
				cmd.Parameters.AddWithValue("@X4", X4);
				cmd.Parameters.AddWithValue("@Y4", Y4);
				cmd.Parameters.AddWithValue("@Z4", Z4);
				cmd.Parameters.AddWithValue("@CENAPOAJM", CenaPoAJM);
				cmd.Parameters.AddWithValue("@KGID", KGID);
				cmd.Parameters.AddWithValue("@SAKCIZA", SAkciza);
				cmd.Parameters.AddWithValue("@SID", StavkaID);

				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Dodaje novu stavku u bazu
		/// </summary>
		/// <param name="dokument"></param>
		/// <param name="roba"></param>
		/// <param name="robaUMagacinu"></param>
		/// <param name="kolicina"></param>
		/// <param name="rabat"></param>
		/// <param name="prodajnaCenaBezPDV"></param>
		/// <returns></returns>
		public static int Insert(
			int godina,
			Dokument dokument,
			Roba roba,
			RobaUMagacinu robaUMagacinu,
			double? kolicina,
			double rabat,
			double? prodajnaCenaBezPDV = null
		)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				return Insert(
					con,
					dokument,
					roba,
					robaUMagacinu,
					kolicina,
					rabat,
					prodajnaCenaBezPDV
				);
			}
		}

		/// <summary>
		/// Dodaje novu stavku u bazu
		/// </summary>
		/// <param name="con"></param>
		/// <param name="dokument"></param>
		/// <param name="roba"></param>
		/// <param name="robaUMagacinu"></param>
		/// <param name="kolicina"></param>
		/// <param name="rabat"></param>
		/// <param name="prodajnaCenaBezPDV"></param>
		/// <returns></returns>
		public static int Insert(
			FbConnection con,
			Dokument dokument,
			Roba roba,
			RobaUMagacinu robaUMagacinu,
			double? kolicina,
			double rabat,
			double? prodajnaCenaBezPDV = null
		)
		{
			lock (_insertLock)
			{
				List<Tarife> tarife = Tarife.List();
				using (
					FbCommand cmd = new FbCommand(
						@"INSERT INTO STAVKA
                (VRDOK, BRDOK, MAGACINID, ROBAID, VRSTA, NAZIV, NABCENSAPOR, FAKTURNACENA, NABCENABT,
                TROSKOVI, NABAVNACENA, PRODCENABP, KOREKCIJA, PRODAJNACENA, DEVIZNACENA, DEVPRODCENA, KOLICINA,
                NIVKOL, TARIFAID, IMAPOREZ, POREZ, RABAT, MARZA, TAKSA, AKCIZA, PROSNAB, PRECENA, PRENAB, PROSPROD,
                MTID, PT, TREN_STANJE, POREZ_ULAZ, DEVNABCENA, POREZ_IZ)
                VALUES (@VRDOK, @BRDOK, @MAGACINID, @ROBAID, 1, @NAZIV, 0, 0, 0, 
                0, @NABAVNACENA, @CENA_BEZ_PDV, 0, @CENA_SA_PDV, 0, 0, @KOL,
                0, @TARIFAID, 0, @POREZ, @RABAT, 0, 0, 0, 0, 0, 0, 0, 
                @MTID, 'P', 0, 0, 0, @POREZ) RETURNING STAVKAID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@VRDOK", dokument.VrDok);
					cmd.Parameters.AddWithValue("@BRDOK", dokument.BrDok);
					cmd.Parameters.AddWithValue("@MAGACINID", dokument.MagacinID);
					cmd.Parameters.AddWithValue("@ROBAID", roba.ID);
					cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
					cmd.Parameters.AddWithValue("@NABAVNACENA", robaUMagacinu.NabavnaCena);
					cmd.Parameters.AddWithValue(
						"@CENA_SA_PDV",
						prodajnaCenaBezPDV == null
							? Procedure.ProdajnaCenaNaDan(
								dokument.MagacinID,
								roba.ID,
								dokument.Datum
							)
							: prodajnaCenaBezPDV
								* (
									1
									+ (
										tarife
											.Where(x => x.TarifaID == roba.TarifaID)
											.FirstOrDefault()
											.Stopa / 100
									)
								)
					);
					cmd.Parameters.AddWithValue(
						"@CENA_BEZ_PDV",
						prodajnaCenaBezPDV == null
							? (double)cmd.Parameters["@CENA_SA_PDV"].Value
								* (
									1
									- (
										tarife
											.Where(x => x.TarifaID == roba.TarifaID)
											.FirstOrDefault()
											.Stopa
										/ (
											100
											+ tarife
												.Where(x => x.TarifaID == roba.TarifaID)
												.FirstOrDefault()
												.Stopa
										)
									)
								)
							: Procedure.ProdajnaCenaNaDan(
								dokument.MagacinID,
								roba.ID,
								dokument.Datum
							)
					);
					cmd.Parameters.AddWithValue("@KOL", kolicina);
					cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
					cmd.Parameters.AddWithValue(
						"@POREZ",
						Tarife
							.BufferedList()
							.Where(x => x.TarifaID == roba.TarifaID)
							.FirstOrDefault()
							.Stopa
					);
					cmd.Parameters.AddWithValue("@RABAT", rabat);
					cmd.Parameters.AddWithValue(
						"@MTID",
						Magacin.Get(DateTime.Now.Year, dokument.MagacinID).MTID
					);

					cmd.Parameters.Add(
						new FbParameter
						{
							ParameterName = "STAVKAID",
							FbDbType = FbDbType.Integer,
							Direction = System.Data.ParameterDirection.Output
						}
					);

					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						var a = ex;
					}
					return Convert.ToInt32(cmd.Parameters["STAVKAID"].Value);
				}
			}
		}

		/// <summary>
		/// Uklanja stavku iz baze
		/// </summary>
		/// <param name="stavkaID"></param>
		public static void Remove(int godina, int stavkaID)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				Remove(con, stavkaID);
			}
		}

		/// <summary>
		/// Uklanja stavku iz baze
		/// </summary>
		/// <param name="stavkaID"></param>
		public static void Remove(FbConnection con, int stavkaID)
		{
			using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA WHERE STAVKAID = @SID", con))
			{
				cmd.Parameters.AddWithValue("@SID", stavkaID);

				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Uklanja stavke iz baze
		/// </summary>
		/// <param name="stavkaID"></param>
		public static void RemoveAllFromDocument(int godina, int vrDok, int brDok)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				RemoveAllFromDocument(con, vrDok, brDok);
			}
		}

		/// <summary>
		/// Uklanja stavke iz baze
		/// </summary>
		/// <param name="stavkaID"></param>
		public static void RemoveAllFromDocument(FbConnection con, int vrDok, int brDok)
		{
			using (
				FbCommand cmd = new FbCommand(
					"DELETE FROM STAVKA WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				cmd.Parameters.AddWithValue("@BRDOK", brDok);

				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Vraca stavku iz baze
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Stavka Get(int godina, int id)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				return Get(con, id);
			}
		}

		/// <summary>
		/// Vraca stavku iz baze
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Stavka Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE STAVKAID = @SID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@SID", id);
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Stavka()
						{
							StavkaID = Convert.ToInt32(dr["STAVKAID"]),
							VrDok = Convert.ToInt32(dr["VRDOK"]),
							BrDok = Convert.ToInt32(dr["BRDOK"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							Vrsta =
								dr["VRSTA"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRSTA"]),
							Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
							NabCenSaPor =
								dr["NABCENSAPOR"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
							FakturnaCena =
								dr["FAKTURNACENA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
							NabCenaBT =
								dr["NABCENABT"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["NABCENABT"]),
							Troskovi =
								dr["TROSKOVI"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["TROSKOVI"]),
							NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
							ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
							Korekcija =
								dr["KOREKCIJA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
							ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
							DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
							DevProdCena =
								dr["DEVPRODCENA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
							Kolicina = Convert.ToDouble(dr["KOLICINA"]),
							NivKol = Convert.ToDouble(dr["NIVKOL"]),
							TarifaID = dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
							ImaPorez =
								dr["IMAPOREZ"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
							Porez = Convert.ToDouble(dr["POREZ"]),
							Rabat = Convert.ToDouble(dr["RABAT"]),
							Marza = Convert.ToDouble(dr["MARZA"]),
							Taksa =
								dr["TAKSA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["TAKSA"]),
							Akciza =
								dr["AKCIZA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["AKCIZA"]),
							ProsNab = Convert.ToDouble(dr["PROSNAB"]),
							PreCena = Convert.ToDouble(dr["PRECENA"]),
							PreNab = Convert.ToDouble(dr["PRENAB"]),
							ProsProd = Convert.ToDouble(dr["PROSPROD"]),
							MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
							PT = Convert.ToChar(dr["PT"]),
							Zvezdica = dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
							TrenStanje =
								dr["TREN_STANJE"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
							PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
							SDatum =
								dr["SDATUM"] is DBNull
									? null
									: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
							DevNabCena =
								dr["DEVNABCENA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
							PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
							X4 = dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
							Y4 = dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
							Z4 = dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
							CenaPoAJM =
								dr["CENAPOAJM"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
							KGID = dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
							SAkciza = Convert.ToDouble(dr["SAKCIZA"])
						};
			}
			return null;
		}

		/// <summary>
		/// Vraca listu svih stavki iz baze
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static Task<List<Stavka>> ListAsync(int godina, string whereQuery = null)
		{
			return Task.Run(() =>
			{
				return List(godina, whereQuery);
			});
		}

		/// <summary>
		/// Vraca listu svih stavki iz baze
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> List(int godina, string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		/// <summary>
		/// Vraca listu svih stavki iz baze
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " WHERE " + whereQuery;

			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA" + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
				}
			}

			return list;
		}

		/// <summary>
		/// Vraca listu svih stavki iz baze
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static Task<List<Stavka>> ListByMagacinIDAsync(int magacinID)
		{
			return Task.Run(() =>
			{
				return ListByMagacinID(magacinID);
			});
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu magacina
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> ListByMagacinID(int magacinID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		/// <summary>
		/// Vraca listu svih stavki iz baze
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static Task<List<Stavka>> ListByMagacinIDAsync(FbConnection con, int magacinID)
		{
			return Task.Run(() =>
			{
				return ListByMagacinID(con, magacinID);
			});
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu magacina
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> ListByMagacinID(FbConnection con, int magacinID)
		{
			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
				}
			}

			return list;
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu robaID-a
		/// </summary>
		/// <param name="con"></param>
		/// <param name="robaID"></param>
		/// <returns></returns>
		public static List<Stavka> ListByRobaID(int robaID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByRobaID(con, robaID);
			}
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu robaID-a
		/// </summary>
		/// <param name="con"></param>
		/// <param name="robaID"></param>
		/// <returns></returns>
		public static List<Stavka> ListByRobaID(FbConnection con, int robaID)
		{
			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE ROBAID = @RID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@RID", robaID);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
				}
			}

			return list;
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu dokumenta
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> ListByDokument(int godina, int vrDok, int brDok)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				return ListByDokument(con, vrDok, brDok);
			}
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu dokumenta
		/// </summary>
		/// <returns></returns>
		public static Task<List<Stavka>> ListByDokumentAsync(int godina, int vrDok, int brDok)
		{
			return Task.Run(() =>
			{
				return ListByDokument(godina, vrDok, brDok);
			});
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu dokumenta
		/// </summary>
		/// <returns></returns>
		public static List<Stavka> ListByDokument(FbConnection con, int vrDok, int brDok)
		{
			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                    STAVKAID,
                                                    VRDOK,
                                                    BRDOK,
                                                    MAGACINID,
                                                    ROBAID,
                                                    VRSTA,
                                                    NAZIV,
                                                    NABCENSAPOR,
                                                    FAKTURNACENA,
                                                    NABCENABT,
                                                    TROSKOVI,
                                                    NABAVNACENA,
                                                    PRODCENABP,
                                                    KOREKCIJA,
                                                    PRODAJNACENA,
                                                    DEVIZNACENA,
                                                    DEVPRODCENA,
                                                    KOLICINA,
                                                    NIVKOL,
                                                    TARIFAID,
                                                    IMAPOREZ,
                                                    POREZ,
                                                    RABAT,
                                                    MARZA,
                                                    TAKSA,
                                                    AKCIZA,
                                                    PROSNAB,
                                                    PRECENA,
                                                    PRENAB,
                                                    PROSPROD,
                                                    MTID,
                                                    PT,
                                                    ZVEZDICA,
                                                    TREN_STANJE,
                                                    POREZ_ULAZ,
                                                    SDATUM,
                                                    DEVNABCENA,
                                                    POREZ_IZ,
                                                    X4,
                                                    Y4,
                                                    Z4,
                                                    CENAPOAJM,
                                                    KGID,
                                                    SAKCIZA
                                                    FROM STAVKA
                                                    WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				cmd.Parameters.AddWithValue("@BRDOK", brDok);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
			}

			return list;
		}

		/// <summary>
		/// Vraca listu stavki iz baze na osnovu dokumenta
		/// </summary>
		/// <returns></returns>
		public static Task<List<Stavka>> ListByDokumentAsync(FbConnection con, int vrDok, int brDok)
		{
			return Task.Run(() =>
			{
				return ListByDokument(con, vrDok, brDok);
			});
		}

		public static List<Stavka> ListByVrDok(int vrDok)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByVrDok(con, vrDok);
			}
		}

		public static List<Stavka> ListByVrDok(FbConnection con, int vrDok)
		{
			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE VRDOK = @VRDOK",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
				}
			}

			return list;
		}

		public static Task<List<Stavka>> ListByVrDokAsync(int vrDok)
		{
			return Task.Run(() =>
			{
				return ListByVrDok(vrDok);
			});
		}

		public static List<Stavka> ListByVrDok(params int[] vrDok)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByVrDok(con, vrDok);
			}
		}

		public static List<Stavka> ListByVrDok(FbConnection con, params int[] vrDok)
		{
			string whereQuery = "";
			foreach (int i in vrDok)
				whereQuery += " OR VRDOK = " + i;

			List<Stavka> list = new List<Stavka>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE 1 = 2" + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Stavka()
							{
								StavkaID = Convert.ToInt32(dr["STAVKAID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Vrsta =
									dr["VRSTA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRSTA"]),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								NabCenSaPor =
									dr["NABCENSAPOR"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
								FakturnaCena =
									dr["FAKTURNACENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
								NabCenaBT =
									dr["NABCENABT"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["NABCENABT"]),
								Troskovi =
									dr["TROSKOVI"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TROSKOVI"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
								Korekcija =
									dr["KOREKCIJA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["KOREKCIJA"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
								DevProdCena =
									dr["DEVPRODCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								NivKol = Convert.ToDouble(dr["NIVKOL"]),
								TarifaID =
									dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
								ImaPorez =
									dr["IMAPOREZ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["IMAPOREZ"]),
								Porez = Convert.ToDouble(dr["POREZ"]),
								Rabat = Convert.ToDouble(dr["RABAT"]),
								Marza = Convert.ToDouble(dr["MARZA"]),
								Taksa =
									dr["TAKSA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TAKSA"]),
								Akciza =
									dr["AKCIZA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["AKCIZA"]),
								ProsNab = Convert.ToDouble(dr["PROSNAB"]),
								PreCena = Convert.ToDouble(dr["PRECENA"]),
								PreNab = Convert.ToDouble(dr["PRENAB"]),
								ProsProd = Convert.ToDouble(dr["PROSPROD"]),
								MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
								PT = Convert.ToChar(dr["PT"]),
								Zvezdica =
									dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
								TrenStanje =
									dr["TREN_STANJE"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["TREN_STANJE"]),
								PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
								SDatum =
									dr["SDATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
								DevNabCena =
									dr["DEVNABCENA"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DEVNABCENA"]),
								PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
								X4 =
									dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
								Y4 =
									dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
								Z4 =
									dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
								CenaPoAJM =
									dr["CENAPOAJM"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["CENAPOAJM"]),
								KGID =
									dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
								SAkciza = Convert.ToDouble(dr["SAKCIZA"])
							}
						);
				}
			}

			return list;
		}

		public static Task<List<Stavka>> ListByVrDokAsync(params int[] vrDok)
		{
			return Task.Run(() =>
			{
				return ListByVrDok(vrDok);
			});
		}

		public static async Task<Termodom.Data.Entities.Komercijalno.StavkaDictionary> DictionaryAsync(
			int bazaID,
			int vrDok,
			int brDok,
			int? godina
		)
		{
			var response = await TDBrain_v3.GetAsync(
				$"/komercijalno/stavka/dictionary?bazaID={bazaID}&vrDok={vrDok}&brdok={brDok}&godina={godina ?? DateTime.Now.Year}"
			);

			if ((int)response.StatusCode == 200)
				return new Termodom.Data.Entities.Komercijalno.StavkaDictionary(
					JsonConvert.DeserializeObject<
						Dictionary<int, Termodom.Data.Entities.Komercijalno.Stavka>
					>(await response.Content.ReadAsStringAsync())
				);
			else if ((int)response.StatusCode == 500)
				throw new Termodom.Data.Exceptions.APIServerException();
			else
				throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
		}

		public static int Count()
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return Count(con);
			}
		}

		public static int Count(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand("SELECT COALESCE(COUNT(ROBAID), 0) FROM STAVKA", con)
			)
			using (FbDataReader dr = cmd.ExecuteReader())
				if (dr.Read())
					return Convert.ToInt32(dr[0]);

			return 0;
		}

		public static Task<int> CountAsync()
		{
			return Task.Run(() =>
			{
				return Count();
			});
		}

		public static Task<int> CountAsync(int godina)
		{
			return Task.Run(() =>
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
					)
				)
				{
					con.Open();
					return Count(con);
				}
			});
		}

		public static int MaxID()
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return MaxID(con);
			}
		}

		public static int MaxID(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand("SELECT COALESCE(MAX(STAVKAID), 0) FROM STAVKA", con)
			)
			using (FbDataReader dr = cmd.ExecuteReader())
				if (dr.Read())
					return Convert.ToInt32(dr[0]);

			return 0;
		}

		public static Task<int> MaxIDAsync()
		{
			return Task.Run(() =>
			{
				return MaxID();
			});
		}

		public static Task<int> MaxIDAsync(int godina)
		{
			return Task.Run(() =>
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
					)
				)
				{
					con.Open();
					return MaxID(con);
				}
			});
		}
	}
}
