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
    public partial class _1365_fm_SastavnicaRastavnica_Pravila_List : Form
    {
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        private Task<fm_Help> _helpFrom { get; set; }

        public _1365_fm_SastavnicaRastavnica_Pravila_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.SastavnicaRastavnica_Pravila_List);
            SetUI();
        }

        private void _1365_fm_SastavnicaRastavnica_Pravila_List_Load(object sender, EventArgs e)
        {
            ReloadPravila();
        }

        private void SetUI()
        {
            this.Text = "Pravila Sastavnice Lista";
        }

        private void ReloadPravila()
        {
            List<TDOffice.DokumentSastavnica> _dokumentSastavnica = TDOffice.DokumentSastavnica.ListByTip(TDOffice.Enums.DokumentSastavnicaTip.Pravilo);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("KorisnikID", typeof(int));
            dt.Columns.Add("TIP", typeof(int));
            dt.Columns.Add("Referent", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));

            foreach (TDOffice.DokumentSastavnica dok in _dokumentSastavnica)
            {
                TDOffice.User dokKorisnik = _korisnici.Result.Where(x => x.ID == dok.Korisnik).FirstOrDefault();
                DataRow dr = dt.NewRow();
                dr["ID"] = dok.ID;
                dr["KorisnikID"] = dok.Korisnik;
                dr["TIP"] = dok.Tip;
                dr["Referent"] = dokKorisnik == null ? "UNKNOWN" : dokKorisnik.Username;
                dr["Naziv"] = dok.Tag;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["ID"].HeaderText = "Br. Dok.";
            dataGridView1.Columns["KorisnikID"].Visible = false;
            dataGridView1.Columns["TIP"].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            using (_1365_fm_SastavnicaRastavnica_Pravila_Index pi = new _1365_fm_SastavnicaRastavnica_Pravila_Index(TDOffice.DokumentSastavnica.Get(ID)))
            {
                pi.ShowDialog();
                ReloadPravila();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sigurno zelite da kreirate novo pravilo?", "Novo pravilo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int noviID = TDOffice.DokumentSastavnica.Insert(-1, Program.TrenutniKorisnik.ID, TDOffice.Enums.DokumentSastavnicaTip.Pravilo, "NOVI");

                using (_1365_fm_SastavnicaRastavnica_Pravila_Index pi = new _1365_fm_SastavnicaRastavnica_Pravila_Index(TDOffice.DokumentSastavnica.Get(noviID)))
                {
                    pi.ShowDialog();
                    ReloadPravila();
                }
            }
        }
    }
}
