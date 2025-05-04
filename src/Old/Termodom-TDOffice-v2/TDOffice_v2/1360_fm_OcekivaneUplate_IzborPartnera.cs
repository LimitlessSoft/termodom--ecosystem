using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class _1360_fm_OcekivaneUplate_IzborPartnera : Form
	{
		private DataTable _partneri;

		public int returnPPID { get; set; }

		public _1360_fm_OcekivaneUplate_IzborPartnera()
		{
			InitializeComponent();
		}

		private void _1360_fm_OcekivaneUplate_IzborPartnera_Load(object sender, EventArgs e)
		{
			LoadPartnerAsync();
		}

		private async void LoadPartnerAsync()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("PPID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));

			foreach (
				Komercijalno.Partner partneri in (await Komercijalno.Partner.ListAsync()).OrderBy(
					x => x.PPID
				)
			)
			{
				DataRow dr = dt.NewRow();
				dr["PPID"] = partneri.PPID;
				dr["Naziv"] = partneri.Naziv;
				dt.Rows.Add(dr);
			}

			_partneri = dt;
			dataGridView1.DataSource = _partneri;
			dataGridView1.Columns["Naziv"].Width = 250;
		}

		private void FilterDGV()
		{
			string _sqlWhere = "1 = 1 and Naziv like  '%" + this.tb_Pretraga.Text + "%'";
			string _sqlOrder = "PPID ASC";

			DataRow[] rows = _partneri.Select(_sqlWhere, _sqlOrder);

			if (rows != null && rows.Count() > 0)
				dataGridView1.DataSource = rows.CopyToDataTable();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			returnPPID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PPID"].Value);
			this.Close();
		}

		private void tb_Pretraga_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				FilterDGV();
				e.Handled = true;
			}
		}

		private void _1360_fm_OcekivaneUplate_IzborPartnera_FormClosing(
			object sender,
			FormClosingEventArgs e
		) { }
	}
}
