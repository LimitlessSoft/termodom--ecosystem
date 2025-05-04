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
	public partial class fm_Firme_List : Form
	{
		public fm_Firme_List()
		{
			MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktirajte administratora!");
			return;

			InitializeComponent();
		}

		private void fm_Firme_List_Load(object sender, EventArgs e)
		{
			UcitajFirme();
		}

		private void UcitajFirme()
		{
			//List<TDOffice.Firma> list = TDOffice.Firma.List();

			//DataTable dt = new DataTable();
			//dt.Columns.Add("ID", typeof(int));
			//dt.Columns.Add("Naziv", typeof(string));
			//dt.Columns.Add("PIB", typeof(string));
			//dt.Columns.Add("MB", typeof(string));
			//dt.Columns.Add("Adresa", typeof(string));
			//dt.Columns.Add("TekuciRacun", typeof(string));
			//dt.Columns.Add("Grad", typeof(string));

			//foreach(TDOffice.Firma f in list)
			//{
			//    DataRow dr = dt.NewRow();
			//    dr["ID"] = f.ID;
			//    dr["Naziv"] = f.Naziv;
			//    dr["PIB"] = f.PIB;
			//    dr["MB"] = f.MB;
			//    dr["Adresa"] = f.Adresa;
			//    dr["TekuciRacun"] = f.TekuciRacun;
			//    dr["Grad"] = f.Grad;

			//    dt.Rows.Add(dr);
			//}

			//dataGridView1.DataSource = dt;

			//dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			using (fm_Firme_Nova n = new fm_Firme_Nova())
				n.ShowDialog();

			UcitajFirme();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			//int idFirme = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

			//using (fm_Firme_Index f = new fm_Firme_Index(TDOffice.Firma.Get(idFirme)))
			//    f.ShowDialog();

			//UcitajFirme();
		}
	}
}
