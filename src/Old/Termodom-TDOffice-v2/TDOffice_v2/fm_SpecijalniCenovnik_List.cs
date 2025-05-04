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
	public partial class fm_SpecijalniCenovnik_List : Form
	{
		public fm_SpecijalniCenovnik_List()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(137030))
			{
				TDOffice.Pravo.NematePravoObavestenje(137030);
				this.Close();
				return;
			}
			InitializeComponent();
		}

		private void fm_SpecijalniCenovnik_List_Load(object sender, EventArgs e)
		{
			UcitajCenovnike();
		}

		private void UcitajCenovnike()
		{
			List<TDOffice.SpecijalniCenovnik> list = TDOffice.SpecijalniCenovnik.List();

			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));

			foreach (TDOffice.SpecijalniCenovnik sc in list)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = sc.ID;
				dr["Naziv"] = sc.Naziv;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string nazivCenovnika = textBox1.Text;
			if (string.IsNullOrWhiteSpace(nazivCenovnika))
			{
				MessageBox.Show("Morate uneti naziv cenovnika!");
				return;
			}

			TDOffice.SpecijalniCenovnik.Insert(nazivCenovnika);

			UcitajCenovnike();

			MessageBox.Show("Cenovnik uspesno kreiran!");
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int cenovnikID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

			using (fm_SpecijalniCenovnik_Index si = new fm_SpecijalniCenovnik_Index(cenovnikID))
				if (!si.IsDisposed)
					si.ShowDialog();
		}
	}
}
