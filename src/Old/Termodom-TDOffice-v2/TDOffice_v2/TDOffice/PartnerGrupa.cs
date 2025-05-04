using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class PartnerGrupa
	{
		public int ID { get; set; }
		public string Naziv { get; set; }

		public static List<PartnerGrupa> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<PartnerGrupa> List(FbConnection con)
		{
			List<PartnerGrupa> list = new List<PartnerGrupa>();
			using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM PARTNER_GRUPA", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new PartnerGrupa()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Naziv = dr["NAZIV"].ToString()
							}
						);
			}

			return list;
		}

		public static Task<List<PartnerGrupa>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
