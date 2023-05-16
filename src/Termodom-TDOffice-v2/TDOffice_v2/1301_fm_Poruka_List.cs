using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class _1301_fm_Poruka_List : Form
    {
        private FbConnection _tdCon { get; set; }
        private static List<TDOffice.Poruka> ListByDatumIncludingCurrentUser(DateTime odDatuma, DateTime doDatuma)
        {
            return TDOffice.Poruka
                .List($@"(PRIMALAC = {Program.TrenutniKorisnik.ID} OR
                    POSILJALAC = {Program.TrenutniKorisnik.ID}) AND
                    DATUM >= '{odDatuma.ToString("dd.MM.yyyy")}' AND
                    DATUM < '{doDatuma.ToString("dd.MM.yyyy")}'");
        }

        /// <summary>
        /// Dvodimenzionalni dictionary koji sadrzi dictionary poruka.
        /// Prvi dictionary - Key: Datum poruke (Vreme nije precizno), Value: Dictionary sa svim porukama na taj dan
        /// Drugi dictionary - key: ID Poruke, Value: Objekat poruke
        /// </summary>
        private Task<Dictionary<DateTime, Dictionary<int, TDOffice.Poruka>>> _porukeBuffer { get; set; }
        private List<TDOffice.User> _korisnici { get; set; }
        private Dictionary<int, TDOffice.User> _korisniciDict { get; set; } = new Dictionary<int, TDOffice.User>();
        private Task<fm_Help> _helpFrom { get; set; }
        public DateTime? MinDatumLoadFunc()
        {
            return DateTime.Now.AddMonths(-6);
        }
        public _1301_fm_Poruka_List()
        {
            InitializeComponent();

            // Kreira i otvara konekciju ka bazi TDOffice-a
            _tdCon = new FbConnection(TDOffice.TDOffice.connectionString);
            _tdCon.Open();

            InitializePorukeBuffer();

            #region UI Disable
            izborKorespodenta_cmb.Enabled = false;
            primljene_rb.Enabled = false;
            poslate_rb.Enabled = false;
            statusSve_rb.Enabled = false;
            do_dtp.Enabled = false;
            od_dtp.Enabled = false;
            cmb_Tip.Enabled = false;
            procitane_rb.Enabled = false;
            neprocitane_rb.Enabled = false;
            tipSve_rb.Enabled = false;
            cmbIzborAkcijePoruka.Enabled = false;
            #endregion
            #region UI - Tipovi poruka
            List<Tuple<int, string>> tipoviPoruka = new List<Tuple<int, string>>();

            foreach (TDOffice.PorukaTip pt in (TDOffice.PorukaTip[])Enum.GetValues(typeof(TDOffice.PorukaTip)))
                tipoviPoruka.Add(new Tuple<int, string>((int)pt, pt.ToString()));

            tipoviPoruka = tipoviPoruka.Prepend(new Tuple<int, string>(-1, " < svi tipovi > ")).ToList();
            cmb_Tip.DisplayMember = "Item2";
            cmb_Tip.ValueMember = "Item1";
            cmb_Tip.DataSource = tipoviPoruka;
            cmb_Tip.SelectedValue = -1;
            #endregion
            #region UI - Korisnici
            _korisnici = TDOffice.User.List(_tdCon);
            _korisnici.Add(new TDOffice.User() { ID = -1, Username = "< SISTEM >" });
            _korisnici.Add(new TDOffice.User() { ID = -2, Username = "< Svi Korisnici >" });
            _korisnici.Sort((a, b) => a.ID.CompareTo(b.ID));

            foreach(TDOffice.User u in _korisnici)
                _korisniciDict.Add(u.ID, u);

            izborKorespodenta_cmb.DisplayMember = "Username";
            izborKorespodenta_cmb.ValueMember = "ID";
            izborKorespodenta_cmb.DataSource = _korisnici;

            izborKorespodenta_cmb.SelectedValue = -2;
            #endregion
            #region UI - Izbor Akcije Filter
            List<Tuple<int, string>> tdtaskovi = new List<Tuple<int, string>>();

            foreach (TDOffice.TDTask t in Enum.GetValues(typeof(TDOffice.TDTask)))
                tdtaskovi.Add(new Tuple<int, string>((int)t, t.ToString()));
            tdtaskovi = tdtaskovi.Prepend(new Tuple<int, string>(-1, " Sve akcije ")).ToList();
            tdtaskovi = tdtaskovi.Prepend(new Tuple<int, string>(-2, " < Izbor akcije > ")).ToList();
            tdtaskovi.OrderBy(x => x.Item1).ToList();
            cmbIzborAkcijePoruka.DisplayMember = "Item2";
            cmbIzborAkcijePoruka.ValueMember = "Item1";
            cmbIzborAkcijePoruka.DataSource = tdtaskovi;
            #endregion
            PrikaziPoruke();

            _helpFrom = this.InitializeHelpModulAsync(Modul._1301_fm_Poruka_List);
            period_gb.DesniKlik_DatumRange(null);

            #region UI Enable
            izborKorespodenta_cmb.Enabled = true;
            primljene_rb.Enabled = true;
            poslate_rb.Enabled = true;
            statusSve_rb.Enabled = true;
            do_dtp.Enabled = true;
            od_dtp.Enabled = true;
            cmb_Tip.Enabled = true;
            procitane_rb.Enabled = true;
            neprocitane_rb.Enabled = true;
            tipSve_rb.Enabled = true;
            cmbIzborAkcijePoruka.Enabled = true;
            #endregion
        }
        private void _1301_fm_Poruka_List_Load(object sender, EventArgs e)
        {
        }

        private void PrikaziPoruke()
        {
            try
            {
                DateTime odDatuma = od_dtp.Value;
                DateTime doDatuma = do_dtp.Value;

                int korespodent = Convert.ToInt32(izborKorespodenta_cmb.SelectedValue);

                int? primalac = primljene_rb.Checked ? (int?)Program.TrenutniKorisnik.ID : korespodent == -2 ? null : (int?)korespodent;
                int? posiljalac = poslate_rb.Checked ? (int?)Program.TrenutniKorisnik.ID : korespodent == -2 ? null : (int?)korespodent;

                int? tipPoruke = Convert.ToInt32(cmb_Tip.SelectedValue) == -1 ? null : (int?)Convert.ToInt32(cmb_Tip.SelectedValue);

                List<TDOffice.Poruka> poruke;

                // Ukoliko su podaci ucitani iz baze u buffer onda koristim buffer,
                // u suportnom citam direktno iz baze
                if (!_porukeBuffer.IsCompleted)
                {
                    string whereQuery = $@"(
                    DATUM >= '{odDatuma.ToString("dd.MM.yyyy")}' AND
                    DATUM < '{doDatuma.AddDays(1).ToString("dd.MM.yyyy")}'
                    ";

                    if(tipSve_rb.Checked)
                    {
                        whereQuery += $" AND (PRIMALAC = {Program.TrenutniKorisnik.ID} OR POSILJALAC = {Program.TrenutniKorisnik.ID})";
                    }
                    else
                    {
                        if (primalac != null)
                            whereQuery += $" AND PRIMALAC = {primalac}";

                        if (posiljalac != null)
                            whereQuery += $" AND POSILJALAC = {posiljalac}";
                    }

                    if (tipPoruke != null)
                        whereQuery += $" AND STATUS = {tipPoruke}";

                    whereQuery += ")";

                    poruke = TDOffice.Poruka.List(_tdCon, whereQuery);
                }
                else
                {
                    poruke = new List<TDOffice.Poruka>();
                    while (odDatuma.Date <= doDatuma.Date)
                    {
                        if (_porukeBuffer.Result.ContainsKey(odDatuma.Date))
                            poruke.AddRange(_porukeBuffer.Result[odDatuma.Date].Values);
                        odDatuma = odDatuma.AddDays(1);
                    }

                    if (tipSve_rb.Checked)
                    {
                        poruke.RemoveAll(x => x.Primalac != Program.TrenutniKorisnik.ID && x.Posiljalac != Program.TrenutniKorisnik.ID);
                    }
                    else
                    {
                        if (primalac != null)
                            poruke.RemoveAll(x => x.Primalac != primalac);

                        if (posiljalac != null)
                            poruke.RemoveAll(x => x.Posiljalac != posiljalac);
                    }

                    if (tipPoruke != null)
                        poruke.RemoveAll(x => (int)x.Status != tipPoruke);
                }

                #region Tip Akcije Poruke
                // Ako je ukljucen ovaj filter, ne mogu kroz bazu filtrirati jer moram gledati naslov tako da bilo
                // da ucitavam iz buffera ili baze, u oba slucaja ovaj filter proveravam tek
                // kada imam poruke u memoriji

                int akcija = Convert.ToInt32(cmbIzborAkcijePoruka.SelectedValue);

                switch (akcija)
                {
                    case -1:
                        poruke.RemoveAll(x => !x.Naslov.StartsWith("<TASK"));
                        break;
                    case 1:
                        poruke.RemoveAll(x => x.Naslov != "<TASK > Nezakljucane MP Kalkulacije");
                        break;
                    case 2:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Automatske Nivelacije");
                        break;
                    case 3:
                        poruke.RemoveAll(x => x.Naslov != "<TASK > Nelogicne marze");
                        break;
                    case 4:
                        poruke.RemoveAll(x => x.Naslov != "Popisi Robu");
                        break;
                    case 5:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Da li ima praznih faktura");
                        break;
                    case 6:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Promet magacina");
                        break;
                    case 7:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Izvestaj minimalnih zaliha");
                        break;
                    case 8:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Izvestaj prekomernih zaliha");
                        break;
                    case 9:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Izvestaj nedovrsenih razduzenja");
                        break;
                    case 10:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Time Keeper");
                        break;
                    case 11:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Izvestaj Ugovora o radu koji isticu u narednih 15 dana");
                        break;
                    case 12:
                        poruke.RemoveAll(x => x.Naslov != "<TASK> Komercijalno Parametri");
                        break;
                }
                #endregion

                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("DatumSlanja", typeof(DateTime));
                dt.Columns.Add("DatumCitanja", typeof(DateTime));
                dt.Columns.Add("Naslov", typeof(string));
                dt.Columns.Add("Posiljalac", typeof(string));
                dt.Columns.Add("Primalac", typeof(string));
                foreach (TDOffice.Poruka p in poruke)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = p.ID;
                    dr["DatumSlanja"] = p.Datum;
                    if (p.DatumCitanja != null)
                        dr["DatumCitanja"] = p.DatumCitanja;
                    dr["Naslov"] = p.Naslov;
                    dr["Posiljalac"] = _korisniciDict[p.Posiljalac].Username;
                    dr["Primalac"] = _korisniciDict[p.Primalac].Username;
                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void InitializePorukeBuffer()
        {
            _porukeBuffer = Task.Run(() =>
            {
                Dictionary<DateTime, Dictionary<int, TDOffice.Poruka>> dict = new Dictionary<DateTime, Dictionary<int, TDOffice.Poruka>>();

                List<TDOffice.Poruka> poruke = TDOffice.Poruka.List();
                foreach (TDOffice.Poruka p in poruke)
                {
                    if (!dict.ContainsKey(p.Datum.Date))
                        dict.Add(p.Datum.Date, new Dictionary<int, TDOffice.Poruka>());

                    dict[p.Datum.Date].Add(p.ID, p);
                }

                return dict;
            });
        }
        private void doDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            PrikaziPoruke();
        }
        private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            PrikaziPoruke();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int currPorukaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                using (_1301_fm_Poruka_Index pi = new _1301_fm_Poruka_Index(TDOffice.Poruka.Get(currPorukaID)))
                {
                    pi.ShowInTaskbar = false;
                    pi.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void nova_btn_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<_1301_fm_Poruka_Nova>().FirstOrDefault() != null)
                return;

            Task.Run(() =>
            {
                using (_1301_fm_Poruka_Nova nova = new _1301_fm_Poruka_Nova())
                    nova.ShowDialog();

                this.Invoke((MethodInvoker)delegate
                {
                    PrikaziPoruke();
                });
            });
        }
        private void tipPorukeRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            if (!(sender as RadioButton).Checked)
                return;

            PrikaziPoruke();
        }
        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int idPoruke = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            TDOffice.Poruka poruka = TDOffice.Poruka.Get(idPoruke);
            _1301_fm_Poruka_Index p = new _1301_fm_Poruka_Index(TDOffice.Poruka.Get(idPoruke));
            p.ShowInTaskbar = false;
            if (poruka.Posiljalac == Program.TrenutniKorisnik.ID)
            p.ProsiriPoruku();
            p.Show();
        }
        private void poslate_rb_Click(object sender, EventArgs e)
        {
            gbIzborPosiljaoca.Text = "Izbor primaoca";
        }
        private void primljene_rb_Click(object sender, EventArgs e)
        {
            gbIzborPosiljaoca.Text = "Izbor posiljaoca";
        }
        private void tipSve_rb_Click(object sender, EventArgs e)
        {
            gbIzborPosiljaoca.Text = "Izbor kontakta";
        }
        private void btn_Refresh_Click(object sender, EventArgs e)
        {   
            PrikaziPoruke();
        }
        private void cmb_Tip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            PrikaziPoruke();
        }
        private void arhivirajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
      
            TDOffice.Poruka tekucaporuka = TDOffice.Poruka.Get(id);
            if (tekucaporuka.Status == TDOffice.PorukaTip.Standard)
            {
                if (tekucaporuka.Primalac == Program.TrenutniKorisnik.ID)
                {
                    tekucaporuka.Arhivirana = true;
                    tekucaporuka.Update();
                }
            }
            else
            {
                if (tekucaporuka.Posiljalac == Program.TrenutniKorisnik.ID)
                {
                    tekucaporuka.Arhivirana = true;
                    tekucaporuka.Update();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }
        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            int idPoruke = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            this.contextMenuStrip1.Enabled = true;
            TDOffice.Poruka poruka;
            poruka = TDOffice.Poruka.Get(idPoruke);

            if (poruka.Posiljalac == Program.TrenutniKorisnik.ID)
                this.contextMenuStrip1.Enabled = false;

            if (poruka.Posiljalac == poruka.Primalac)
                this.contextMenuStrip1.Enabled = true;

            if (poruka.DatumCitanja == null)
                this.contextMenuStrip1.Enabled = false;
        }
        private void cmbIzborPosiljaoca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            PrikaziPoruke();
        }

        private void cmbIzborAkcijePoruka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            PrikaziPoruke();
        }
    }
}
