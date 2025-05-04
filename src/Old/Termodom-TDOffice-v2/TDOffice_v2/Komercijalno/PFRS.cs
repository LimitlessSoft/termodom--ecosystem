using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	public class PFRS
	{
		public int PFRID { get; set; }
		public string Name { get; set; }
		public string Verzija { get; set; }
		public string Url { get; set; }
		public int CanPrint { get; set; }
		public string PrinterName { get; set; }
		public int? ReportID { get; set; }
		public string JID { get; set; }
		public int AddRefNbr { get; set; }

		public static List<PFRS> List(int godina)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<PFRS> List(FbConnection con)
		{
			List<PFRS> list = new List<PFRS>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PFRID, NAME, VERZIJA, URL, CAN_PRINT, PRINTER_NAME, REPORTID, JID, ADD_REF_NBR FROM PFRS",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new PFRS()
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
			return list;
		}
	}
}
