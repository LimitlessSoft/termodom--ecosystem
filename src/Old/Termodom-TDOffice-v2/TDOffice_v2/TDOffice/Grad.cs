using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Grad
	{
		public int ID { get; set; }
		public string Naziv { get; set; }

		public Grad() { }

		public static Grad Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Grad Get(FbConnection con, int id)
		{
			using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM GRAD WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new Grad()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Naziv = dr["NAZIV"].ToString()
						};
					}
				}
			}

			return null;
		}

		public static Task<Grad> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static List<Grad> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<Grad> List(FbConnection con)
		{
			List<Grad> list = new List<Grad>();
			using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM GRAD", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						list.Add(
							new Grad()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Naziv = dr["NAZIV"].ToString()
							}
						);
					}
				}
			}
			return list;
		}

		public static Task<List<Grad>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
