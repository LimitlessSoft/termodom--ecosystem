using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TDOffice_v2
{
	public partial class fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima : Form
	{
		private DataTable _dataTable { get; set; }
		private Models.Periodi _periodi { get; set; }
		private string _tipIzvestaja { get; set; }
		private List<int> _listaRobe { get; set; } = new List<int>();
		private List<int> _listaGrafikon { get; set; } = new List<int>();
		public bool DozvoliSlanjeIzvestaja
		{
			get { return posaljiKaoIzvestaj_btn.Enabled; }
			set { posaljiKaoIzvestaj_btn.Enabled = value; }
		}
		private Task<fm_Help> _help { get; set; }

		public fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima(
			DataTable dt,
			Models.Periodi periodi,
			string tipIzv,
			List<int> listaRobe
		)
		{
			InitializeComponent();
			_dataTable = dt;
			_periodi = periodi;
			_tipIzvestaja = tipIzv;
			_listaRobe = listaRobe;

			_help = this.InitializeHelpModulAsync(
				Modul.fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima
			);
		}

		private void fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_Load(object sender, EventArgs e)
		{
			DataTable zbirDT = new DataTable();
			zbirDT.Columns.Add("MAGACIN", typeof(string));
			zbirDT.Columns.Add("GRAFIKON", typeof(bool));

			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("KGOD" + godina))
					zbirDT.Columns.Add("ZbirKolicina" + godina, typeof(double));

				if (_dataTable.Columns.Contains("VGOD" + godina))
					zbirDT.Columns.Add("ZbirVrednosti" + godina, typeof(double));
			}

			DataRow zbirDR = zbirDT.NewRow();
			zbirDR["MAGACIN"] = "SUMMARU";

			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("KGOD" + godina))
					zbirDR["ZbirKolicina" + godina] = Convert.ToDouble(
						_dataTable.Compute("Sum(KGOD" + godina + ")", string.Empty)
					);
				if (_dataTable.Columns.Contains("VGOD" + godina))
					zbirDR["ZbirVrednosti" + godina] = Convert.ToDouble(
						_dataTable.Compute("Sum(VGOD" + godina + ")", string.Empty)
					);
			}
			zbirDT.Rows.Add(zbirDR);
			//zbirDT.Columns.Add("GRAFIKON", typeof(bool));

			List<int> magaciniUTabeli = _dataTable
				.AsEnumerable()
				.Select(x => Convert.ToInt32(x["MAGACINID"]))
				.Distinct()
				.ToList();
			foreach (int magacin in magaciniUTabeli)
			{
				zbirDR = zbirDT.NewRow();
				zbirDR["MAGACIN"] = magacin;
				zbirDR["GRAFIKON"] = false;
				foreach (int godina in _periodi.Godine.OrderBy(x => x))
				{
					double zk = 0;
					double zv = 0;
					try
					{
						if (_dataTable.Columns.Contains("KGOD" + godina))
							zk = Convert.ToDouble(
								_dataTable
									.Select("MAGACINID = " + magacin)
									.CopyToDataTable()
									.Compute("Sum(KGOD" + godina + ")", string.Empty)
							);
						if (_dataTable.Columns.Contains("VGOD" + godina))
							zv = Convert.ToDouble(
								_dataTable
									.Select("MAGACINID = " + magacin)
									.CopyToDataTable()
									.Compute("Sum(VGOD" + godina + ")", string.Empty)
							);
					}
					catch { }
					if (_dataTable.Columns.Contains("KGOD" + godina))
						zbirDR["ZbirKolicina" + godina] = zk;
					if (_dataTable.Columns.Contains("VGOD" + godina))
						zbirDR["ZbirVrednosti" + godina] = zv;
				}
				zbirDT.Rows.Add(zbirDR);
			}

			dataGridView1.DataSource = zbirDT;
			dataGridView1.Visible = true;

			dataGridView1.Columns["MAGACIN"].ReadOnly = true;
			dataGridView1.Columns["GRAFIKON"].ReadOnly = false;

			int y = 0;

			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				y++;

				Color c = Color.FromArgb(
					Random.Next(150, 255),
					Random.Next(150, 255),
					Random.Next(150, 255)
				);

				if (_dataTable.Columns.Contains("KGOD" + godina))
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
					dataGridView1.Columns["ZbirKolicina" + godina].ReadOnly = true;
				}

				if (_dataTable.Columns.Contains("VGOD" + godina))
				{
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.Format =
						"#,##0.00 RSD";
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.Alignment =
						DataGridViewContentAlignment.MiddleRight;
					dataGridView1.Columns["ZbirVrednosti" + godina].Width = 200;
					dataGridView1.Columns["ZbirVrednosti" + godina].DisplayIndex =
						!_dataTable.Columns.Contains("KGOD" + godina) ? y : y + _periodi.Count;
					dataGridView1.Columns["ZbirVrednosti" + godina].HeaderText =
						$"ZBIR VREDNOSTI GOD {godina} [{_periodi[godina].OdDatuma.ToString("dd.MM.yyyy")} - {_periodi[godina].DoDatuma.ToString("dd.MM.yyyy")}]";
					dataGridView1.Columns["ZbirVrednosti" + godina].DefaultCellStyle.BackColor = c;
					dataGridView1.Columns["ZbirVrednosti" + godina].ReadOnly = true;
				}
			}
			foreach (int gr in _listaGrafikon)
			{
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					if (Convert.ToInt32(row.Cells["MAGACIN"]) == gr)
					{
						row.Cells["GRAFIKON"].Value = true;
					}
				}
			}

			NamestiKolone();
			OsveziChart();
		}

		private void NamestiKolone()
		{
			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (_dataTable.Columns.Contains("KGOD" + godina))
					dataGridView1.Columns["ZbirKolicina" + godina].Visible =
						prikaziKolicine_cb.Checked;

				if (_dataTable.Columns.Contains("VGOD" + godina))
					dataGridView1.Columns["ZbirVrednosti" + godina].Visible =
						prikaziVrednosti_cb.Checked;
			}
		}

		private void prikaziVrednosti_cb_CheckedChanged(object sender, EventArgs e)
		{
			NamestiKolone();
			OsveziChart();
		}

		private void prikaziKolicine_cb_CheckedChanged(object sender, EventArgs e)
		{
			NamestiKolone();
			OsveziChart();
		}

		private void posaljiKaoIzvestaj_btn_Click(object sender, EventArgs e)
		{
			using (
				fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup st =
					new fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima_PosaljiKaoIzvestaj_Setup(
						_dataTable,
						_periodi,
						_tipIzvestaja
					)
			)
				st.ShowDialog();
		}

		private void help_btn_Click(object sender, EventArgs e)
		{
			_help.Result.ShowDialog();
		}

		private void listRobe_btn_Click(object sender, EventArgs e)
		{
			List<int> nLista = new List<int>();
			;
			using (
				fm_Izvestaj_Prodaja_Roba_Index ind = new fm_Izvestaj_Prodaja_Roba_Index(
					_dataTable,
					_periodi,
					"asd",
					nLista,
					nLista
				)
			)
			{
				ind.DozvoliSlanjeIzvestaja = false;
				ind.DozvoliPrikazZbiraPoMagacinu = false;
				ind.DozvoliPrikazZbiraPoRobi = false;
				ind.ShowDialog();
			}
		}

		private void OsveziChart()
		{
			chart1.Series.Clear();
			chart1.Titles.Clear();
			chart1.Titles.Add("Zbirno po magacinama KOLICINE");
			chart2.Series.Clear();
			chart2.Titles.Clear();
			chart2.Titles.Add("Zbirno po magacinama VREDNOST");
			List<DataRow> drs = new List<DataRow>();
			foreach (DataGridViewRow row in dataGridView1.Rows)
				if (
					!(row.Cells["GRAFIKON"].Value is DBNull)
					&& Convert.ToBoolean(row.Cells["GRAFIKON"].Value) == true
				)
				{
					DataRow dr = ((DataRowView)row.DataBoundItem).Row;
					drs.Add(dr);
				}
			if (drs.Count == 0)
			{
				chart1.DataSource = null;
				chart1.DataBind();
				chart2.DataSource = null;
				chart2.DataBind();
				return;
			}
			DataTable _newDataTable = drs.CopyToDataTable();

			foreach (int godina in _periodi.Godine.OrderBy(x => x))
			{
				if (prikaziKolicine_cb.Checked)
				{
					Series s = new Series();
					chart1.Series.Add(s);
					s.LabelFormat = "#,##0.00";
					s.XValueMember = "MAGACIN";
					s.YValueMembers = "ZbirKolicina" + godina.ToString();
					s.IsValueShownAsLabel = true;
					s.LegendText = "ZbirKolicina " + godina.ToString();
				}

				if (prikaziVrednosti_cb.Checked)
				{
					Series ss = new Series();
					chart2.Series.Add(ss);
					ss.LabelFormat = "#,##0.00 RSD";
					ss.XValueMember = "MAGACIN";
					ss.YValueMembers = "ZbirVrednosti" + godina.ToString();
					ss.IsValueShownAsLabel = true;
					ss.LegendText = "ZbirVrednosti " + godina.ToString();
				}
			}

			chart1.DataSource = _newDataTable;
			chart1.DataBind();
			chart2.DataSource = _newDataTable;
			chart2.DataBind();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OsveziChart();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dataGridView1.Columns["GRAFIKON"].Index)
				dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != dataGridView1.Columns["GRAFIKON"].Index)
				dataGridView1.EndEdit();
			OsveziChart();
		}

		private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e) { }
	}
}
