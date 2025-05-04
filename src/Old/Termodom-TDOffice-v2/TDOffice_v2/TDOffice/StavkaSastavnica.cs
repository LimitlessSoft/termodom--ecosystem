using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class StavkaSastavnica
	{
		public int ID { get; set; }
		public int SastavnicaID { get; set; }
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
					"Update STAVKA_SASTAVNICA SET SASTAVNICA_ID = @SID, ROBAID = @RID, KOLICINA = @K WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@SID", SastavnicaID);
				cmd.Parameters.AddWithValue("@RID", RobaID);
				cmd.Parameters.AddWithValue("@K", Kolicina);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(int ID, int robaID, double kolicina)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, ID, robaID, kolicina);
			}
		}

		public static int Insert(FbConnection con, int sastavnicaID, int robaID, double kolicina)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO STAVKA_SASTAVNICA (ID, SASTAVNICA_ID, ROBAID, KOLICINA) 
                VALUES 
                 (((SELECT COALESCE(MAX(ID), 0) FROM STAVKA_SASTAVNICA) + 1), @SID, @RID, @K)
                 RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.Add("ID", FbDbType.Integer).Direction = System
					.Data
					.ParameterDirection
					.Output;
				cmd.Parameters.AddWithValue(@"SID", sastavnicaID);
				cmd.Parameters.AddWithValue(@"RID", robaID);
				cmd.Parameters.AddWithValue(@"K", kolicina);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static StavkaSastavnica Get(int sastavnicaid)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, sastavnicaid);
			}
		}

		public static StavkaSastavnica Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, SASTAVNICA_ID, ROBAID, KOLICINA FROM STAVKA_SASTAVNICA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new StavkaSastavnica()
						{
							ID = Convert.ToInt32(dr["ID"]),
							SastavnicaID = Convert.ToInt32(dr["SASTAVNICA_ID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							Kolicina = Convert.ToDouble(dr["KOLICINA"]),
						};
			}
			return null;
		}

		public static List<StavkaSastavnica> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return (List(con));
			}
		}

		public static List<StavkaSastavnica> List(FbConnection con)
		{
			List<StavkaSastavnica> list = new List<StavkaSastavnica>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, SASTAVNICA_ID, ROBAID, KOLICINA FROM STAVKA_SASTAVNICA",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new StavkaSastavnica()
							{
								ID = Convert.ToInt32(dr["ID"]),
								SastavnicaID = Convert.ToInt32(dr["SASTAVNICA_ID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"])
							}
						);
			}
			return list;
		}

		public static List<StavkaSastavnica> ListBySastavnicaID(int sastavnicaID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListBySastavnicaID(con, sastavnicaID);
			}
		}

		public static List<StavkaSastavnica> ListBySastavnicaID(FbConnection con, int sastavnicaID)
		{
			List<StavkaSastavnica> list = new List<StavkaSastavnica>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, SASTAVNICA_ID, ROBAID, KOLICINA FROM STAVKA_SASTAVNICA WHERE SASTAVNICA_ID = @SID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@SID", sastavnicaID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new StavkaSastavnica()
							{
								ID = Convert.ToInt32(dr["ID"]),
								SastavnicaID = Convert.ToInt32(dr["SASTAVNICA_ID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"])
							}
						);
			}
			return list;
		}

		public static void Delete(int sastavnicaid)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Delete(con, sastavnicaid);
			}
		}

		public static void Delete(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand("DELETE FROM STAVKA_SASTAVNICA WHERE ID = @ID", con)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
