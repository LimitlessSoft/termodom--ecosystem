using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Dokument0634
	{
		public int ID { get; set; }
		public DateTime Datum { get; set; }
		public int UserID { get; set; }
		public int MagacinID { get; set; }
		public int Status { get; set; } // 0 otkljucan / 1 zakljucan
		public int? KomercijalnoFaktura { get; set; }

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
					@"UPDATE DOKUMENT_0634 SET MAGACINID = @MID, STATUS = @STATUS, KOMERCIJALNO_FAKTURA = @KOMERCFAKTURA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", MagacinID);
				cmd.Parameters.AddWithValue("@STATUS", Status);
				cmd.Parameters.AddWithValue("@KOMERCFAKTURA", KomercijalnoFaktura);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static Dokument0634 Get(int brDok)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, brDok);
			}
		}

		public static Dokument0634 Get(FbConnection con, int brDok)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, USERID, MAGACINID, STATUS, KOMERCIJALNO_FAKTURA FROM DOKUMENT_0634 WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", brDok);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new Dokument0634()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							UserID = Convert.ToInt32(dr["USERID"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							Status = Convert.ToInt32(dr["STATUS"]),
							KomercijalnoFaktura =
								dr["KOMERCIJALNO_FAKTURA"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["KOMERCIJALNO_FAKTURA"])
						};
					}
				}
			}
			return null;
		}

		public static int Insert(int userID, int magacinID, int status, int? komercijalnoFaktura)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, userID, magacinID, status, komercijalnoFaktura);
			}
		}

		public static int Insert(
			FbConnection con,
			int userID,
			int magacinID,
			int status,
			int? komercijalnoFaktura
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO DOKUMENT_0634
                (ID, DATUM, USERID, MAGACINID, STATUS, KOMERCIJALNO_FAKTURA)
                VALUES
                (((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_0634) + 1), @DATUM, @USERID, @MAGACINID, @STATUS, @KF)
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

				cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
				cmd.Parameters.AddWithValue("@USERID", userID);
				cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
				cmd.Parameters.AddWithValue("@STATUS", status);
				cmd.Parameters.AddWithValue("@KF", komercijalnoFaktura);

				cmd.ExecuteNonQuery();

				if (cmd.Parameters["ID"].Value != null)
					return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
			throw new Exception("Error inserting new dokument!");
		}

		public static List<Dokument0634> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, null);
			}
		}

		public static List<Dokument0634> List(FbConnection con, string queryString)
		{
			if (string.IsNullOrWhiteSpace(queryString))
				queryString = "";

			queryString = " OR " + queryString;
			List<Dokument0634> list = new List<Dokument0634>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, DATUM, USERID, MAGACINID, STATUS, KOMERCIJALNO_FAKTURA FROM DOKUMENT_0634 WHERE 1 = 2"
						+ queryString,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						list.Add(
							new Dokument0634()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								Status = Convert.ToInt32(dr["STATUS"]),
								KomercijalnoFaktura =
									dr["KOMERCIJALNO_FAKTURA"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["KOMERCIJALNO_FAKTURA"])
							}
						);
					}
				}
			}
			return list;
		}

		public static List<Dokument0634> List(string queryString)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, queryString);
			}
		}
	}
}
