using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	public class JedMere
	{
		public string JM { get; set; }
		public string Naziv { get; set; }
		public int? FKod { get; set; }
		public double? Odnos { get; set; }
		public string OdnosJM { get; set; }
		public int? EkspID { get; set; }
		public string EF_JM { get; set; }

		public static List<JedMere> List(FbConnection con)
		{
			List<JedMere> list = new List<JedMere>();
			using (FbCommand cmd = new FbCommand("SELECT * FROM JEDMERE", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new JedMere()
							{
								JM = dr["JM"].ToString(),
								Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
								FKod =
									dr["F_KOD"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["F_KOD"]),
								Odnos =
									dr["ODNOS"] is DBNull
										? null
										: (double?)Convert.ToDouble(dr["ODNOS"]),
								OdnosJM =
									dr["ODNOS_JM"] is DBNull ? null : dr["ODNOS_JM"].ToString(),
								EkspID =
									dr["EKSPID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["EKSPID"]),
								EF_JM = dr["EF_JM"] is DBNull ? null : dr["EF_JM"].ToString()
							}
						);
			}
			return list;
		}
	}
}
