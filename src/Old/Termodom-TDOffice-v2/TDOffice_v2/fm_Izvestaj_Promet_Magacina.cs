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
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class fm_Izvestaj_Promet_Magacina : Form
	{
		private DateTimePicker dtp { get; set; }
		private Boolean _initializedtp { get; set; }
		private Task<fm_Help> _helpFrom { get; set; }

		private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } =
			Task.Run(() =>
			{
				return Komercijalno
					.Magacin.ListAsync()
					.Result.Where(x =>
						new int[]
						{
							12,
							13,
							14,
							15,
							16,
							17,
							18,
							19,
							20,
							21,
							22,
							23,
							24,
							25,
							26,
							27,
							28
						}.Contains(x.ID)
					)
					.ToList();
			});

		public fm_Izvestaj_Promet_Magacina()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.Promet_Magacina);
			DataTable dt = new DataTable();
			dt.Columns.Add("Godina", typeof(int));
			dt.Columns.Add("KoristiSe", typeof(bool));
			dt.Columns.Add("OdDatuma", typeof(DateTime));
			dt.Columns.Add("DoDatuma", typeof(DateTime));

			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
			{
				DataRow dr = dt.NewRow();
				dr["Godina"] = godina;
				dr["KoristiSe"] = false;
				dr["OdDatuma"] = new DateTime(godina, 1, 1);
				dr["DoDatuma"] = new DateTime(godina, DateTime.Now.Month, DateTime.Now.Day - 1);
				dt.Rows.Add(dr);
			}

			godine_dgv.DataSource = dt;
			godine_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			godine_dgv.Sort(godine_dgv.Columns["Godina"], ListSortDirection.Descending);
			godine_panel.DesniKlik_DatumRangeDGV();
		}

		private void fm_Izvestaj_Promet_Magacina_Load(object sender, EventArgs e)
		{
			NamestiMagacineCLBAsync();
		}

		private Task NamestiMagacineCLBAsync()
		{
			return Task.Run(() =>
			{
				_komercijalnoMagacini.Wait();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							magacini_clb.DataSource = _komercijalnoMagacini.Result;
							magacini_clb.DisplayMember = "Naziv";
							magacini_clb.ValueMember = "ID";
						}
				);
			});
		}

		private void prikaziIzvestaj_btn_Click(object sender, EventArgs e)
		{
			prikaziIzvestaj_btn.Enabled = false;
			btn_PosaljiIzvestaj.Enabled = false;
			Task.Run(() =>
			{
				ConcurrentDictionary<int, Tuple<DateTime, DateTime>> periodi =
					new ConcurrentDictionary<int, Tuple<DateTime, DateTime>>();
				foreach (DataGridViewRow row in godine_dgv.Rows)
					if (Convert.ToBoolean(row.Cells["KoristiSe"].Value))
						periodi[Convert.ToInt32(row.Cells["Godina"].Value)] = new Tuple<
							DateTime,
							DateTime
						>(
							Convert.ToDateTime(row.Cells["OdDatuma"].Value),
							Convert.ToDateTime(row.Cells["DoDatuma"].Value)
						);

				List<int> magacini = magacini_clb
					.CheckedItems.OfType<Komercijalno.Magacin>()
					.Select(x => x.ID)
					.ToList();

				if (periodi.Count == 0)
				{
					MessageBox.Show("Morate izabrati barem jednu godinu!");
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								prikaziIzvestaj_btn.Enabled = true;
								btn_PosaljiIzvestaj.Enabled = true;
							}
					);
					return;
				}

				if (magacini.Count == 0)
				{
					MessageBox.Show("Morate izabrati barem jedan magacin!");
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								prikaziIzvestaj_btn.Enabled = true;
								btn_PosaljiIzvestaj.Enabled = true;
							}
					);
					return;
				}
				DataTable dt = new DataTable();
				foreach (int godina in periodi.Keys.OrderBy(x => x))
				{
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = $"Ucitavanje {godina} ...";
							}
					);
					using (
						FbConnection con = new FbConnection(
							Komercijalno.Komercijalno.CONNECTION_STRING[godina]
						)
					)
					{
						con.Open();
						using (
							FbCommand cmd = new FbCommand(
								@"
SELECT t1.MAGACINID, SUM(t1.vGOD"
									+ godina
									+ @" - t2.vGOD"
									+ godina
									+ @") AS vGOD"
									+ godina
									+ @"
FROM
    (
    SELECT s.MAGACINID, COALESCE(SUM(s.KOLICINA * s.PRODAJNACENA * (100 - s.rabat)/100), 0) AS vGOD"
									+ godina
									+ @"
    FROM STAVKA s
    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
    WHERE d.FLAG = 1 AND d.MAGACINID IN ("
									+ string.Join(", ", magacini)
									+ @") AND (d.VRDOK = 13 OR d.VRDOK = 15) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY s.MAGACINID ORDER BY S.MAGACINID desc
    ) as t1
    LEFT OUTER JOIN
    (
    SELECT s.MAGACINID, COALESCE(SUM(s.KOLICINA * s.PRODAJNACENA * (100 - s.rabat)/100), 0) AS vGOD"
									+ godina
									+ @"
    FROM STAVKA s
    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
    WHERE d.MAGACINID IN ("
									+ string.Join(", ", magacini)
									+ @") AND (d.VRDOK = 22) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY s.MAGACINID ORDER BY S.MAGACINID desc
    ) as t2 on t1.MAGACINID = t2.MAGACINID
    GROUP BY t1.MAGACINID",
								con
							)
						)
						{
							cmd.Parameters.AddWithValue("@ODDATUMA", periodi[godina].Item1);
							cmd.Parameters.AddWithValue("@DODATUMA", periodi[godina].Item2);
							using (FbDataAdapter da = new FbDataAdapter(cmd))
							{
								DataTable tempDT = new DataTable();
								if (dt.Rows.Count == 0)
								{
									da.Fill(dt);
									dt.PrimaryKey = new DataColumn[] { dt.Columns["MAGACINID"] };
									continue;
								}

								da.Fill(tempDT);
								tempDT.PrimaryKey = new DataColumn[]
								{
									tempDT.Columns["MAGACINID"]
								};

								dt.Merge(tempDT, false, MissingSchemaAction.Add);
							}
						}
					}
				}

				DataTable finalDT = dt.Clone();
				finalDT.Columns["MagacinID"].DataType = typeof(string);

				foreach (DataRow r in dt.Rows)
					finalDT.ImportRow(r);

				DataRow dr = finalDT.NewRow();
				dr["MagacinID"] = "SUMMARU";
				foreach (int godina in periodi.Keys.OrderBy(x => x))
					dr["vGOD" + godina] = finalDT.Compute($"Sum(vGOD{godina})", string.Empty);
				finalDT.Rows.Add(dr);

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							int y = 0;

							status_lbl.Text = $"";
							dataGridView1.DataSource = finalDT;
							dataGridView1.Columns["MagacinID"].HeaderText = "Magacin";
							foreach (int godina in periodi.Keys.OrderBy(x => x))
							{
								y++;

								dataGridView1.Columns["vGOD" + godina].DefaultCellStyle.Format =
									"#,##0.00 RSD";
								dataGridView1.Columns["vGOD" + godina].DefaultCellStyle.Alignment =
									DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["vGOD" + godina].Width = 150;
								dataGridView1.Columns["vGOD" + godina].HeaderText =
									$"GOD {godina} [{periodi[godina].Item1.ToString("dd.MM.yyyy")} - {periodi[godina].Item2.ToString("dd.MM.yyyy")}]";
							}
							prikaziIzvestaj_btn.Enabled = true;
							btn_PosaljiIzvestaj.Enabled = true;
						}
				);
			});
		}

		private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < magacini_clb.Items.Count; i++)
				magacini_clb.SetItemChecked(i, true);
		}

		private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < magacini_clb.Items.Count; i++)
				magacini_clb.SetItemChecked(i, false);
		}

		private void btn_Help_Click(object sender, EventArgs e)
		{
			_helpFrom.Result.ShowDialog();
		}

		private void godine_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (
				godine_dgv.Columns[e.ColumnIndex].DataPropertyName == "OdDatuma"
				|| godine_dgv.Columns[e.ColumnIndex].DataPropertyName == "DoDatuma"
			)
			{
				// initialize DateTimePicker
				dtp = new DateTimePicker();
				dtp.Format = DateTimePickerFormat.Short;
				dtp.Visible = true;
				dtp.Value = DateTime.Parse(godine_dgv.CurrentCell.Value.ToString());

				// set size and location
				var rect = godine_dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
				dtp.Size = new Size(rect.Width, rect.Height);
				dtp.Location = new Point(rect.X, rect.Y);

				// attach events
				dtp.CloseUp += new EventHandler(dtp_CloseUp);
				dtp.TextChanged += new EventHandler(dtp_OnTextChange);

				godine_dgv.Controls.Add(dtp);
				_initializedtp = true;
			}
		}

		private void dtp_OnTextChange(object sender, EventArgs e)
		{
			godine_dgv.CurrentCell.Value = dtp.Text.ToString();
		}

		// on close of cell, hide dtp
		void dtp_CloseUp(object sender, EventArgs e)
		{
			dtp.Visible = false;
		}

		private void btn_PosaljiIzvestaj_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count == 0)
			{
				MessageBox.Show("Niste kreirali izvestaj");
				return;
			}

			DataTable dataTable = (DataTable)dataGridView1.DataSource;

			Dictionary<int, Tuple<DateTime, DateTime>> periodi =
				new Dictionary<int, Tuple<DateTime, DateTime>>();
			foreach (DataGridViewRow row in godine_dgv.Rows)
				if (Convert.ToBoolean(row.Cells["KoristiSe"].Value))
					periodi[Convert.ToInt32(row.Cells["Godina"].Value)] = new Tuple<
						DateTime,
						DateTime
					>(
						Convert.ToDateTime(row.Cells["OdDatuma"].Value),
						Convert.ToDateTime(row.Cells["DoDatuma"].Value)
					);

			using (
				fm_Izvestaj_Promet_Magacina_PosaljiKaoIzvestaj pmi =
					new fm_Izvestaj_Promet_Magacina_PosaljiKaoIzvestaj(dataTable, periodi)
			)
				pmi.ShowDialog();
		}

		private void godine_dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (!_initializedtp)
				return;
			dtp.Visible = false;
		}
	}
}
