using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public static class StanjeRacunaManager
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static StanjeRacuna Get(FbConnection con, string tekuciRacun)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT * FROM STANJE_RACUNA WHERE RACUN = @RACUN",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@RACUN", tekuciRacun);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new StanjeRacuna()
						{
							Racun = dr["RACUN"].ToString(),
							PPID = Convert.ToInt32(dr["PPID"]),
							PocDuguje = Convert.ToDouble(dr["POC_DUGUJE"]),
							PocPotrazuje = Convert.ToDouble(dr["POC_POTRAZUJE"]),
							Duguje = Convert.ToDouble(dr["DUGUJE"]),
							Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
							KasaID =
								dr["KASAID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KASAID"]),
							DozvoljeniMinus =
								dr["DOZVOLJENI_MINUS"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["DOZVOLJENI_MINUS"]),
							DozvoljeniMinusVaziDo =
								dr["DM_VAZI_DO"] is DBNull
									? null
									: (DateTime?)Convert.ToDateTime(dr["DM_VAZI_DO"])
						};
					}
				}
			}
			return null;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static StanjeRacunaDictionary Dictionary(FbConnection con)
		{
			Dictionary<string, StanjeRacuna> dict = new Dictionary<string, StanjeRacuna>();
			using (FbCommand cmd = new FbCommand("SELECT * FROM STANJE_RACUNA", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						dict.Add(
							dr["RACUN"].ToString(),
							new StanjeRacuna()
							{
								Racun = dr["RACUN"].ToString(),
								PPID = Convert.ToInt32(dr["PPID"]),
								PocDuguje = Convert.ToDouble(dr["POC_DUGUJE"]),
								PocPotrazuje = Convert.ToDouble(dr["POC_POTRAZUJE"]),
								Duguje = Convert.ToDouble(dr["DUGUJE"]),
								Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
								KasaID =
									dr["KASAID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["KASAID"]),
								DozvoljeniMinus =
									dr["DOZVOLJENI_MINUS"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["DOZVOLJENI_MINUS"]),
								DozvoljeniMinusVaziDo =
									dr["DM_VAZI_DO"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DM_VAZI_DO"])
							}
						);
					}
				}
			}
			return new StanjeRacunaDictionary(dict);
		}
	}
}
