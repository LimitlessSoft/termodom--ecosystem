using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class _7_fm_Komercijalno_Roba_Kartica : Form
    {
        private int _robaID;
        private int _magacinID { get; set; }
        private Boolean _loaded { get; set; }

        private List<int> vrDokKojiPovecavajuStanje = new List<int>() {
            0,
            1,
            2,
            3,
            11,
            12,
            16,
            18,
            22,
            26,
            30,
            992
        };
        private List<int> vrDokKojiSmanjujuStanje = new List<int>() {
            13,
            14,
            15,
            17,
            19,
            23,
            28,
            29,
            35,
            993
        };

        public List<int> vidljiviVrDok = null;

        public bool KolonaVidljiva_ProdajnaCena { get; set; } = true;
        public bool KolonaVidljiva_NabavnaaCena { get; set; } = true;

        private Task<List<Komercijalno.Partner>> _komercijalnoPartneri = Komercijalno.Partner.ListAsync(DateTime.Now.Year);
        private Task<List<Komercijalno.Magacin>> _magacini = Task.Run(async () =>
        {
            List<Komercijalno.Magacin> list = await Komercijalno.Magacin.ListAsync();

            list.Add(new Komercijalno.Magacin()
            {
                ID = -1,
                Naziv = "Svi magacini"
            });

            return list;
        });
        private DataTable _dataTable { get; set; }
        private Task<Dictionary<int, List<Stavka>>> _komercijalnoStavke { get; set; }
        private Task<List<Dokument>> _komercijalnoDokumentiMagacina { get; set; }
        private Task<Termodom.Data.Entities.Komercijalno.VrstaDokDictionary> _komercijalnoVrstaDokumenta { get; set; } = VrstaDokManager.DictionaryAsync();

        public _7_fm_Komercijalno_Roba_Kartica()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(500001))
            {
                TDOffice.Pravo.NematePravoObavestenje(500001);
                this.Close();
                return;
            }
            InitializeComponent();
        }
        public _7_fm_Komercijalno_Roba_Kartica(int robaID, int magacinID)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(500001))
            {
                TDOffice.Pravo.NematePravoObavestenje(500001);
                this.Close();
                return;
            }
            InitializeComponent();

            _robaID = robaID;
            _magacinID = magacinID;
        }

        private void _7_fm_TDPopis_Kartica_Load(object sender, EventArgs e)
        {
            if (_robaID > 0 && _magacinID > 0)
                UcitajKarticu(_robaID, _magacinID);
            PodesiUI();
            _loaded = true;

            Task.Run(() =>
            {
                _magacini.Wait();

                this.Invoke((MethodInvoker)delegate
                {
                    magacin_cmb.DisplayMember = "Naziv";
                    magacin_cmb.ValueMember = "ID";
                    magacin_cmb.DataSource = _magacini.Result.OrderBy(x => x.ID).ToList();
                    magacin_cmb.SelectedValue = _magacinID;
                    magacin_cmb.Enabled = true;
                });
            });

            Task.Run(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    partner_cmb.Enabled = false;
                    partner_cmb.Items.Add("Ucitavanje...");
                    partner_cmb.SelectedIndex = 0;
                });

                _komercijalnoPartneri.Result.Add(new Komercijalno.Partner() { PPID = -1, Naziv = " <<< Izaberi Partnera >>> " });
                _komercijalnoPartneri.Result.RemoveAll(x => string.IsNullOrWhiteSpace(x.Naziv));
                _komercijalnoPartneri.Result.Sort((x, y) => x.PPID.CompareTo(y.PPID));

                this.Invoke((MethodInvoker)delegate
                {
                    partner_cmb.DisplayMember = "Naziv";
                    partner_cmb.ValueMember = "PPID";
                    partner_cmb.DataSource = _komercijalnoPartneri.Result;

                    partner_cmb.SelectedValue = -1;
                    partner_cmb.Enabled = true;
                });

            });
        }

        private void PodesiUI()
        {
            racunajVazecuNabavnuCenuToolStripMenuItem.Text = LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu ? "Nemoj Racunati Vazecu Nabavnu Cenu" : "Racunaj Vazecu Nabavnu Cenu";
            racunajVazecuNabavnuCenuToolStripMenuItem.BackColor = LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu ? Color.LightGreen : Color.Coral;

            boldujDokument36ToolStripMenuItem.Text = LocalSettings.Settings.KarticaRobe_BoldujDokument36 ? "Nemoj Dodatno Bojiti Dokument Ulazne Ponude 36" : "Dodatno Oboji Dokument Ulazne Ponude 36";
            boldujDokument36ToolStripMenuItem.BackColor = LocalSettings.Settings.KarticaRobe_BoldujDokument36 ? Color.LightGreen : Color.Coral;
        }

        private void Bolduj36()
        {
            if (!LocalSettings.Settings.KarticaRobe_BoldujDokument36)
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["VrDok"].Value) == 36)
                {
                    row.DefaultCellStyle.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                    row.DefaultCellStyle.ForeColor = Color.Purple;
                }
            }
        }

        public void UcitajKarticu(int robaID, int magacinID)
        {
            _robaID = robaID;
            _magacinID = magacinID;
            Roba roba = Roba.Get(DateTime.Now.Year, robaID);
            this.Text = roba == null ? "undefined" : roba.Naziv;

            _komercijalnoStavke = Task.Run(() =>
            {
                Dictionary<int, List<Stavka>> dict = new Dictionary<int, List<Stavka>>();
                dict[DateTime.Now.Year] = Stavka.List(DateTime.Now.Year, $"ROBAID = {robaID} AND MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");
                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year - 1]))
                {
                    con.Open();
                    dict[DateTime.Now.Year - 1] = Stavka.List(con, $"ROBAID = {robaID} AND MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");
                }
                return dict;
            });

            _komercijalnoDokumentiMagacina = Task.Run(() =>
            {
                List<Dokument> all = Dokument.List($"MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");

                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year - 1]))
                {
                    con.Open();
                    all.AddRange(Dokument.List(con, $"MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)"));
                }

                List<Dokument> doks = new List<Dokument>();

                foreach (int godina in _komercijalnoStavke.Result.Keys)
                    foreach (Stavka s in _komercijalnoStavke.Result[godina])
                        doks.Add(all.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum.Year == godina));

                return doks;
            });

            DataTable dt = new DataTable();
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("VrDok", typeof(int));
            dt.Columns.Add("Dokument", typeof(string));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("Kolicina", typeof(double));
            dt.Columns.Add("Stanje", typeof(double));
            dt.Columns.Add("NabavnaCena", typeof(double));

            if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
                dt.Columns.Add("PoslednjaNabavnaCena", typeof(double));

            dt.Columns.Add("ProdajnaCena", typeof(double));
            dt.Columns.Add("Dobavljac", typeof(string));
            dt.Columns.Add("InterniKomentar", typeof(string));
            dt.Columns.Add("Komentar", typeof(string));
            dt.Columns.Add("PPID", typeof(int));

            double trenutnoStanje = 0;
            int partnerID = 0;
            string DobavljacNaziv = "N";

            foreach (int godina in _komercijalnoStavke.Result.Keys)
            {
                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godina]))
                {
                    con.Open();
                    foreach (Stavka s in _komercijalnoStavke.Result[godina])
                    {
                        Komercijalno.Dokument dok = _komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x != null && x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina);
                        if (dok == null)
                            continue;

                        Komentari komentari = Komentari.Get(con, dok.VrDok, dok.BrDok);
                        partnerID = Convert.ToInt32(dok.PPID);

                        Partner PPNaziv = _komercijalnoPartneri.Result.FirstOrDefault(x => x.PPID == partnerID);
                        if (PPNaziv == null)
                            DobavljacNaziv = "Nepoznat";
                        else
                            DobavljacNaziv = PPNaziv.Naziv;

                        // Ukoliko je dokument storniram ne diram trenutno stanje
                        trenutnoStanje += s.Kolicina * (_komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x != null && x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina).KodDok == 0 ?
                            (vrDokKojiPovecavajuStanje.Contains(s.VrDok) ?
                                1 :
                                vrDokKojiSmanjujuStanje.Contains(s.VrDok) ?
                                    -1 :
                                    0)
                                : 0);

                        DataRow row = dt.NewRow();
                        DateTime dtemp = _komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina).Datum;
                        DateTime datum = new DateTime(dtemp.Year, dtemp.Month, dtemp.Day, s.VrDok == 7 ? 0 : 1, 0, 0);
                        row["Datum"] = datum;
                        row["VrDok"] = s.VrDok;
                        row["Dokument"] = _komercijalnoVrstaDokumenta.Result[s.VrDok].NazivDok + (s.VrDok == 36 ? " ( " + ((DateTime)dok.DatRoka).ToString("dd.MM.yyyy") + " ) " : "");
                        row["BrDok"] = s.BrDok;
                        row["Kolicina"] = s.Kolicina;
                        row["Stanje"] = trenutnoStanje;
                        row["NabavnaCena"] = s.NabavnaCena;
                        row["ProdajnaCena"] = s.ProdCenaBP == 0 ? s.ProdajnaCena : (s.ProdCenaBP + (s.Korekcija == null ? 0 : s.Korekcija));
                        row["Dobavljac"] = DobavljacNaziv;

                        if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
                            row["PoslednjaNabavnaCena"] = GetRealnaNabavnaCena(dok.Datum);

                        row["InterniKomentar"] = komentari == null ? "" : komentari.IntKomentar;
                        row["Komentar"] = komentari == null ? "" : komentari.Komentar;
                        row["PPID"] = partnerID;

                        dt.Rows.Add(row);
                    }
                }
            }

            if (vidljiviVrDok != null)
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    if (!vidljiviVrDok.Contains(Convert.ToInt32(dt.Rows[i]["VrDok"])))
                        dt.Rows.RemoveAt(i);

            dataGridView1.DataSource = dt;
            _dataTable = dt;

            if (dataGridView1.Rows.Count == 0)
                return;

            dataGridView1.Sort(this.dataGridView1.Columns["Datum"], ListSortDirection.Ascending);

            dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView1.Columns["Datum"].Width = 80;
            dataGridView1.Columns["VrDok"].Visible = false;
            dataGridView1.Columns["BrDok"].Width = 80;

            dataGridView1.Columns["Dokument"].Width = 200;

            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["Stanje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Stanje"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["NabavnaCena"].Width = 90;
            dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna Cena";
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

            if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
            {
                dataGridView1.Columns["PoslednjaNabavnaCena"].Visible = true;
                dataGridView1.Columns["PoslednjaNabavnaCena"].Width = 90;
                dataGridView1.Columns["PoslednjaNabavnaCena"].HeaderText = "Vazeca Nabavna Cena";
                dataGridView1.Columns["PoslednjaNabavnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["PoslednjaNabavnaCena"].DefaultCellStyle.Format = "#,##0.00";
            }

            dataGridView1.Columns["Dobavljac"].Width = 200;

            if (KolonaVidljiva_NabavnaaCena)
            {
                dataGridView1.Columns["ProdajnaCena"].Width = 90;
                dataGridView1.Columns["ProdajnaCena"].HeaderText = "Prodajna Cena";
                dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";
            }
            else
            {
                dataGridView1.Columns["ProdajnaCena"].Visible = false;
            }

            Bolduj36();
            ObojiPeriodePoslednjihNabavnihCena();
            Tarife t = Tarife.Get(roba.TarifaID);

            DataTable cene = new DataTable();
            cene.Columns.Add("Magacin", typeof(string));
            cene.Columns.Add("CenaBezPDV", typeof(double));
            cene.Columns.Add("CenaSaPDV", typeof(double));

            DataRow dr50 = cene.NewRow();
            DataRow drMag = cene.NewRow();

            double pc50 = Procedure.ProdajnaCenaNaDan(50, robaID, DateTime.Now);
            double pcMag = Procedure.ProdajnaCenaNaDan(magacinID, robaID, DateTime.Now);
            bool oduzimajPDVSaPcMag = _magacini.Result.FirstOrDefault(x => x.ID == magacinID).Vrsta == 2;

            dr50["Magacin"] = "VP Mag: 50";
            dr50["CenaBezPDV"] = pc50;
            dr50["CenaSaPDV"] = pc50 * (double)(1 + ((double)t.Stopa / (double)100));

            drMag["Magacin"] = _magacini.Result.FirstOrDefault(x => x.ID == magacinID).Naziv;
            drMag["CenaBezPDV"] = oduzimajPDVSaPcMag ? ((double)(pcMag * (1 - (double)((double)t.Stopa / (double)(100 + t.Stopa))))) : pcMag;
            drMag["CenaSaPDV"] = pcMag;

            cene.Rows.Add(dr50);
            cene.Rows.Add(drMag);

            cene_dgv.DataSource = cene;
            cene_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            cene_dgv.Columns["CenaBezPDV"].HeaderText = "Cena Bez PDV";
            cene_dgv.Columns["CenaBezPDV"].DefaultCellStyle.Format = "#,##0.00 RSD";

            cene_dgv.Columns["CenaSaPDV"].HeaderText = "Cena Sa PDV";
            cene_dgv.Columns["CenaSaPDV"].DefaultCellStyle.Format = "#,##0.00 RSD";
        }

        public void UcitajKarticuMagacinID(int robaID, int magacinID)
        {
            Roba roba = Roba.Get(DateTime.Now.Year, robaID);
            this.Text = roba == null ? "undefined" : roba.Naziv;

            _komercijalnoStavke = Task.Run(() =>
            {
                Dictionary<int, List<Stavka>> dict = new Dictionary<int, List<Stavka>>();
                dict[DateTime.Now.Year] = Stavka.List(DateTime.Now.Year, $"ROBAID = {robaID} AND MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");
                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year - 1]))
                {
                    con.Open();
                    dict[DateTime.Now.Year - 1] = Stavka.List(con, $"ROBAID = {robaID} AND MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");
                }
                return dict;
            });

            _komercijalnoDokumentiMagacina = Task.Run(() =>
            {
                List<Dokument> all = Dokument.List($"MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)");

                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year - 1]))
                {
                    con.Open();
                    all.InsertRange(0, Dokument.List(con, $"MAGACINID = {magacinID} AND VRDOK IN (0, 1, 2, 7, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 25, 26, 27, 28, 29, 30, 35, 36)"));
                }

                List<Dokument> doks = new List<Dokument>();

                foreach (int godina in _komercijalnoStavke.Result.Keys)
                    foreach (Stavka s in _komercijalnoStavke.Result[godina])
                        doks.Add(all.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum.Year == godina));

                return doks;
            });

            DataTable dt = new DataTable();
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("VrDok", typeof(int));
            dt.Columns.Add("Dokument", typeof(string));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("Kolicina", typeof(double));
            dt.Columns.Add("Stanje", typeof(double));
            dt.Columns.Add("NabavnaCena", typeof(double));

            if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
                dt.Columns.Add("PoslednjaNabavnaCena", typeof(double));

            dt.Columns.Add("ProdajnaCena", typeof(double));
            dt.Columns.Add("Dobavljac", typeof(string));
            dt.Columns.Add("InterniKomentar", typeof(string));
            dt.Columns.Add("Komentar", typeof(string));
            dt.Columns.Add("PPID", typeof(int));

            double trenutnoStanje = 0;
            int partnerID = 0;
            string DobavljacNaziv = "N";

            foreach (int godina in _komercijalnoStavke.Result.Keys)
            {
                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godina]))
                {
                    con.Open();
                    foreach (Stavka s in _komercijalnoStavke.Result[godina])
                    {
                        Komercijalno.Dokument dok = _komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina);
                        Komentari komentari = Komentari.Get(con, dok.VrDok, dok.BrDok);
                        partnerID = Convert.ToInt32(dok.PPID);

                        Partner PPNaziv = _komercijalnoPartneri.Result.FirstOrDefault(x => x.PPID == partnerID);
                        if (PPNaziv == null)
                            DobavljacNaziv = "Nepoznat";
                        else
                            DobavljacNaziv = PPNaziv.Naziv;

                        // Ukoliko je dokument storniram ne diram trenutno stanje
                        trenutnoStanje += s.Kolicina * (_komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina).KodDok == 0 ?
                            (vrDokKojiPovecavajuStanje.Contains(s.VrDok) ?
                                1 :
                                vrDokKojiSmanjujuStanje.Contains(s.VrDok) ?
                                    -1 :
                                    0)
                                : 0);

                        DataRow row = dt.NewRow();
                        DateTime dtemp = _komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok && x.Datum != null && x.Datum.Year == godina).Datum;
                        DateTime datum = new DateTime(dtemp.Year, dtemp.Month, dtemp.Day, s.VrDok == 7 ? 0 : 1, 0, 0);
                        row["Datum"] = datum;
                        row["VrDok"] = s.VrDok;
                        row["Dokument"] = _komercijalnoVrstaDokumenta.Result[s.VrDok].NazivDok + (s.VrDok == 36 ? " ( " + ((DateTime)dok.DatRoka).ToString("dd.MM.yyyy") + " ) " : "");
                        row["BrDok"] = s.BrDok;
                        row["Kolicina"] = s.Kolicina;
                        row["Stanje"] = trenutnoStanje;
                        row["NabavnaCena"] = s.NabavnaCena;
                        row["ProdajnaCena"] = s.ProdCenaBP == 0 ? s.ProdajnaCena : (s.ProdCenaBP + (s.Korekcija == null ? 0 : s.Korekcija));
                        row["Dobavljac"] = DobavljacNaziv;

                        if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
                            row["PoslednjaNabavnaCena"] = GetRealnaNabavnaCena(dok.Datum);

                        row["InterniKomentar"] = komentari == null ? "" : komentari.IntKomentar;
                        row["Komentar"] = komentari == null ? "" : komentari.Komentar;
                        row["PPID"] = partnerID;

                        dt.Rows.Add(row);
                    }
                }
            }

            if (vidljiviVrDok != null)
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    if (!vidljiviVrDok.Contains(Convert.ToInt32(dt.Rows[i]["VrDok"])))
                        dt.Rows.RemoveAt(i);

            dataGridView1.DataSource = dt;
            _dataTable = dt;

            if (dataGridView1.Rows.Count == 0)
                return;

            dataGridView1.Sort(this.dataGridView1.Columns["Datum"], ListSortDirection.Ascending);

            dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView1.Columns["Datum"].Width = 80;

            dataGridView1.Columns["VrDok"].Visible = false;
            dataGridView1.Columns["BrDok"].Width = 80;

            dataGridView1.Columns["Dokument"].Width = 200;

            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["Stanje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Stanje"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["NabavnaCena"].Width = 90;
            dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna Cena";
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

            if (LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
            {
                dataGridView1.Columns["PoslednjaNabavnaCena"].Visible = true;
                dataGridView1.Columns["PoslednjaNabavnaCena"].Width = 90;
                dataGridView1.Columns["PoslednjaNabavnaCena"].HeaderText = "Vazeca Nabavna Cena";
                dataGridView1.Columns["PoslednjaNabavnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["PoslednjaNabavnaCena"].DefaultCellStyle.Format = "#,##0.00";
            }

            dataGridView1.Columns["Dobavljac"].Width = 200;

            if (KolonaVidljiva_NabavnaaCena)
            {
                dataGridView1.Columns["ProdajnaCena"].Width = 90;
                dataGridView1.Columns["ProdajnaCena"].HeaderText = "Prodajna Cena";
                dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";
            }
            else
            {
                dataGridView1.Columns["ProdajnaCena"].Visible = false;
            }

            Bolduj36();
            ObojiPeriodePoslednjihNabavnihCena();
            Tarife t = Tarife.Get(roba.TarifaID);

            DataTable cene = new DataTable();
            cene.Columns.Add("Magacin", typeof(string));
            cene.Columns.Add("CenaBezPDV", typeof(double));
            cene.Columns.Add("CenaSaPDV", typeof(double));

            DataRow dr50 = cene.NewRow();
            DataRow drMag = cene.NewRow();

            double pc50 = Procedure.ProdajnaCenaNaDan(50, robaID, DateTime.Now);
            double pcMag = Procedure.ProdajnaCenaNaDan(magacinID, robaID, DateTime.Now);
            bool oduzimajPDVSaPcMag = _magacini.Result.FirstOrDefault(x => x.ID == magacinID).Vrsta == 2;

            dr50["Magacin"] = "VP Mag: 50";
            dr50["CenaBezPDV"] = pc50;
            dr50["CenaSaPDV"] = pc50 + (pc50 * (double)((double)t.Stopa / (double)(100 + t.Stopa)));

            drMag["Magacin"] = _magacini.Result.FirstOrDefault(x => x.ID == magacinID).Naziv;
            drMag["CenaBezPDV"] = pcMag;
            drMag["CenaSaPDV"] = oduzimajPDVSaPcMag ? ((double)(pcMag * (1 - (double)((double)t.Stopa / (double)(100 + t.Stopa))))) : pcMag;

            cene.Rows.Add(dr50);
            cene.Rows.Add(drMag);

            cene_dgv.DataSource = cene;
            cene_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            cene_dgv.Columns["CenaBezPDV"].HeaderText = "Cena Bez PDV";
            cene_dgv.Columns["CenaBezPDV"].DefaultCellStyle.Format = "#,##0.00 RSD";

            cene_dgv.Columns["CenaSaPDV"].HeaderText = "Cena Sa PDV";
            cene_dgv.Columns["CenaSaPDV"].DefaultCellStyle.Format = "#,##0.00 RSD";
        }

        private double GetRealnaNabavnaCena(DateTime datum)
        {
            Dokument dokument36 = _komercijalnoDokumentiMagacina.Result.FirstOrDefault(x => x.VrDok == 36 && datum >= x.Datum && datum <= x.DatRoka);

            if (dokument36 != null)
                return _komercijalnoStavke.Result[dokument36.Datum.Year].FirstOrDefault(x => x.VrDok == dokument36.VrDok && x.BrDok == dokument36.BrDok).NabavnaCena;

            List<Dokument> dokumentiKojiDolazeUObzir = new List<Dokument>(_komercijalnoDokumentiMagacina.Result);
            dokumentiKojiDolazeUObzir.RemoveAll(x => x.Datum > datum || !new int[] { 0, 1, 2, 3 }.Contains(x.VrDok));
            dokumentiKojiDolazeUObzir.Sort((y, x) => x.Datum.CompareTo(y.Datum));

            Dokument vazeciDokumentNabavke = dokumentiKojiDolazeUObzir.FirstOrDefault();

            return _komercijalnoStavke.Result[vazeciDokumentNabavke.Datum.Year].FirstOrDefault(x => x.VrDok == vazeciDokumentNabavke.VrDok && x.BrDok == vazeciDokumentNabavke.BrDok).NabavnaCena;
        }
        private void ObojiPeriodePoslednjihNabavnihCena()
        {
            if (!LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu)
                return;

            double poslednjaPoslednjaNabavnaCena = -1;
            Color currColor = Color.FromArgb(Random.Next(125, 255), Random.Next(125, 255), Random.Next(125, 255));
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                double tnc = Convert.ToDouble(row.Cells["PoslednjaNabavnaCena"].Value);
                if (poslednjaPoslednjaNabavnaCena != tnc)
                {
                    poslednjaPoslednjaNabavnaCena = tnc;
                    currColor = Color.FromArgb(Random.Next(125, 255), Random.Next(125, 255), Random.Next(125, 255));
                }

                row.Cells["PoslednjaNabavnaCena"].Style.BackColor = currColor;
            }
        }

        private void racunajVazecuNabavnuCenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu = !LocalSettings.Settings.KarticaRobe_RacunajVazecuNabavnuCenu;
            LocalSettings.Update();
            PodesiUI();
            MessageBox.Show("Izmene su primenjene i vazece od sledeceg otvaranja kartice robe!");
        }

        private void boldujDokument36ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalSettings.Settings.KarticaRobe_BoldujDokument36 = !LocalSettings.Settings.KarticaRobe_BoldujDokument36;
            LocalSettings.Update();
            PodesiUI();
            ObojiPeriodePoslednjihNabavnihCena();
            MessageBox.Show("Zavrseno!");
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiPeriodePoslednjihNabavnihCena();
        }

        private void _7_fm_Komercijalno_Roba_Kartica_Shown(object sender, EventArgs e)
        {
            ObojiPeriodePoslednjihNabavnihCena();
        }

        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!magacin_cmb.Enabled)
                return;

            partner_cmb.SelectedValue = -1;
            if (Convert.ToInt32(magacin_cmb.SelectedValue) == -1)
            {
                UcitajKarticu(_robaID, 50);
                return;
            }
            _magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
            UcitajKarticu(_robaID, _magacinID);
        }

        private void partner_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;
            if (Convert.ToInt32(partner_cmb.SelectedValue) == -1)
            {
                UcitajKarticu(_robaID, _magacinID);
                return;
            }
            string selectString = "";
            int ppid = Convert.ToInt32(partner_cmb.SelectedValue);
            selectString = " PPID =" + ppid;
            DataRow[] rows = _dataTable.Copy().Select(selectString);
            DataTable selectTable = new DataTable();
            selectTable = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();
            dataGridView1.DataSource = selectTable;
        }

        private void prikaziKomentarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kom = dataGridView1.SelectedRows[0].Cells["Komentar"].Value.ToStringOrDefault();

            if (string.IsNullOrWhiteSpace(kom))
                MessageBox.Show("Dokument nema komentar!");
            else
                MessageBox.Show(kom);
        }

        private void prikaziInterniKomentarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kom = dataGridView1.SelectedRows[0].Cells["InterniKomentar"].Value.ToStringOrDefault();

            if (string.IsNullOrWhiteSpace(kom))
                MessageBox.Show("Dokument nema interni komentar!");
            else
                MessageBox.Show(kom);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            button1.BackColor = !this.TopMost ? Color.Red : Color.Green;
        }
    }
}
