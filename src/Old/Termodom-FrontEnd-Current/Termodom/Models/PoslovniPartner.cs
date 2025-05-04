using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Termodom.Models
{
	/// <summary>
	/// Predstavlja poslovnog partnera
	/// Poslovni partneri su sacuvani u bazi
	/// Podaci u bazi se ne pune rucno vec se vuku iz Komercijalnog!
	/// </summary>
	public class PoslovniPartner
	{
		public int PPID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string PIB { get; set; }

		public static string GetName(int PPID)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				using (
					MySqlCommand cmd = new MySqlCommand(
						"SELECT NAZIV FROM POSLOVNI_PARTNER WHERE PPID = @P",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@P", PPID);

					using (MySqlDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
							return dr[0].ToString();
					}
				}
			}
			return "Undefined";
		}

		public static PoslovniPartner GetPartner(int PPID)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				using (
					MySqlCommand cmd = new MySqlCommand(
						"SELECT PPID, NAZIV, ADRESA, PIB FROM POSLOVNI_PARTNER WHERE PPID = @P",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@P", PPID);

					using (MySqlDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
							return new PoslovniPartner()
							{
								PPID = Convert.ToInt32(dr["PPID"]),
								Naziv = dr["NAZIV"].ToString(),
								Adresa = dr["ADRESA"].ToString(),
								PIB = dr["PIB"].ToString()
							};
					}
				}
			}
			return null;
		}
	}
}
