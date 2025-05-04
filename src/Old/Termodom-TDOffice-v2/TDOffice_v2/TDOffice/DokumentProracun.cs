using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class DokumentProracun
	{
		public int ID { get; set; }
		public DateTime Datum { get; set; }
		public int? PPID { get; set; }
		public int MagacinID { get; set; }
		public int UserID { get; set; }
		public DokumentStatus Status { get; set; }
		public string Komentar { get; set; }
		public string InterniKomentar { get; set; }
		public int? KomercijalnoProracunBroj { get; set; }
		public Komercijalno.NacinUplate NUID { get; set; }

		private static ManualResetEventSlim _updateMRE = new ManualResetEventSlim(true);

		public void Update()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE DOKUMENT_PRORACUN SET
                USERID = @U,
                PPID = @PPID,
                MAGACINID = @M,
                STATUS = @S,
                KOMENTAR = @K,
                INTERNI_KOMENTAR = @IK,
                KOMERCIJALNO_PRORACUN_BROJ = @KBR,
                NUID = @NUID
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@U", UserID);
				cmd.Parameters.AddWithValue("@PPID", PPID);
				cmd.Parameters.AddWithValue("@M", MagacinID);
				cmd.Parameters.AddWithValue("@S", (int)Status);
				cmd.Parameters.AddWithValue("@K", Komentar);
				cmd.Parameters.AddWithValue("@IK", InterniKomentar);
				cmd.Parameters.AddWithValue("@KBR", KomercijalnoProracunBroj);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@NUID", NUID);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(
			DateTime datum,
			int? ppid,
			int magacinID,
			int userID,
			string komentar,
			string interniKomentar,
			int nuid
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, datum, ppid, magacinID, userID, komentar, interniKomentar, nuid);
			}
		}

		public static int Insert(
			FbConnection con,
			DateTime datum,
			int? ppid,
			int magacinID,
			int userID,
			string komentar,
			string interniKomentar,
			int nuid
		)
		{
			_updateMRE.Wait();
			_updateMRE.Reset();

			int nextID = MaxID(con) + 1;

			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_PRORACUN (ID, DATUM, PPID, MAGACINID, USERID, STATUS, KOMENTAR, INTERNI_KOMENTAR, NUID) VALUES
                (@ID, @DATUM, @PPID, @MAGACINID, @USERID, @STATUS, @KOMETNAR, @IK, @NUID)",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", nextID);
				cmd.Parameters.AddWithValue("@DATUM", datum);
				cmd.Parameters.AddWithValue("@PPID", ppid);
				cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
				cmd.Parameters.AddWithValue("@USERID", userID);
				cmd.Parameters.AddWithValue("@STATUS", (int)DokumentStatus.Otkljucan);
				cmd.Parameters.AddWithValue("@KOMETNAR", komentar);
				cmd.Parameters.AddWithValue("@IK", interniKomentar);
				cmd.Parameters.AddWithValue("@NUID", nuid);

				cmd.ExecuteNonQuery();
			}
			_updateMRE.Set();

			return nextID;
		}

		public static DokumentProracun Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static DokumentProracun Get(FbConnection con, int id)
		{
			_updateMRE.Wait();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, DATUM, PPID, MAGACINID,
                USERID, STATUS, KOMENTAR, INTERNI_KOMENTAR, KOMERCIJALNO_PRORACUN_BROJ, NUID FROM DOKUMENT_PRORACUN WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new DokumentProracun()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							UserID = Convert.ToInt32(dr["USERID"]),
							Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"]),
							InterniKomentar = dr["INTERNI_KOMENTAR"].ToStringOrDefault(),
							Komentar = dr["KOMENTAR"].ToStringOrDefault(),
							KomercijalnoProracunBroj =
								dr["KOMERCIJALNO_PRORACUN_BROJ"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["KOMERCIJALNO_PRORACUN_BROJ"]),
							NUID = (Komercijalno.NacinUplate)Convert.ToInt32(dr["NUID"])
						};
				}
			}

			return null;
		}

		public static List<DokumentProracun> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<DokumentProracun> List(FbConnection con)
		{
			_updateMRE.Wait();
			List<DokumentProracun> list = new List<DokumentProracun>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, DATUM, PPID, MAGACINID,
                USERID, STATUS, KOMENTAR, INTERNI_KOMENTAR, KOMERCIJALNO_PRORACUN_BROJ, NUID FROM DOKUMENT_PRORACUN",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new DokumentProracun()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								PPID =
									dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"]),
								InterniKomentar = dr["INTERNI_KOMENTAR"].ToStringOrDefault(),
								Komentar = dr["KOMENTAR"].ToStringOrDefault(),
								KomercijalnoProracunBroj =
									dr["KOMERCIJALNO_PRORACUN_BROJ"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["KOMERCIJALNO_PRORACUN_BROJ"]),
								NUID = (Komercijalno.NacinUplate)Convert.ToInt32(dr["NUID"])
							}
						);
				}
			}

			return list;
		}

		public static int MaxID(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_PRORACUN",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return Convert.ToInt32(dr[0]);
				}
			}

			return 0;
		}
	}
}
