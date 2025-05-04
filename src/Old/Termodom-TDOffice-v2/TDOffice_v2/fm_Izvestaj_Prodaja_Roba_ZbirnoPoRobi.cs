using System;
using System.Collections.Concurrent;
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
	public partial class fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi : Form
	{
		private DataTable _dataTable { get; set; }
		private Models.Periodi _periodi { get; set; }
		private Task<List<Komercijalno.Roba>> _komercijalnoRoba { get; set; } =
			Komercijalno.Roba.ListAsync(DateTime.Now.Year);
		public bool DozvoliSlanjeIzvestaja
		{
			get { return posaljiKaoIzvestaj_btn.Enabled; }
			set { posaljiKaoIzvestaj_btn.Enabled = value; }
		}
		private Task<fm_Help> _help { get; set; }

		public fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi(DataTable dt, Models.Periodi periodi)
		{
			InitializeComponent();

			_dataTable = dt;
			_periodi = periodi;

			_help = this.InitializeHelpModulAsync(Modul.fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi);
		}

		private void fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi_Load(object sender, EventArgs e)
		{
			DataTable zbirDT = new DataTable();
			zbirDT.Columns.Add("ROBAID", typeof(int));
			zbirDT.Columns.Add("PROIZVOD", typeof(string));
			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("kGOD" + godina))
					zbirDT.Columns.Add("ZbirKolicina" + godina, typeof(double));

				if (_dataTable.Columns.Contains("vGOD" + godina))
					zbirDT.Columns.Add("ZbirVrednosti" + godina, typeof(double));
			}

			DataRow zbirDR = zbirDT.NewRow();
			zbirDR["ROBAID"] = "-1";
			zbirDR["PROIZVOD"] = "SUMMARU";
			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("kGOD" + godina))
					zbirDR["ZbirKolicina" + godina] = Convert.ToDouble(
						_dataTable.Compute("Sum(kGOD" + godina + ")", string.Empty)
					);
				if (_dataTable.Columns.Contains("vGOD" + godina))
					zbirDR["ZbirVrednosti" + godina] = Convert.ToDouble(
						_dataTable.Compute("Sum(vGOD" + godina + ")", string.Empty)
					);
			}
			zbirDT.Rows.Add(zbirDR);

			List<int> robaUTabeli = _dataTable
				.AsEnumerable()
				.Select(x => Convert.ToInt32(x["ROBAID"]))
				.Distinct()
				.ToList();
			foreach (int robaID in robaUTabeli)
			{
				zbirDR = zbirDT.NewRow();
				zbirDR["ROBAID"] = robaID;
				zbirDR["PROIZVOD"] = _komercijalnoRoba
					.Result.FirstOrDefault(x => x.ID == robaID)
					.Naziv;
				foreach (int godina in _periodi.Godine.OrderBy(x => x))
				{
					double zk = 0;
					double zv = 0;
					try
					{
						if (_dataTable.Columns.Contains("kGOD" + godina))
							zk = Convert.ToDouble(
								_dataTable
									.Select("ROBAID = " + robaID)
									.CopyToDataTable()
									.Compute("Sum(kGOD" + godina + ")", string.Empty)
							);
						if (_dataTable.Columns.Contains("vGOD" + godina))
							zv = Convert.ToDouble(
								_dataTable
									.Select("ROBAID = " + robaID)
									.CopyToDataTable()
									.Compute("Sum(vGOD" + godina + ")", string.Empty)
							);
					}
					catch { }
					if (_dataTable.Columns.Contains("kGOD" + godina))
						zbirDR["ZbirKolicina" + godina] = zk;
					if (_dataTable.Columns.Contains("vGOD" + godina))
						zbirDR["ZbirVrednosti" + godina] = zv;
				}
				zbirDT.Rows.Add(zbirDR);
			}

			dataGridView1.DataSource = zbirDT;
			dataGridView1.Visible = true;
			int y = 1;

			dataGridView1.Columns["ROBAID"].Visible = false;
			dataGridView1.Columns["PROIZVOD"].Width = 300;
			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				y++;

				Color c = Color.FromArgb(
					Random.Next(150, 255),
					Random.Next(150, 255),
					Random.Next(150, 255)
				);

				if (_dataTable.Columns.Contains("kGOD" + godina))
				{
					dataGridView1.Columns["ZbirKolicina" + godina].DefaultCellStyle.Format =
						"#,##0";
					dataGridView1.Columns["ZbirKolicina" + godina].DefaultCellStyle.Alignment =
						DataGridViewContentAlignment.MiddleRight;
					dataGridView1.Columns["ZbirKolicina" + godina].Width = 100;
					dataGridView1.Columns["ZbirKolicina" + godina].DisplayIndex = y;
					dataGridView1.Columns["ZbirKolicina" + godina].HeaderText =
						$"ZBIR KOLICINE GOD {godina} [{_periodi[godina].OdDatuma.ToString("dd.MM.yyyy")} - {_periodi[godina].DoDatuma.ToString("dd.MM.yyyy")}]";
					dataGridView1.Columns["ZbirKolicina" + godina].DefaultCellStyle.BackColor = c;
				}

				if (_dataTable.Columns.Contains("vGOD" + godina))
				{
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.Format =
						"#,##0.00 RSD";
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.Alignment =
						DataGridViewContentAlignment.MiddleRight;
					dataGridView1.Columns["ZbirVrednosti" + godina].Width = 200;
					dataGridView1.Columns["ZbirVrednosti" + godina].DisplayIndex =
						!_dataTable.Columns.Contains("kGOD" + godina) ? y : y + _periodi.Count;
					dataGridView1.Columns["ZbirVrednosti" + godina].HeaderText =
						$"ZBIR VREDNOSTI GOD {godina} [{_periodi[godina].OdDatuma.ToString("dd.MM.yyyy")} - {_periodi[godina].DoDatuma.ToString("dd.MM.yyyy")}]";
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.BackColor = c;
				}
			}

			NamestiKolone();
		}

		private void NamestiKolone()
		{
			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("kGOD" + godina))
					dataGridView1.Columns["ZbirKolicina" + godina].Visible =
						prikaziKolicine_cb.Checked;

				if (_dataTable.Columns.Contains("vGOD" + godina))
					dataGridView1.Columns["ZbirVrednosti" + godina].Visible =
						prikaziVrednosti_cb.Checked;
			}
		}

		private void prikaziKolicine_cb_CheckedChanged(object sender, EventArgs e)
		{
			NamestiKolone();
		}

		private void prikaziVrednosti_cb_CheckedChanged(object sender, EventArgs e)
		{
			NamestiKolone();
		}

		private void posaljiKaoIzvestaj_btn_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Nije implementirano!");
		}

		private void help_btn_Click(object sender, EventArgs e)
		{
			_help.Result.ShowDialog();
		}
	}
}
