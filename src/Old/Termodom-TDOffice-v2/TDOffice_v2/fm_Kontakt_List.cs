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
	public partial class fm_Kontakt_List : Form
	{
		public int RND = Random.Next(1, 1000);
		private Task<List<TDOffice.Partner>> _partneri { get; set; } = TDOffice.Partner.ListAsync();
		private DataTable _dt { get; set; } = new DataTable();

		public fm_Kontakt_List()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133700))
			{
				TDOffice.Pravo.NematePravoObavestenje(133700);
				this.Close();
				return;
			}
			InitializeComponent();

			_dt.Columns.Add("ID", typeof(int));
			_dt.Columns.Add("Naziv", typeof(string));
			_dt.Columns.Add("Mobilni", typeof(string));
			_dt.Columns.Add("SMSBlok", typeof(int));
			dataGridView1.DataSource = _dt;

			foreach (DataColumn col in _dt.Columns)
				cmb_PoljePretrage.Items.Add(col.ColumnName);

			cmb_PoljePretrage.SelectedItem = "Naziv";
		}

		private void fm_Kontakt_List_Load(object sender, EventArgs e)
		{
			PopuniDGV();
		}

		private void PopuniDGV()
		{
			_dt.Clear();
			dataGridView1.SuspendLayout();
			foreach (TDOffice.Partner p in _partneri.Result)
			{
				DataRow dr = _dt.NewRow();
				dr["ID"] = p.ID;
				dr["Naziv"] = p.Naziv;
				dr["Mobilni"] = p.Mobilni;
				dr["SMSBlok"] = p.SMSBlok ? 1 : 0;
				_dt.Rows.Add(dr);
			}

			for (int i = 0; i < dataGridView1.Rows.Count; i++)
				if (Convert.ToInt32(dataGridView1.Rows[i].Cells["SMSBlok"].Value) == 1)
					dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;

			dataGridView1.ResumeLayout();
			slogova_lbl.Text = "Slogova: " + _dt.Rows.Count.ToString();
		}

		private void PretragaEnter()
		{
			dataGridView1.ClearSelection();
			string kolona = cmb_PoljePretrage.SelectedItem.ToString();
			string input = txt_Pretraga.Text;

			if (string.IsNullOrWhiteSpace(input))
			{
				dataGridView1.FirstDisplayedScrollingRowIndex = 0;
				dataGridView1.Rows[0].Selected = true;
				dataGridView1.Focus();
				dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Naziv"];
				return;
			}

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				string vrednostCelije = row.Cells[kolona].Value.ToString();
				if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
				{
					dataGridView1.FirstDisplayedScrollingRowIndex =
						row.Index > 0 ? row.Index - 1 : 0;
					dataGridView1.Rows[row.Index].Selected = true;
					dataGridView1.Focus();
					dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells["Naziv"];
					return;
				}
			}
		}

		private void PretragaCtrlA()
		{
			string selectString = "";
			string input = txt_Pretraga.Text;
			string[] inputElemets = input.Split('+');

			foreach (object o in cmb_PoljePretrage.Items)
			{
				for (int i = 0; i < inputElemets.Length; i++)
					selectString +=
						"CONVERT("
						+ o.ToString()
						+ ", System.String) LIKE '%"
						+ inputElemets[i]
						+ "%' AND ";

				selectString = selectString.Remove(selectString.Length - 4);
				selectString += " OR ";
			}

			selectString = selectString.Remove(selectString.Length - 4);
			DataTable dataTable = _dt;
			DataRow[] rows = dataTable.Copy().Select(selectString);
			dataTable = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

			dataGridView1.DataSource = dataTable;
			slogova_lbl.Text = "Slogova: " + rows.Count().ToString();
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			using (fm_Kontakt_Novi kn = new fm_Kontakt_Novi())
				kn.ShowDialog();

			_partneri = TDOffice.Partner.ListAsync();
			Task.WaitAll(_partneri);
			PopuniDGV();
		}

		private void blokirani_btn_Click(object sender, EventArgs e)
		{
			List<TDOffice.Partner> par = TDOffice
				.Partner.List()
				.Where(x => x.SMSBlok == true)
				.ToList();
			if (par.Count <= 0)
			{
				MessageBox.Show("Nema blokiranih kontakata");
				return;
			}
			using (
				DataGridViewSelectBox sb = new DataGridViewSelectBox(
					_dt.Copy().Select("SMSBlok = 1").AsEnumerable().CopyToDataTable()
				)
			)
			{
				sb.ShowDialog();
			}
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) { }

		private void blokirajToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133701))
			{
				TDOffice.Pravo.NematePravoObavestenje(133701);
				return;
			}
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);

			TDOffice.Partner par = TDOffice.Partner.Get(id);
			par.SMSBlok = true;
			par.Update();

			MessageBox.Show("Kontakt uspesno blokiran!");
			_partneri = TDOffice.Partner.ListAsync();
			Task.WaitAll(_partneri);
			PopuniDGV();
		}

		private void odBlokirajToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);

			TDOffice.Partner par = TDOffice.Partner.Get(id);
			par.SMSBlok = false;
			par.Update();

			MessageBox.Show("Kontakt uspesno odblokiran!");
			_partneri = TDOffice.Partner.ListAsync();
			Task.WaitAll(_partneri);
			PopuniDGV();
		}

		private void txt_Pretraga_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				PretragaEnter();
				dataGridView1.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				PretragaCtrlA();
				dataGridView1.Focus();
			}
		}
	}
}
