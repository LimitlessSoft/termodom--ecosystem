using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Magacin
	{
		public int ID { get; set; }
		public int NadredjeniMagacin { get; set; }
		public int MagacinRazduzenja { get; set; }
		public int FirmaID { get; set; }
		public string Adresa { get; set; }
		public string Opstina { get; set; }

		public Magacin() { }

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
					"UPDATE MAGACIN SET NADREDJENI_MAGACIN = @NM, MAGACIN_RAZDUZENJA = @MR, FIRMAID = @FID, ADRESA = @ADRESA, OPSTINA = @OPSTINA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@NM", NadredjeniMagacin);
				cmd.Parameters.AddWithValue("@MR", MagacinRazduzenja);
				cmd.Parameters.AddWithValue("@FID", FirmaID);
				cmd.Parameters.AddWithValue("@ADRESA", Adresa);
				cmd.Parameters.AddWithValue("@OPSTINA", Opstina);

				cmd.ExecuteNonQuery();
			}
		}

		public static void Insert(
			int id,
			int nadredjeniMagacin,
			int magacinRazduzenja,
			int firmaID,
			string adresa,
			string opstina
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Insert(con, id, nadredjeniMagacin, magacinRazduzenja, firmaID, adresa, opstina);
			}
		}

		public static void Insert(
			FbConnection con,
			int id,
			int nadredjeniMagacin,
			int magacinRazduzenja,
			int firmaID,
			string adresa,
			string opstina
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO MAGACIN (ID, NADREDJENI_MAGACIN, MAGACIN_RAZDUZENJA, FIRMAID, ADRESA, OPSTINA) VALUES (@ID, @NM, @MR, @FID, @ADRESA, @OPSTINA)",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.Parameters.AddWithValue("@NM", nadredjeniMagacin);
				cmd.Parameters.AddWithValue("@MR", magacinRazduzenja);
				cmd.Parameters.AddWithValue("@FID", firmaID);
				cmd.Parameters.AddWithValue("@ADRESA", adresa);
				cmd.Parameters.AddWithValue("@OPSTINA", opstina);

				cmd.ExecuteNonQuery();
			}
		}

		public static Magacin Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Magacin Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, NADREDJENI_MAGACIN, MAGACIN_RAZDUZENJA, FIRMAID, ADRESA, OPSTINA FROM MAGACIN WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Magacin()
						{
							ID = Convert.ToInt32(dr["ID"]),
							MagacinRazduzenja = Convert.ToInt32(dr["MAGACIN_RAZDUZENJA"]),
							NadredjeniMagacin = Convert.ToInt32(dr["NADREDJENI_MAGACIN"]),
							FirmaID = Convert.ToInt32(dr["FIRMAID"]),
							Adresa = dr["ADRESA"].ToString(),
							Opstina = dr["OPSTINA"].ToString()
						};
			}

			return null;
		}

		public static Task<Magacin> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static List<Magacin> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<Magacin> List(FbConnection con)
		{
			List<Magacin> list = new List<Magacin>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, NADREDJENI_MAGACIN, MAGACIN_RAZDUZENJA, FIRMAID, ADRESA, OPSTINA FROM MAGACIN",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Magacin()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinRazduzenja = Convert.ToInt32(dr["MAGACIN_RAZDUZENJA"]),
								NadredjeniMagacin = Convert.ToInt32(dr["NADREDJENI_MAGACIN"]),
								FirmaID = Convert.ToInt32(dr["FIRMAID"]),
								Adresa = dr["ADRESA"].ToString(),
								Opstina = dr["OPSTINA"].ToString()
							}
						);
			}
			return list;
		}

		public static Task<List<Magacin>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
