using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.DB.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public class IzvodManager
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static IzvodDictionary Dictionary(
			FbConnection con,
			List<string>? queryParameters = null
		)
		{
			Dictionary<int, Izvod> dict = new Dictionary<int, Izvod>();

			string whereQuery = "";

			if (queryParameters != null && queryParameters.Count > 0)
				whereQuery = " WHERE " + string.Join(" AND ", queryParameters);

			using (FbCommand cmd = new FbCommand("SELECT * FROM IZVOD " + whereQuery, con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						int izvodID = Convert.ToInt32(dr["IZVODID"]);
						dict.Add(
							izvodID,
							new Izvod()
							{
								IzvodId = izvodID,
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								PPID = Convert.ToInt32(dr["PPID"]),
								SifPlac = dr["SIFPLAC"].ToString(),
								Duguje = Convert.ToDouble(dr["DUGUJE"]),
								Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
								Rasporedjen = Convert.ToInt32(dr["RASPOREDJEN"]),
								RDuguje = Convert.ToDouble(dr["RDUGUJE"]),
								RPotrazuje = Convert.ToDouble(dr["RPOTRAZUJE"]),
								R = Convert.ToInt32(dr["R"]),
								Konto = dr["KONTO"] is DBNull ? null : dr["KONTO"].ToString(),
								Valuta = dr["VALUTA"].ToString(),
								Kurs = Convert.ToDouble(dr["KURS"]),
								PozNaBroj =
									dr["POZNABROJ"] is DBNull ? null : dr["POZNABROJ"].ToString(),
								ZiroRacun =
									dr["ZIRORACUN"] is DBNull ? null : dr["ZIRORACUN"].ToString(),
								UPPID = Convert.ToInt32(dr["UPPID"]),
								Svrha = dr["SVRHA"] is DBNull ? null : dr["SVRHA"].ToString(),
								ED1 = dr["ED1"] is DBNull ? null : dr["ED1"].ToString(),
								PozNaBrojOd = dr["SVRHA"] is DBNull ? null : dr["SVRHA"].ToString(),
								VirmanID =
									dr["VIRMANID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VIRMANID"]),
								PoPDVBroj =
									dr["POPDVBROJ"] is DBNull ? null : dr["POPDVBROJ"].ToString(),
							}
						);
					}
				}
			}

			return new IzvodDictionary(dict);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <param name="tekuciRacun"></param>
		/// <returns></returns>
		public static double DugujeSum(FbConnection con, string tekuciRacun)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"
SELECT COALESCE(SUM(IZVOD.DUGUJE), 0) FROM DOKUMENT LEFT OUTER JOIN IZVOD
ON DOKUMENT.VRDOK = IZVOD.VRDOK AND DOKUMENT.BRDOK = IZVOD.BRDOK
WHERE DOKUMENT.OPISUPL = @ZR
",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ZR", tekuciRacun);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return Convert.ToDouble(dr[0]);
				}
			}
			return 0;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <param name="tekuciRacun"></param>
		/// <returns></returns>
		public static double PotrazujeSum(FbConnection con, string tekuciRacun)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"
SELECT COALESCE(SUM(IZVOD.POTRAZUJE), 0) FROM DOKUMENT LEFT OUTER JOIN IZVOD
ON DOKUMENT.VRDOK = IZVOD.VRDOK AND DOKUMENT.BRDOK = IZVOD.BRDOK
WHERE DOKUMENT.OPISUPL = @ZR
",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ZR", tekuciRacun);
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return Convert.ToDouble(dr[0]);
				}
			}
			return 0;
		}
	}
}
