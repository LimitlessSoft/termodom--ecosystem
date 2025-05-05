using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Termodom.Models
{
	// TODO: Transfer u API
	public class CenovnikGrupa
	{
		public int ID { get; set; }
		public string Naziv { get; set; }

		private static object _insertLock = new object();

		private static int _GetMaxID(MySqlConnection con)
		{
			using (
				MySqlCommand cmd = new MySqlCommand(
					"SELECT MAX(CENOVNIK_GRUPAID) FROM CENOVNIK_GRUPA",
					con
				)
			)
			{
				using (MySqlDataReader dt = cmd.ExecuteReader())
				{
					if (dt.Read())
						return Convert.ToInt32(dt[0]);
				}
				throw new Exception("Nije pronadjen ID");
			}
		}

		/// <summary>
		/// Kreira novu cenovnu grupu u bazi i vraca njen ID
		/// </summary>
		/// <param name="Naziv"></param>
		/// <returns></returns>
		public static int Insert(string naziv)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return Insert(con, naziv);
			}
		}

		/// <summary>
		/// Kreira novu cenovnu grupu u bazi i vraca njen ID
		/// </summary>
		/// <param name="Naziv"></param>
		/// <returns></returns>
		public static int Insert(MySqlConnection con, string Naziv)
		{
			lock (_insertLock)
			{
				using (
					MySqlCommand cmd = new MySqlCommand(
						"INSERT INTO CENOVNIK_GRUPA(NAZIV) VALUES(@NAZIV)",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@NAZIV", Naziv);
					cmd.ExecuteNonQuery();
					return _GetMaxID(con);
				}
			}
		}

		/// <summary>
		/// Vraca cenovnik grupu iz baze
		/// </summary>
		/// <returns></returns>
		public static CenovnikGrupa Get(int id)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				return Get(con, id);
			}
		}

		/// <summary>
		/// Vraca cenovnik grupu iz baze
		/// </summary>
		/// <returns></returns>
		public static CenovnikGrupa Get(MySqlConnection con, int id)
		{
			CenovnikGrupa c = new CenovnikGrupa();
			// TODO: Implement
			using (
				MySqlCommand cmd = new MySqlCommand(
					"SELECT NAZIV FROM CENOVNIK_GRUPA WHERE CENOVNIK_GRUPAID = @ID",
					con
				)
			)
			{
				using (MySqlDataReader dt = cmd.ExecuteReader())
				{
					if (dt.Read())
					{
						c.Naziv = dt[0].ToString();
						c.ID = id;
					}
				}
			}
			return c;
		}

		/// <summary>
		/// Vraca listu cenovnih grupa iz baze
		/// </summary>
		/// <returns></returns>
		public static List<CenovnikGrupa> List()
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return List(con);
			}
		}

		/// <summary>
		/// Vraca listu cenovnih grupa iz baze
		/// </summary>
		/// <returns></returns>
		public static List<CenovnikGrupa> List(MySqlConnection con)
		{
			List<CenovnikGrupa> list = new List<CenovnikGrupa>();

			using (
				MySqlCommand cmd = new MySqlCommand(
					"SELECT CENOVNIK_GRUPAID, NAZIV FROM CENOVNIK_GRUPA",
					con
				)
			)
			using (MySqlDataReader dr = cmd.ExecuteReader())
				while (dr.Read())
					list.Add(
						new CenovnikGrupa()
						{
							ID = Convert.ToInt32(dr["CENOVNIK_GRUPAID"]),
							Naziv = dr["NAZIV"].ToString()
						}
					);
			return list;
		}

		/// <summary>
		/// Brise cenivnik grupu iz baze pod datim id-om
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(int id)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				Delete(con, id);
			}
		}

		/// <summary>
		/// Brise cenivnik grupu iz baze pod datim id-om
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(MySqlConnection con, int id)
		{
			using (
				MySqlCommand cmd = new MySqlCommand(
					"DELETE FROM CENOVNIK_GRUPA WHERE CENOVNIK_GRUPAID = @ID",
					con
				)
			)
			{
				cmd.ExecuteNonQuery();
			}
			throw new NotImplementedException();
		}
	}
}
