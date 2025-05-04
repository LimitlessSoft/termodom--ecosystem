using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public static class PFRManager
	{
		/// <summary>
		/// Vraca dictionary PFR objekta iz baze za datu bazu i godinu
		/// </summary>
		/// <param name="bazaId"></param>
		/// <param name="godinaBaze">Ukoliko se prosledi NULL bice koriscena trenutna godina</param>
		/// <returns></returns>
		public static PFRDictionary Dictionary(int bazaId, int? godinaBaze)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[
						bazaId,
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
		/// Vraca dictinary PFR objekata iz baze
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		public static PFRDictionary Dictionary(FbConnection con)
		{
			Dictionary<int, PFR> dict = new Dictionary<int, PFR>();

			using (FbCommand cmd = new FbCommand("SELECT * FROM PFRS", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						dict.Add(
							Convert.ToInt32(dr["PFRID"]),
							new PFR()
							{
								PFRID = Convert.ToInt32(dr["PFRID"]),
								Name = dr["NAME"].ToString(),
								Verzija = dr["VERZIJA"].ToString(),
								Url = dr["URL"].ToString(),
								CanPrint = Convert.ToInt32(dr["CAN_PRINT"]),
								PrinterName = dr["PRINTER_NAME"].ToStringOrDefault(),
								ReportID = dr["REPORTID"] is DBNull ? null : (int?)dr["REPORTID"],
								JID = dr["JID"].ToStringOrDefault(),
								AddRefNbr = Convert.ToInt32(dr["ADD_REF_NBR"])
							}
						);
					}
				}
			}

			return new PFRDictionary(dict);
		}
	}
}
