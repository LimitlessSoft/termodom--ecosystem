using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Config
{
	public class Zaposleni
	{
		public Int32 ZapID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string Telefon { get; set; }
		public string Email { get; set; }
		public string OrgJed { get; set; }
		public string Korisnik { get; set; }
		public string ELPotpis { get; set; }
		public Int16? Grupa { get; set; } = null;
		public Int16? GrupaID { get; set; } = null;
		public Int16? PravaGrupe { get; set; } = null;
		public Int16? NZapID { get; set; } = null;
		public Int16? EkspID { get; set; } = null;
		public string MBroj { get; set; }
		public string BRLK { get; set; }
		public string EmailPass { get; set; }
		public string EmailReply { get; set; }
		public Int32? WebUserID { get; set; } = null;
		public Int32? CustID { get; set; } = null;
		public Int32? PPID { get; set; } = null;

		public void Update()
		{
			using (FbConnection con = new FbConnection(Config.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE ZAPOSLENI SET NAZIV = @NAZIV, ADRESA = @ADRESA, TELEFON = @TEL, EMAIL = @EMAIL,
                 ORGJED = @ORG, KORISNIK = @KOR, EL_POTPIS = @EP, GRUPA = @GR, GRUPAID = @GID, PRAVA_GRUPE = @PGR, NZAPID = @NZID, EKSPID = @EKSPID,
                 MBROJ = @MBR, BRLK  = @BLK, EMAIL_PASS = @EPAS, EMAIL_REPLY = @ER, WEB_USERID = @WUID, CUST_ID = @CID, PPID = @PPID
                 WHERE ZAPID = @ZID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@NAZIV", Naziv);
				cmd.Parameters.AddWithValue("@ADRESA", Adresa);
				cmd.Parameters.AddWithValue("@TEL", Telefon);
				cmd.Parameters.AddWithValue("@EMAIL", Email);
				cmd.Parameters.AddWithValue("@ORG", OrgJed);
				cmd.Parameters.AddWithValue("@KOR", Korisnik);
				cmd.Parameters.AddWithValue("@EP", ELPotpis);
				cmd.Parameters.AddWithValue("@GR", Grupa);
				cmd.Parameters.AddWithValue("@GID", GrupaID);
				cmd.Parameters.AddWithValue("@PGR", PravaGrupe);
				cmd.Parameters.AddWithValue("@NZID", NZapID);
				cmd.Parameters.AddWithValue("@EKSPID", EkspID);
				cmd.Parameters.AddWithValue("@MBR", MBroj);
				cmd.Parameters.AddWithValue("@BLK", BRLK);
				cmd.Parameters.AddWithValue("@EPAS", EmailPass);
				cmd.Parameters.AddWithValue("@ER", EmailReply);
				cmd.Parameters.AddWithValue("@WUID", WebUserID);
				cmd.Parameters.AddWithValue("@CID", CustID);
				cmd.Parameters.AddWithValue("@PPID", PPID);
				cmd.Parameters.AddWithValue("@ZID", ZapID);

				cmd.ExecuteNonQuery();
			}
		}

		public static Zaposleni Get(int zapID)
		{
			using (FbConnection con = new FbConnection(Config.connectionString))
			{
				con.Open();
				return Get(con, zapID);
			}
		}

		public static Zaposleni Get(FbConnection con, int zapID)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ZAPID, NAZIV, ADRESA, TELEFON, EMAIL, ORGJED, KORISNIK, 
                EL_POTPIS, GRUPA, GRUPAID, PRAVA_GRUPE, NZAPID, EKSPID, MBROJ, BRLK, EMAIL_PASS, EMAIL_REPLY,
                WEB_USERID, CUST_ID, PPID FROM ZAPOSLENI  WHERE ZAPID = @ZID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ZID", zapID);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new Zaposleni()
						{
							ZapID = Convert.ToInt32(dr["ZAPID"]),
							Naziv = dr["NAZIV"].ToString(),
							Adresa = dr["ADRESA"] is DBNull ? null : dr["ADRESA"].ToString(),
							Telefon = dr["TELEFON"] is DBNull ? null : dr["TELEFON"].ToString(),
							Email = dr["EMAIL"] is DBNull ? null : dr["EMAIL"].ToString(),
							OrgJed = dr["ORGJED"] is DBNull ? null : dr["ORGJED"].ToString(),
							Korisnik = dr["KORISNIK"] is DBNull ? null : dr["KORISNIK"].ToString(),
							ELPotpis =
								dr["EL_POTPIS"] is DBNull ? null : dr["EL_POTPIS"].ToString(),
							Grupa =
								dr["GRUPA"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["GRUPA"]),
							GrupaID =
								dr["GRUPAID"] is DBNull
									? (Int16?)null
									: Convert.ToInt16(dr["GRUPAID"]),
							PravaGrupe =
								dr["PRAVA_GRUPE"] is DBNull
									? (Int16?)null
									: Convert.ToInt16(dr["PRAVA_GRUPE"]),
							NZapID =
								dr["NZAPID"] is DBNull
									? (Int16?)null
									: Convert.ToInt16(dr["NZAPID"]),
							EkspID =
								dr["EKSPID"] is DBNull
									? (Int16?)null
									: Convert.ToInt16(dr["EKSPID"]),
							MBroj = dr["MBROJ"] is DBNull ? null : dr["MBROJ"].ToString(),
							BRLK = dr["BRLK"] is DBNull ? null : dr["BRLK"].ToString(),
							EmailPass =
								dr["EMAIL_PASS"] is DBNull ? null : dr["EMAIL_PASS"].ToString(),
							EmailReply =
								dr["EMAIL_REPLY"] is DBNull ? null : dr["EMAIL_REPLY"].ToString(),
							WebUserID =
								dr["WEB_USERID"] is DBNull
									? (Int32?)null
									: Convert.ToInt32(dr["WEB_USERID"]),
							CustID = Convert.ToInt32(dr["CUST_ID"]),
							PPID = dr["PPID"] is DBNull ? (Int32?)null : Convert.ToInt32(dr["PPID"])
						};
			}

			return null;
		}

		public static List<Zaposleni> List()
		{
			using (FbConnection con = new FbConnection(Config.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<Zaposleni> List(FbConnection con)
		{
			List<Zaposleni> list = new List<Zaposleni>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ZAPID, NAZIV, ADRESA, TELEFON, EMAIL, ORGJED, KORISNIK, 
                EL_POTPIS, GRUPA, GRUPAID, PRAVA_GRUPE, NZAPID, EKSPID, MBROJ, BRLK, EMAIL_PASS, EMAIL_REPLY,
                WEB_USERID, CUST_ID, PPID FROM ZAPOSLENI",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new Zaposleni()
							{
								ZapID = Convert.ToInt16(dr["ZAPID"]),
								Naziv = dr["NAZIV"].ToString(),
								Adresa = dr["ADRESA"] is DBNull ? null : dr["ADRESA"].ToString(),
								Telefon = dr["TELEFON"] is DBNull ? null : dr["TELEFON"].ToString(),
								Email = dr["EMAIL"] is DBNull ? null : dr["EMAIL"].ToString(),
								OrgJed = dr["ORGJED"] is DBNull ? null : dr["ORGJED"].ToString(),
								Korisnik =
									dr["KORISNIK"] is DBNull ? null : dr["KORISNIK"].ToString(),
								ELPotpis =
									dr["EL_POTPIS"] is DBNull ? null : dr["EL_POTPIS"].ToString(),
								Grupa =
									dr["GRUPA"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["GRUPA"]),
								GrupaID =
									dr["GRUPAID"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["GRUPAID"]),
								PravaGrupe =
									dr["PRAVA_GRUPE"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["PRAVA_GRUPE"]),
								NZapID =
									dr["NZAPID"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["NZAPID"]),
								EkspID =
									dr["EKSPID"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["EKSPID"]),
								MBroj = dr["MBROJ"] is DBNull ? null : dr["MBROJ"].ToString(),
								BRLK = dr["BRLK"] is DBNull ? null : dr["BRLK"].ToString(),
								EmailPass =
									dr["EMAIL_PASS"] is DBNull ? null : dr["EMAIL_PASS"].ToString(),
								EmailReply =
									dr["EMAIL_REPLY"] is DBNull
										? null
										: dr["EMAIL_REPLY"].ToString(),
								WebUserID =
									dr["WEB_USERID"] is DBNull
										? (Int32?)null
										: Convert.ToInt32(dr["WEB_USERID"]),
								CustID = Convert.ToInt32(dr["CUST_ID"]),
								PPID =
									dr["PPID"] is DBNull
										? (Int32?)null
										: Convert.ToInt32(dr["PPID"])
							}
						);
			}
			return list;
		}

		public static Task<List<Zaposleni>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
