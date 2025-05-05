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
	public partial class fm_Firme_Nova : Form
	{
		public fm_Firme_Nova()
		{
			InitializeComponent();
		}

		private void kreiraj_btn_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktirajte administratora!");
			//try
			//{
			//    TDOffice.Firma.Insert(naziv_txt.Text, pib_txt.Text, mb_txt.Text, adresa_txt.Text, tekuciRacun_txt.Text, grad_txt.Text);
			//    MessageBox.Show("Firma uspesno kreirana!");
			//    this.Close();
			//}
			//catch(Exceptions.FailedDatabaseInsertException ex)
			//{
			//    MessageBox.Show(ex.Message);
			//}
			//catch
			//{
			//    MessageBox.Show("Doslo je do greske u programu!");
			//}
		}
	}
}
