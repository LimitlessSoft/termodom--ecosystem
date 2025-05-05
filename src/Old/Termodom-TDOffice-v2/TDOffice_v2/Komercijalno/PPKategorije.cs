using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	class PPKategorije
	{
		public string KatNaziv { get; set; }
		public int KatID { get; set; }

		public PPKategorije() { }

		public static List<PPKategorije> List()
		{
			List<PPKategorije> ppk = new List<PPKategorije>();
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return List(con);
			}
		}

		public static List<PPKategorije> List(FbConnection con)
		{
			List<PPKategorije> list = new List<PPKategorije>();
			using (FbCommand cmd = new FbCommand("SELECT KATID, KATNAZIV FROM PPKATEGORIJE", con))
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new PPKategorije()
							{
								KatID = Convert.ToInt32(dr["KATID"]),
								KatNaziv = Convert.ToString(dr["KATNAZIV"]),
							}
						);
			}
			return list;
		}

		public static Task<List<PPKategorije>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
