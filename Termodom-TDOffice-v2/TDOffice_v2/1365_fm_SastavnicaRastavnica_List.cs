using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class _1365_fm_SastavnicaRastavnica_List : Form
    {
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        private Task<fm_Help> _helpFrom { get; set; }

        private bool _loaded { get; set; } = false;

        public _1365_fm_SastavnicaRastavnica_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.SastavnicaRastavnica_List);
            SetUI();
        }

        private void _1365_fm_SastavnicaRastavnica_List_Load(object sender, EventArgs e)
        {
            UcitajSastavnice();
            _loaded = true;
        }

        private void SetUI()
        {
            Magacin.ListAsync().ContinueWith((prev) =>
            {
                var magacinList = prev.Result;
                magacinList.Add(new Magacin() { ID = -1, Naziv = " < Izaberi magacin > " });
                magacinList.Sort((x, y) => x.ID.CompareTo(y.ID));
                this.Invoke((MethodInvoker)delegate
                {
                    magacini_cmb.DisplayMember = "Naziv";
                    magacini_cmb.ValueMember = "ID";
                    magacini_cmb.DataSource = magacinList;
                    magacini_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;

                    this.Text = "Lista kreiranih sastavnica za " + magacini_cmb.Text;
                });
            });
        }

        private void UcitajSastavnice()
        {
            int mid = Convert.ToInt32(magacini_cmb.SelectedValue);

            if (mid < 0)
                return;

            List<TDOffice.DokumentSastavnica> _dokumentSastavnica = TDOffice.DokumentSastavnica.ListByMagacinID(mid);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("KorisnikID", typeof(int));
            dt.Columns.Add("TIP", typeof(int));
            dt.Columns.Add("Referent", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("Status", typeof(int));

            foreach (TDOffice.DokumentSastavnica dok in _dokumentSastavnica)
            {
                TDOffice.User dokKorisnik = _korisnici.Result.Where(x => x.ID == dok.Korisnik).FirstOrDefault();
                DataRow dr = dt.NewRow();
                dr["ID"] = dok.ID;
                dr["KorisnikID"] = dok.Korisnik;
                dr["TIP"] = dok.Tip;
                dr["Referent"] = dokKorisnik == null ? "UNKNOWN" : dokKorisnik.Username;
                dr["Naziv"] = dok.Tag;
                dr["Status"] = dok.Status;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["ID"].HeaderText = "Br. Dok.";
            dataGridView1.Columns["KorisnikID"].Visible = false;
            dataGridView1.Columns["TIP"].Visible = false;
            dataGridView1.Columns["Status"].Visible = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["Status"].Value) == 1)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            this.Text = "Lista kreiranih sastavnica za " + magacini_cmb.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(Program.TrenutniKorisnik.KomercijalnoUserID == null)
            {
                MessageBox.Show("Ne mozete kreirati sastavnicu jer nemate povezan nalog komercijalnog poslovanja!\nZahtevajte administratoru da u vasim podesavanjima dodeli ID nekog naloga iz komercijalnog!");
                return;
            }

            int mid = Convert.ToInt32(magacini_cmb.SelectedValue);
            if (mid < 0)
            {
                MessageBox.Show("Niste izabrali ili nemate pripadajuci magacin");
                return;
            }
               
            if (MessageBox.Show("Sigurno zelite da kreirate novu sastavnicu?", "Nova sastavnica", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                
                int noviID = TDOffice.DokumentSastavnica.Insert(mid, Program.TrenutniKorisnik.ID, TDOffice.Enums.DokumentSastavnicaTip.Sastavnica, "NOVI");

                using (_1365_fm_SastavnicaRastavnica_Index sri = new _1365_fm_SastavnicaRastavnica_Index(TDOffice.DokumentSastavnica.Get(noviID)))
                {
                    sri.ShowDialog();
                    UcitajSastavnice();
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            using (_1365_fm_SastavnicaRastavnica_Index sri = new _1365_fm_SastavnicaRastavnica_Index(TDOffice.DokumentSastavnica.Get(ID)))
            {
                sri.ShowDialog();
                UcitajSastavnice();
            }
        }

        private void magacini_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UcitajSastavnice();
        }
    }
}
