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
	public partial class _1348_fm_KomercijalnoPartner_List : Form
	{
		private DataTable _dt { get; set; } = new DataTable();
		private Task<List<Komercijalno.Partner>> _partneri { get; set; }

		public _1348_fm_KomercijalnoPartner_List()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(137020))
			{
				TDOffice.Pravo.NematePravoObavestenje(137020);
				this.Close();
				return;
			}
			InitializeComponent();
			_partneri = Task.Run(() =>
			{
				List<Komercijalno.Partner> list = Komercijalno.Partner.ListAsync().Result;

				DataTable dt = new DataTable();
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("Naziv", typeof(string));

				foreach (Komercijalno.Partner p in list)
				{
					DataRow dr = dt.NewRow();
					dr["PPID"] = p.PPID;
					dr["Naziv"] = p.Naziv;
					dt.Rows.Add(dr);
				}

				_dt = dt.Copy();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = _dt;
							dataGridView1.Columns["Naziv"].Width = 800;
						}
				);
				return list;
			});
			dataGridView1.DataSource = _dt;
		}

		private void _1348_fm_KomercijalnoPartner_List_Load(object sender, EventArgs e) { }

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int ppid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PPID"].Value);

			using (_1348_fm_Partner_Index pi = new _1348_fm_Partner_Index(ppid))
				if (!pi.IsDisposed)
					pi.ShowDialog();
		}

		private void textBox1_TextChanged(object sender, EventArgs e) { }

		private void button1_Click(object sender, EventArgs e)
		{
			string input = textBox1.Text;
			string selectString = $" ";
			string[] inputElemets = input.Split('+');

			for (int i = 0; i < inputElemets.Length; i++)
				selectString += "NAZIV LIKE '%" + inputElemets[i] + "%' AND ";

			selectString = selectString.Remove(selectString.Length - 4);

			DataRow[] rows = _dt.Copy().Select(selectString);
			dataGridView1.DataSource =
				rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();
		}
	}
}
