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
    public partial class Menadzment_Fakturisi0634_List : Form
    {
        public Menadzment_Fakturisi0634_List()
        {
            InitializeComponent();
        }

        private void Menadzment_Fakturisi0634_List_Load(object sender, EventArgs e)
        {
            magacin_cmb.Enabled = false;
            magacin_cmb.ValueMember = "ID";
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
            magacin_cmb.Enabled = true;
            magacin_cmb.SelectedValue = 13;

            ucitaj_btn.PerformClick();
        }

        private void nova_btn_Click(object sender, EventArgs e)
        {
            int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);

            int novaFaktura = TDOffice.Dokument0634.Insert(Program.TrenutniKorisnik.ID, magacinID, 0, null);

            using (Menadzment_Fakturisi0634_Index f = new Menadzment_Fakturisi0634_Index(novaFaktura))
                f.ShowDialog();
        }

        private void Ucitaj_Click(object sender, EventArgs e)
        {
            int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);

            DataTable dt = new DataTable();
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("MagacinID", typeof(int));

            foreach (TDOffice.Dokument0634 dok in TDOffice.Dokument0634.List("MAGACINID = " + magacinID))
            {
                DataRow dr = dt.NewRow();
                dr["BrDok"] = dok.ID;
                dr["Datum"] = dok.Datum;
                dr["MagacinID"] = dok.MagacinID;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int brDok = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["BrDok"].Value);

            using (Menadzment_Fakturisi0634_Index f = new Menadzment_Fakturisi0634_Index(brDok))
                f.ShowDialog();
        }
    }
}
