using FirebirdSql.Data.FirebirdClient;
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
    public partial class fm_Zaposleni_List : Form
    {

        private DataTable _dt { get; set; } = new DataTable();

        public fm_Zaposleni_List()
        {
            if(!Program.TrenutniKorisnik.ImaPravo(171000))
            {
                TDOffice.Pravo.NematePravoObavestenje(171000);
                return;
            }

            InitializeComponent();

            _dt.Columns.Add("ID", typeof(int));
            _dt.Columns.Add("Ime", typeof(string));
            _dt.Columns.Add("Prezime", typeof(string));
            dataGridView1.DataSource = _dt;

            foreach (DataColumn col in _dt.Columns)
                cmb_PoljePretrage.Items.Add(col.ColumnName);

            cmb_PoljePretrage.SelectedItem = "Ime";
        }

        private void PopuniDGV()
        {
            List<TDOffice.Zaposleni> zaposleni = TDOffice.Zaposleni.List();
            _dt.Clear();
             
            dataGridView1.SuspendLayout();
            foreach (TDOffice.Zaposleni zap in zaposleni)
            {
                DataRow dr = _dt.NewRow();
                dr["ID"] = zap.ID;
                dr["Ime"] = zap.Ime;
                dr["Prezime"] = zap.Prezime;
                //dr["FirmaNaziv"] = _firma.Where(x => x.ID == zap.Firma).ToList().FirstOrDefault().Naziv;
                _dt.Rows.Add(dr);
            }
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
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[kolona];
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string vrednostCelije = row.Cells[kolona].Value.ToString();
                if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index > 0 ? row.Index - 1 : 0;
                    dataGridView1.Rows[row.Index].Selected = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells[kolona];
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
                    selectString += "CONVERT(" + o.ToString() + ", System.String) LIKE '%" + inputElemets[i] + "%' AND ";

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
            if (MessageBox.Show("Sigurno zelite da unesete novog zaposlenog?", "Novi zaposleni", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int zapID = TDOffice.Zaposleni.Insert("unknown", "unknown");
                using (fm_Zaposleni_Index zi = new fm_Zaposleni_Index(TDOffice.Zaposleni.Get(zapID)))
                    zi.ShowDialog();
                PopuniDGV();
            }
                
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            using (fm_Zaposleni_Index zapi = new fm_Zaposleni_Index(TDOffice.Zaposleni.Get(ID)))
            {
                zapi.ShowDialog();
                PopuniDGV();
            }
        }

        private void fm_Zaposleni_List_Load(object sender, EventArgs e)
        {
            PopuniDGV();    
        }

        private void tb_Pretraga_KeyDown(object sender, KeyEventArgs e)
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

        private void UkloniZaposlenog_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sigurno zelite da uklonite zaposlenog?", "Uklanjanje zaposlenog", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            TDOffice.Zaposleni.DeleteAsync(id);
            PopuniDGV();
        }
    }
}
