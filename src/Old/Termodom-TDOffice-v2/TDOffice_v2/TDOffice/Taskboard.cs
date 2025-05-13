using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class Taskboard
	{
		public int ID { get; set; }
		public string Naziv { get; set; }
		public int KorisnikID { get; set; }
		public ItemStatus Status { get; set; }

		public static Taskboard Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Taskboard Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, NAZIV, KORISNIKID FROM TASKBOARD WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Taskboard()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Naziv = dr["NAZIV"].ToString(),
							KorisnikID = Convert.ToInt32(dr["KORISNIKID"])
						};
			}

			return null;
		}

		public static List<Taskboard> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static Task<List<Taskboard>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static List<Taskboard> List(FbConnection con)
		{
			List<Taskboard> list = new List<Taskboard>();
			using (
				FbCommand cmd = new FbCommand("SELECT ID, NAZIV, KORISNIKID FROM TASKBOARD", con)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Taskboard()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Naziv = dr["NAZIV"].ToString(),
								KorisnikID = Convert.ToInt32(dr["KORISNIKID"])
							}
						);
			}

			return list;
		}

		public static int Insert(string naziv, int korisnikID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, naziv, korisnikID);
			}
		}

		public static int Insert(FbConnection con, string naziv, int korisnikID)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO TASKBOARD (ID, NAZIV, KORISNIKID) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM TASKBOARD) + 1), @N, @KID) RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@N", naziv);
				cmd.Parameters.AddWithValue("@KID", korisnikID);
				cmd.Parameters.Add(
					new FbParameter("ID", FbDbType.Integer)
					{
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}
	}
}
