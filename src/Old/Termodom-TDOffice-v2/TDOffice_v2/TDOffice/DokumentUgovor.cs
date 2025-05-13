using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using TDOffice_v2.TDOffice.Enums;

namespace TDOffice_v2.TDOffice
{
	public class DokumentUgovor
	{
		public int ID { get; set; }
		public DokumentUgovorTip Tip { get; set; }
		public string Naziv { get; set; }
		public DateTime Datum { get; set; }
		public int KorisnikID { get; set; }
		public int MagacinID { get; set; } // Objekat
		public int Kupac { get; set; }
		public int Prodavac { get; set; }

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
					@"UPDATE DOKUMENT_UGOVOR
                SET
                TIP = @TIP,
                NAZIV = @NAZ,
                DATUM = @DATUM,
                KORISNIK_ID = @KID,
                MAGACIN_ID = @MID,
                KUPAC = @KUPAC,
                PRODAVAC = @PROD
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@NAZ", Naziv);
				cmd.Parameters.AddWithValue("@TIP", (int)Tip);
				cmd.Parameters.AddWithValue("@DATUM", Datum);
				cmd.Parameters.AddWithValue("@KID", Kupac);
				cmd.Parameters.AddWithValue("@MID", MagacinID);
				cmd.Parameters.AddWithValue("@KUPAC", Kupac);
				cmd.Parameters.AddWithValue("@PROD", Prodavac);

				cmd.ExecuteNonQuery();
			}
		}

		public static DokumentUgovor Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static DokumentUgovor Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, TIP, DATUM, KORISNIK_ID, MAGACIN_ID, KUPAC, NAZIV, PRODAVAC
FROM DOKUMENT_UGOVOR WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new DokumentUgovor()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Tip = (DokumentUgovorTip)Convert.ToInt32(dr["TIP"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							KorisnikID = Convert.ToInt32(dr["KORISNIK_ID"]),
							MagacinID = Convert.ToInt32(dr["MAGACIN_ID"]),
							Kupac = Convert.ToInt32(dr["KUPAC"]),
							Naziv = dr["NAZIV"].ToString(),
							Prodavac = Convert.ToInt32(dr["PRODAVAC"])
						};
			}

			return null;
		}

		public static List<DokumentUgovor> List(string whereQuery)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<DokumentUgovor> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			List<DokumentUgovor> list = new List<DokumentUgovor>();

			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, TIP, DATUM, KORISNIK_ID, MAGACIN_ID, KUPAC, NAZIV, PRODAVAC
FROM DOKUMENT_UGOVOR WHERE 1 = 1 " + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentUgovor()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Tip = (DokumentUgovorTip)Convert.ToInt32(dr["TIP"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIK_ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACIN_ID"]),
								Naziv = dr["NAZIV"].ToString(),
								Kupac = Convert.ToInt32(dr["KUPAC"]),
								Prodavac = Convert.ToInt32(dr["PRODAVAC"])
							}
						);
			}

			return list;
		}

		public static int Insert(
			DokumentUgovorTip tip,
			string naziv,
			int korisnikID,
			int magacinID,
			int kupac,
			int prodavac
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, tip, naziv, korisnikID, magacinID, kupac, prodavac);
			}
		}

		public static int Insert(
			FbConnection con,
			DokumentUgovorTip tip,
			string naziv,
			int korisnikID,
			int magacinID,
			int kupac,
			int prodavac
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_UGOVOR
(ID, TIP, DATUM, KORISNIK_ID, MAGACIN_ID, KUPAC, NAZIV, PRODAVAC)
VALUES
(((SELECT COALESCE(MAX(ID), 0)) + 1), @TIP, @DATUM, @KID, @MID, @KUP, @NAZ, @PROD)",
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

				cmd.Parameters.AddWithValue("@TIP", (int)tip);
				cmd.Parameters.AddWithValue("@KID", korisnikID);
				cmd.Parameters.AddWithValue("@MID", magacinID);
				cmd.Parameters.AddWithValue("@KUP", kupac);
				cmd.Parameters.AddWithValue("@NAZ", naziv);
				cmd.Parameters.AddWithValue("@PROD", prodavac);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}
	}
}
