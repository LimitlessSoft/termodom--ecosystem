using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class StavkaRazduzenjaMagacina
	{
		public int ID { get; set; }
		public int RobaID { get; set; }
		public int DokumentID { get; set; }
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
					@"UPDATE STAVKA_RAZDUZENJE_MAGACINA SET
                ROBAID = @rid,
                KOLICINA = @K,
                DOKUMENT_ID = @did
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@rid", RobaID);
				cmd.Parameters.AddWithValue("@K", Kolicina);
				cmd.Parameters.AddWithValue("@did", DokumentID);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(int robaID, double kolicina, int dokumentID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, robaID, kolicina, dokumentID);
			}
		}

		public static int Insert(FbConnection con, int robaID, double kolicina, int dokumentID)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO STAVKA_RAZDUZENJE_MAGACINA
                (ID, ROBAID, KOLICINA, DOKUMENT_ID)
                VALUES
                (((SELECT COALESCE(MAX(ID), 0) FROM STAVKA_RAZDUZENJE_MAGACINA) + 1), @RID, @Kol, @DID)",
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

				cmd.Parameters.AddWithValue("@RID", robaID);
				cmd.Parameters.AddWithValue("@Kol", kolicina);
				cmd.Parameters.AddWithValue("@DID", dokumentID);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static StavkaRazduzenjaMagacina Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static StavkaRazduzenjaMagacina Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, ROBAID, KOLICINA, DOKUMENT_ID FROM STAVKA_RAZDUZENJE_MAGACINA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new StavkaRazduzenjaMagacina()
						{
							ID = Convert.ToInt32(dr["ID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							Kolicina = Convert.ToDouble(dr["KOLICINA"]),
							DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"])
						};
				}
			}

			return null;
		}

		public static List<StavkaRazduzenjaMagacina> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<StavkaRazduzenjaMagacina> List(FbConnection con)
		{
			List<StavkaRazduzenjaMagacina> list = new List<StavkaRazduzenjaMagacina>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, ROBAID, KOLICINA, DOKUMENT_ID FROM STAVKA_RAZDUZENJE_MAGACINA",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new StavkaRazduzenjaMagacina()
							{
								ID = Convert.ToInt32(dr["ID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"])
							}
						);
				}
			}

			return list;
		}

		public static List<StavkaRazduzenjaMagacina> ListByDokumentID(int DokumentID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByDokumentID(con, DokumentID);
			}
		}

		public static List<StavkaRazduzenjaMagacina> ListByDokumentID(
			FbConnection con,
			int DokumentID
		)
		{
			List<StavkaRazduzenjaMagacina> list = new List<StavkaRazduzenjaMagacina>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, ROBAID, KOLICINA, DOKUMENT_ID FROM STAVKA_RAZDUZENJE_MAGACINA WHERE DOKUMENT_ID = @DID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@DID", DokumentID);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new StavkaRazduzenjaMagacina()
							{
								ID = Convert.ToInt32(dr["ID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"])
							}
						);
				}
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
				FbCommand cmd = new FbCommand(
					"DELETE FROM STAVKA_RAZDUZENJE_MAGACINA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
