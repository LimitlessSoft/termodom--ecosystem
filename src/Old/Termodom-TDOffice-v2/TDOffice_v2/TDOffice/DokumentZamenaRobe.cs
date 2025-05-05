using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;

namespace TDOffice_v2.TDOffice
{
	public class DokumentZamenaRobe
	{
		public class Info
		{
			public List<Tuple<int, double>> VracaSe { get; set; } = new List<Tuple<int, double>>();
			public List<Tuple<int, double>> UzimaSe { get; set; } = new List<Tuple<int, double>>();
			public List<Komercijalno.Stavka> StaroStanjeStavkiDokumenta { get; set; }
		}

		public int ID { get; set; }
		public DateTime Datum { get; set; }
		public int MagacinID { get; set; }
		public int UserID { get; set; }
		public DokumentStatus Status { get; set; }
		public string Komentar { get; set; }
		public string InterniKomentar { get; set; }
		public int? MPRacun { get; set; }
		public int? NovokreiraniMPRacun { get; set; } = null;
		public bool Realizovana { get; set; } = false;
		public double TrosakZamene { get; set; } = 8;
		public string FiskalniIsecak { get; set; }
		public string Log { get; set; }
		public Info Tag { get; set; } = null;

		public DokumentZamenaRobe() { }

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
					@"UPDATE DOKUMENT_ZAMENA_ROBE SET
                                                        USERID = @U,
                                                        STATUS = @S,
                                                        KOMENTAR = @K,
                                                        INTERNI_KOMENTAR = @IK,
                                                        MP_RACUN = @MP,
                                                        NOVI_MP_RACUN = @NMP,
                                                        REALIZOVANA = @REAL,
                                                        TROSAK_ZAMENE = @TZ,
                                                        FISKALNI_ISECAK = @FIS,
                                                        LOG = @LOG,
                                                        TAG = @TAG
                                                        WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@U", UserID);
				cmd.Parameters.AddWithValue("@S", Status);
				cmd.Parameters.AddWithValue("@K", Komentar);
				cmd.Parameters.AddWithValue("@IK", InterniKomentar);
				cmd.Parameters.AddWithValue("@MP", MPRacun);
				cmd.Parameters.AddWithValue("@NMP", NovokreiraniMPRacun);
				cmd.Parameters.AddWithValue("@REAL", Realizovana);
				cmd.Parameters.AddWithValue("@TZ", TrosakZamene);
				cmd.Parameters.AddWithValue("@FIS", FiskalniIsecak);
				cmd.Parameters.AddWithValue("@LOG", Log);
				cmd.Parameters.AddWithValue(
					"@TAG",
					System.Text.Encoding.UTF8.GetBytes(
						Newtonsoft.Json.JsonConvert.SerializeObject(Tag)
					)
				);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Vraca dokument zamene robe iz baze na osnovu id-a
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DokumentZamenaRobe Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		/// <summary>
		/// Vraca dokument zamene robe iz baze na osnovu id-a
		/// </summary>
		/// <param name="con"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DokumentZamenaRobe Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR,
                INTERNI_KOMENTAR, MP_RACUN, NOVI_MP_RACUN, REALIZOVANA, TROSAK_ZAMENE, FISKALNI_ISECAK,
                LOG, TAG FROM DOKUMENT_ZAMENA_ROBE WHERE ID = @id",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@id", id);
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new DokumentZamenaRobe()
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
							MPRacun =
								dr["MP_RACUN"] is DBNull
									? (int?)null
									: Convert.ToInt32(dr["MP_RACUN"]),
							NovokreiraniMPRacun =
								dr["NOVI_MP_RACUN"] is DBNull
									? (int?)null
									: Convert.ToInt32(dr["NOVI_MP_RACUN"]),
							Realizovana = Convert.ToInt32(dr["REALIZOVANA"]) != 0,
							TrosakZamene = Convert.ToDouble(dr["TROSAK_ZAMENE"]),
							FiskalniIsecak =
								dr["FISKALNI_ISECAK"] is DBNull
									? null
									: dr["FISKALNI_ISECAK"].ToString(),
							Log = dr["LOG"] is DBNull ? null : dr["LOG"].ToString(),
							Tag =
								dr["TAG"] is DBNull
									? new Info()
									: JsonConvert.DeserializeObject<Info>(
										Encoding.UTF8.GetString((byte[])dr["TAG"])
									)
						};
			}
			return null;
		}

		/// <summary>
		/// Vraca listu svih dokumenata zamene robe iz baze
		/// </summary>
		/// <returns></returns>
		public static List<DokumentZamenaRobe> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		/// <summary>
		/// Vraca listu svih dokumenata zamene robe iz baze
		/// </summary>
		/// <returns></returns>
		public static List<DokumentZamenaRobe> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			List<DokumentZamenaRobe> list = new List<DokumentZamenaRobe>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, MP_RACUN, NOVI_MP_RACUN, REALIZOVANA, TROSAK_ZAMENE, FISKALNI_ISECAK, LOG,
TAG FROM DOKUMENT_ZAMENA_ROBE WHERE 1 = 1 " + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentZamenaRobe()
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
								MPRacun =
									dr["MP_RACUN"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["MP_RACUN"]),
								NovokreiraniMPRacun =
									dr["NOVI_MP_RACUN"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["NOVI_MP_RACUN"]),
								Realizovana = Convert.ToInt32(dr["REALIZOVANA"]) != 0,
								TrosakZamene = Convert.ToDouble(dr["TROSAK_ZAMENE"]),
								FiskalniIsecak =
									dr["FISKALNI_ISECAK"] is DBNull
										? null
										: dr["FISKALNI_ISECAK"].ToString(),
								Log = dr["LOG"] is DBNull ? null : dr["LOG"].ToString(),
								Tag =
									dr["TAG"] is DBNull
										? new Info()
										: JsonConvert.DeserializeObject<Info>(
											Encoding.UTF8.GetString((byte[])dr["TAG"])
										)
							}
						);
			}
			return list;
		}

		/// <summary>
		/// Dodaje novi dokument u bazu i vraca id novokreiranog dokumenta
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="magacinID"></param>
		/// <param name="status"></param>
		/// <param name="komentar"></param>
		/// <param name="interniKomentar"></param>
		/// <returns></returns>
		public static int Insert(
			int userID,
			int magacinID,
			int status,
			string komentar,
			string interniKomentar
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, userID, magacinID, status, komentar, interniKomentar);
			}
		}

		/// <summary>
		/// Dodaje novi dokument u bazu i vraca id novokreiranog dokumenta
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="magacinID"></param>
		/// <param name="status"></param>
		/// <param name="komentar"></param>
		/// <param name="interniKomentar"></param>
		public static int Insert(
			FbConnection con,
			int userID,
			int magacinID,
			int status,
			string komentar,
			string interniKomentar
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_ZAMENA_ROBE
                        (ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, REALIZOVANA, TROSAK_ZAMENE)
                        VALUES (((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_ZAMENA_ROBE) + 1), @D, @U, @M, @S, @K, @IK, 0, 10)
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
				cmd.Parameters.Add(
					new FbParameter
					{
						ParameterName = "ID",
						FbDbType = FbDbType.Integer,
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}
	}
}
