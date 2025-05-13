using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	public static class BankaManager
	{
		public static BankaDictionary Dictionary(FbConnection con)
		{
			Dictionary<int, Banka> dict = new Dictionary<int, Banka>();
			using (FbCommand cmd = new FbCommand("SELECT * FROM BANKA", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						dict.Add(
							Convert.ToInt32(dr["BANKAID"]),
							new Banka()
							{
								BankaID = Convert.ToInt32(dr["BANKAID"]),
								MaxCek = Convert.ToDouble(dr["MAXCEK"]),
								Naziv = dr["NAZIV"].ToString(),
								Primamo = Convert.ToInt32(dr["PRIMAMO"]),
								ZiroRacun = dr["ZIRORACUN"].ToString()
							}
						);
					}
				}
			}
			return new BankaDictionary(dict);
		}
	}
}
