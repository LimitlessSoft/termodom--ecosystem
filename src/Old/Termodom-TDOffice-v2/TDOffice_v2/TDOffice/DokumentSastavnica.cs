using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using TDOffice_v2.TDOffice.Enums;

namespace TDOffice_v2.TDOffice
{
	public class DokumentSastavnica
	{
		public int ID { get; set; }
		public int MagacinID { get; set; }
		public DokumentStatus Status { get; set; } = DokumentStatus.Otkljucan;
		public DateTime? Datum { get; set; }
		public int Korisnik { get; set; }
		public int? BrDokKom { get; set; }
		public DokumentSastavnicaTip Tip { get; set; } = DokumentSastavnicaTip.Sastavnica;
		public string Tag { get; set; }

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
					@"UPDATE DOKUMENT_SASTAVNICA SET DATUM = @D,
                BRDOK_KOM = @BRDOKKOM, TIP = @TIP, TAG = @TAG, STATUS =@ST
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@D", Datum);
				cmd.Parameters.AddWithValue("@TIP", Tip);
				cmd.Parameters.AddWithValue("@BRDOKKOM", BrDokKom);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@TAG", Tag);
				cmd.Parameters.AddWithValue("@ST", Status);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(int magacinID, int korisnik, DokumentSastavnicaTip tip, string tag)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, magacinID, korisnik, tip, tag);
			}
		}

		public static int Insert(
			FbConnection con,
			int magacinID,
			int korisnik,
			DokumentSastavnicaTip tip,
			string tag
		)
		{
			if (tip == DokumentSastavnicaTip.Pravilo && string.IsNullOrWhiteSpace(tag))
				throw new Exception("Pravilo mora imati dodeljen naziv (tag)");

			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_SASTAVNICA (ID, MAGACINID, DATUM, KORISNIK, TIP, TAG, STATUS)
                                            VALUES
                                            (((SELECT COALESCE(MAX(ID), 0) from DOKUMENT_SASTAVNICA) + 1), @MID, @D, @KOR, @T, @TAG, @STATUS)
                                            RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.Add("ID", FbDbType.Integer).Direction = System
					.Data
					.ParameterDirection
					.Output;
				cmd.Parameters.AddWithValue("@STATUS", (int)DokumentStatus.Otkljucan);
				cmd.Parameters.AddWithValue("@MID", magacinID);
				cmd.Parameters.AddWithValue("@D", DateTime.Now);
				cmd.Parameters.AddWithValue("@KOR", korisnik);
				cmd.Parameters.AddWithValue("@T", tip);
				cmd.Parameters.AddWithValue("@TAG", tag);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static DokumentSastavnica Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static DokumentSastavnica Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, KORISNIK, BRDOK_KOM, TIP, TAG, MAGACINID, STATUS FROM DOKUMENT_SASTAVNICA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new DokumentSastavnica()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum =
								dr["DATUM"] is DBNull
									? null
									: (DateTime?)Convert.ToDateTime(dr["DATUM"]),
							Korisnik = Convert.ToInt32(dr["KORISNIK"]),
							Tip = (DokumentSastavnicaTip)Convert.ToInt32(dr["TIP"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							BrDokKom =
								dr["BRDOK_KOM"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["BRDOK_KOM"]),
							Tag = dr["TAG"] is DBNull ? "" : dr["TAG"].ToString(),
							Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"])
						};
			}

			return null;
		}

		public static List<DokumentSastavnica> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<DokumentSastavnica> List(FbConnection con)
		{
			List<DokumentSastavnica> list = new List<DokumentSastavnica>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, KORISNIK, BRDOK_KOM, TIP, MAGACINID, STATUS, TAG FROM DOKUMENT_SASTAVNICA",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentSastavnica()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Datum =
									dr["DATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DATUM"]),
								Korisnik = Convert.ToInt32(dr["KORISNIK"]),
								Tag = dr["TAG"] is DBNull ? null : dr["TAG"].ToString(),
								Tip = (DokumentSastavnicaTip)Convert.ToInt32(dr["TIP"]),
								BrDokKom =
									dr["BRDOK_KOM"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["BRDOK_KOM"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"])
							}
						);
			}

			return list;
		}

		public static List<DokumentSastavnica> ListByMagacinID(int magacinID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		public static List<DokumentSastavnica> ListByMagacinID(FbConnection con, int magacinID)
		{
			List<DokumentSastavnica> list = new List<DokumentSastavnica>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, KORISNIK, BRDOK_KOM, TIP, MAGACINID, STATUS, TAG FROM DOKUMENT_SASTAVNICA WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentSastavnica()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Datum =
									dr["DATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DATUM"]),
								Korisnik = Convert.ToInt32(dr["KORISNIK"]),
								Tag = dr["TAG"] is DBNull ? null : dr["TAG"].ToString(),
								Tip = (DokumentSastavnicaTip)Convert.ToInt32(dr["TIP"]),
								BrDokKom =
									dr["BRDOK_KOM"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["BRDOK_KOM"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"])
							}
						);
			}

			return list;
		}

		public static List<DokumentSastavnica> ListByTip(DokumentSastavnicaTip tip)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByTip(con, tip);
			}
		}

		public static List<DokumentSastavnica> ListByTip(
			FbConnection con,
			DokumentSastavnicaTip tip
		)
		{
			List<DokumentSastavnica> list = new List<DokumentSastavnica>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, KORISNIK, BRDOK_KOM, TIP, MAGACINID, TAG, STATUS FROM DOKUMENT_SASTAVNICA WHERE TIP = @TIP",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@TIP", (int)tip);

				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
					{
						list.Add(
							new DokumentSastavnica()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Datum =
									dr["DATUM"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DATUM"]),
								Korisnik = Convert.ToInt32(dr["KORISNIK"]),
								Tag = dr["TAG"] is DBNull ? null : dr["TAG"].ToString(),
								Tip = (DokumentSastavnicaTip)Convert.ToInt32(dr["TIP"]),
								BrDokKom =
									dr["BRDOK_KOM"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["BRDOK_KOM"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["STATUS"])
							}
						);
					}
			}

			return list;
		}
	}
}
