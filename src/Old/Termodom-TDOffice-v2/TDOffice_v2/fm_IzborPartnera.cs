using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDOffice_v2.EventHandlers;

namespace TDOffice_v2
{
	public partial class fm_IzborPartnera : Form
	{
		#region Buffer
		private List<Komercijalno.Partner> _partner;

		#endregion
		public delegate void IzborPartneraSelectEventHandler(IzborPartneraSelectArgs args);
		public IzborPartneraSelectEventHandler IzborPartneraSelect;

		/// <summary>
		/// Tabela koja sadrzi sve podatke
		/// </summary>
		private DataTable baseDataTable { get; set; } = null;

		/// <summary>
		/// Tabela koja sadrzi samo prikazane podatke unutar DGV-a
		/// </summary>
		private DataTable dataGridViewDataTable { get; set; } = null;
		public bool DozvoliMultiSelect
		{
			get { return _dozvoliMultiselect; }
			set
			{
				_dozvoliMultiselect = value;
				dataGridView1.MultiSelect = value;
			}
		}
		private bool _dozvoliMultiselect = false;

		public fm_IzborPartnera()
		{
			InitializeComponent();
			SetupDGV();
			SetupUI();
			LoadData();
			dataGridViewDataTable = baseDataTable.Copy();
			UpdateDGV();
		}

		private void SetupDGV()
		{
			DataTable tempData = new DataTable();
			tempData.Columns.Add("PPID", typeof(int));
			tempData.Columns.Add("PIB", typeof(string));
			tempData.Columns.Add("Naziv", typeof(string));
			tempData.Columns.Add("Adresa", typeof(string));
			tempData.Columns.Add("Posta", typeof(string));
			tempData.Columns.Add("Telefon", typeof(string));
			tempData.Columns.Add("Fax", typeof(string));
			tempData.Columns.Add("Mobilni", typeof(string));
			tempData.Columns.Add("Kontakt", typeof(string));

			dataGridView1.DataSource = tempData;
		}

		private void SetupUI()
		{
			foreach (DataColumn col in (dataGridView1.DataSource as DataTable).Columns)
				comboBox1.Items.Add(col.ColumnName);

			comboBox1.SelectedItem = "Naziv";
		}

		private void LoadData()
		{
			_partner = Komercijalno.Partner.ListAsync().Result;
			DataTable temp = (dataGridView1.DataSource as DataTable).Clone();
			foreach (Komercijalno.Partner partneri in _partner)
			{
				DataRow dr = temp.NewRow();
				dr["PPID"] = partneri.PPID;
				dr["PIB"] = partneri.PIB;
				dr["Naziv"] = partneri.Naziv;
				dr["Adresa"] = partneri.Adresa;
				dr["Posta"] = partneri.Posta;
				dr["Telefon"] = partneri.Telefon;
				dr["Fax"] = partneri.Fax;
				dr["Mobilni"] = partneri.Mobilni;
				dr["Kontakt"] = partneri.Kontakt;

				temp.Rows.Add(dr);
			}

			baseDataTable = temp;
		}

		private void UpdateDGV()
		{
			dataGridView1.DataSource = dataGridViewDataTable;

			if (dataGridView1.Rows.Count == 0)
				return;

			dataGridView1.Columns["PPID"].Visible = false;
			dataGridView1.Columns["PIB"].Width = 150;
			dataGridView1.Columns["Naziv"].Width = 150;
			dataGridView1.Columns["Adresa"].Width = 300;
			dataGridView1.Columns["Posta"].Width = 50;
			dataGridView1.Columns["Telefon"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleCenter;
			dataGridView1.Columns["Fax"].Width = 80;
			dataGridView1.Columns["Mobilni"].Width = 80;
			dataGridView1.Columns["Kontakt"].Width = 100;

			slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;

			this.dataGridView1.Sort(
				this.dataGridView1.Columns["Naziv"],
				ListSortDirection.Ascending
			);
		}

		private void FilterCtrlA()
		{
			string selectString = "";
			string input = textBox1.Text;
			string[] inputElemets = input.Split('+');

			foreach (object o in comboBox1.Items)
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

			DataRow[] rows = baseDataTable.Copy().Select(selectString);
			dataGridViewDataTable =
				rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

			UpdateDGV();
		}

		private void FilterEnter()
		{
			dataGridView1.ClearSelection();
			string kolona = comboBox1.SelectedItem.ToString();
			string input = textBox1.Text;

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

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int ppid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PPID"].Value);
			if (IzborPartneraSelect != null)
				IzborPartneraSelect(new IzborPartneraSelectArgs() { PPID = ppid });
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				int ppid = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["PPID"].Value
				);
				if (IzborPartneraSelect != null)
					IzborPartneraSelect(new IzborPartneraSelectArgs() { PPID = ppid });
			}
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				FilterEnter();
				dataGridView1.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				FilterCtrlA();
				dataGridView1.Focus();
			}
		}

		private void btn_ctrlA_Click(object sender, EventArgs e)
		{
			FilterCtrlA();
		}

		private void fm_IzborPartnera_Shown(object sender, EventArgs e)
		{
			textBox1.Focus();
			textBox1.SelectAll();
		}
	}
}
