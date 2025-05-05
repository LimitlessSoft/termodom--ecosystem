using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	class Proizvodjac
	{
		public string ProID { get; set; }
		public string Naziv { get; set; }
		public string Zemlja { get; set; }
		public string Adresa { get; set; }

		public Proizvodjac() { }

		public static List<Proizvodjac> List()
		{
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

		public static List<Proizvodjac> List(FbConnection con)
		{
			List<Proizvodjac> Pro = new List<Proizvodjac>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PROID, NAZIV, ZEMLJA, ADRESA FROM PROIZVODJAC",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						Pro.Add(
							new Proizvodjac()
							{
								ProID = Convert.ToString(dr["PROID"]),
								Naziv = Convert.ToString(dr["NAZIV"]),
								Zemlja = Convert.ToString(dr["ZEMLJA"]),
								Adresa = Convert.ToString(dr["ADRESA"])
							}
						);
			}
			return Pro;
		}
	}
}
