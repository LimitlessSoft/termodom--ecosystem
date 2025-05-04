using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	public static class ProizvodjacManager
	{
		public static ProizvodjacDictionary Dictionary(FbConnection con)
		{
			Dictionary<string, Proizvodjac> dict = new Dictionary<string, Proizvodjac>();
			List<Proizvodjac> Pro = new List<Proizvodjac>();
			using (FbCommand cmd = new FbCommand("SELECT * FROM PROIZVODJAC", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						dict.Add(
							Convert.ToString(dr["PROID"]),
							new Proizvodjac()
							{
								ProID = Convert.ToString(dr["PROID"]),
								Naziv = Convert.ToString(dr["NAZIV"]),
								Zemlja = Convert.ToString(dr["ZEMLJA"]),
								Adresa = Convert.ToString(dr["ADRESA"])
							}
						);
			}
			return new ProizvodjacDictionary(dict);
		}
	}
}
