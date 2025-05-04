using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public static class TarifeManager
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="godinaBaze"></param>
		/// <returns></returns>
		public static TarifaDictionary Dictionary(int? godinaBaze = null)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[
						DB.Settings.MainMagacinKomercijalno,
						godinaBaze ?? DateTime.Now.Year
					]
				)
			)
			{
				con.Open();
				return Dictionary(con);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static TarifaDictionary Dictionary(FbConnection con)
		{
			Dictionary<string, Tarifa> dict = new Dictionary<string, Tarifa>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE",
					con
				)
			)
			using (FbDataReader dr = cmd.ExecuteReader())
				while (dr.Read())
					dict.Add(
						dr["TARIFAID"].ToString(),
						new Termodom.Data.Entities.Komercijalno.Tarifa()
						{
							TarifaID = dr["TARIFAID"].ToString(),
							Naziv = dr["NAZIV"].ToString(),
							Stopa =
								dr["STOPA"] is DBNull
									? null
									: (double?)Convert.ToDouble(dr["STOPA"]),
							FKod =
								dr["F_KOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["F_KOD"]),
							Vrsta = Convert.ToInt32(dr["VRSTA"])
						}
					);
			return new TarifaDictionary(dict);
		}
	}
}
