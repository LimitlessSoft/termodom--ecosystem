using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class DokumentRazduzenjaMagacina
	{
		public int ID { get; set; }
		public int MagacinID { get; set; }
		public DateTime Datum { get; set; }
		public int KorisnikID { get; set; }
		public DokumentRazduzenjaMagacinaStatus Status { get; set; }
		public string Komentar { get; set; }
		public string InterniKomentar { get; set; }
		public int KomercijalnoBrDokOut { get; set; }

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
					@"UPDATE DOKUMENT_RAZDUZENJE_MAGACINA SET
                MAGACINID = @M,
                DATUM = @D,
                KORISNIKID = @K,
                STATUS = @S,
                KOMENTAR = @KOM,
                INTERNI_KOMENTAR = @IK,
                KOMERCIJALNO_BRDOKOUT = @KBO
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@M", MagacinID);
				cmd.Parameters.AddWithValue("@D", Datum);
				cmd.Parameters.AddWithValue("@S", Status);
				cmd.Parameters.AddWithValue("@K", KorisnikID);
				cmd.Parameters.AddWithValue("@KOM", Komentar);
				cmd.Parameters.AddWithValue("@IK", InterniKomentar);
				cmd.Parameters.AddWithValue("@KBO", KomercijalnoBrDokOut);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(
			int magacinID,
			DateTime datum,
			int korisnikID,
			DokumentRazduzenjaMagacinaStatus status
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, magacinID, datum, korisnikID, status, null, null);
			}
		}

		public static int Insert(
			int magacinID,
			DateTime datum,
			int korisnikID,
			DokumentRazduzenjaMagacinaStatus status,
			string interniKomentar,
			string komentar
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, magacinID, datum, korisnikID, status, komentar, interniKomentar);
			}
		}

		public static int Insert(
			FbConnection con,
			int magacinID,
			DateTime datum,
			int kID,
			DokumentRazduzenjaMagacinaStatus status,
			string komentar,
			string interniKomentar
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_RAZDUZENJE_MAGACINA
                (ID, DATUM, KORISNIKID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR)
                VALUES
                (((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_RAZDUZENJE_MAGACINA) + 1), @DATUM, @KOID, @MAGACINID, @STATUS, @KOMENTAR, @IK)
                RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.Add(
					new FbParameter("ID", FbDbType.Integer)
					{
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.Parameters.AddWithValue("@DATUM", datum);
				cmd.Parameters.AddWithValue("@KOID", kID);
				cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
				cmd.Parameters.AddWithValue("@STATUS", (int)status);
				cmd.Parameters.AddWithValue("@KOMENTAR", komentar);
				cmd.Parameters.AddWithValue("@IK", interniKomentar);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static DokumentRazduzenjaMagacina Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static DokumentRazduzenjaMagacina Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, MAGACINID, DATUM, KORISNIKID, STATUS, KOMENTAR, INTERNI_KOMENTAR, KOMERCIJALNO_BRDOKOUT FROM DOKUMENT_RAZDUZENJE_MAGACINA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new DokumentRazduzenjaMagacina()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
							Status = (DokumentRazduzenjaMagacinaStatus)
								Convert.ToInt32(dr["STATUS"]),
							InterniKomentar = dr["INTERNI_KOMENTAR"].ToStringOrDefault(),
							Komentar = dr["KOMENTAR"].ToStringOrDefault(),
							KomercijalnoBrDokOut =
								dr["KOMERCIJALNO_BRDOKOUT"] is DBNull
									? 0
									: Convert.ToInt32(dr["KOMERCIJALNO_BRDOKOUT"])
						};
				}
			}

			return null;
		}

		public static List<DokumentRazduzenjaMagacina> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<DokumentRazduzenjaMagacina> List(
			FbConnection con,
			string whereQuery = null
		)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			List<DokumentRazduzenjaMagacina> list = new List<DokumentRazduzenjaMagacina>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, MAGACINID, DATUM, KORISNIKID, STATUS, KOMENTAR, INTERNI_KOMENTAR, KOMERCIJALNO_BRDOKOUT
                FROM DOKUMENT_RAZDUZENJE_MAGACINA
                WHERE 1 = 1",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new DokumentRazduzenjaMagacina()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
								Status = (DokumentRazduzenjaMagacinaStatus)
									Convert.ToInt32(dr["STATUS"]),
								InterniKomentar = dr["INTERNI_KOMENTAR"].ToStringOrDefault(),
								Komentar = dr["KOMENTAR"].ToStringOrDefault(),
								KomercijalnoBrDokOut =
									dr["KOMERCIJALNO_BRDOKOUT"] is DBNull
										? 0
										: Convert.ToInt32(dr["KOMERCIJALNO_BRDOKOUT"])
							}
						);
				}
			}

			return list;
		}

		public static List<DokumentRazduzenjaMagacina> ListByMagacinID(int magacinID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		public static List<DokumentRazduzenjaMagacina> ListByMagacinID(
			FbConnection con,
			int magacinID
		)
		{
			List<DokumentRazduzenjaMagacina> list = new List<DokumentRazduzenjaMagacina>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, MAGACINID, DATUM, KORISNIKID, STATUS, KOMENTAR, INTERNI_KOMENTAR, KOMERCIJALNO_BRDOKOUT FROM DOKUMENT_RAZDUZENJE_MAGACINA WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new DokumentRazduzenjaMagacina()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
								Status = (DokumentRazduzenjaMagacinaStatus)
									Convert.ToInt32(dr["STATUS"]),
								InterniKomentar = dr["INTERNI_KOMENTAR"].ToStringOrDefault(),
								Komentar = dr["KOMENTAR"].ToStringOrDefault(),
								KomercijalnoBrDokOut =
									dr["KOMERCIJALNO_BRDOKOUT"] is DBNull
										? 0
										: Convert.ToInt32(dr["KOMERCIJALNO_BRDOKOUT"])
							}
						);
				}
			}

			return list;
		}
	}
}
