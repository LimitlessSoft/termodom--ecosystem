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
	public partial class fm_Help : Form
	{
		private TDOffice.Modul _modul { get; set; }

		public fm_Help(TDOffice.Modul modul)
		{
			InitializeComponent();
			_modul = modul;
			UcitajModul();
		}

		public static fm_Help Generate(TDOffice.Modul modul)
		{
			return new fm_Help(modul);
		}

		public static Task<fm_Help> GenerateAsync(TDOffice.Modul modul)
		{
			return Task.Run(() =>
			{
				return Generate(modul);
			});
		}

		private void UcitajModul()
		{
			rtb_Komentar.Text = _modul.Komentar;
			rtb_InterniKomentar.Text = _modul.InterniKomentar;
		}

		private void rtb_Komentar_TextChanged(object sender, EventArgs e)
		{
			odustaniOdCuvanjaHelpa_btn.Show();
			sacuvajHelp_btn.Show();
		}

		private void rtb_InterniKomentar_TextChanged(object sender, EventArgs e)
		{
			odustaniOdCuvanjaHelpa_btn.Show();
			sacuvajHelp_btn.Show();
		}

		private void sacuvajHelp_btn_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Da li sigurno zelite da sacuvate uradjene izmene?",
					"Potvrdi",
					MessageBoxButtons.YesNo
				) == DialogResult.No
			)
			{
				UcitajModul();
				odustaniOdCuvanjaHelpa_btn.Hide();
				sacuvajHelp_btn.Hide();
				return;
			}

			_modul.InterniKomentar = rtb_InterniKomentar.Text;
			_modul.Komentar = rtb_Komentar.Text;
			_modul.Update();
			UcitajModul();
			odustaniOdCuvanjaHelpa_btn.Hide();
			sacuvajHelp_btn.Hide();
		}

		private void odustaniOdCuvanjaHelpa_btn_Click(object sender, EventArgs e)
		{
			UcitajModul();
			odustaniOdCuvanjaHelpa_btn.Hide();
			sacuvajHelp_btn.Hide();
		}

		private void fm_Help_FormClosed(object sender, FormClosedEventArgs e) { }

		private void fm_Help_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (odustaniOdCuvanjaHelpa_btn.Visible)
			{
				if (
					MessageBox.Show(
						"Imate izmene, zelite li da ih sacuvate ?",
						"Potvrdi",
						MessageBoxButtons.YesNo
					) == DialogResult.No
				)
				{
					UcitajModul();
					odustaniOdCuvanjaHelpa_btn.Hide();
					sacuvajHelp_btn.Hide();
					return;
				}

				_modul.InterniKomentar = rtb_InterniKomentar.Text;
				_modul.Komentar = rtb_Komentar.Text;
				_modul.Update();
				UcitajModul();
				odustaniOdCuvanjaHelpa_btn.Hide();
				sacuvajHelp_btn.Hide();
			}
			e.Cancel = true;
			this.Hide();
		}
	}
}
