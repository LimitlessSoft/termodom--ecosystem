using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class fm_Zaposleni_Index : Form
	{
		private TDOffice.Zaposleni _zaposleni { get; set; }

		private bool _loaded { get; set; }

		public fm_Zaposleni_Index(TDOffice.Zaposleni zaposleni)
		{
			InitializeComponent();
			_zaposleni = zaposleni;

			txt_Ime.Text = _zaposleni.Ime;
			txt_Prezime.Text = _zaposleni.Prezime;

			ReloadData();
		}

		public void ReloadData()
		{
			firma_txt.Text = "Nema Aktivan Ugovor O Radu!";

			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("PocetakTrajanja", typeof(DateTime));
			dt.Columns.Add("KrajTrajanja", typeof(DateTime));

			List<TDOffice.ZaposleniUgovorORadu> zuor = TDOffice.ZaposleniUgovorORadu.List(
				"ZAPOSLENI_ID = " + _zaposleni.ID.ToString()
			);

			foreach (TDOffice.ZaposleniUgovorORadu z in zuor)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = z.ID;
				dr["PocetakTrajanja"] = z.PocetakTrajanja;
				dr["KrajTrajanja"] = z.KrajTrajanja;
				dt.Rows.Add(dr);
			}
			dataGridView1.DataSource = dt;
			dataGridView1.Columns["PocetakTrajanja"].HeaderText = "Pocetak ugovora";
			dataGridView1.Columns["KrajTrajanja"].HeaderText = "Kraj ugovora";
			dataGridView1.Columns["ID"].HeaderText = "br. ugovora";
			dataGridView1.Columns["ID"].Width = 65;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_zaposleni.Update();
			MessageBox.Show("Izmene uspesno sacuvane!");
		}

		private void txt_Ime_TextChanged(object sender, EventArgs e)
		{
			_zaposleni.Ime = txt_Ime.Text;
		}

		private void txt_Prezime_TextChanged(object sender, EventArgs e)
		{
			_zaposleni.Prezime = txt_Prezime.Text;
		}

		private void fm_Zaposleni_Index_Load(object sender, EventArgs e)
		{
			_loaded = true;
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			using (
				fm_Zaposleni_UgovorOZaposljenju_Novi zu = new fm_Zaposleni_UgovorOZaposljenju_Novi(
					_zaposleni
				)
			)
				zu.ShowDialog();
			ReloadData();
		}

		private void UkloniUgovor_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Sigurno zelite da uklonite ugovor?",
					"Uklanjanje ugovora",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);
			TDOffice.ZaposleniUgovorORadu.Delete(id);
			ReloadData();
		}

		private void btn_ObrisiZaposlenog_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Sigurno zelite da uklonite zaposlenog?",
					"Uklanjanje zaposlenog",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;
			TDOffice.Zaposleni.DeleteAsync(_zaposleni.ID);
			Close();
		}
	}
}
