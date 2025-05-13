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
	public partial class fm_PrikazIzvestajPrometMagacina : Form
	{
		private DataTable _dataTable { get; set; }
		private Task<fm_Help> _helpFrom { get; set; }

		public fm_PrikazIzvestajPrometMagacina(DataTable dataTable)
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.fm_PrikazIzvestajPrometMagacina);
			_dataTable = dataTable;
			dataGridView1.DataSource = _dataTable;
			setDGV();
		}

		private void setDGV()
		{
			for (int j = 1; j < dataGridView1.Columns.Count; ++j)
			{
				dataGridView1.Columns[j].DefaultCellStyle.Format = "#,##0.00 RSD";
				dataGridView1.Columns[j].DefaultCellStyle.Alignment =
					DataGridViewContentAlignment.MiddleRight;
				dataGridView1.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
			this.Text = "Promet magacina ";
		}

		private void btn_Help_Click(object sender, EventArgs e)
		{
			_helpFrom.Result.ShowDialog();
		}
	}
}
