using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Cekovi_List : Form
    {
        private Task<List<Komercijalno.Magacin>> magacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);

        private bool _loaded = false;

        private List<TDOffice.Cek> _pripremaZaStampu = new List<TDOffice.Cek>();
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();

        private DataTable _listaCekovaDT { get; set; }

        public static int? _stampaPrijemnoMestoBuffer { get; set; }
        public static int? _stampaMestoBuffer { get; set; }
        public static int? _stampaFirmaBuffer { get; set; }
        public static DateTime? _stampaDatumBuffer { get; set; }

        public fm_Cekovi_List()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(136100))
            {
                Task.Run(() =>
                {
                    TDOffice.Pravo.NematePravoObavestenje(136100);
                });
                this.Close();
                return;
            }

            InitializeComponent();

            magacini.Result.Add(new Komercijalno.Magacin() { Naziv = " < svi magacini >", ID = -1 });
            magacini.Result.Sort((x, y) => x.ID.CompareTo(y.ID));
            magacin_cmb.DataSource = magacini.Result;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            odDatuma_dtp.Value = DateTime.Now;
            doDatuma_dtp.Value = DateTime.Now.AddDays(30);

            magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
            statusFilter_cmb.SelectedIndex = 0;

            magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136102);

            dodajUPripremuZaStampuToolStripMenuItem.Enabled = Program.TrenutniKorisnik.ImaPravo(136103);
            btn_Stampaj.Enabled = Program.TrenutniKorisnik.ImaPravo(136103);

            panel3.DesniKlik_DatumRange();
            UcitajCekove();
        }
        private void fm_Cekovi_List_Load(object sender, EventArgs e)
        {
            UcitajCekove();

            _loaded = true;
        }

        private void UcitajCekove()
        {
            int magacinID = (int)magacin_cmb.SelectedValue;
            List<TDOffice.Cek> cekovi = magacinID > 0 ? TDOffice.Cek.ListByMagacinID(magacinID) : TDOffice.Cek.List();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("StatusText", typeof(string));
            dt.Columns.Add("MagacinID", typeof(int));
            dt.Columns.Add("MagacinText", typeof(string));
            dt.Columns.Add("PodnosilacBanka", typeof(int));
            dt.Columns.Add("TRGradjana", typeof(string));
            dt.Columns.Add("BrojCeka", typeof(string));
            dt.Columns.Add("DatumValute", typeof(DateTime));
            dt.Columns.Add("Vrednost", typeof(string));
            dt.Columns.Add("Zaduzen", typeof(int));
            dt.Columns.Add("ZaduzenText", typeof(string));

            foreach(TDOffice.Cek c in cekovi.Where(x => x.DatumValute.Date >= odDatuma_dtp.Value.Date && x.DatumValute.Date <= doDatuma_dtp.Value.Date))
            {
                if (statusFilter_cmb.SelectedIndex > 0)
                    if (statusFilter_cmb.SelectedIndex == 1 && c.Status == TDOffice.Enums.CekStatus.Nerealizovan || statusFilter_cmb.SelectedIndex == 2 && c.Status == TDOffice.Enums.CekStatus.Realizovan)
                        continue;

                DataRow dr = dt.NewRow();

                dr["ID"] = c.ID;
                dr["Datum"] = c.Datum;
                dr["Status"] = (int)c.Status;
                dr["StatusText"] = c.Status.ToString();
                dr["MagacinID"] = c.MagacinID;
                dr["MagacinText"] = magacini.Result.Where(x => x.ID == c.MagacinID).FirstOrDefault().Naziv;
                dr["PodnosilacBanka"] = c.PodnosilacBanka;
                dr["TRGradjana"] = c.TRGradjana;
                dr["BrojCeka"] = c.BrojCeka;
                dr["DatumValute"] = c.DatumValute;
                dr["Vrednost"] = c.Vrednost.ToString();
                if(c.Zaduzio != null)
                    dr["Zaduzen"] = c.Zaduzio;
                dr["ZaduzenText"] = c.Zaduzio != null ? "Zaduzen" : "Nije Zaduzen";

                dt.Rows.Add(dr);
            }

            DataTable sefDT = new DataTable();
            sefDT.Columns.Add("ID", typeof(int));
            sefDT.Columns.Add("DatumValute", typeof(DateTime));
            sefDT.Columns.Add("Vrednost", typeof(string));
            sefDT.Columns.Add("Zaduzio", typeof(string));

            foreach(TDOffice.Cek c in cekovi.Where(x => x.Zaduzio != null))
            {
                DataRow dr = sefDT.NewRow();
                dr["ID"] = c.ID;
                dr["DatumValute"] = c.DatumValute;
                dr["Vrednost"] = c.Vrednost.ToString();
                dr["Zaduzio"] = _korisnici.Result.FirstOrDefault(x => x.ID == c.Zaduzio).Username;
                sefDT.Rows.Add(dr);
            }

            DataTable pripremaZaStampuDT = new DataTable();
            pripremaZaStampuDT.Columns.Add("ID", typeof(int));
            pripremaZaStampuDT.Columns.Add("DatumValute", typeof(DateTime));
            pripremaZaStampuDT.Columns.Add("Vrednost", typeof(string));

            foreach (TDOffice.Cek c in _pripremaZaStampu)
            {
                DataRow dr = pripremaZaStampuDT.NewRow();
                dr["ID"] = c.ID;
                dr["DatumValute"] = c.DatumValute;
                dr["Vrednost"] = c.Vrednost.ToString();
                pripremaZaStampuDT.Rows.Add(dr);
            }

            _listaCekovaDT = dt.Copy();
            dataGridView1.DataSource = dt;
            dataGridView2.DataSource = sefDT;
            dataGridView3.DataSource = pripremaZaStampuDT;

            if (dataGridView1.Rows.Count == 0)
                return;

            dataGridView1.Columns["ID"].Visible = true;
            dataGridView1.Columns["Status"].Visible = false;
            dataGridView1.Columns["MagacinID"].Visible = false;
            dataGridView1.Columns["Zaduzen"].Visible = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            Oboji(null);
        }
        private void Oboji(int? rowID)
        {
            for(int i = rowID == null ? 0 : (int)rowID; i < (rowID == null ? dataGridView1.Rows.Count : ((int)rowID + 1)); i++)
            {
                if (dataGridView1.Rows[i].Cells["Zaduzen"].Value is DBNull)
                    dataGridView1.Rows[i].Cells["ZaduzenText"].Style.BackColor = Color.White;
                else
                    dataGridView1.Rows[i].Cells["ZaduzenText"].Style.BackColor = Color.Orange;


                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Status"].Value) == (int)TDOffice.Enums.CekStatus.Realizovan)
                    dataGridView1.Rows[i].Cells["StatusText"].Style.BackColor = Color.Green;
                else
                    dataGridView1.Rows[i].Cells["StatusText"].Style.BackColor = Color.Coral;
            }
        }
        private void nova_btn_Click(object sender, EventArgs e)
        {
            using (fm_Cekovi_Novi cn = new fm_Cekovi_Novi())
            {
                if (cn.IsDisposed)
                    return;

                cn.MagacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
                cn.ShowDialog();
            }

            UcitajCekove();
        }
        private void ukloniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            TDOffice.Cek c = TDOffice.Cek.Get(id);

            if (c.Datum.Date == DateTime.Now)
            {
                if (!Program.TrenutniKorisnik.ImaPravo(136106))
                {
                    TDOffice.Pravo.NematePravoObavestenje(136106);
                    return;
                }
            }
            else
            {
                if (!Program.TrenutniKorisnik.ImaPravo(136107))
                {
                    TDOffice.Pravo.NematePravoObavestenje(136107);
                    return;
                }
            }
            TDOffice.Cek.Delete(id);

            UcitajCekove();
        }
        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            UcitajCekove();
        }
        private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            UcitajCekove();
        }
        private void doDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            UcitajCekove();
        }
        public void SetMagacin(int magacinID)
        {
            magacin_cmb.SelectedValue = magacinID;
        }

        private void zaduziToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136104))
            {
                TDOffice.Pravo.NematePravoObavestenje(136104);
                return;
            }
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            TDOffice.Cek c = TDOffice.Cek.Get(id);
            if(c.Zaduzio != null)
            {
                MessageBox.Show("Cek je vec zaduzen!");
                return;
            }
            c.Zaduzio = Program.TrenutniKorisnik.ID;
            c.Update();

            dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Zaduzen"].Value = true;
            dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ZaduzenText"].Value = "Zaduzen";

            UcitajCekove();

            MessageBox.Show("Cek uspesno zaduzen!");
        }

        private void realizujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(136105))
            {
                TDOffice.Pravo.NematePravoObavestenje(136105);
                return;
            }
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            TDOffice.Cek c = TDOffice.Cek.Get(id);
            if (c.Status == TDOffice.Enums.CekStatus.Realizovan)
            {
                MessageBox.Show("Cek je vec realizovan!");
                return;
            }

            using(fm_Cekovi_RazduziBox db = new fm_Cekovi_RazduziBox(c))
            {
                db.ShowDialog();
            }
            UcitajCekove();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            realizujToolStripMenuItem.Enabled = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Status"].Value) == 0;

            if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Zaduzen"].Value is DBNull)
                zaduziToolStripMenuItem.Enabled = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Status"].Value) == 0;
            else
                realizujToolStripMenuItem.Enabled = true;
        }

        private void btn_Stampaj_Click(object sender, EventArgs e)
        {
            if(_pripremaZaStampu.Count == 0)
            {
                MessageBox.Show("Nemate ni jedan cek u pripremi za stampu!");
                return;
            }
            using (fm_Cekovi_Stampa_Setup s = new fm_Cekovi_Stampa_Setup(_pripremaZaStampu))
            {
                if (_stampaDatumBuffer != null)
                    s.SetDatumCeka((DateTime)_stampaDatumBuffer);
                if (_stampaFirmaBuffer != null)
                    s.SetFirma((int)_stampaFirmaBuffer);
                if (_stampaMestoBuffer != null)
                    s.SetMesto((int)_stampaMestoBuffer);
                if (_stampaPrijemnoMestoBuffer != null)
                    s.SetPrijemnoMesto((int)_stampaPrijemnoMestoBuffer);

                s.ShowDialog();
            }
        }

        private void dodajUPripremuZaStampuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewCell sc in dataGridView1.SelectedCells)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[sc.RowIndex].Cells["ID"].Value);

                if (_pripremaZaStampu.Any(x => x.ID == id))
                {
                    MessageBox.Show("Cek se vec nalazi u pripremi za stampu!");
                    return;
                }
                TDOffice.Cek c = TDOffice.Cek.Get(id);
                if (c.Status == TDOffice.Enums.CekStatus.Nerealizovan)
                {
                    MessageBox.Show("Ne mozete stampati cek koji nije obelezen kao realizovan!");
                    return;
                }
                _pripremaZaStampu.Add(c);
            }

            UcitajCekove();
        }

        private void statusFilter_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UcitajCekove();
        }

        private void pretraga_txt_TextChanged(object sender, EventArgs e)
        {
            string input = pretraga_txt.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridView1.DataSource = _listaCekovaDT;
                Oboji(null);
                return;
            }

            DataRow[] result = _listaCekovaDT.Copy().Select($"BrojCeka LIKE '%{input}%' OR TRGradjana LIKE '%{input}%'");
            if (result == null || result.Length == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }
            dataGridView1.DataSource = result.CopyToDataTable();
            Oboji(null);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(136105))
            {
                TDOffice.Pravo.NematePravoObavestenje(136105);
                return;
            }
            int id = Convert.ToInt32(dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells["ID"].Value);
            TDOffice.Cek c = TDOffice.Cek.Get(id);
            if (c.Status == TDOffice.Enums.CekStatus.Realizovan)
            {
                MessageBox.Show("Cek je vec realizovan!");
                return;
            }

            using (fm_Cekovi_RazduziBox db = new fm_Cekovi_RazduziBox(c))
            {
                db.ShowDialog();
            }
            UcitajCekove();
        }

        private void dataGridView1_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Oboji(null);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            Oboji(null);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int cekID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            using (fm_Cekovi_Index ci = new fm_Cekovi_Index(cekID))
                if (!ci.IsDisposed)
                {
                    ci.ShowDialog();
                    UcitajCekove();
                }
        }
    }
}
