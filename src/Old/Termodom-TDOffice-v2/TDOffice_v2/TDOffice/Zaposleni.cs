using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Zaposleni
	{
		public int ID { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }

		public Zaposleni() { }

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
					"UPDATE ZAPOSLENI SET IME = @I, PREZIME = @P WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@I", Ime);
				cmd.Parameters.AddWithValue("@P", Prezime);

				cmd.ExecuteNonQuery();
			}
		}

		public Task UpdateAsync()
		{
			return Task.Run(() =>
			{
				Update();
			});
		}

		public override string ToString()
		{
			return $"{Ime} {Prezime}";
		}

		public static int Insert(string ime, string prezime)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, ime, prezime);
			}
		}

		public static int Insert(FbConnection con, string ime, string prezime)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO ZAPOSLENI (ID, IME, PREZIME) VALUES (((SELECT COALESCE(MAX(ID), 0) from ZAPOSLENI) + 1), @I, @P) RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.Add("ID", FbDbType.Integer).Direction = System
					.Data
					.ParameterDirection
					.Output;
				cmd.Parameters.AddWithValue("@I", ime);
				cmd.Parameters.AddWithValue("@P", prezime);

				cmd.ExecuteNonQuery();
				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static Task<int> InsertAsync(FbConnection con, string ime, string prezime)
		{
			return Task.Run(() =>
			{
				return Insert(con, ime, prezime);
			});
		}

		public static Zaposleni Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Zaposleni Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, IME, PREZIME  FROM ZAPOSLENI WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Zaposleni()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Ime = dr["IME"].ToString(),
							Prezime = dr["PREZIME"].ToString()
						};
			}

			return null;
		}

		public static Task<Zaposleni> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static List<Zaposleni> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<Zaposleni> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;
			List<Zaposleni> list = new List<Zaposleni>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, IME, PREZIME FROM ZAPOSLENI WHERE 1 = 1" + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Zaposleni()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Ime = dr["IME"].ToString(),
								Prezime = dr["PREZIME"].ToString()
							}
						);
			}
			return list;
		}

		public static Task<List<Zaposleni>> ListAsync(string whereQuery = null)
		{
			return Task.Run(() =>
			{
				return List(whereQuery);
			});
		}

		/// <summary>
		/// Brise zaposlenog iz baze zajedno sa svim njegovim ugovorima
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Delete(con, id);
			}
		}

		/// <summary>
		/// Brise zaposlenog iz baze zajedno sa svim njegovim ugovorima
		/// </summary>
		/// <param name="con"></param>
		/// <param name="id"></param>
		public static void Delete(FbConnection con, int id)
		{
			using (FbCommand cmd = new FbCommand("DELETE FROM ZAPOSLENI WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.ExecuteNonQuery();

				cmd.CommandText = "DELETE FROM ZAPOSLENI_UGOVOR_O_RADU WHERE ZAPOSLENI_ID = @ID";
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Brise zaposlenog iz baze zajedno sa svim njegovim ugovorima
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Task DeleteAsync(int id)
		{
			return Task.Run(() =>
			{
				Delete(id);
			});
		}
	}
}
