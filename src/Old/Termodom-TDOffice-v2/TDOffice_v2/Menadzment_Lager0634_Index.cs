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
	public partial class Menadzment_Lager0634_Index : Form
	{
		private Task<TDOffice.Config<Dictionary<int, Dictionary<int, double>>>> _lager =
			TDOffice.Config<Dictionary<int, Dictionary<int, double>>>.GetAsync(
				TDOffice.ConfigParameter.Lager0634
			);
		private Task<List<Komercijalno.Roba>> _robaKomercijalno { get; set; } =
			Komercijalno.Roba.ListAsync(DateTime.Now.Year);
		private bool _izborRobe { get; set; } = false;

		public int MagacinID
		{
			get { return Convert.ToInt32(magacin_cmb.SelectedValue); }
			set
			{
				magacin_cmb.SelectedValue = value;
				UcitajPodatke();
			}
		}

		public delegate void RobaSelectEvent(int robaID, double kolicina);
		public RobaSelectEvent OnRobaSelect;

		public bool IzborRobe
		{
			get { return _izborRobe; }
			set
			{
				ucitaj_btn.Enabled = !value;
				magacin_cmb.Enabled = !value;
				_izborRobe = value;
			}
		}

		public Menadzment_Lager0634_Index()
		{
			InitializeComponent();
		}

		private void Menadzment_Lager0634_Index_Load(object sender, EventArgs e)
		{
			magacin_cmb.ValueMember = "ID";
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
			magacin_cmb.Enabled = true;

			magacin_cmb.SelectedValue = 12;
			UcitajPodatke();
		}

		private void UcitajPodatke()
		{
			int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);

			if (!_lager.Result.Tag.ContainsKey(magacinID))
			{
				MessageBox.Show("Dati magacin nema nijednu stavku na lageru!");

				dataGridView1.DataSource = null;
				return;
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("KatBr", typeof(string));
			dt.Columns.Add("Proizvod", typeof(string));
			dt.Columns.Add("Kolicina", typeof(double));
			dt.Columns.Add("JM", typeof(string));

			foreach (int key in _lager.Result.Tag[magacinID].Keys)
			{
				Komercijalno.Roba r = _robaKomercijalno.Result.FirstOrDefault(x => x.ID == key);

				DataRow dr = dt.NewRow();
				dr["RobaID"] = key;
				dr["KatBr"] = r == null ? "GRESKA" : r.KatBr;
				dr["Proizvod"] = r == null ? "GRESKA" : r.Naziv;
				dr["Kolicina"] = _lager.Result.Tag[magacinID][key];
				dr["JM"] = r == null ? "GRESKA" : r.JM;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;

			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.##";
		}

		private void ucitaj_btn_Click(object sender, EventArgs e)
		{
			UcitajPodatke();
		}

		private void dataGridView1_CellContentDoubleClick(
			object sender,
			DataGridViewCellEventArgs e
		) { }

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (!IzborRobe)
				return;

			int robaID = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value
			);

			using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu"))
			{
				ib.ShowDialog();

				double kol = 0;

				if (!double.TryParse(ib.returnData, out kol) || kol <= 0)
				{
					MessageBox.Show("Neispravna kolicina!");
					return;
				}

				OnRobaSelect(robaID, kol);
			}
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				if (!IzborRobe)
					return;

				int robaID = Convert.ToInt32(
					dataGridView1
						.Rows[dataGridView1.SelectedCells[0].RowIndex]
						.Cells["RobaID"]
						.Value
				);

				using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu"))
				{
					ib.ShowDialog();

					double kol = 0;

					if (!double.TryParse(ib.returnData, out kol) || kol <= 0)
					{
						MessageBox.Show("Neispravna kolicina!");
						return;
					}

					OnRobaSelect(robaID, kol);
				}
			}
		}
	}
}
