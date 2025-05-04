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
	public partial class _1365_fm_SastavnicaRastavnica_Pravila_Index : Form
	{
		private TDOffice.DokumentSastavnica _sastavnica { get; set; }
		private TDOffice.StavkaSastavnica _stavkaSastavnica { get; set; }
		private Task<fm_Help> _helpFrom { get; set; }
		private bool _izmena { get; set; } = false;

		public _1365_fm_SastavnicaRastavnica_Pravila_Index(TDOffice.DokumentSastavnica sastavnica)
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.SastavnicaRastavnica_Pravila_Index);
			_sastavnica = sastavnica;
		}

		private void _1365_fm_SastavnicaRastavnica_Pravila_Index_Load(object sender, EventArgs e)
		{
			ReloadSastavnicaPravila();
		}

		private void ReloadSastavnicaPravila()
		{
			List<TDOffice.StavkaSastavnica> stavkeSastavnice =
				TDOffice.StavkaSastavnica.ListBySastavnicaID(_sastavnica.ID);

			List<Komercijalno.Roba> svaRoba = Komercijalno.Roba.List(DateTime.Now.Year);

			// Smanjuje se
			DataTable dt = new DataTable();

			// Povecava se
			DataTable dt1 = new DataTable();

			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("ROBAID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));
			dt.Columns.Add("KatBr", typeof(string));
			dt.Columns.Add("KOLICINA", typeof(double));
			dt.Columns.Add("JM", typeof(string));

			dt1.Columns.Add("ID", typeof(int));
			dt1.Columns.Add("ROBAID", typeof(int));
			dt1.Columns.Add("Naziv", typeof(string));
			dt1.Columns.Add("KatBr", typeof(string));
			dt1.Columns.Add("KOLICINA", typeof(double));
			dt1.Columns.Add("JM", typeof(string));

			foreach (TDOffice.StavkaSastavnica ss in stavkeSastavnice)
			{
				Komercijalno.Roba roba = svaRoba.Where(x => x.ID == ss.RobaID).FirstOrDefault();
				DataRow dr = ss.Kolicina < 0 ? dt.NewRow() : dt1.NewRow();
				dr["ID"] = ss.ID;
				dr["ROBAID"] = ss.RobaID;
				dr["KOLICINA"] = ss.Kolicina;
				dr["Naziv"] = roba == null ? "UNKNOWN" : roba.Naziv;
				dr["KatBr"] = roba == null ? "UNKNOWN" : roba.KatBr;
				dr["JM"] = roba == null ? "UNKNOWN" : roba.JM;

				if (ss.Kolicina < 0)
					dt.Rows.Add(dr);
				else
					dt1.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;

			dataGridView1.Columns["ID"].Visible = false;

			dataGridView1.Columns["RobaID"].Visible = false;

			dataGridView1.Columns["Naziv"].Width = 220;
			dataGridView1.Columns["Naziv"].ReadOnly = true;

			dataGridView1.Columns["KatBr"].Width = 150;
			dataGridView1.Columns["KatBr"].ReadOnly = true;

			dataGridView1.Columns["KOLICINA"].Width = 65;
			dataGridView1.Columns["KOLICINA"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["KOLICINA"].DefaultCellStyle.BackColor = Color.Yellow;

			dataGridView2.DataSource = dt1;

			dataGridView2.Columns["ID"].Visible = false;

			dataGridView2.Columns["RobaID"].Visible = false;

			dataGridView2.Columns["Naziv"].Width = 220;
			dataGridView2.Columns["Naziv"].ReadOnly = true;

			dataGridView2.Columns["KatBr"].Width = 150;
			dataGridView2.Columns["KatBr"].ReadOnly = true;

			dataGridView2.Columns["KOLICINA"].Width = 65;
			dataGridView2.Columns["KOLICINA"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView2.Columns["KOLICINA"].DefaultCellStyle.BackColor = Color.Yellow;

			this.tb_Naziv.Text = _sastavnica.Tag;
			this.tb_IDSastavnice.Text = _sastavnica.ID.ToString();
			_izmena = false;
			button1.Visible = false;
			this.Text =
				"Pravilo Sastavnice [" + _sastavnica.ID.ToString() + "] - " + _sastavnica.Tag;
		}

		private void NapuniDGV1(int rid)
		{
			List<TDOffice.StavkaSastavnica> ss = TDOffice
				.StavkaSastavnica.List()
				.Where(x => x.SastavnicaID == _sastavnica.ID && x.RobaID == rid)
				.ToList();

			if (ss.Count > 0)
				return;

			string rd;
			using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu koju zelite umanjiti!"))
			{
				ib.ShowDialog();
				rd = ib.returnData;
			}

			double kolicina;

			try
			{
				kolicina = Convert.ToDouble(rd);
			}
			catch (Exception)
			{
				MessageBox.Show("Neispravna kolicina!");
				return;
			}

			if (kolicina >= 0)
			{
				MessageBox.Show("Uneta kolicina mora biti negativna");
				return;
			}

			TDOffice.StavkaSastavnica.Insert(_sastavnica.ID, rid, kolicina);

			ReloadSastavnicaPravila();
		}

		private void NapuniDGV2(int rid)
		{
			List<TDOffice.StavkaSastavnica> ssp = TDOffice
				.StavkaSastavnica.List()
				.Where(x => x.SastavnicaID == _sastavnica.ID && x.RobaID == rid)
				.ToList();
			if (ssp.Count > 0)
			{
				return;
			}
			string rd;
			using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu koju zelite povecati!"))
			{
				ib.ShowDialog();
				rd = ib.returnData;
			}
			double kolicina = Convert.ToDouble(rd);
			if (kolicina <= 0)
			{
				MessageBox.Show("Uneta kolicina mora biti pozitivna");
				return;
			}
			TDOffice.StavkaSastavnica.Insert(_sastavnica.ID, rid, kolicina);
			ReloadSastavnicaPravila();
		}

		private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			using (IzborRobe ir = new IzborRobe())
			{
				ir.MagacinID = 50;
				ir.PrikaziKolonuCena = false;
				ir.OnRobaClickHandler += (Komercijalno.RobaUMagacinu[] robaUMagacinu) =>
				{
					foreach (Komercijalno.RobaUMagacinu rum in robaUMagacinu)
					{
						int robaID = rum.RobaID;

						NapuniDGV1(robaID);
					}
				};
				ir.ShowDialog();
			}
		}

		private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			using (IzborRobe ir = new IzborRobe())
			{
				ir.MagacinID = 50;
				ir.PrikaziKolonuCena = false;
				ir.OnRobaClickHandler += (Komercijalno.RobaUMagacinu[] robaUMagacinu) =>
				{
					foreach (Komercijalno.RobaUMagacinu rum in robaUMagacinu)
					{
						int robaID = rum.RobaID;
						NapuniDGV2(robaID);
					}
				};
				ir.ShowDialog();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_sastavnica.Update();
			_izmena = false;
			button1.Visible = false;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			_sastavnica.Tag = this.tb_Naziv.Text;
			this.button1.Visible = true;
			_izmena = true;
		}

		private void _1365_fm_SastavnicaRastavnica_Pravila_Index_FormClosing(
			object sender,
			FormClosingEventArgs e
		)
		{
			if (_izmena)
				if (
					MessageBox.Show(
						"Imate nesnimljenih izmena, da li ih zelite snimiti?",
						"Izmene",
						MessageBoxButtons.YesNo
					) == DialogResult.Yes
				)
					button1.PerformClick();
		}

		private void brisiSelektovanuStavkuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);
			TDOffice.StavkaSastavnica.Delete(id);
			ReloadSastavnicaPravila();
		}

		private void brisiSelektovanuStavkuToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(
				dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells["ID"].Value
			);
			TDOffice.StavkaSastavnica.Delete(id);
			ReloadSastavnicaPravila();
		}

		private void dataGridView1_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		)
		{
			if (dataGridView1.Columns[e.ColumnIndex].Name == "KOLICINA")
			{
				int id = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);
				_stavkaSastavnica = TDOffice.StavkaSastavnica.Get(id);

				double novaKolicina = Convert.ToDouble(e.FormattedValue);
				if (novaKolicina >= 0)
				{
					MessageBox.Show("Uneta kolicina mora biti negativna");
					e.Cancel = true;
					return;
				}
				_stavkaSastavnica.Kolicina = novaKolicina;
				_stavkaSastavnica.Update();
			}
		}

		private void dataGridView2_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		)
		{
			if (dataGridView2.Columns[e.ColumnIndex].Name == "KOLICINA")
			{
				int id = Convert.ToInt32(
					dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells["ID"].Value
				);

				_stavkaSastavnica = TDOffice.StavkaSastavnica.Get(id);

				double novaKolicina = Convert.ToDouble(e.FormattedValue);

				if (novaKolicina <= 0)
				{
					MessageBox.Show("Uneta kolicina mora biti pozitivna");
					e.Cancel = true;
					return;
				}
				_stavkaSastavnica.Kolicina = novaKolicina;
				_stavkaSastavnica.Update();
			}
		}
	}
}
