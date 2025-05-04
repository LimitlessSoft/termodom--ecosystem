using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Stavka0634
	{
		public int ID { get; set; }
		public int BrDok { get; set; }
		public int RobaID { get; set; }
		public double Kolicina { get; set; }

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
					"UPDATE STAVKA_0634 SET KOLICINA = @KOL WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@KOL", Kolicina);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static Stavka0634 Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Stavka0634 Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, ROBAID, BRDOK, KOLICINA FROM STAVKA0634 WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Stavka0634()
						{
							ID = Convert.ToInt32(dr["ID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							Kolicina = Convert.ToDouble(dr["KOLICINA"]),
							BrDok = Convert.ToInt32(dr["BRDOK"])
						};
			}
			return null;
		}

		public static List<Stavka0634> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, null);
			}
		}

		public static List<Stavka0634> List(string queryString)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, queryString);
			}
		}

		public static List<Stavka0634> List(FbConnection con, string queryString)
		{
			if (string.IsNullOrWhiteSpace(queryString))
				queryString = "";

			queryString = " OR " + queryString;
			List<Stavka0634> list = new List<Stavka0634>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, ROBAID, BRDOK, KOLICINA FROM STAVKA_0634 WHERE 1 = 2 "
						+ queryString,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Stavka0634()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								BrDok = Convert.ToInt32(dr["BRDOK"])
							}
						);
			}
			return list;
		}

		public static int Insert(int brDok, int robaID, double kolicina)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, brDok, robaID, kolicina);
			}
		}

		public static int Insert(FbConnection con, int brDok, int robaID, double kolicina)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO STAVKA_0634 (ID, ROBAID, BRDOK, KOLICINA) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM STAVKA_0634) + 1), @ROBAID, @BRDOK, @KOLICINA) RETURNING ID",
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
				cmd.Parameters.AddWithValue("@BRDOK", brDok);
				cmd.Parameters.AddWithValue("@KOLICINA", kolicina);
				cmd.Parameters.AddWithValue("@ROBAID", robaID);

				cmd.ExecuteNonQuery();

				if (cmd.Parameters["ID"].Value != null)
					return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}

			throw new Exception("Error inserting stavka!");
		}

		public static void Delete(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Delete(con, id);
			}
		}

		public static void Delete(FbConnection con, int id)
		{
			using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA_0634 WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
