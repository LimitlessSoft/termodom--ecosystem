using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;
using TDOffice_v2.DTO.TDBrain_v3.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
	public class Partner
	{
		#region Properties
		public int PPID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string Posta { get; set; }
		public string Mesto { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string Kontakt { get; set; }
		public string MBroj { get; set; }
		public string MestoID { get; set; }
		public int OpstinaID { get; set; }
		public int DrzavaID { get; set; }
		public int ZapID { get; set; }
		public string Valuta { get; set; }
		public string Mobilni { get; set; }
		public Int64? Kategorija { get; set; }
		public float DozvoljeniMinus { get; set; }
		public int? NPPID { get; set; }
		public string PIB { get; set; }
		public int? ImaUgovor { get; set; }
		public int VrstaCenovnika { get; set; }
		public int RefID { get; set; }
		public int DrzavljanstvoID { get; set; }
		public int ZanimanjeID { get; set; }
		public int WEB_Status { get; set; }
		public int GPPID { get; set; }
		public int Cene_od_grupe { get; set; }
		public int? VPCID { get; set; }
		public double? PROCPC { get; set; }
		public int PDVO { get; set; }
		public string NazivZaStampu { get; set; }
		public string KatNaziv { get; set; }
		public int KatID { get; set; }
		public int Aktivan { get; set; }
		public double Duguje { get; set; }
		public double Potrazuje { get; set; }
		#endregion
		public Partner() { }

		// Updateuje sve informacije iz trenutnog ovjekta u bazu
		public void Update()
		{
			throw new Exception("Mora se prebaciti da radi preko TDBrain-a");
			if (this.PPID == 0)
				throw new Exception("Korisnik ne postoji u bazi!");

			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						@"UPDATE OR INSERT INTO PARTNER (PPID, NAZIV, ADRESA, POSTA, TELEFON, MOBILNI, FAX, EMAIL, KONTAKT, PIB, KATEGORIJA,
                    AKTIVAN, MESTOID, MBROJ, OPSTINAID, DRZAVAID, REFID, PDVO, NPPID, NAZIVZASTAMPU, VALUTA, DOZVOLJENIMINUS,
                    IMAUGOVOR, VRSTACENOVNIKA, DRZAVLJANSTVOID, ZANIMANJEID, WEB_STATUS, GPPID, CENE_OD_GRUPE, VPCID, PROCPC) 
                    VALUES 
                    (@PPID, @NAZIV, @ADRESA, @POSTA, @TELEFON, @MOBILNI, @FAX, @EMAIL, @KONTAKT, @PIB, @KATEGORIJA, @AKTIVAN,
                    @MESTOID, @MBROJ, @OPSTINAID, @DRZAVAID, @REFID, @PDVO, @NPPID, @NAZIVZASTAMPU, @VALUTA, @DOZVOLJENIMINUS,
                    @IMAUGOVOR, @VRSTACENOVNIKA, @DRZAVLJANSTVOID, @ZANIMANJEID, @WEB_STATUS, @GPPID, @CENE_OD_GRUPE, @VPCID, @PROCPC) MATCHING (PPID)",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@PPID", this.PPID);
					cmd.Parameters.AddWithValue("@NAZIV", this.Naziv);
					cmd.Parameters.AddWithValue("@ADRESA", this.Adresa);
					cmd.Parameters.AddWithValue("@POSTA", this.Posta);
					cmd.Parameters.AddWithValue("@TELEFON", this.Telefon);
					cmd.Parameters.AddWithValue("@MOBILNI", this.Mobilni);
					cmd.Parameters.AddWithValue("@FAX", this.Fax);
					cmd.Parameters.AddWithValue("@EMAIL", this.Email);
					cmd.Parameters.AddWithValue("@KONTAKT", this.Kontakt);
					cmd.Parameters.AddWithValue("@PIB", this.PIB);
					cmd.Parameters.AddWithValue("@KATEGORIJA", this.Kategorija);
					cmd.Parameters.AddWithValue("@AKTIVAN", this.Aktivan);
					cmd.Parameters.AddWithValue("@MESTOID", this.MestoID);
					cmd.Parameters.AddWithValue("@MBROJ", this.MBroj);
					cmd.Parameters.AddWithValue("@OPSTINAID", this.OpstinaID);
					cmd.Parameters.AddWithValue("@DRZAVAID", this.DrzavaID);
					cmd.Parameters.AddWithValue("@REFID", this.RefID);
					cmd.Parameters.AddWithValue("@PDVO", this.PDVO);
					cmd.Parameters.AddWithValue("@NPPID", this.PPID);
					cmd.Parameters.AddWithValue("@NAZIVZASTAMPU", this.Naziv);
					cmd.Parameters.AddWithValue("@VALUTA", this.Valuta);
					cmd.Parameters.AddWithValue("@DOZVOLJENIMINUS", this.DozvoljeniMinus);
					cmd.Parameters.AddWithValue("@IMAUGOVOR", this.ImaUgovor);
					cmd.Parameters.AddWithValue("@VRSTACENOVNIKA", this.VrstaCenovnika);
					cmd.Parameters.AddWithValue("@DRZAVLJANSTVOID", this.DrzavljanstvoID);
					cmd.Parameters.AddWithValue("@ZANIMANJEID", this.ZanimanjeID);
					cmd.Parameters.AddWithValue("@WEB_STATUS", this.WEB_Status);
					cmd.Parameters.AddWithValue("@GPPID", this.GPPID);
					cmd.Parameters.AddWithValue("@CENE_OD_GRUPE", this.Cene_od_grupe);
					cmd.Parameters.AddWithValue("@VPCID", this.VPCID);
					cmd.Parameters.AddWithValue("@PROCPC", this.PROCPC);
					cmd.ExecuteNonQuery();
				}
			}

			//throw new NotImplementedException();
		}

		public static void NapuniCheckedListBox(ref CheckedListBox checkedListBox, string kat)
		{
			Int64 k;
			Int64 kolicnik;
			Int64 Izlaz;
			int kategorija;
			kategorija = 0;
			k = Convert.ToInt64(kat);
			while (k > 0)
			{
				kolicnik = k / 2;
				Izlaz = k - kolicnik * 2;
				if (Izlaz == 1)
				{
					checkedListBox.SetItemChecked(kategorija, true);
				}

				kategorija = kategorija + 1;
				k = kolicnik;
			}
		}

		public static List<int> KategorijeUKobasici(Int64? kobasica)
		{
			if (kobasica == null)
				return new List<int>();

			List<int> list = new List<int>();
			Int64 k;
			Int64 kolicnik;
			Int64 Izlaz;
			int kategorija;
			kategorija = 0;
			k = (Int64)kobasica;
			while (k > 0)
			{
				kolicnik = k / 2;
				Izlaz = k - kolicnik * 2;
				if (Izlaz == 1)
				{
					list.Add(kategorija);
				}

				kategorija = kategorija + 1;
				k = kolicnik;
			}
			return list;
		}

		public static Int64? GetKatKobasicu(CheckedListBox checkedListBox)
		{
			if (checkedListBox.Items.Count == 0)
				return null;

			Int64 kat = 0;
			for (int i = 0; i < checkedListBox.Items.Count - 1; i++)
			{
				if (checkedListBox.GetItemChecked(i))
				{
					kat = kat + stepenrobakategorije(i);
				}
			}
			return kat;
		}

		private static Int64 stepenrobakategorije(int b)
		{
			Int64 strbk;
			if (b > 0)
			{
				strbk = 1;
				for (int j = 1; j <= b; j++)
				{
					strbk = strbk * 2;
				}
			}
			else
			{
				strbk = 1;
			}
			return Math.Abs(strbk);
		}

		public static async Task<Partner> GetAsync(int ppid)
		{
			var result = await TDBrain_v3.GetAsync("/komercijalno/partner/get?ppid=" + ppid);
			if ((int)result.StatusCode == 200)
			{
				string responseString = await result.Content.ReadAsStringAsync();
				DTO.TDBrain_v3.Komercijalno.PartnerGetDTO dto =
					JsonConvert.DeserializeObject<DTO.TDBrain_v3.Komercijalno.PartnerGetDTO>(
						responseString
					);
				return dto.ToPartner();
			}
			else if ((int)result.StatusCode == 204)
			{
				return null;
			}
			else if ((int)result.StatusCode == 400)
			{
				throw new Exception("Nije prosledjen parametar u requestu!");
			}
			else
			{
				throw new Exception("Greska prilikom komunikacije sa partnerom!");
			}
		}

		public static async Task<Partner> GetAsync(string pib)
		{
			var result = await TDBrain_v3.GetAsync("/komercijalno/partner/get?pib=" + pib);
			if ((int)result.StatusCode == 200)
			{
				string responseString = await result.Content.ReadAsStringAsync();
				DTO.TDBrain_v3.Komercijalno.PartnerGetDTO dto =
					JsonConvert.DeserializeObject<DTO.TDBrain_v3.Komercijalno.PartnerGetDTO>(
						responseString
					);
				return dto.ToPartner();
			}
			else if ((int)result.StatusCode == 204)
			{
				return null;
			}
			else if ((int)result.StatusCode == 400)
			{
				throw new Exception("Nije prosledjen parametar u requestu!");
			}
			else
			{
				throw new Exception("Greska prilikom komunikacije sa partnerom!");
			}
		}

		public static async Task<List<Partner>> ListAsync(int? godina = null)
		{
			List<string> par = new List<string>();
			if (godina != null)
				par.Add("godina=" + godina);

			var result = await TDBrain_v3.GetAsync(
				"/komercijalno/partner/list?" + (par.Count > 0 ? string.Join("&", par) : "")
			);
			if ((int)result.StatusCode == 200)
			{
				string responseString = await result.Content.ReadAsStringAsync();
				List<PartnerGetDTO> dto = JsonConvert.DeserializeObject<List<PartnerGetDTO>>(
					responseString
				);

				return PartnerGetDTO.ToPartnerList(dto);
			}
			else if ((int)result.StatusCode == 204)
			{
				return null;
			}
			else if ((int)result.StatusCode == 400)
			{
				throw new Exception("Nije prosledjen parametar u requestu!");
			}
			else
			{
				throw new Exception("Greska prilikom komunikacije sa partnerom!");
			}
		}

		public static int MaxID()
		{
			throw new Exception("Mora se prebaciti da radi preko TDBrain-a");
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				using (FbCommand cmd = new FbCommand("SELECT MAX(PPID) FROM PARTNER", con))
				{
					using (FbDataReader dr = cmd.ExecuteReader())
						if (dr.Read())
							return dr[0] is DBNull ? 0 : Convert.ToInt32(dr[0]);
				}
			}

			return 0;
		}

		public static async Task<int> Insert(
			string naziv,
			string adresa,
			string posta,
			string telefon,
			string mobilni,
			string fax,
			string email,
			string kontakt,
			string pib,
			Int64? kategorija,
			int aktivan,
			string mestoID,
			string mbroj,
			int opstinaID,
			int drzavaID,
			int refID,
			int pdvo,
			string nazivZaStampu,
			string valuta,
			double dozvoljeniMinus,
			int? imaUgovor,
			int vrstaCenovnika,
			int drzavljanstvoID,
			int zanimanjeID,
			int webStatus,
			int gppID,
			int ceneOdGrupe,
			int? vpcID,
			double? procpc
		)
		{
			var response = await TDBrain_v3.PostAsync(
				"/komercijalno/partner/insert",
				new Dictionary<string, string>()
				{
					{ "naziv", naziv },
					{ "nazivZaStampu", naziv },
					{ "adresa", adresa },
					{ "posta", posta },
					{ "mobilni", mobilni },
					{ "email", email },
					{ "kontakt", kontakt },
					{ "pib", pib },
					{ "kategorija", kategorija.ToString() },
					{ "mestoID", mestoID },
					{ "mbroj", mbroj },
					{ "refID", refID.ToString() },
					{ "zapID", refID.ToString() },
					{ "pdvo", pdvo.ToString() },
					{ "valuta", valuta },
					{ "imaUgovor", imaUgovor.ToString() },
					{ "vrstaCenovnika", vrstaCenovnika.ToString() },
					{ "opstinaID", opstinaID.ToString() },
					{ "zanimanjeID", zanimanjeID.ToString() },
					{ "webStatus", webStatus.ToString() },
					{ "gppID", gppID.ToString() },
					{ "vpCID", vpcID.ToString() },
					{ "propc", procpc.ToString() },
					{ "telefon", telefon },
					{ "fax", fax },
					{ "dozvoljeniMinus", dozvoljeniMinus.ToString() },
					{ "ceneOdGrupe", ceneOdGrupe.ToString() },
					{ "aktivan", aktivan.ToString() },
					{ "drzavljanstvoID", drzavljanstvoID.ToString() }
				}
			);

			if ((int)response.StatusCode == 201)
				return Convert.ToInt32(await response.Content.ReadAsStringAsync());

			if ((int)response.StatusCode == 409)
				throw new Exception(await response.Content.ReadAsStringAsync());

			throw new Exception(
				$"Status Code: {(int)response.StatusCode}\n{await response.Content.ReadAsStringAsync()}"
			);
		}
	}
}
