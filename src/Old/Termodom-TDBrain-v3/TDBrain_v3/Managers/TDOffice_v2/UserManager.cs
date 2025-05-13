using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Managers.TDOffice_v2
{
	public static class UserManager
	{
		public static UserDictionary Dictionary()
		{
			Dictionary<int, User> dict = new Dictionary<int, User>();

			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()
				)
			)
			{
				con.Open();
				using (FbCommand cmd = new FbCommand("SELECT * FROM USERS", con))
				{
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							dict.Add(
								Convert.ToInt32(dr["ID"]),
								new User()
								{
									ID = Convert.ToInt32(dr["ID"]),
									Username = dr["USERNAME"].ToString(),
									PW = dr["PW"].ToString(),
									MagacinID = Convert.ToInt32(dr["MAGACINID"]),
									KomercijalnoUserID =
										dr["KOMERCIJALNO_USER_ID"] is DBNull
											? null
											: Convert.ToInt32(dr["KOMERCIJALNO_USER_ID"]),
									OpomeniZaNeizvrseniZadatak = Convert.ToInt32(
										dr["OPOMENI_ZA_NEIZVRSENI_ZADATAK"]
									),
									BonusZakljucavanjaCount = Convert.ToInt32(
										dr["BONUS_ZAKLJUCAVANJA_COUNT"]
									),
									BonusZakljucavanjaLimit = Convert.ToDouble(
										dr["BONUS_ZAKLJUCAVANJA_LIMIT"]
									),
								}
							);
						}
					}
				}
			}

			return new UserDictionary(dict);
		}
	}
}
