using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Dtos.TDApi;

namespace TDOffice_v2.Forms.Menadzment
{
	public partial class fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama : Form
	{
		private class PripremaDokumenataRequest
		{
			public int MagacinId { get; set; }
			public int VrDok { get; set; }
			public DateTime OdDatuma { get; set; }
			public DateTime DoDatuma { get; set; }
			public int Namena { get; set; }
			public int NacinPlacanja { get; set; }
		}

		private class RazduziMagacinRequest
		{
			public PripremaDokumenataRequest Izvor { get; set; }
			public bool NoviDokument { get; set; }
			public int DestinacijaVrDok { get; set; }
			public int? DestinacijaBrDok { get; set; }
			public int? DestinacijaMagacinId { get; set; }
			public int? DestinacijaNacinPlacanja { get; set; }
			public int? DestinacijaNamena { get; set; }
			public int? DestinacijaReferent { get; set; }
			public int? DestinacijaZaposleni { get; set; }
			public int NakonAkcijePostaviIzvornimNacinPlacanjaNa { get; set; }
		}

		private class MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto
		{
			public int UkupanBrojDokumenata { get; set; }
			public decimal ZbirnaVrednostDokumenataSaCenamaNaDanasnjiDan { get; set; }
		}

		public fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama()
		{
			InitializeComponent();
		}

		private void fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama_Load(object sender, EventArgs e)
		{
			this.Enabled = false;
			UcitajUIAsync();
		}

		private async Task UcitajUIAsync()
		{
			var naciniPlacanja = new List<NacinPlacanjaDto>()
			{
				new NacinPlacanjaDto() { Id = 1, Naziv = "Virmanom" },
				new NacinPlacanjaDto() { Id = 5, Naziv = "Gotovinom" },
				new NacinPlacanjaDto() { Id = 18, Naziv = "Utovar (priveriti - 18)" },
			};

			try
			{
				var magaciniTask = TDAPI.GetAsync<List<MagacinDto>>("/magacini");
				var vrsteDokumenataTask = TDAPI.GetAsync<List<VrstaDokumentaDto>>(
					"/vrste-dokumenata"
				);
				var naciniPlacanjaTask = TDAPI.GetAsync<List<NacinPlacanjaDto>>("/nacini-placanja");
				var nameneTask = TDAPI.GetAsync<List<NamenaDto>>("/namene-dokumenta");

				izvorMagacin_cmb.DataSource = new List<MagacinDto>((await magaciniTask).Payload);
				izvorMagacin_cmb.DisplayMember = "Naziv";
				izvorMagacin_cmb.ValueMember = "MagacinId";

				destinacijaNoviDokumentMagacin_cmb.DataSource = new List<MagacinDto>(
					(await magaciniTask).Payload
				);
				destinacijaNoviDokumentMagacin_cmb.DisplayMember = "Naziv";
				destinacijaNoviDokumentMagacin_cmb.ValueMember = "MagacinId";

				izvorVrDok_cmb.DataSource = new List<VrstaDokumentaDto>(
					(await vrsteDokumenataTask).Payload
				);
				izvorVrDok_cmb.DisplayMember = "Naziv";
				izvorVrDok_cmb.ValueMember = "Id";

				destinacijaVrDok_cmb.DataSource = new List<VrstaDokumentaDto>(
					(await vrsteDokumenataTask).Payload
				);
				destinacijaVrDok_cmb.DisplayMember = "Naziv";
				destinacijaVrDok_cmb.ValueMember = "Id";

				izvorNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>(naciniPlacanja);
				//izvorNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>((await naciniPlacanjaTask).Payload);
				izvorNacinPlacanja_cmb.DisplayMember = "Naziv";
				izvorNacinPlacanja_cmb.ValueMember = "Id";

				//destinacijaNoviDokumentNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>((await naciniPlacanjaTask).Payload);
				destinacijaNoviDokumentNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>(
					naciniPlacanja
				);
				destinacijaNoviDokumentNacinPlacanja_cmb.DisplayMember = "Naziv";
				destinacijaNoviDokumentNacinPlacanja_cmb.ValueMember = "Id";

				//svimIzvornimDokumentimaPromeniNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>((await naciniPlacanjaTask).Payload);
				svimIzvornimDokumentimaPromeniNacinPlacanja_cmb.DataSource =
					new List<NacinPlacanjaDto>(naciniPlacanja);
				svimIzvornimDokumentimaPromeniNacinPlacanja_cmb.DisplayMember = "Naziv";
				svimIzvornimDokumentimaPromeniNacinPlacanja_cmb.ValueMember = "Id";

				izvorNamena_cmb.DataSource = new List<NamenaDto>((await nameneTask).Payload);
				izvorNamena_cmb.DisplayMember = "Naziv";
				izvorNamena_cmb.ValueMember = "Id";

				destinacijaNoviDokumentNamena_cmb.DataSource = new List<NamenaDto>(
					(await nameneTask).Payload
				);
				destinacijaNoviDokumentNamena_cmb.DisplayMember = "Naziv";
				destinacijaNoviDokumentNamena_cmb.ValueMember = "Id";

				this.Enabled = true;
				napuniPostojeciDokument_rb.Checked = true;
				destinacijaNoviDokument_gb.Enabled = false;
				destinacijaPostojeciDokument_gb.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				this.Enabled = true;
			}
		}

		private void pripremiDokumente_btn_Click(object sender, EventArgs e)
		{
			_ = PripremiDokumenteAsync();
		}

		private async Task PripremiDokumenteAsync()
		{
			try
			{
				this.Enabled = false;
				var response = await TDAPI.GetAsync<
					PripremaDokumenataRequest,
					MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto
				>(
					"/menadzment-razduzenje-magacina-po-otpremnicama/priprema-dokumenata",
					new PripremaDokumenataRequest()
					{
						OdDatuma = odDatuma_dtp.Value,
						DoDatuma = doDatuma_dtp.Value,
						MagacinId = (int)izvorMagacin_cmb.SelectedValue,
						NacinPlacanja = (int)izvorNacinPlacanja_cmb.SelectedValue,
						Namena = (int)izvorNamena_cmb.SelectedValue,
						VrDok = (int)izvorVrDok_cmb.SelectedValue
					}
				);

				if (response.Status == System.Net.HttpStatusCode.BadRequest)
				{
					MessageBox.Show("Bad request");
					foreach (string msg in response.Errors)
						MessageBox.Show(msg);

					this.Enabled = true;
					return;
				}

				if (response.Status != System.Net.HttpStatusCode.OK)
				{
					MessageBox.Show("Greska komunikacije sa API!");
					this.Enabled = true;
					return;
				}

				zbirnaVrednostDokumenataNaDanasnjiDan_txt.Text =
					response.Payload.ZbirnaVrednostDokumenataSaCenamaNaDanasnjiDan.ToString(
						"#,##0.00 RSD"
					);
				this.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				this.Enabled = true;
			}
		}

		private void napuniNoviDokument_rb_CheckedChanged(object sender, EventArgs e)
		{
			destinacijaNoviDokument_gb.Enabled = false;
			destinacijaPostojeciDokument_gb.Enabled = true;
		}

		private void kreirajNoviDokument_rb_CheckedChanged(object sender, EventArgs e)
		{
			MessageBox.Show("Jos uvek nije implementirano!");
			return;
			destinacijaNoviDokument_gb.Enabled = true;
			destinacijaPostojeciDokument_gb.Enabled = false;
		}

		private void izvrsiRazduzenje_btn_Click(object sender, EventArgs e)
		{
			_ = IzvrsiRazduzenje();
		}

		private async Task IzvrsiRazduzenje()
		{
			try
			{
				this.Enabled = false;
				var response = await TDAPI.PostRawAsync<RazduziMagacinRequest>(
					"/menadzment-razduzenje-magacina-po-otpremnicama/razduzi-magacin",
					new RazduziMagacinRequest()
					{
						Izvor = new PripremaDokumenataRequest()
						{
							OdDatuma = odDatuma_dtp.Value,
							DoDatuma = doDatuma_dtp.Value,
							MagacinId = (int)izvorMagacin_cmb.SelectedValue,
							NacinPlacanja = (int)izvorNacinPlacanja_cmb.SelectedValue,
							Namena = (int)izvorNamena_cmb.SelectedValue,
							VrDok = (int)izvorVrDok_cmb.SelectedValue
						},
						NoviDokument = false,
						DestinacijaBrDok = Convert.ToInt32(
							destinacijPostojeciDokumentBrDok_txt.Text
						),
						NakonAkcijePostaviIzvornimNacinPlacanjaNa = (int)
							svimIzvornimDokumentimaPromeniNacinPlacanja_cmb.SelectedValue,
						DestinacijaVrDok = Convert.ToInt32(destinacijaVrDok_cmb.SelectedValue),
					}
				);

				if (response.NotOk)
					MessageBox.Show("Greska prilikom razduzivanja magacina!");
				else
					MessageBox.Show("Magacin uspesno razduzen!");
				this.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				this.Enabled = true;
			}
		}
	}
}
