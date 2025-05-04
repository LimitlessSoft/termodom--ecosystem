using FirebirdSql.Data.FirebirdClient;
using TDBrain_v3.DB;

namespace TDBrain_v3.Managers.TDOffice_v2
{
	/// <summary>
	/// Klasa koja sluzi za komunikaciju sa tabelom Grad
	/// </summary>
	public static class GradManager
	{
		/// <summary>
		/// Vraca objekat Grad za prosjednjeni ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Termodom.Data.Entities.TDOffice_v2.Grad? Get(int id)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()
				)
			)
			{
				con.Open();
				return Get(Connection.TDOffice_v2, id);
			}
		}

		/// <summary>
		/// Vraca objekat Grad za prosjednjeni ID unutar prosledjene konekcije
		/// </summary>
		/// <param name="con"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Termodom.Data.Entities.TDOffice_v2.Grad? Get(FbConnection con, int id)
		{
			using (FbCommand cmd = new FbCommand("SELECT * FROM GRAD WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new Termodom.Data.Entities.TDOffice_v2.Grad()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Naziv = dr["NAZIV"].ToString()
						};
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Vraca dictionary objekata grad
		/// </summary>
		/// <returns></returns>
		public static Termodom.Data.Entities.TDOffice_v2.GradDictionary Dictionary()
		{
			Dictionary<int, Termodom.Data.Entities.TDOffice_v2.Grad> dict =
				new Dictionary<int, Termodom.Data.Entities.TDOffice_v2.Grad>();

			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()
				)
			)
			{
				con.Open();
				using (FbCommand cmd = new FbCommand("SELECT * FROM GRAD", con))
				{
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							dict.Add(
								Convert.ToInt32(dr["ID"]),
								new Termodom.Data.Entities.TDOffice_v2.Grad()
								{
									ID = Convert.ToInt32(dr["ID"]),
									Naziv = dr["NAZIV"].ToString()
								}
							);
						}
					}
				}
			}

			return new Termodom.Data.Entities.TDOffice_v2.GradDictionary(dict);
		}
	}
}
