using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	public class Kontakt
	{
		public int PPID { get; set; }
		public int KONID { get; set; }
		public string Naziv { get; set; }
		public string Telefon { get; set; }
		public string Mobilni { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public int? RMID { get; set; }
		public int? Aktivan { get; set; }

		public Kontakt() { }

		public static Kontakt Get(int ppid, int konid)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return Get(con, ppid, konid);
			}
		}

		public static Kontakt Get(FbConnection con, int ppid, int konid)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PPID, KONID, NAZIV, TELEFON, MOBILNI, FAX, EMAIL, RMID, AKTIVAN FROM KONTAKTI WHERE PPID = @PPID AND KONID = @KONID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@PPID", ppid);
				cmd.Parameters.AddWithValue("@KONID", konid);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new Kontakt()
						{
							PPID = Convert.ToInt32(dr["PPID"]),
							KONID = Convert.ToInt32(dr["KONID"]),
							Naziv = dr["NAZIV"].ToString(),
							Telefon = dr["TELEFON"].ToStringOrDefault(),
							Mobilni = dr["MOBILNI"].ToStringOrDefault(),
							Fax = dr["FAX"].ToStringOrDefault(),
							Email = dr["EMAIL"].ToStringOrDefault(),
							RMID = dr["RMID"] is DBNull ? null : (int?)Convert.ToInt32(dr["RMID"]),
							Aktivan = Convert.ToInt32(dr["AKTIVAN"])
						};
				}
			}
			return null;
		}

		public static Task<Kontakt> GetAsync(int ppid, int konid)
		{
			return Task.Run(() =>
			{
				return Get(ppid, konid);
			});
		}

		public static List<Kontakt> List()
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

		public static List<Kontakt> List(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PPID, KONID, NAZIV, TELEFON, MOBILNI, FAX, EMAIL, RMID, AKTIVAN FROM KONTAKTI",
					con
				)
			)
			{
				List<Kontakt> list = new List<Kontakt>();
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Kontakt()
							{
								PPID = Convert.ToInt32(dr["PPID"]),
								KONID = Convert.ToInt32(dr["KONID"]),
								Naziv = dr["NAZIV"].ToString(),
								Telefon = dr["TELEFON"].ToStringOrDefault(),
								Mobilni = dr["MOBILNI"].ToStringOrDefault(),
								Fax = dr["FAX"].ToStringOrDefault(),
								Email = dr["EMAIL"].ToStringOrDefault(),
								RMID =
									dr["RMID"] is DBNull ? null : (int?)Convert.ToInt32(dr["RMID"]),
								Aktivan = Convert.ToInt32(dr["AKTIVAN"])
							}
						);
				}
				return list;
			}
		}

		public static Task<List<Kontakt>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
