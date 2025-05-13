using System.Reflection.Metadata.Ecma335;
using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public static class PartnerManager
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <param name="ppid"></param>
		/// <returns></returns>
		public static Partner? Get(FbConnection con, int ppid)
		{
			var dict = Dictionary(con, new List<string>() { $"PPID = {ppid}" });

			return dict.ContainsKey(ppid) ? dict[ppid] : null;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="con"></param>
		/// <param name="whereParameters"></param>
		/// <returns></returns>
		public static PartnerDictionary Dictionary(
			FbConnection con,
			List<string>? whereParameters = null
		)
		{
			string whereQuery = "";

			if (whereParameters != null && whereParameters.Count > 0)
				whereQuery = " WHERE " + string.Join(" AND ", whereParameters);

			Dictionary<int, Partner> dict = new Dictionary<int, Partner>();
			using (FbCommand cmd = new FbCommand(@"SELECT * FROM PARTNER " + whereQuery, con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						dict.Add(
							Convert.ToInt32(dr["PPID"]),
							new Partner()
							{
								PPID = Convert.ToInt32(dr["PPID"]),
								Naziv = Convert.ToString(dr["NAZIV"]),
								Adresa =
									dr["ADRESA"] is DBNull ? null : Convert.ToString(dr["ADRESA"]),
								Posta =
									dr["POSTA"] is DBNull ? null : Convert.ToString(dr["POSTA"]),
								MestoID =
									dr["MESTOID"] is DBNull
										? null
										: Convert.ToString(dr["MESTOID"]),
								Telefon =
									dr["TELEFON"] is DBNull
										? null
										: Convert.ToString(dr["TELEFON"]),
								Fax = dr["FAX"] is DBNull ? null : Convert.ToString(dr["FAX"]),
								Email =
									dr["EMAIL"] is DBNull ? null : Convert.ToString(dr["EMAIL"]),
								Kontakt =
									dr["KONTAKT"] is DBNull
										? null
										: Convert.ToString(dr["KONTAKT"]),
								PIB = dr["PIB"] is DBNull ? null : Convert.ToString(dr["PIB"]),
								Kategorija =
									dr["KATEGORIJA"] is DBNull
										? (Int64?)null
										: Convert.ToInt64(dr["KATEGORIJA"]),
								MBroj =
									dr["MBROJ"] is DBNull ? null : Convert.ToString(dr["MBROJ"]),
								Mobilni =
									dr["MOBILNI"] is DBNull
										? null
										: Convert.ToString(dr["MOBILNI"]),
								Aktivan = Convert.ToInt32(dr["AKTIVAN"]),
								PDVO = Convert.ToInt32(dr["PDVO"]),
								DrzavaID =
									dr["DRZAVAID"] is DBNull ? 0 : Convert.ToInt32(dr["DRZAVAID"]),
								Duguje = Convert.ToDouble(dr["DUGUJE"]),
								Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
							}
						);
			}
			return new PartnerDictionary(dict);
		}
	}
}
