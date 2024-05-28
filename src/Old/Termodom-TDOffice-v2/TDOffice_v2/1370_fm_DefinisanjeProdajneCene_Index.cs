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
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class _1370_fm_DefinisanjeProdajneCene_Index : Form
    {
        private Dokument dokument { get; set; } = null;
        private DataTable dt { get; set; } = null;
        private Task<fm_Help> _helpFrom { get; set; }

        private _7_fm_Komercijalno_Roba_Kartica _fmKarticaRobe { get; set; } = new _7_fm_Komercijalno_Roba_Kartica()
        {
            vidljiviVrDok = new List<int>() { 0, 1, 2, 21, 36 }
        };

        public _1370_fm_DefinisanjeProdajneCene_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.DefinisanjeProdajneCene_Index);
            gb_AnalizaCena.DesniKlik_DatumRange(null);
            ToggleUI(false);
            _ = SetUIAsync();
        }

        private void _1370_fm_DefinisanjeProdajneCene_Index_Load(object sender, EventArgs e)
        {

        }

        private void ToggleUI(bool state)
        {
            this.rb_PoslednjaNavavnaCena.Enabled = state;
        }

        private async Task SetUIAsync()
        {
            List<Termodom.Data.Entities.Komercijalno.VrstaDok> vrdokList = (await VrstaDokManager.DictionaryAsync()).Values.Where(x => new int[] { 4, 13, 15, 32, 34 }.Contains(x.VrDok)).ToList();
            vrdokList.Add(new Termodom.Data.Entities.Komercijalno.VrstaDok() { VrDok = -1, NazivDok = " < vrsta dokumenta > " });
            vrdokList.Sort((x, y) => x.VrDok.CompareTo(y.VrDok));

            cmb_VrstaDokumenta.DataSource = vrdokList;
            cmb_VrstaDokumenta.DisplayMember = "NazivDok";
            cmb_VrstaDokumenta.ValueMember = "VrDok";

            List<Termodom.Data.Entities.Komercijalno.VrstaDok> vrdokListPd = (await VrstaDokManager.DictionaryAsync()).Values.Where(x => new int[] { 1, 2 }.Contains(x.VrDok)).ToList();
            vrdokListPd.Add(new Termodom.Data.Entities.Komercijalno.VrstaDok() { VrDok = -1, NazivDok = " < vrsta dokumenta > " });
            vrdokListPd.Sort((x, y) => x.VrDok.CompareTo(y.VrDok));

            cmb_PoDokumentu.DataSource = vrdokListPd;
            cmb_PoDokumentu.DisplayMember = "NazivDok";
            cmb_PoDokumentu.ValueMember = "VrDok";

            dtp_Do.Value = new DateTime(DateTime.Now.Year, 12, 31);
            dtp_Od.Value = DateTime.Today.AddDays(-90);

            dt = new DataTable();
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("StavkaID", typeof(int));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("Kolicina", typeof(double));
            dt.Columns.Add("ProdajnaCena", typeof(double));
            dt.Columns.Add("Rabat", typeof(double));
            dt.Columns.Add("NabavnaCena", typeof(double));
            dt.Columns.Add("Marza", typeof(double));
            dt.Columns.Add("MaxRabat", typeof(double));
            dt.Columns.Add("BuducaMarza", typeof(double));
            dt.Columns.Add("BuduciRabat", typeof(double));
            dt.Columns.Add("BuducaCena", typeof(double));
            dt.Columns.Add("Ima36", typeof(bool));
            dt.Columns.Add("NabavnaVrednost", typeof(int));
            dt.Columns.Add("RazlikaUCeni", typeof(int));
            dt.Columns.Add("ProdajnaVrednost", typeof(int));

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["RobaID"].Visible = false;

            dataGridView1.Columns["StavkaID"].Visible = false;

            dataGridView1.Columns["KatBr"].Width = 100;

            dataGridView1.Columns["Naziv"].Width = 300;

            dataGridView1.Columns["Kolicina"].Width = 50;

            dataGridView1.Columns["ProdajnaCena"].Width = 90;
            dataGridView1.Columns["ProdajnaCena"].HeaderText = "Prodajna Cena";
            dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

            dataGridView1.Columns["Rabat"].Width = 60;
            dataGridView1.Columns["Rabat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Rabat"].DefaultCellStyle.Format = "#,##0.00 \\%";

            dataGridView1.Columns["NabavnaCena"].Width = 90;
            dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna Cena";
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

            dataGridView1.Columns["Marza"].Width = 60;
            dataGridView1.Columns["Marza"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Marza"].DefaultCellStyle.Format = "#,##0.00 \\%";
            dataGridView1.Columns["Marza"].DefaultCellStyle.BackColor = Color.Aquamarine;

            dataGridView1.Columns["MaxRabat"].Width = 90;
            dataGridView1.Columns["MaxRabat"].HeaderText = "Max Rabat";
            dataGridView1.Columns["MaxRabat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["MaxRabat"].DefaultCellStyle.Format = "#,##0.00 \\%";
            dataGridView1.Columns["MaxRabat"].DefaultCellStyle.BackColor = Color.Aquamarine;

            dataGridView1.Columns["BuducaMarza"].Width = 90;
            dataGridView1.Columns["BuducaMarza"].HeaderText = "Budaca Marza";
            dataGridView1.Columns["BuducaMarza"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["BuducaMarza"].DefaultCellStyle.Format = "#,##0.00 \\%";
            dataGridView1.Columns["BuducaMarza"].DefaultCellStyle.BackColor = Color.Bisque;

            dataGridView1.Columns["BuduciRabat"].Width = 90;
            dataGridView1.Columns["BuduciRabat"].HeaderText = "Buduci Rabat";
            dataGridView1.Columns["BuduciRabat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["BuduciRabat"].DefaultCellStyle.Format = "#,##0.00 \\%";
            dataGridView1.Columns["BuduciRabat"].DefaultCellStyle.BackColor = Color.Bisque;

            dataGridView1.Columns["BuducaCena"].Width = 90;
            dataGridView1.Columns["BuducaCena"].HeaderText = "Buduca Cena";
            dataGridView1.Columns["BuducaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["BuducaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["BuducaCena"].DefaultCellStyle.BackColor = Color.Bisque;

            dataGridView1.Columns["Ima36"].Visible = false;

            dataGridView1.Columns["NabavnaVrednost"].Width = 120;
            dataGridView1.Columns["NabavnaVrednost"].HeaderText = "Nabavna Vrednost";
            dataGridView1.Columns["NabavnaVrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NabavnaVrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["NabavnaVrednost"].DefaultCellStyle.BackColor = Color.Aquamarine;

            dataGridView1.Columns["RazlikaUCeni"].Width = 120;
            dataGridView1.Columns["RazlikaUCeni"].HeaderText = "Razlika U Ceni";
            dataGridView1.Columns["RazlikaUCeni"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["RazlikaUCeni"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["RazlikaUCeni"].DefaultCellStyle.BackColor = Color.Aquamarine;

            dataGridView1.Columns["ProdajnaVrednost"].Width = 120;
            dataGridView1.Columns["ProdajnaVrednost"].HeaderText = "Prodajna Vrednost";
            dataGridView1.Columns["ProdajnaVrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["ProdajnaVrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["ProdajnaVrednost"].DefaultCellStyle.BackColor = Color.Aquamarine;

            ToggleUI(true);
        }

        private void rb_ProsecnaNabavnaCena_Click(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            this.cmb_PoDokumentu.SelectedIndex = 0;
            this.cmb_PoDokumentu.Enabled = this.rb_PoDokumentu.Checked;
            this.tb_BrDokPoDokumentu.Enabled = this.rb_PoDokumentu.Checked;
        }
        private void btnAnaliziraj_Click(object sender, EventArgs e)
        {
            if (this.cmb_VrstaDokumenta.SelectedIndex == 0) { MessageBox.Show("Niste izabrali vrstu dokumenta"); return; }

            int vrDok = Convert.ToInt32(cmb_VrstaDokumenta.SelectedValue);
            int brDok = Convert.ToInt32(tb_BrojDokumenta.Text);
            dokument = Dokument.Get(DateTime.Now.Year, vrDok, brDok);

            if (dokument.MagacinID != Program.TrenutniKorisnik.MagacinID && !Program.TrenutniKorisnik.ImaPravo(137001))
            {
                // Korisnik nema pravo rada sa svim magacinima, znaci ima samo sa svojim
                TDOffice.Pravo.NematePravoObavestenje(137001);
                return;
            }

            // Provera da li postoji dokument
            if (dokument == null) { MessageBox.Show("Dokument" + " nije pronadjen"); return; }

            if ((DateTime.Now - dokument.Datum).TotalDays > 3)
                MessageBox.Show("Dokument koji analizirate je stariji od 3 dana!");

            bool faliPodatak = false;
            DataTable stavkeDokumentaDT = new DataTable();

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                using (FbCommand cmd = new FbCommand(@"SELECT
                s.ROBAID, s.STAVKAID, r.KATBR, s.NAZIV, s.KOLICINA, (s.PRODCENABP + s.KOREKCIJA) as PRODAJNACENA, s.RABAT
                FROM STAVKA s
                LEFT OUTER JOIN ROBA r on s.ROBAID = r.ROBAID
                WHERE s.VRDOK = @VRDOK AND s.BRDOK = @BRDOK", con))
                {
                    cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                    cmd.Parameters.AddWithValue("@BRDOK", brDok);
                    using (FbDataAdapter da = new FbDataAdapter(cmd))
                    {
                        da.Fill(stavkeDokumentaDT);
                    }
                }
            }

            foreach (DataRow dr in stavkeDokumentaDT.Rows)
            {
                int robaID = Convert.ToInt32(dr["ROBAID"]);

                if (Math.Abs(Procedure.ProdajnaCenaNaDan(150, robaID, dokument.Datum) - Convert.ToDouble(dr["PRODAJNACENA"])) > 0.1)
                {
                    MessageBox.Show("U dokumentu postoje cene koje se ne slazu sa realnim cenama na taj dan!");
                    break;
                }
            }

            dt.Clear();

            double ukupnaVrednostDokumentaBezPopusta = 0;
            double ukupnaVrednostDokumentaSaPopustom = 0;
            double ukupnaNabavnaVrednostDokumenta = 0;

            List<Komercijalno.Dokument> dokumentiNabavke = Dokument.List($"MAGACINID = 150 AND VRDOK IN (0, 1, 2, 36)");
            List<Komercijalno.Stavka> stavkeNabavke = Stavka.List(DateTime.Now.Year, $"ROBAID IN (" + string.Join(", ", stavkeDokumentaDT.AsEnumerable().Select(p => p.Field<int>("ROBAID")).ToList())
                + $") AND MAGACINID = 150 AND VRDOK IN (0, 1, 2, 36)");

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                foreach (DataRow row in stavkeDokumentaDT.Rows)
                {
                    int robaID = Convert.ToInt32(row["ROBAID"]);
                    double prodajnaCena = Convert.ToDouble(row["PRODAJNACENA"]);
                    double nabavnaCena = rb_PoslednjaNavavnaCena.Checked
                        ? Komercijalno.Komercijalno.GetRealnaNabavnaCena(robaID, DateTime.Now, dokumentiNabavke, stavkeNabavke)
                        : Komercijalno.Komercijalno.GetProsecnaNabavnaCena(con, robaID, dtp_Od.Value, dtp_Do.Value);
                    double kolicina = Convert.ToDouble(row["KOLICINA"]);
                    double rabat = Convert.ToDouble(row["RABAT"]);
                    double prodajnaVrednostBezPopusta = prodajnaCena * kolicina;
                    double prodajnaVrednostSaPopustom = (prodajnaCena * (1 - (rabat / 100))) * kolicina;
                    double nabavnaVrednost = nabavnaCena * kolicina;
                    double marza = (prodajnaCena / nabavnaCena - 1) * 100;
                    double maxRabat = (nabavnaCena / prodajnaCena - 1) * -100;

                    ukupnaVrednostDokumentaBezPopusta += prodajnaVrednostBezPopusta;
                    ukupnaVrednostDokumentaSaPopustom += prodajnaVrednostSaPopustom;
                    ukupnaNabavnaVrednostDokumenta += nabavnaVrednost;

                    if (nabavnaCena == 0)
                        faliPodatak = true;

                    DataRow dr = dt.NewRow();
                    dr["RobaID"] = row["ROBAID"];
                    dr["StavkaID"] = row["STAVKAID"];
                    dr["KatBr"] = row["KATBR"];
                    dr["Naziv"] = row["NAZIV"];
                    dr["Kolicina"] = kolicina;
                    dr["ProdajnaCena"] = prodajnaCena;
                    dr["Rabat"] = rabat;
                    dr["NabavnaCena"] = nabavnaCena;
                    dr["Marza"] = marza;
                    dr["MaxRabat"] = maxRabat;
                    dr["BuducaMarza"] = 0;
                    dr["BuduciRabat"] = 0;
                    dr["BuducaCena"] = 0;
                    dr["Ima36"] = ImaVrDok36(con, robaID);
                    dr["NabavnaVrednost"] = nabavnaVrednost;
                    dr["RazlikaUCeni"] = prodajnaVrednostSaPopustom - nabavnaVrednost;
                    dr["ProdajnaVrednost"] = prodajnaVrednostSaPopustom;
                    dt.Rows.Add(dr);
                }
            }
            dataGridView1.DataSource = dt;

            nabavnaVrednost_txt.Text = ukupnaNabavnaVrednostDokumenta.ToString("#,##0.00 RSD");
            rucVrednost_txt.Text = (ukupnaVrednostDokumentaSaPopustom - ukupnaNabavnaVrednostDokumenta).ToString("#,##0.00 RSD");
            prodajnaVrednost_txt.Text = ukupnaVrednostDokumentaSaPopustom.ToString("#,##0.00 RSD");
            vrednostBezPopusta_txt.Text = ukupnaVrednostDokumentaBezPopusta.ToString("#,##0.00 RSD");

            tb_DatiRabat.Text = (((ukupnaVrednostDokumentaSaPopustom / ukupnaVrednostDokumentaBezPopusta) - 1) * (-100)).ToString("0.##");
            tb_Marza.Text = (((ukupnaVrednostDokumentaBezPopusta / ukupnaNabavnaVrednostDokumenta) - 1) * (100)).ToString("0.##");
            tb_PreracunataMarza.Text = (((ukupnaNabavnaVrednostDokumenta / ukupnaVrednostDokumentaBezPopusta) - 1) * (-100)).ToString("0.##");
            realizovanaMarza_txt.Text = (((ukupnaVrednostDokumentaSaPopustom / ukupnaNabavnaVrednostDokumenta) - 1) * (100)).ToString("0.##");

            gb_MarzaNaNivouDokumenta.BackColor = faliPodatak ? Color.Red : Color.Green;

            ObojiIma36();
        }
        private void ObojiIma36()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Ima36"].Value) == true)
                {
                    row.DefaultCellStyle.ForeColor = Color.Purple;
                    row.DefaultCellStyle.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Regular);
                }
            }
        }
        private bool ImaVrDok36(FbConnection con, int robaID)
        {
            using (FbCommand cmd = new FbCommand(@"
SELECT COUNT(s.VRDOK)
FROM STAVKA s
LEFT OUTER JOIN DOKUMENT d on s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
WHERE 
d.VRDOK = 36
AND d.DATUM >= @DATUM_OD
AND d.DATUM <= @DATUM_DO
AND s.ROBAID = @ROBAID
AND d.MAGACINID = 150
", con))
            {
                cmd.Parameters.AddWithValue("@ROBAID", robaID);
                cmd.Parameters.AddWithValue("@DATUM_OD", dtp_Od.Value);
                cmd.Parameters.AddWithValue("@DATUM_DO", dtp_Do.Value);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        if (Convert.ToInt32(dr[0]) > 0)
                            return true;
            }

            return false;
        }
        private void cmb_VrstaDokumenta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void tb_BrojDokumenta_TextChanged(object sender, EventArgs e)
        {

        }
        private void tb_BrojDokumenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyCode.IsNumber() && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void tb_BrDokPoDokumentu_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyCode.IsNumber() && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void btnFormirajCene_Click(object sender, EventArgs e)
        {
            using (_1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene u = new _1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene())
            {
                u.ShowDialog();
                double vrednost = u.returnValue;
                _1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene.PraviloDefinisanjaCene pravilo = u.returnPravilo;

                DataTable dtTemp = dt.Copy();

                foreach (DataRow dr in dtTemp.Rows)
                {
                    double nabavnaCena = Convert.ToDouble(dr["NabavnaCena"]);
                    double prodajnaCena = Convert.ToDouble(dr["ProdajnaCena"]);
                    double maxMoguciRabat = Convert.ToDouble(dr["MaxRabat"]);

                    double udeoRabata = maxMoguciRabat * (vrednost / 100);
                    if (LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat < udeoRabata)
                    {
                        udeoRabata = LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat;
                    }
                    if (nabavnaCena == 0 || prodajnaCena == 0)
                        continue;

                    double buducaCena = pravilo == _1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene.PraviloDefinisanjaCene.PlusZeljenaMarza ?
                    nabavnaCena * (1 + (double)(vrednost / 100)) :
                    prodajnaCena * ((double)1 - (udeoRabata / 100));

                    if ((((buducaCena / prodajnaCena) - 1) * -100) > LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat)
                    {
                        buducaCena = prodajnaCena * ((double)1 - (LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat / 100));
                    }

                    dr["BuducaMarza"] = ((buducaCena / nabavnaCena) - 1) * 100;
                    dr["BuduciRabat"] = ((buducaCena / prodajnaCena) - 1) * -100;
                    dr["BuducaCena"] = buducaCena;
                }

                dt = dtTemp;
                dataGridView1.DataSource = dt;

                ObojiIma36();
            }

            MessageBox.Show("Cene formirane!");
        }
        private void btn_UnesiRabat_Click(object sender, EventArgs e)
        {
            _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje.FormatRabata formatRabata = _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje.FormatRabata.Standard;

            using (_1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje formatiranje = new _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje())
            {
                formatiranje.ShowDialog();
                formatRabata = formatiranje.returnValue;
            }

            foreach (DataRow row in (dataGridView1.DataSource as DataTable).Rows)
            {
                if (Convert.ToDouble(row["ProdajnaCena"]) > 0 && Convert.ToDouble(row["NabavnaCena"]) > 0)
                {
                    int stavkaID;
                    try
                    {
                        stavkaID = Convert.ToInt32(row["StavkaID"]);
                        double buduciRabat = Math.Round(Convert.ToDouble(row["BuduciRabat"]), formatRabata == _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje.FormatRabata.BezDecimala ? 0 : 2);

                        Stavka s = Stavka.Get(DateTime.Now.Year, stavkaID);
                        s.Rabat = buduciRabat;
                        s.Update(DateTime.Now.Year);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Doslo je do greske prilikom unosa rabata za stavku: " + row["Naziv"]);
                    }
                }
            }

            dokument.Presaberi();

            btnAnaliziraj.PerformClick();
            MessageBox.Show("Rabat unet u dokument!");
        }
        private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = this.dataGridView1.CurrentRow;
            int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);

            if (_fmKarticaRobe.IsDisposed)
            {
                _fmKarticaRobe = new _7_fm_Komercijalno_Roba_Kartica();

                List<int> list = new List<int>();
                list.Add(1);
                list.Add(2);
                list.Add(36);

                _fmKarticaRobe.vidljiviVrDok = list;
            }

            _fmKarticaRobe.UcitajKarticu(robaID, 150);
            _fmKarticaRobe.TopMost = true;
            _fmKarticaRobe.Show();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_fmKarticaRobe.Visible)
                return;

            var row = this.dataGridView1.CurrentRow;
            int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);

            _fmKarticaRobe.UcitajKarticu(robaID, 150);
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!_fmKarticaRobe.Visible)
                return;

            var row = this.dataGridView1.CurrentRow;
            int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);

            _fmKarticaRobe.UcitajKarticu(robaID, 150);
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }
    }
}
