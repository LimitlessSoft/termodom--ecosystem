using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2
{
	public partial class fm_Firme_Index : Form
	{
		private Firma _firma { get; set; }

		private bool _loaded = false;

		public fm_Firme_Index(Firma firma)
		{
			InitializeComponent();
			_firma = firma;
			PopulateData();
			this.Text = "<" + _firma.Naziv + ">";
		}

		private void PopulateData()
		{
			tb_Naziv.Text = _firma.Naziv;
			tb_Adresa.Text = _firma.Adresa;
			tb_Grad.Text = _firma.Grad;
			tb_mb.Text = _firma.MB;
			tb_PIB.Text = _firma.PIB;
			tb_TekuciRacun.Text = _firma.TR;
		}

		private void fm_Firme_Index_Load(object sender, EventArgs e)
		{
			_loaded = true;
		}

		private void tb_Naziv_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.Naziv = tb_Naziv.Text;
		}

		private void tb_Adresa_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.Adresa = tb_Adresa.Text;
		}

		private void tb_PIB_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.PIB = tb_PIB.Text;
		}

		private void tb_mb_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.MB = tb_mb.Text;
		}

		private void tb_TekuciRacun_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.TR = tb_TekuciRacun.Text;
		}

		private void tb_Grad_TextChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;
			_firma.Grad = tb_Grad.Text;
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktiraje administratora!");
			//_firma.Update();
			MessageBox.Show("Uspesno izvrsene izmene", "Izmena podataka firme");
			Close();
		}
	}
}
