using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace TDOffice_v2
{
	public partial class _1366_fm_NalogZaPrevoz_Index : Form
	{
		private TDOffice.NalogZaPrevoz _nalog { get; set; }
		private Task<fm_Help> _helpFrom { get; set; }
		private Task<List<TDOffice.Partner>> _tdOfficePartneri { get; set; } =
			TDOffice.Partner.ListAsync();
		private Task<TDOffice.Config<List<int>>> _tdOfficePrevoznici { get; set; } =
			TDOffice.Config<List<int>>.GetAsync((int)TDOffice.ConfigParameter.ListaIDevaPrevoznika);

		private bool _loaded = false;
		private bool _izmena
		{
			get { return __izmena; }
			set
			{
				btn_Sacuvaj.Enabled = _nalog.Status == 0 ? value : false;
				btn_Sacuvaj.BackColor = value ? Control.DefaultBackColor : Color.DimGray;
				btn_OdbaciIzmene.Enabled = _nalog.Status == 0 ? value : false;
				btn_OdbaciIzmene.BackColor = value ? Control.DefaultBackColor : Color.DimGray;

				__izmena = value;
			}
		}
		private bool __izmena = false;

		public _1366_fm_NalogZaPrevoz_Index(TDOffice.NalogZaPrevoz nalogZaPrevoz)
		{
			InitializeComponent();

			this.magacin_cmb.Enabled = false;

			_helpFrom = this.InitializeHelpModulAsync(Modul.NalogZaPrevoz_Index);
			_nalog = nalogZaPrevoz;

			_izmena = false;

			_ = SetUIAsync()
				.ContinueWith(
					(before) =>
					{
						this.Invoke(
							(MethodInvoker)
								delegate
								{
									ReloadDestinacija();
								}
						);
					}
				);
		}

		private void _1366_fm_NalogZaPrevoz_Index_Load(object sender, EventArgs e)
		{
			List<Tuple<int, string>> prevoznici = new List<Tuple<int, string>>();
			prevoznici.Add(new Tuple<int, string>(0, " <<< Izaberi Prevoznika >>> "));
			foreach (int prevoznikID in _tdOfficePrevoznici.Result.Tag)
				//prevoznici.Add(new Tuple<int, string>(prevoznikID, _tdOfficePartneri.Result.FirstOrDefault(x => x.ID == prevoznikID).Naziv));
				prevoznici.Add(
					new Tuple<int, string>(prevoznikID, TDOffice.Partner.Get(prevoznikID).Naziv)
				);

			prevoznici.Sort((x, y) => x.Item1.CompareTo(y.Item1));

			prevoznik_cmb.ValueMember = "Item1";
			prevoznik_cmb.DisplayMember = "Item2";
			prevoznik_cmb.DataSource = prevoznici;

			prevoznik_cmb.SelectedValue = _nalog.Prevoznik;

			_loaded = true;
		}

		private async Task SetUIAsync()
		{
			this.BackColor = _nalog.Status == 0 ? Color.Green : Color.Red;
			status_btn.BackgroundImage =
				_nalog.Status == 0
					? global::TDOffice_v2.Properties.Resources.key_green
					: global::TDOffice_v2.Properties.Resources.key_red;

			novaDestinacija_btn.Enabled = _nalog.Status == 0;

			btn_Sacuvaj.Enabled = false;
			btn_OdbaciIzmene.Enabled = false;

			magacin_cmb.DataSource = await Komercijalno.Magacin.ListAsync();
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";
			magacin_cmb.SelectedValue = _nalog.MagacinID;
			magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136604)
				? _nalog.Status == 0
				: false;

			this.magacin_cmb.Enabled = true;

			placanje_cmb.SelectedIndex = (int)_nalog.NacinPlacanja;
			placanje_cmb.Enabled = _nalog.Status == 0;

			prevoznik_cmb.SelectedValue = _nalog.Prevoznik;
			prevoznik_cmb.Enabled = _nalog.Status == 0;

			cenaPrevoznikaBezPDV_txt.Text = _nalog.NamaNaplacenPrevozBezPDV.ToStringOrDefault();
			cenaPrevoznikaBezPDV_txt.Enabled = _nalog.Status == 0;

			datum_dtp.Value = _nalog.Datum;
			datum_dtp.Enabled = _nalog.Status == 0;
		}

		private void ReloadDestinacija()
		{
			for (int i = flowLayoutPanel1.Controls.Count - 1; i >= 0; i--)
				if (flowLayoutPanel1.Controls[i] is Button)
					if (flowLayoutPanel1.Controls[i].Name.Contains("destinacija"))
						flowLayoutPanel1.Controls.RemoveAt(i);

			foreach (TDOffice.NalogZaPrevoz.Destinacija dest in _nalog.Tag.Destinacije)
			{
				Button btn = new Button();
				btn.Width = 150;
				btn.Height = 50;
				btn.Text = dest.Adresa;
				btn.Name = "destinacija" + _nalog.Tag.Destinacije.IndexOf(dest) + "_btn";
				btn.Click += (sender, e) =>
				{
					using (
						_1366_fm_NalogZaPrevoz_Odrediste o = new _1366_fm_NalogZaPrevoz_Odrediste(
							dest,
							_nalog
						)
					)
					{
						o.ShowDialog();
						ReloadDestinacija();
					}
				};
				flowLayoutPanel1.Controls.Add(btn);
			}
		}

		private void datum_dtp_ValueChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_nalog.Datum = datum_dtp.Value;
			_izmena = true;
		}

		private void placanje_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_nalog.NacinPlacanja = (TDOffice.NalogZaPrevoz.NacinPlacanjaPrevozniku)
				placanje_cmb.SelectedIndex;
			_izmena = true;
		}

		private void cenaPrevoznikaBezPDV_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_nalog.NamaNaplacenPrevozBezPDV = Convert.ToDouble(cenaPrevoznikaBezPDV_txt.Text);
			_izmena = true;
		}

		private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.magacin_cmb.Enabled == false)
				return;

			_nalog.MagacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
			_izmena = true;
		}

		private void novaDestinacija_btn_Click(object sender, EventArgs e)
		{
			TDOffice.NalogZaPrevoz.Destinacija dest = new TDOffice.NalogZaPrevoz.Destinacija();
			_nalog.Tag.Destinacije.Add(dest);
			_nalog.Update();
			using (
				_1366_fm_NalogZaPrevoz_Odrediste o = new _1366_fm_NalogZaPrevoz_Odrediste(
					dest,
					_nalog
				)
			)
			{
				o.ShowDialog();
				ReloadDestinacija();
			}
		}

		private void status_btn_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(_nalog.Status == 0 ? 136602 : 136603))
			{
				TDOffice.Pravo.NematePravoObavestenje(_nalog.Status == 0 ? 136602 : 136603);
				return;
			}

			if (_nalog.Status == 0)
			{
				// Provera ipsravnosti podataka

				if (_nalog.Prevoznik == 0)
				{
					MessageBox.Show("Polje prevoznik mora biti popunjeno!");
					return;
				}

				foreach (TDOffice.NalogZaPrevoz.Destinacija dest in _nalog.Tag.Destinacije)
				{
					if (string.IsNullOrWhiteSpace(dest.Adresa))
					{
						MessageBox.Show("Adresa u destinaciji nije popunjena!");
						return;
					}

					if (string.IsNullOrWhiteSpace(dest.MiKupcuNaplatili))
					{
						MessageBox.Show("Mi kupcu naplatili u destinaciji nije popunjeno!");
						return;
					}

					if (string.IsNullOrWhiteSpace(dest.Veza))
					{
						MessageBox.Show("Veza u destinaciji nije popunjena!");
						return;
					}
				}
			}

			_nalog.Status = _nalog.Status == 0 ? 1 : 0;
			_nalog.Update();
			SetUIAsync();
			_izmena = false;
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			_nalog.Update();
			_izmena = false;
		}

		private void btn__Click(object sender, EventArgs e)
		{
			_nalog = TDOffice.NalogZaPrevoz.Get(_nalog.ID);
			SetUIAsync();
			ReloadDestinacija();
			_izmena = false;
		}

		private void _1366_fm_NalogZaPrevoz_Index_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (_izmena)
			{
				if (
					MessageBox.Show(
						"Imate izmene. Zelite  da ih sacuvate?",
						"Izmene na nalogu",
						MessageBoxButtons.YesNo
					) == DialogResult.Yes
				)
				{
					_nalog.Update();
					_izmena = false;
				}
			}
		}

		private void btn_Stampaj_Click(object sender, EventArgs e)
		{
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			if (_nalog.Status != 1)
			{
				MessageBox.Show("Nalog mora biti zakljucan!");
				return;
			}

			int leftMargin = 20;
			int y = 30;

			PdfDocument document = new PdfDocument();
			document.Info.Title = "Nalog za prevoz";

			PdfPage page = document.AddPage();

			XGraphics gfx = XGraphics.FromPdfPage(page);

			XFont h1 = new XFont("Arial", 24, XFontStyle.Bold);
			XFont p = new XFont("Arial", 16, XFontStyle.Regular);
			XFont p1 = new XFont("Arial", 14, XFontStyle.Regular);

			gfx.DrawString(
				TDOffice.Partner.Get(_nalog.Prevoznik).Naziv,
				h1,
				XBrushes.Black,
				new Point(leftMargin, y)
			);
			y += 24;
			gfx.DrawString(
				_nalog.NacinPlacanja.ToString(),
				p,
				XBrushes.Black,
				new Point(leftMargin, y)
			);
			y += 16;
			gfx.DrawString(
				"Prevoznik naplatio nama: " + _nalog.NamaNaplacenPrevozBezPDV.ToString("#,##0.00"),
				p,
				XBrushes.Black,
				new Point(leftMargin, y)
			);
			y += 16;
			gfx.DrawString(
				_nalog.Datum.ToString("dd.MM.yyyy"),
				p,
				XBrushes.Black,
				new Point(leftMargin, y)
			);
			y += 42;
			gfx.DrawString(
				"Nalog za prevoz broj: " + _nalog.ID,
				h1,
				XBrushes.Black,
				new Point(leftMargin * 3, y)
			);
			y += 42;

			int oIndex = 1;
			int tabMargin = 130;
			foreach (TDOffice.NalogZaPrevoz.Destinacija dest in _nalog.Tag.Destinacije)
			{
				gfx.DrawString(
					$"Odrediste {oIndex}: ",
					p1,
					XBrushes.Black,
					new Point(leftMargin, y)
				);
				gfx.DrawString(
					dest.Adresa,
					p1,
					XBrushes.Black,
					new Point(leftMargin + tabMargin, y)
				);
				y += 14;

				gfx.DrawString($"Veza: ", p1, XBrushes.Black, new Point(leftMargin, y));
				gfx.DrawString(dest.Veza, p1, XBrushes.Black, new Point(leftMargin + tabMargin, y));
				y += 14;

				List<Komercijalno.Stavka> stavkeDokumenta = Komercijalno.Stavka.ListByDokument(
					DateTime.Now.Year,
					15,
					Convert.ToInt32(dest.MiKupcuNaplatili)
				);
				double miKupcuNaplatiliPrevoz = stavkeDokumenta
					.Where(x => _1366_fm_NalogZaPrevoz_Odrediste.uslugaIDPrevoza.Contains(x.RobaID))
					.Sum(x => x.ProdCenaBP * x.Kolicina);

				gfx.DrawString($"Kupcu naplatili: ", p1, XBrushes.Black, new Point(leftMargin, y));
				gfx.DrawString(
					dest.NacinPlacanja == TDOffice.Enums.NalogZaPrevozOdredisteNacinPlacanja.NaRuke
						? dest.MiKupcuNaplatili
						: miKupcuNaplatiliPrevoz.ToString("#,##0.00 RSD + PDV (MP)"),
					p1,
					XBrushes.Black,
					new Point(leftMargin + tabMargin, y)
				);
				y += 14;

				if (!string.IsNullOrWhiteSpace(dest.Kontakt))
				{
					gfx.DrawString($"Kontakt: ", p1, XBrushes.Black, new Point(leftMargin, y));
					gfx.DrawString(
						dest.Kontakt,
						p1,
						XBrushes.Black,
						new Point(leftMargin + tabMargin, y)
					);
					y += 14;
				}

				if (!string.IsNullOrWhiteSpace(dest.Naplatiti))
				{
					gfx.DrawString($"Naplatiti: ", p1, XBrushes.Black, new Point(leftMargin, y));
					gfx.DrawString(
						dest.Naplatiti,
						p1,
						XBrushes.Black,
						new Point(leftMargin + tabMargin, y)
					);
					y += 14;
				}

				y += 24;
			}

			gfx.DrawLine(
				XPens.Black,
				new Point((int)(page.Width - 220), 100),
				new Point((int)(page.Width - 25), 100)
			);
			gfx.DrawLine(
				XPens.Black,
				new Point((int)(page.Width - 220), 101),
				new Point((int)(page.Width - 25), 101)
			);
			gfx.DrawString(
				$"Referent",
				p1,
				XBrushes.Black,
				new Point((int)(page.Width - 120), 110),
				XStringFormats.Center
			);

			gfx.DrawLine(
				XPens.Black,
				new Point((int)(page.Width - 220), 230),
				new Point((int)(page.Width - 25), 230)
			);
			gfx.DrawLine(
				XPens.Black,
				new Point((int)(page.Width - 220), 231),
				new Point((int)(page.Width - 25), 231)
			);
			gfx.DrawString(
				$"Magacioner",
				p1,
				XBrushes.Black,
				new Point((int)(page.Width - 120), 240),
				XStringFormats.Center
			);

			string fn = Path.Combine(Path.GetTempPath(), "NalogZaPrevoz.pdf");
			document.Save(fn);
			var pr = new Process();
			pr.StartInfo = new ProcessStartInfo(fn) { UseShellExecute = true };
			pr.Start();
		}

		private void upravljajPrevoznicimaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Prevoznik_List pl = new fm_Prevoznik_List())
				pl.ShowDialog();
		}

		private void prevoznik_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_nalog.Prevoznik = Convert.ToInt32(prevoznik_cmb.SelectedValue);
			_izmena = true;
		}
	}
}
