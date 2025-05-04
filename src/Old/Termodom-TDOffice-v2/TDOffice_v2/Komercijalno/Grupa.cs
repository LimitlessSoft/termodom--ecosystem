using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	public class Grupa
	{
		public string GrupaID { get; set; }
		public string Naziv { get; set; }
		public int CenaPoDim { get; set; }
		public int NikadPopust { get; set; }
		public int Vrsta { get; set; }
		public int RokPlacanja { get; set; }
		public int Web_Update { get; set; }
		public string NGrupaID { get; set; }
		public double Marza { get; set; }

		public Grupa() { }

		public static List<Grupa> List()
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

		public static List<Grupa> List(FbConnection con)
		{
			List<Grupa> Gr = new List<Grupa>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT GRUPAID, NAZIV, CENAPODIM, MARZA, NIKADPOPUST, VRSTA, ROKPLACANJA, WEB_UPDATE, NGRUPAID FROM GRUPA",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						Gr.Add(
							new Grupa()
							{
								GrupaID = Convert.ToString(dr["GRUPAID"]),
								Naziv = Convert.ToString(dr["NAZIV"]),
								CenaPoDim = Convert.ToInt32(dr["CENAPODIM"]),
								//Marza = dr["MARZA"] is DBNull ? null : (double?)Convert.ToDouble(dr["MARZA"]),//Convert.ToDouble(dr["MARZA"]),
								NikadPopust = Convert.ToInt32(dr["NIKADPOPUST"]),
								Vrsta = Convert.ToInt32(dr["VRSTA"]),
								RokPlacanja = Convert.ToInt32(dr["ROKPLACANJA"]),
								Web_Update = Convert.ToInt32(dr["WEB_UPDATE"]),
								NGrupaID = Convert.ToString(dr["NGRUPAID"])
							}
						);
			}
			return Gr;
		}
	}
}
