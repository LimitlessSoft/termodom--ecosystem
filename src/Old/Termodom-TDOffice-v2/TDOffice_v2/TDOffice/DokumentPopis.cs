using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;

namespace TDOffice_v2.TDOffice
{
	public class DokumentPopis
	{
		private static readonly Buffer<List<DokumentPopis>> _buffer = new Buffer<
			List<DokumentPopis>
		>(List);
		public int ID { get; set; }
		public DateTime Datum { get; set; }
		public int MagacinID { get; set; }
		public int UserID { get; set; }
		public DokumentStatus Status { get; set; }
		public string Komentar { get; set; }
		public string InterniKomentar { get; set; }
		public PopisType Tip { get; set; }
		public int? KomercijalnoPopisBrDok { get; set; }
		public int? KomercijalnoNarudzbenicaBrDok { get; set; }
		public int SpecijalStampa { get; set; }
		public int? ZaduzenjeBrDokKomercijalno { get; set; }
		public string Napomena { get; set; }

		private string _zabelezenoStanjePopisaRaw { get; set; }
		private DataTable _zabelezenoStanjePopisaDT { get; set; } = null;
		public DataTable ZabelezenoStanjePopisa
		{
			get
			{
				if (_zabelezenoStanjePopisaDT == null)
					_zabelezenoStanjePopisaDT = string.IsNullOrWhiteSpace(
						_zabelezenoStanjePopisaRaw
					)
						? new DataTable()
						: JsonConvert.DeserializeObject<DataTable>(_zabelezenoStanjePopisaRaw);

				return _zabelezenoStanjePopisaDT;
			}
		}

		public DokumentPopis() { }

		/// <summary>
		/// Azurira podatke u bazi. Osnov: ID
		/// </summary>
		public void Update()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		/// <summary>
		/// Azurira podatke u bazi. Osnov: ID
		/// </summary>
		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE DOKUMENT_POPIS SET
                                                        USERID = @U,
                                                        MAGACINID = @M,
                                                        STATUS = @S,
                                                        KOMENTAR = @K,
                                                        INTERNI_KOMENTAR = @IK,
                                                        TIP = @T,
                                                        KOMERCIJALNO_POPIS_BRDOK = @KPB,
                                                        KOMERCIJALNO_NARUDZBENICA_BRDOK = @KNB,
                                                        SPECIJAL_STAMPA = @SS,
                                                        ZADUZENJE_BRDOK_KOMERCIJALNO = @ZDKK,
                                                        ZABELEZENO_STANJE_POPISA = @ZSP,
                                                        NAPOMENA = @NAPOM
                                                        WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@U", UserID);
				cmd.Parameters.AddWithValue("@M", MagacinID);
				cmd.Parameters.AddWithValue("@S", Status);
				cmd.Parameters.AddWithValue("@K", Komentar);
				cmd.Parameters.AddWithValue("@IK", InterniKomentar);
				cmd.Parameters.AddWithValue("@T", (int)Tip);
				cmd.Parameters.AddWithValue("@KPB", KomercijalnoPopisBrDok);
				cmd.Parameters.AddWithValue("@KNB", KomercijalnoNarudzbenicaBrDok);
				cmd.Parameters.AddWithValue("@SS", SpecijalStampa);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@ZDKK", ZaduzenjeBrDokKomercijalno);
				cmd.Parameters.AddWithValue("@ZSP", _zabelezenoStanjePopisaRaw);
				cmd.Parameters.AddWithValue("@NAPOM", Napomena);
				cmd.ExecuteNonQuery();

				AzurirajStanjeMagacinaUPopisuAsync(MagacinID);
			}
		}

		/// <summary>
		/// Setuje zabelezeno stanje popisa u objektu, jos uvek ne u bazi!
		/// </summary>
		public void SetZabelezenoStanjePopisa(DataTable dataTable)
		{
			_zabelezenoStanjePopisaDT = dataTable;
			_zabelezenoStanjePopisaRaw = JsonConvert.SerializeObject(dataTable);
		}

		/// <summary>
		/// Vraca dokument iz baze
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DokumentPopis Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		/// <summary>
		/// Vraca dokument iz baze
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DokumentPopis Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, DATUM, USERID, MAGACINID, STATUS,
                    KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK, KOMERCIJALNO_NARUDZBENICA_BRDOK,
                    SPECIJAL_STAMPA, ZADUZENJE_BRDOK_KOMERCIJALNO, ZABELEZENO_STANJE_POPISA, NAPOMENA FROM DOKUMENT_POPIS WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new DokumentPopis()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							UserID = Convert.ToInt32(dr["USERID"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"]),
							Komentar = dr["KOMENTAR"] is DBNull ? "" : dr["KOMENTAR"].ToString(),
							InterniKomentar =
								dr["INTERNI_KOMENTAR"] is DBNull
									? ""
									: dr["INTERNI_KOMENTAR"].ToString(),
							Tip = (PopisType)Convert.ToInt32(dr["TIP"]),
							KomercijalnoPopisBrDok =
								dr["KOMERCIJALNO_POPIS_BRDOK"] is DBNull
									? (int?)null
									: Convert.ToInt32(dr["KOMERCIJALNO_POPIS_BRDOK"]),
							KomercijalnoNarudzbenicaBrDok =
								dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"] is DBNull
									? (int?)null
									: Convert.ToInt32(dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"]),
							SpecijalStampa = Convert.ToInt32(dr["SPECIJAL_STAMPA"]),
							ZaduzenjeBrDokKomercijalno =
								dr["ZADUZENJE_BRDOK_KOMERCIJALNO"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["ZADUZENJE_BRDOK_KOMERCIJALNO"]),
							_zabelezenoStanjePopisaRaw = dr["ZABELEZENO_STANJE_POPISA"]
								.ToStringOrDefault(),
							Napomena = dr["NAPOMENA"].ToStringOrDefault()
						};
			}

			return null;
		}

		/// <summary>
		/// Vraca listu svih dokumenata iz baze
		/// </summary>
		/// <returns></returns>
		public static List<DokumentPopis> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		/// <summary>
		/// Vraca listu svih dokumenata iz baze
		/// </summary>
		/// <returns></returns>
		public static List<DokumentPopis> List(string whereQuery)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		/// <summary>
		/// Vraca listu svih dokumenata iz baze
		/// </summary>
		/// <returns></returns>
		public static List<DokumentPopis> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = "AND " + whereQuery;

			List<DokumentPopis> list = new List<DokumentPopis>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK,
KOMERCIJALNO_NARUDZBENICA_BRDOK, SPECIJAL_STAMPA, ZADUZENJE_BRDOK_KOMERCIJALNO, ZABELEZENO_STANJE_POPISA,
NAPOMENA
FROM DOKUMENT_POPIS
WHERE 1 = 1 " + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentPopis()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"]),
								Komentar =
									dr["KOMENTAR"] is DBNull ? "" : dr["KOMENTAR"].ToString(),
								InterniKomentar =
									dr["INTERNI_KOMENTAR"] is DBNull
										? ""
										: dr["INTERNI_KOMENTAR"].ToString(),
								Tip = (PopisType)Convert.ToInt32(dr["TIP"]),
								KomercijalnoPopisBrDok =
									dr["KOMERCIJALNO_POPIS_BRDOK"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["KOMERCIJALNO_POPIS_BRDOK"]),
								KomercijalnoNarudzbenicaBrDok =
									dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"]),
								SpecijalStampa = Convert.ToInt32(dr["SPECIJAL_STAMPA"]),
								ZaduzenjeBrDokKomercijalno =
									dr["ZADUZENJE_BRDOK_KOMERCIJALNO"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["ZADUZENJE_BRDOK_KOMERCIJALNO"]),
								_zabelezenoStanjePopisaRaw = dr["ZABELEZENO_STANJE_POPISA"]
									.ToStringOrDefault(),
								Napomena = dr["NAPOMENA"].ToStringOrDefault()
							}
						);
			}

			_buffer.Set(list);
			return list;
		}

		/// <summary>
		/// Vraca listu svih dokumenata iz buffera sa ogranicenom zastareloscu.
		/// Ukoliko je podatak zastareo uzece najnoviji i osvezice buffer.
		/// </summary>
		/// <param name="outdateTimeout"></param>
		/// <returns></returns>
		public static List<DokumentPopis> BufferedList(TimeSpan outdateTimeout)
		{
			return _buffer.Get(outdateTimeout);
		}

		/// <summary>
		/// Vraca listu dokumenata iz baze zavisno od magacina
		/// </summary>
		/// <returns></returns>
		public static List<DokumentPopis> ListByMagacinID(int magacinID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		/// <summary>
		/// Vraca listu dokumenata iz baze zavisno od magacina
		/// </summary>
		/// <returns></returns>
		public static List<DokumentPopis> ListByMagacinID(FbConnection con, int magacinID)
		{
			List<DokumentPopis> list = new List<DokumentPopis>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR,
                    INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK, KOMERCIJALNO_NARUDZBENICA_BRDOK,
                    SPECIJAL_STAMPA, ZADUZENJE_BRDOK_KOMERCIJALNO, ZABELEZENO_STANJE_POPISA, NAPOMENA FROM DOKUMENT_POPIS WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);

				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentPopis()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"]),
								Komentar =
									dr["KOMENTAR"] is DBNull ? "" : dr["KOMENTAR"].ToString(),
								InterniKomentar =
									dr["INTERNI_KOMENTAR"] is DBNull
										? ""
										: dr["INTERNI_KOMENTAR"].ToString(),
								Tip = (PopisType)Convert.ToInt32(dr["TIP"]),
								KomercijalnoPopisBrDok =
									dr["KOMERCIJALNO_POPIS_BRDOK"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["KOMERCIJALNO_POPIS_BRDOK"]),
								KomercijalnoNarudzbenicaBrDok =
									dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"]),
								SpecijalStampa = Convert.ToInt32(dr["SPECIJAL_STAMPA"]),
								ZaduzenjeBrDokKomercijalno =
									dr["ZADUZENJE_BRDOK_KOMERCIJALNO"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["ZADUZENJE_BRDOK_KOMERCIJALNO"]),
								_zabelezenoStanjePopisaRaw = dr["ZABELEZENO_STANJE_POPISA"]
									.ToStringOrDefault(),
								Napomena = dr["NAPOMENA"].ToStringOrDefault()
							}
						);
			}

			return list;
		}

		/// <summary>
		/// Dodaje novi dokument popisa u bazu i vraca id novokreiranog dokumenta
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="magacinID"></param>
		/// <param name="status"></param>
		/// <param name="komentar"></param>
		/// <param name="interniKomentar"></param>
		/// <param name="tip"></param>
		/// <param name="komercijalnoPopisBrDok"></param>
		/// <param name="komercijalnoNarudzbenicaBrDok"></param>
		/// <returns></returns>
		public static int Insert(
			int userID,
			int magacinID,
			int status,
			string komentar,
			string interniKomentar,
			PopisType tip,
			int? komercijalnoPopisBrDok,
			int? komercijalnoNarudzbenicaBrDok
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(
					con,
					userID,
					magacinID,
					status,
					komentar,
					interniKomentar,
					tip,
					komercijalnoPopisBrDok,
					komercijalnoNarudzbenicaBrDok
				);
			}
		}

		/// <summary>
		/// Dodaje novi dokument popisa u bazu i vraca id novokreiranog dokumenta
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="magacinID"></param>
		/// <param name="status"></param>
		/// <param name="komentar"></param>
		/// <param name="interniKomentar"></param>
		/// <param name="tip"></param>
		/// <param name="komercijalnoPopisBrDok"></param>
		/// <param name="komercijalnoNarudzbenicaBrDok"></param>
		/// <returns></returns>
		public static int Insert(
			FbConnection con,
			int userID,
			int magacinID,
			int status,
			string komentar,
			string interniKomentar,
			PopisType tip,
			int? komercijalnoPopisBrDok,
			int? komercijalnoNarudzbenicaBrDok
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_POPIS
                        (ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK, KOMERCIJALNO_NARUDZBENICA_BRDOK)
                        VALUES (((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_POPIS) + 1), @D, @U, @M, @S, @K, @IK, @T, @KPB, @KNB)
                        RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@D", DateTime.Now);
				cmd.Parameters.AddWithValue("@U", userID);
				cmd.Parameters.AddWithValue("@M", magacinID);
				cmd.Parameters.AddWithValue("@S", status);
				cmd.Parameters.AddWithValue("@K", komentar);
				cmd.Parameters.AddWithValue("@IK", interniKomentar);
				cmd.Parameters.AddWithValue("@T", (int)tip);
				cmd.Parameters.AddWithValue("@KPB", komercijalnoPopisBrDok);
				cmd.Parameters.AddWithValue("@KNB", komercijalnoNarudzbenicaBrDok);
				cmd.Parameters.Add(
					new FbParameter
					{
						ParameterName = "ID",
						FbDbType = FbDbType.Integer,
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.ExecuteNonQuery();

				AzurirajStanjeMagacinaUPopisuAsync(magacinID);
				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		private static Task AzurirajStanjeMagacinaUPopisuAsync(int magacinID)
		{
			return Task.Run(() =>
			{
				bool imaOtvorenihPopisa = ListByMagacinID(magacinID)
					.Any(x => x.Status == DokumentStatus.Otkljucan);

				Config<List<int>> config = Config<List<int>>.Get(ConfigParameter.MagacinUPopisu);

				if (!config.Tag.Contains(magacinID) && imaOtvorenihPopisa)
					config.Tag.Add(magacinID);
				else if (config.Tag.Contains(magacinID) && !imaOtvorenihPopisa)
					config.Tag.Remove(magacinID);

				config.UpdateOrInsert();

				MagacinClan magacinVlasnik = magacinVlasnik = MagacinClan
					.ListByMagacinID(magacinID)
					.Where(x => x.Tip == Enums.MagacinClanTip.Vlasnik)
					.FirstOrDefault();

				if (magacinVlasnik == null)
					return;

				//Komercijalno.PravaZap pzap = Komercijalno.PravaZap.Get(magacinVlasnik.KorisnikID, 1, 10017);
				//pzap.V = imaOtvorenihPopisa ? (Int16)0 : (Int16)1;
				//pzap.Update();
			});
		}
	}
}
