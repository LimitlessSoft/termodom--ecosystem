using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class fm_Zaposleni_UgovorOZaposljenju_Novi : Form
	{
		private TDOffice.Zaposleni _zaposleni { get; set; }

		//private List<TDOffice.Firma> firmaList = TDOffice.Firma.List();

		public fm_Zaposleni_UgovorOZaposljenju_Novi(TDOffice.Zaposleni zaposleni)
		{
			InitializeComponent();
			_zaposleni = zaposleni;

			//firmaList.Add(new TDOffice.Firma() { ID = -1, Naziv = " < Izaberi firmu > " });
			//firmaList.Sort((x, y) => x.ID.CompareTo(y.ID));

			//cmb_Firma.DisplayMember = "Naziv";
			//cmb_Firma.ValueMember = "ID";
			//cmb_Firma.DataSource = firmaList;
			//cmb_Firma.SelectedValue = -1;

			txt_Ime.Text = _zaposleni.Ime;
			txt_Prezime.Text = _zaposleni.Prezime;
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			int firma = Convert.ToInt32(cmb_Firma.SelectedValue);
			if (firma == -1)
			{
				MessageBox.Show("Niste izabrali FIRMU!!!");
				return;
			}

			List<TDOffice.ZaposleniUgovorORadu> zuorSortedByKrajTrajanjaDesc =
				TDOffice.ZaposleniUgovorORadu.List(
					"ZAPOSLENI_ID = " + _zaposleni.ID.ToString() + " order by KRAJ_TRAJANJA DESC"
				);

			if (dtp_Pocetak.Value.Date >= dtp_Kraj.Value.Date)
			{
				MessageBox.Show("Datum pocetka mora biti pre datuma kraja ugovora!");
				return;
			}

			#region Provera validnosti perioda ugovora

			List<TDOffice.ZaposleniUgovorORadu> ugovoriUKontinuitetu =
				new List<TDOffice.ZaposleniUgovorORadu>();
			TDOffice.ZaposleniUgovorORadu poslednjiUgovor =
				zuorSortedByKrajTrajanjaDesc.FirstOrDefault();
			foreach (TDOffice.ZaposleniUgovorORadu zu in zuorSortedByKrajTrajanjaDesc)
			{
				// ========================================
				// Proverava preklapanje datuma
				// ========================================
				if (
					dtp_Pocetak.Value.Date <= zu.KrajTrajanja.Date
					&& zu.PocetakTrajanja.Date <= dtp_Kraj.Value.Date
				)
				{
					MessageBox.Show(
						"Period trajanja ovog ugovora bi se preklopio sa postojecim ugovorom br."
							+ zu.ID.ToString()
							+ "\nNe mogu kreirati ovaj ugovor!",
						"Preklapanje datuma"
					);
					return;
				}
				// =========================================
				// =========================================
				// =========================================

				// =========================================
				// Pripremam proveru perioda provedenog u istoj firmi
				// =========================================
				if (
					zu.Firma != poslednjiUgovor.Firma
					|| (poslednjiUgovor.PocetakTrajanja.Date - zu.KrajTrajanja.Date).TotalDays > 0
				)
					break;

				ugovoriUKontinuitetu.Add(zu);
				poslednjiUgovor = zu;
				// =========================================
				// =========================================
				// =========================================
			}

			// =========================================
			// Pripremam proveru perioda provedenog u istoj firmi
			// =========================================
			if (
				(dtp_Kraj.Value.Date - dtp_Pocetak.Value.Date).TotalDays
					+ ugovoriUKontinuitetu.Sum(x =>
						(x.KrajTrajanja.Date - x.PocetakTrajanja.Date).TotalDays
					)
				> 730
			)
			{
				if (
					MessageBox.Show(
						"Zaposleni je vec imao ugvor u ovoj firmi i sa ovim ugovorom ce mu ukupno vreme preci 2 godine.\nDa li zelite nastaviti?",
						"Provera kontinuiteta ugovora",
						MessageBoxButtons.YesNo
					) != DialogResult.Yes
				)
					return;
			}
			// =========================================
			// =========================================
			// =========================================

			#endregion

			TDOffice.ZaposleniUgovorORadu.Insert(
				firma,
				_zaposleni.ID,
				dtp_Pocetak.Value,
				dtp_Kraj.Value
			);
			MessageBox.Show(
				$"Uspesno kreiran novi Ugovor o radu za zaposlenog {_zaposleni.ToString()}"
			);

			this.Close();
		}
	}
}
