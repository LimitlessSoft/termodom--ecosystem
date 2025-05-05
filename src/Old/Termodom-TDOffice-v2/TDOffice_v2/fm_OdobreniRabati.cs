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
	public partial class fm_OdobreniRabati : Form
	{
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
		private List<int> _godine { get; set; } =
			new List<int>() { 2022, 2021, 2020, 2019, 2018, 2017 };
		private Task<fm_Help> _helpFrom { get; set; }

		public fm_OdobreniRabati()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.OdobreniRabat);
			SetDGV();
			datum_panel.DesniKlik_DatumRange(null);
			doDatuma_dtp.Value = new DateTime(
				DateTime.Now.Year,
				DateTime.Now.Month,
				DateTime.Now.Day - 1
			);
			odDatuma_dtp.Value = new DateTime(
				DateTime.Now.Year,
				DateTime.Now.Month,
				DateTime.Now.Day - 1
			);
			cmb_Godine.DataSource = _godine;
		}

		private void fm_OdobreniRabati_Load(object sender, EventArgs e)
		{
			NamestiMagacineCLBAsync();
		}

		private void SetDGV()
		{
			DataTable _dt = new DataTable();
			_dt.Columns.Add("UKUPANPROMET", typeof(double));
			_dt.Columns.Add("UKUPANRABAT", typeof(double));
			_dt.Columns.Add("MAGACINID", typeof(int));
			_dt.Columns.Add("PROCENAT", typeof(double));

			dataGridView1.DataSource = _dt;
			dataGridView1.Columns["UKUPANPROMET"].DefaultCellStyle.Format = "#,##0.00 RSD";
			dataGridView1.Columns["UKUPANPROMET"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["UKUPANPROMET"].Width = 150;

			dataGridView1.Columns["UKUPANRABAT"].DefaultCellStyle.Format = "#,##0.00 RSD";
			dataGridView1.Columns["UKUPANRABAT"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["UKUPANRABAT"].Width = 150;

			dataGridView1.Columns["PROCENAT"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["PROCENAT"].Width = 70;

			dataGridView1.Columns["MAGACINID"].Width = 70;
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

		private void btn_Prikazi_Click(object sender, EventArgs e)
		{
			/// Onemogucavam UI
			btn_Prikazi.Enabled = false;

			/// Uzimam selektovanu godinu
			int godina = Convert.ToInt32(cmb_Godine.SelectedValue);

			/// Hvatam listu magacina iz checklistbox-a
			List<int> magacini = magacini_clb
				.CheckedItems.OfType<Komercijalno.Magacin>()
				.Select(x => x.ID)
				.ToList();

			if (magacini.Count == 0)
			{
				MessageBox.Show("Morate izabrati barem jedan magacin!");
				btn_Prikazi.Enabled = true;
				return;
			}

			DataTable odobreniRabatiRezultat = Models.Izvestaj.OdobreniRabati(
				magacini,
				godina,
				odDatuma_dtp.Value,
				doDatuma_dtp.Value
			);
			dataGridView1.DataSource = odobreniRabatiRezultat;
			btn_Prikazi.Enabled = true;
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

		private void cmb_Godine_SelectedIndexChanged(object sender, EventArgs e)
		{
			int godina = Convert.ToInt32(cmb_Godine.SelectedValue);
			DateTime dtdo = doDatuma_dtp.Value;
			this.doDatuma_dtp.Value = new DateTime(godina, dtdo.Month, dtdo.Day);
			DateTime dtod = odDatuma_dtp.Value;
			this.odDatuma_dtp.Value = new DateTime(godina, dtod.Month, dtod.Day);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count == 0)
			{
				MessageBox.Show("Niste kreirali izvestaj");
				return;
			}

			DataTable dataTable = (DataTable)dataGridView1.DataSource;
			using (
				fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj or =
					new fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj(dataTable)
			)
				or.ShowDialog();
		}
	}
}
