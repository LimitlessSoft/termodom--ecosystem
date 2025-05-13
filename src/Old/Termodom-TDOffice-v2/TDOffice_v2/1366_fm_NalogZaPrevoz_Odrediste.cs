using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class _1366_fm_NalogZaPrevoz_Odrediste : Form
	{
		private TDOffice.NalogZaPrevoz.Destinacija _destinacija { get; set; }
		private TDOffice.NalogZaPrevoz.Destinacija _originalDestinacija { get; set; }
		private TDOffice.NalogZaPrevoz _nalogZaPrevoz { get; set; }
		private Komercijalno.Dokument dokument { get; set; } = null;

		private bool _loaded = false;
		private bool _izmena
		{
			get { return __izmena; }
			set
			{
				btn_Sacuvaj.Enabled = _nalogZaPrevoz.Status == 0 ? value : false;
				btn_OdbaciIzmene.Enabled = _nalogZaPrevoz.Status == 0 ? value : false;

				__izmena = value;
			}
		}
		private bool __izmena = false;

		/// <summary>
		///
		/// </summary>
		public static List<int> uslugaIDPrevoza = new List<int>()
		{
			1081,
			1082,
			1083,
			1084,
			1085,
			1492,
			1513,
			1514,
			1663,
			1687,
			1911,
			1924,
			4657
		};

		public TDOffice.NalogZaPrevoz.Destinacija Destinacija
		{
			get { return _destinacija; }
		}

		public _1366_fm_NalogZaPrevoz_Odrediste(
			TDOffice.NalogZaPrevoz.Destinacija destinacija,
			TDOffice.NalogZaPrevoz nalogZaPrevoz
		)
		{
			InitializeComponent();

			_originalDestinacija = destinacija;
			_destinacija = destinacija;
			_nalogZaPrevoz = nalogZaPrevoz;

			SetUI();
		}

		private void SetUI()
		{
			_izmena = false;

			miKupcuNaplatili_lbl.Text = "";

			Tuple<int, string>[] knList1 = new Tuple<int, string>[2]
			{
				new Tuple<int, string>(1, "MP Racun"),
				new Tuple<int, string>(0, "Drugi osnov")
			};

			veza_cmb.DataSource = knList1;
			veza_cmb.ValueMember = "Item1";
			veza_cmb.DisplayMember = "Item2";
			veza_cmb.SelectedValue = (int)_destinacija.Osnov;

			Tuple<int, string>[] knList = new Tuple<int, string>[2]
			{
				new Tuple<int, string>(0, "Kesom"),
				new Tuple<int, string>(1, "Kroz MP Racun")
			};

			miKupcuNaplatili_cmb.DataSource = knList;
			miKupcuNaplatili_cmb.ValueMember = "Item1";
			miKupcuNaplatili_cmb.DisplayMember = "Item2";
			miKupcuNaplatili_cmb.SelectedValue = (int)_destinacija.NacinPlacanja;

			_destinacija = _originalDestinacija;

			adresa_txt.Text = _destinacija.Adresa;
			adresa_txt.Enabled = _nalogZaPrevoz.Status == 0;

			kontakt_txt.Text = _destinacija.Kontakt;
			kontakt_txt.Enabled = _nalogZaPrevoz.Status == 0;

			miKupcuNaplatili_txt.Text = _destinacija.MiKupcuNaplatili;
			miKupcuNaplatili_txt.Enabled = _nalogZaPrevoz.Status == 0;

			naplatiti_txt.Text = _destinacija.Naplatiti;
			naplatiti_txt.Enabled = _nalogZaPrevoz.Status == 0;

			veza_txt.Text = _destinacija.Veza;
			veza_txt.Enabled = _nalogZaPrevoz.Status == 0;

			btn_Izbrisi.Enabled = _nalogZaPrevoz.Status == 0 ? true : false;
			veza_cmb.Enabled = _nalogZaPrevoz.Status == 0 ? true : false;
			miKupcuNaplatili_cmb.Enabled = _nalogZaPrevoz.Status == 0 ? true : false;
			if ((int)_destinacija.NacinPlacanja == 1)
			{
				try
				{
					List<Komercijalno.Stavka> stavke = Komercijalno
						.Stavka.ListByDokument(
							DateTime.Now.Year,
							15,
							Convert.ToInt32(miKupcuNaplatili_txt.Text)
						)
						.Where(x => x.MagacinID == _nalogZaPrevoz.MagacinID)
						.ToList();

					if (stavke.Count(x => uslugaIDPrevoza.Contains(x.RobaID)) == 0)
					{
						miKupcuNaplatili_lbl.Text =
							"Nije pronadjena stavka prevoza u datom mp racunu";
						return;
					}

					// U MP racunu postoji prevoz

					miKupcuNaplatili_lbl.Text = stavke
						.Where(x => uslugaIDPrevoza.Contains(x.RobaID))
						.Sum(x => x.ProdCenaBP * x.Kolicina)
						.ToString("#,##0.00 RSD + PDV");
				}
				catch (Exception)
				{
					miKupcuNaplatili_lbl.Text = "Nije pronadjena stavka prevoza u datom mp racunu";
					return;
				}
			}
		}

		private void _1366_fm_NalogZaPrevoz_Odrediste_Load(object sender, EventArgs e)
		{
			_loaded = true;
		}

		private void _1366_fm_NalogZaPrevoz_Odrediste_FormClosed(
			object sender,
			FormClosedEventArgs e
		)
		{
			if (_izmena == true)
			{
				if (
					MessageBox.Show(
						"Imate izmene. Zelite  da ih sacuvate?",
						"Izmene na nalogu",
						MessageBoxButtons.YesNo
					) == DialogResult.Yes
				)
				{
					btn_Sacuvaj.PerformClick();
					_izmena = false;
				}
				else
				{
					_izmena = false;
				}
			}
			this.Close();
		}

		private void adresa_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.Adresa = adresa_txt.Text;

			_izmena = true;
		}

		private void veza_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.Veza = veza_txt.Text;

			_izmena = true;
		}

		private void miKupcuNaplatili_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			if ((miKupcuNaplatili_cmb.SelectedItem as Tuple<int, string>).Item1 == 1)
			{
				// Korisnik je slektovao da je prevoz u mp racunu
				try
				{
					List<Komercijalno.Stavka> stavke = Komercijalno
						.Stavka.ListByDokument(
							DateTime.Now.Year,
							15,
							Convert.ToInt32(miKupcuNaplatili_txt.Text)
						)
						.Where(x => x.MagacinID == _nalogZaPrevoz.MagacinID)
						.ToList();

					if (stavke.Count(x => uslugaIDPrevoza.Contains(x.RobaID)) == 0)
					{
						miKupcuNaplatili_lbl.Text =
							"Nije pronadjena stavka prevoza u datom mp racunu";
						return;
					}

					// U MP racunu postoji prevoz

					miKupcuNaplatili_lbl.Text = stavke
						.Where(x => uslugaIDPrevoza.Contains(x.RobaID))
						.Sum(x => x.ProdCenaBP * x.Kolicina)
						.ToString("#,##0.00 RSD + PDV");
				}
				catch (Exception)
				{
					miKupcuNaplatili_lbl.Text = "Nije pronadjena stavka prevoza u datom mp racunu";
					return;
				}
			}

			_destinacija.MiKupcuNaplatili = miKupcuNaplatili_txt.Text;
			_izmena = true;
		}

		private void kontakt_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.Kontakt = kontakt_txt.Text;

			_izmena = true;
		}

		private void naplatiti_txt_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.Naplatiti = naplatiti_txt.Text;

			_izmena = true;
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			if (
				(veza_cmb.SelectedItem as Tuple<int, string>).Item1
				== Convert.ToInt32(TDOffice.Enums.NalogZaPrevozOsnov.MPRacun)
			)
			{
				try
				{
					Komercijalno.Dokument dok = Komercijalno.Dokument.Get(
						_nalogZaPrevoz.Datum.Year,
						15,
						Convert.ToInt32(veza_txt.Text.ToString())
					);

					if (dok == null)
					{
						MessageBox.Show("Dokument veza" + this.veza_txt.Text + " nije pronadjen");
						return;
					}
					if (dok.MagacinID != _nalogZaPrevoz.MagacinID)
					{
						MessageBox.Show(
							"Dokument veza" + this.veza_txt.Text + " ne pripada vasem magacinu"
						);
						return;
					}
					if (dok.Flag != 1)
					{
						MessageBox.Show("Dokument veza" + this.veza_txt.Text + " nije zakljucan");
						return;
					}
				}
				catch (Exception)
				{
					MessageBox.Show("Neispravan MP Racun veza");
					return;
				}
			}

			#region Proveravamo ako je izabran nacin placanja po MP Racunu da li je mp racun ispravan
			if (
				(miKupcuNaplatili_cmb.SelectedItem as Tuple<int, string>).Item1
				== (int)TDOffice.Enums.NalogZaPrevozOdredisteNacinPlacanja.KrozRacun
			)
			{
				// Korisnik je slektovao da je prevoz u mp racunu
				try
				{
					List<Komercijalno.Stavka> stavke = Komercijalno
						.Stavka.ListByDokument(
							DateTime.Now.Year,
							15,
							Convert.ToInt32(miKupcuNaplatili_txt.Text)
						)
						.Where(x => x.MagacinID == _nalogZaPrevoz.MagacinID)
						.ToList();

					if (stavke.Count(x => uslugaIDPrevoza.Contains(x.RobaID)) == 0)
					{
						MessageBox.Show("U datom mp racunu ne postoji ni jedna stavka prevoza!");
						return;
					}

					// U MP racunu postoji prevoz
					// Nastavljam normalno
				}
				catch (Exception)
				{
					MessageBox.Show("Neispravan MP Racun");
					return;
				}
			}
			#endregion

			_nalogZaPrevoz.Tag.Destinacije.Remove(_destinacija);
			_nalogZaPrevoz.Tag.Destinacije.Add(_destinacija);
			_nalogZaPrevoz.Update();

			if (!string.IsNullOrWhiteSpace(kontakt_txt.Text))
				TDOffice.Partner.InsertBrziKontakt(kontakt_txt.Text);

			_izmena = false;
			this.Close();
		}

		private void btn_OdbaciIzmene_Click(object sender, EventArgs e)
		{
			SetUI();
			_izmena = false;
		}

		private void btn_Izbrisi_Click(object sender, EventArgs e)
		{
			_nalogZaPrevoz.Tag.Destinacije.Remove(_destinacija);
			_nalogZaPrevoz.Update();
			_izmena = false;
			this.Close();
		}

		private void veza_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.Osnov = (TDOffice.Enums.NalogZaPrevozOsnov)
				(veza_cmb.SelectedItem as Tuple<int, string>).Item1;
			_izmena = true;
		}

		private void miKupcuNaplatili_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			_destinacija.NacinPlacanja = (TDOffice.Enums.NalogZaPrevozOdredisteNacinPlacanja)
				(miKupcuNaplatili_cmb.SelectedItem as Tuple<int, string>).Item1;
			_izmena = true;
		}
	}
}
