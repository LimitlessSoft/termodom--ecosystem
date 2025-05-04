using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;

namespace TDOffice_v2.TDOffice
{
	public enum PorukaTip
	{
		Standard = 1,
		Sticky = 2,
		Expanding = 3
	}

	public class Poruka
	{
		public int ID { get; set; }
		public int Posiljalac { get; set; }
		public int Primalac { get; set; }
		public DateTime Datum { get; set; }
		public DateTime? DatumCitanja { get; set; } // Procitana
		public string Naslov { get; set; }
		public string Tekst { get; set; }
		public PorukaTip Status { get; set; } = PorukaTip.Standard; // Prikazana

		public PorukaAdditionalInfo Tag
		{
			get
			{
				if (_rawTag == null)
					_rawTag = Encoding.UTF8.GetBytes(
						JsonConvert.SerializeObject(new PorukaAdditionalInfo())
					);

				return JsonConvert.DeserializeObject<PorukaAdditionalInfo>(
					Encoding.UTF8.GetString(_rawTag)
				);
			}
			set { _rawTag = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)); }
		}
		public byte[] _rawTag { get; set; }
		public bool Arhivirana { get; set; }

		public Enums.PorukaTipPrimaoca TipPrimaoca { get; set; }
		public int? Paket { get; set; }

		public Poruka()
		{
			// TODO > srediti klasu SQL
		}

		/// <summary>
		/// Azurira sledece kolone u bazi: DATUM_CITANJA, STATUS, TAG
		/// </summary>
		/// <param name="con"></param>
		public void Update()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		/// <summary>
		/// Azurira sledece kolone u bazi: DATUM_CITANJA, STATUS, TAG
		/// </summary>
		/// <param name="con"></param>
		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"UPDATE PORUKA SET DATUM_CITANJA = @DC, STATUS = @S, TAG = @T, ARHIVIRANA = @ARH WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@DC", DatumCitanja);
				cmd.Parameters.AddWithValue("@S", Status);
				cmd.Parameters.AddWithValue("@T", _rawTag);
				cmd.Parameters.AddWithValue("@ARH", Arhivirana ? 1 : 0);

				cmd.ExecuteNonQuery();
			}
		}

		public static Poruka Get(int porukaID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						"SELECT ID, POSILJALAC, PRIMALAC, DATUM, DATUM_CITANJA, NASLOV, TEKST, STATUS, TAG, ARHIVIRANA, TIPPRIMAOCA, PAKET FROM PORUKA WHERE ID = @ID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", porukaID);
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
						{
							return new Poruka()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Posiljalac = Convert.ToInt32(dr["POSILJALAC"]),
								Primalac = Convert.ToInt32(dr["PRIMALAC"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								DatumCitanja =
									dr["DATUM_CITANJA"] is DBNull
										? (DateTime?)null
										: Convert.ToDateTime(dr["DATUM_CITANJA"]),
								Naslov = dr["NASLOV"].ToString(),
								Tekst = dr["TEKST"].ToString(),
								Status = (PorukaTip)Convert.ToInt32(dr["STATUS"]),
								_rawTag =
									dr["TAG"] is DBNull
										? Encoding.UTF8.GetBytes(
											JsonConvert.SerializeObject(new PorukaAdditionalInfo())
										)
										: (byte[])dr["TAG"],
								Arhivirana = Convert.ToInt32(dr["ARHIVIRANA"]) == 1 ? true : false,
								TipPrimaoca = (Enums.PorukaTipPrimaoca)
									Convert.ToInt32(dr["TIPPRIMAOCA"]),
								Paket =
									dr["PAKET"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["PAKET"])
							};
						}
					}
				}
			}
			return null;
		}

		public static DateTime? MinDatum()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				using (FbCommand cmd = new FbCommand("SELECT MIN(DATUM) FROM PORUKA", con))
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return Convert.ToDateTime(dr[0]);
			}

			return null;
		}

		public static List<Poruka> List(DateTime odDatuma, DateTime doDatuma)
		{
			return List(
				"DATUM >= '"
					+ odDatuma.ToSystemShortDateFormatString()
					+ "' AND DATUM < '"
					+ doDatuma.ToSystemShortDateFormatString()
					+ "'"
			);
		}

		public static List<Poruka> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<Poruka> List(FbConnection con, string whereQuery = null)
		{
			List<Poruka> list = new List<Poruka>();
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " WHERE " + whereQuery;

			using (
				FbCommand cmd = new FbCommand(
					$@"SELECT ID, POSILJALAC, PRIMALAC, DATUM, DATUM_CITANJA, NASLOV, TEKST, STATUS, TAG,
                ARHIVIRANA, TIPPRIMAOCA, PAKET FROM PORUKA" + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						list.Add(
							new Poruka()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Posiljalac = Convert.ToInt32(dr["POSILJALAC"]),
								Primalac = Convert.ToInt32(dr["PRIMALAC"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								DatumCitanja =
									dr["DATUM_CITANJA"] is DBNull
										? (DateTime?)null
										: Convert.ToDateTime(dr["DATUM_CITANJA"]),
								Naslov = dr["NASLOV"].ToString(),
								Tekst = dr["TEKST"].ToString(),
								Status = (PorukaTip)Convert.ToInt32(dr["STATUS"]),
								_rawTag =
									dr["TAG"] is DBNull
										? Encoding.UTF8.GetBytes(
											JsonConvert.SerializeObject(new PorukaAdditionalInfo())
										)
										: (byte[])dr["TAG"],
								Arhivirana = Convert.ToInt32(dr["ARHIVIRANA"]) == 1 ? true : false,
								TipPrimaoca = (Enums.PorukaTipPrimaoca)
									Convert.ToInt32(dr["TIPPRIMAOCA"]),
								Paket =
									dr["PAKET"] is DBNull
										? (int?)null
										: Convert.ToInt32(dr["PAKET"])
							}
						);
					}
				}
			}
			return list;
		}

		public static Task<List<Poruka>> ListAsync(string whereQuery = null)
		{
			return Task.Run(() =>
			{
				return List(whereQuery);
			});
		}

		public static void Insert(Poruka poruka)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						@"INSERT INTO PORUKA 
                    (ID, POSILJALAC, PRIMALAC, DATUM, DATUM_CITANJA, NASLOV, TEKST, STATUS, TAG, TIPPRIMAOCA, PAKET) 
                    VALUES 
                    (((SELECT COALESCE(MAX(ID), 0) FROM PORUKA) + 1), @POSILJALAC, @PRIMALAC, @DATUM, NULL, @NASLOV, @TEKST, @STATUS, @TAG, @TIPPRIMAOCA, @PAKET)",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@POSILJALAC", poruka.Posiljalac);
					cmd.Parameters.AddWithValue("@PRIMALAC", poruka.Primalac);
					cmd.Parameters.AddWithValue("@DATUM", poruka.Datum);
					cmd.Parameters.AddWithValue("@NASLOV", poruka.Naslov);
					cmd.Parameters.AddWithValue("@TEKST", poruka.Tekst);
					cmd.Parameters.AddWithValue("@STATUS", poruka.Status);
					cmd.Parameters.AddWithValue("@TAG", poruka._rawTag);
					cmd.Parameters.AddWithValue("@TIPPRIMAOCA", poruka.TipPrimaoca);
					cmd.Parameters.AddWithValue("@PAKET", poruka.Paket);
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void Insert(
			int[] primaoci,
			string naslov,
			string tekst,
			PorukaAdditionalInfo additionalInfo = null
		)
		{
			foreach (int i in primaoci)
				Insert(
					new Poruka()
					{
						Datum = DateTime.Now,
						Naslov = naslov,
						Posiljalac = Program.TrenutniKorisnik.ID,
						Primalac = i,
						Status = PorukaTip.Standard,
						_rawTag = Encoding.UTF8.GetBytes(
							JsonConvert.SerializeObject(additionalInfo)
						),
						Tekst = tekst
					}
				);
		}

		public static int MaxPaketID()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						"SELECT COALESCE(MAX(PAKET) + 1, 1) AS MAXID FROM PORUKA",
						con
					)
				)
				{
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
							return dr[0] is DBNull ? 0 : Convert.ToInt32(dr[0]);
					}
				}
			}
			return 1;
		}
	}
}
