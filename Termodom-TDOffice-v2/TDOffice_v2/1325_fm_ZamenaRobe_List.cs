using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class _1325_fm_ZamenaRobe_List : Form
    {
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        private Task<fm_Help> _helpFrom { get; set; }
        private List<Magacin> _magaciniKomercijalno { get; set; } = Magacin.ListAsync().Result;

        public _1325_fm_ZamenaRobe_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul._1325_fm_ZamenaRobe_List);
            SetUI();
        }

        private void SetUI()
        {
            List<Magacin> magacinList = _magaciniKomercijalno;
            magacinList.Add(new Magacin() { ID = -1, Naziv = " < Izaberi magacin > " });
            magacinList.Sort((x, y) => x.ID.CompareTo(y.ID));

            cmb_Magacin.DisplayMember = "Naziv";
            cmb_Magacin.ValueMember = "ID";
            cmb_Magacin.DataSource = magacinList;
        }
        private void _1325_fm_ZamenaRobe_List_Load(object sender, EventArgs e)
        {
            this.cmb_Magacin.SelectedValue = Program.TrenutniKorisnik.MagacinID > 0 ? Program.TrenutniKorisnik.MagacinID : -1;

            ReloadData();

            cmb_Magacin.Enabled = Program.TrenutniKorisnik.ImaPravo(132505);
        }

        private void ReloadData()
        {
            int mid = Convert.ToInt32(cmb_Magacin.SelectedValue);
            List<TDOffice.DokumentZamenaRobe> zameneRobe =
                TDOffice.DokumentZamenaRobe.List($@"DATUM >= '{odDatuma_dtp.Value.Date.ToString("dd.MM.yyyy")}'
                    AND DATUM <= '{doDatuma_dtp.Value.Date.AddDays(1).ToString("dd.MM.yyyy")}'" + (mid > 0 ? $" AND MAGACINID = {mid}" : ""));
            
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("MagacinID", typeof(int)));
            dt.Columns.Add(new DataColumn("Magacin", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(int)));
            dt.Columns.Add(new DataColumn("UserID", typeof(int)));
            dt.Columns.Add(new DataColumn("Referent", typeof(string)));
            dt.Columns.Add(new DataColumn("Realizovana", typeof(bool)));

            foreach (TDOffice.DokumentZamenaRobe p in zameneRobe)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = p.ID;
                dr["Date"] = p.Datum;
                dr["MagacinID"] = p.MagacinID;
                Komercijalno.Magacin mag = _magaciniKomercijalno.FirstOrDefault(x => x.ID == p.MagacinID);
                dr["Magacin"] = p.MagacinID.ToString() + " - " + (mag == null ? "unknown" : mag.Naziv);
                dr["Status"] = (int)p.Status;
                dr["UserID"] = p.UserID;
                dr["Referent"] = _korisnici.Result.Where(x => x.ID == p.UserID).FirstOrDefault().Username;
                dr["Realizovana"] = p.Realizovana;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            if(dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Sort(dataGridView1.Columns["ID"], ListSortDirection.Ascending);
                dataGridView1.Columns["ID"].Width = 50;
                dataGridView1.Columns["Date"].Width = 80;
                dataGridView1.Columns["Date"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dataGridView1.Columns["MagacinID"].Visible = false;
                dataGridView1.Columns["Magacin"].Width = 300;
                dataGridView1.Columns["Status"].Visible = false;
                dataGridView1.Columns["UserID"].Visible = false;
                dataGridView1.Columns["Referent"].Width = 200;
                dataGridView1.Columns["Realizovana"].Visible = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(row.Cells["Status"].Value) == 1)
                    {
                        if(Convert.ToBoolean(row.Cells["Realizovana"].Value))
                        {
                            row.DefaultCellStyle.ForeColor = Color.OrangeRed;
                            row.DefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
                        }
                        else
                        {
                            row.DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void nova_btn_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(132501))
            {
                TDOffice.Pravo.NematePravoObavestenje(132501);
                return;
            }

            if (Convert.ToInt32(cmb_Magacin.SelectedValue) <= 0)
            {
                MessageBox.Show("Niste izabrali magacin!");
                return;
            }

            if (MessageBox.Show("Da li zelite da kreirate nalog za magacin \n " + this.cmb_Magacin.Text, "Zamena robe ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            //int newID = TDOffice.DokumentZamenaRobe.Insert(Program.TrenutniKorisnik.ID, Program.TrenutniKorisnik.MagacinID, 0, null, null);
            int newID = TDOffice.DokumentZamenaRobe.Insert(Program.TrenutniKorisnik.ID, Convert.ToInt32(cmb_Magacin.SelectedValue), 0, null, null);

            Task.Run(() =>
            {
                using (fm_ZamenaRobe2_Index zr = new fm_ZamenaRobe2_Index(newID))
                    zr.ShowDialog();
            });
            ReloadData();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int dokID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            Task.Run(() =>
            {
                using (fm_ZamenaRobe2_Index zr = new fm_ZamenaRobe2_Index(dokID))
                    zr.ShowDialog();
            });

            Program.GCCollectWithDelayAsync();
        }

        private void cmb_Magacin_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void hELPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }

        private void doDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }
    }
}
