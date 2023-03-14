using FirebirdSql.Data.FirebirdClient;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.PDFSharpARExtensions;


namespace TDOffice_v2
{
    public partial class _7_fm_TDPopis_Index : Form
    {
        #region Properties
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        private fm_LagerListaStavki_Index _lagerListaForm { get; set; } = null;
        private Task<fm_Help> _helpFrom { get; set; }
        private TDOffice.DokumentPopis _TDOffice_popis { get; set; }
        private Komercijalno.Dokument _Komercijalno_popis { get; set; }
        private Komercijalno.Dokument _Komercijalno_narudzbenica { get; set; }
        private Task<List<Komercijalno.Roba>> _komercijalno_roba { get; set; }
        private Task<List<Komercijalno.RobaUMagacinu>> _komercijalno_robaUMagacinu { get; set; }

        private Task<List<Komercijalno.Stavka>> _komercijalno_stavka_za_magacin { get; set; }
        private Task<List<Komercijalno.Stavka>> _komercijalno_popis_stavke { get; set; }
        private Task<List<Komercijalno.Stavka>> _komercijalno_narudzbenica_stavke { get; set; }
        private Task<List<Komercijalno.Dokument>> _komercijalno_dokumenti_za_magacin { get; set; }

        private List<Tuple<int, double>> _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice { get; set; }
        private List<Tuple<int, double>> _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice { get; set; }
        private Task<List<Komercijalno.Dokument>> _komercijalno_izlazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice { get; set; }
        private Task<List<Komercijalno.Dokument>> _komercijalno_ulazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice { get; set; }

        private bool _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice_loaded { get; set; } = false;
        private bool _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice_loaded { get; set; } = false;
        private bool _suspendValidation { get; set; } = true;
        private bool _loaded { get; set; } = false;

        private DataTable currentDgvData;

        private DataTable _dgvDT2;

        private IzborRobe _ir { get; set; }
        private bool _izborRobePrikazan = false;
        private List<string> _log = new List<string>();
        private _7_fm_Komercijalno_Roba_Kartica _fmKarticaRobe { get; set; } = null;
        private ManualResetEventSlim _karticaRobeInitializedMRE { get; set; } = new ManualResetEventSlim(false);
        #endregion

        #region MRE
        #endregion

        public static List<int> izlazniVrDok { get; set; } = new List<int>()
                {
                    15,
                    19,
                    17,
                    23,
                    13,
                    14,
                    27,
                    25,
                    28
                };
        public static List<int> ulazniVrDok { get; set; } = new List<int>()
                {
                    0,
                    11,
                    18,
                    16,
                    22,
                    1,
                    2,
                    26,
                    29,
                    30
                };

        private Task<List<TDOffice.StavkaPopis>> _stavke;

        public _7_fm_TDPopis_Index(TDOffice.DokumentPopis popis)
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Popis_Index);
            _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(popis.MagacinID);

            dataGridView1.Size = new Size(panel2.Width - 10, panel2.Height - groupBox1.Height - 20);
            detaljnaAnalizaToggleToolStripMenuItem1.Text = LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu ? "Sakrij detaljnu analizu" : "Prikazi detaljnu analizu";

            _TDOffice_popis = popis;

            Task.Run(() =>
            {
                if (!Program.TrenutniKorisnik.ImaPravo(500001))
                {
                    _karticaRobeInitializedMRE.Set();
                    return;
                }

                _fmKarticaRobe = new _7_fm_Komercijalno_Roba_Kartica();
                _karticaRobeInitializedMRE.Set();
            });

            _komercijalno_roba = Komercijalno.Roba.ListAsync(DateTime.Now.Year);

        }

        private void _7_fm_TDPopis_Index_Load(object sender, EventArgs e)
        {
            ReloadUI();
            ReloadStavki();
            SetupDGV();
            ColorDGV();
            SetDGV2();
            _komercijalno_stavka_za_magacin = Komercijalno.Stavka.ListByMagacinIDAsync(_TDOffice_popis.MagacinID);
            _komercijalno_dokumenti_za_magacin = Komercijalno.Dokument.ListByMagacinIDAsync(_TDOffice_popis.Datum.Year, _TDOffice_popis.MagacinID);
            _komercijalno_izlazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice = Task.Run(() =>
            {
                return _komercijalno_dokumenti_za_magacin.Result.Where(x => izlazniVrDok.Contains(x.VrDok) && x.Datum >= _Komercijalno_popis.Datum && x.MagacinID == _TDOffice_popis.MagacinID).ToList();
            });
            _komercijalno_ulazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice = Task.Run(() =>
            {
                return _komercijalno_dokumenti_za_magacin.Result.Where(x => x.VrDok != 0 && ulazniVrDok.Contains(x.VrDok) && x.Datum >= _Komercijalno_popis.Datum && x.MagacinID == _TDOffice_popis.MagacinID).ToList();
            });
            LoadUkupniIzlazSvihStavkiRelativnoAsync();
            LoadUkupniUlazSvihStavkiRelativnoAsync();

            if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok == null)
            {
                popisKomercijalnoNarudzbenicaDatum_txt.Visible = false;
                komercijalnoNarudzbenica_txt.Visible = false;
                label2.Visible = false;
            }
            if (Program.TrenutniKorisnik.ImaPravo(700004))
                unosStavkeTipUnosa_cmb.Checked = LocalSettings.Settings.TDPopis_PrilikomUnosaStavkeUnosimNarucenuKolicinu;
            else
                unosStavkeTipUnosa_cmb.Visible = false;

            NamestiUIPoZaduzenju();

            _loaded = true;
        }

        private void SetDGV2()
        {
            _dgvDT2 = new DataTable();
            _dgvDT2.Columns.Add("Parametar", typeof(string));
            _dgvDT2.Columns.Add("Prosek3Meseca", typeof(string));
            _dgvDT2.Columns.Add("Prosek1Mesec", typeof(string));

            dataGridView2.DataSource = _dgvDT2;

            dataGridView2.Columns["Parametar"].Width = 140;
            dataGridView2.Columns["Parametar"].HeaderText = "Parametar";
            dataGridView2.Columns["Parametar"].ReadOnly = true;

            dataGridView2.Columns["Prosek3Meseca"].Width = 120;
            dataGridView2.Columns["Prosek3Meseca"].HeaderText = "Prosek 3 meseca";
            dataGridView2.Columns["Prosek3Meseca"].ReadOnly = true;

            dataGridView2.Columns["Prosek1Mesec"].Width = 120;
            dataGridView2.Columns["Prosek1Mesec"].HeaderText = "Prosek 1 mesec";
            dataGridView2.Columns["Prosek1Mesec"].ReadOnly = true;

            if (_loaded)
                return;

            DataRow dr = _dgvDT2.NewRow();
            dr["Parametar"] = "Prosecno izaslo mesecno";
            dr["Prosek3Meseca"] = "0.00";
            dr["Prosek1Mesec"] = "0.00";
            _dgvDT2.Rows.Add(dr);

            DataRow dr1 = _dgvDT2.NewRow();
            dr1["Parametar"] = "MAX 15 + 19";
            dr1["Prosek3Meseca"] = "0.00";
            dr1["Prosek1Mesec"] = "0.00";
            _dgvDT2.Rows.Add(dr1);

            DataRow dr2 = _dgvDT2.NewRow();
            dr2["Parametar"] = "Prosecno prodato mesecno";
            dr2["Prosek3Meseca"] = "0.00";
            dr2["Prosek1Mesec"] = "0.00";
            _dgvDT2.Rows.Add(dr2);
        }
        private void SetupDGV()
        {
            dataGridView1.Columns["StavkaID"].Visible = false;
            dataGridView1.Columns["RobaID"].Visible = false;

            dataGridView1.Columns["KatBr"].Width = 130;
            dataGridView1.Columns["KatBr"].HeaderText = "Kat. Br.";
            dataGridView1.Columns["KatBr"].ReadOnly = true;

            dataGridView1.Columns["KatBrPro"].Width = 130;
            dataGridView1.Columns["KatBrPro"].HeaderText = "Kat. Br.";
            dataGridView1.Columns["KatBrPro"].ReadOnly = true;

            dataGridView1.Columns["Naziv"].Width = 300;
            dataGridView1.Columns["Naziv"].ReadOnly = true;

            dataGridView1.Columns["KomercijalnoPopisStavkaID"].Visible = false;
            dataGridView1.Columns["KomercijalnoNarudzbenicaStavkaID"].Visible = false;

            dataGridView1.Columns["KomercijalnoPopisKolicina"].Width = 80;
            dataGridView1.Columns["KomercijalnoPopisKolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["KomercijalnoPopisKolicina"].HeaderText = "(1) Komercijalno Popis";
            dataGridView1.Columns["KomercijalnoPopisKolicina"].DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;
            dataGridView1.Columns["KomercijalnoPopisKolicina"].ReadOnly = true;

            dataGridView1.Columns["Kolicina"].Width = 80;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["Kolicina"].HeaderText = "(2) Zatecena kolicina";
            dataGridView1.Columns["Kolicina"].ReadOnly = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? false : true;
            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan)
                dataGridView1.Columns["Kolicina"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

            dataGridView1.Columns["Skart"].Width = 80;
            dataGridView1.Columns["Skart"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Skart"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["Skart"].HeaderText = "(3) Skart";
            dataGridView1.Columns["Skart"].ReadOnly = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? false : true;
            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan)
                dataGridView1.Columns["Skart"].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;

            dataGridView1.Columns["PorucenaKolicina"].Width = 80;
            dataGridView1.Columns["PorucenaKolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["PorucenaKolicina"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["PorucenaKolicina"].HeaderText = "(4) Porucena kolicina";
            dataGridView1.Columns["PorucenaKolicina"].Visible = _TDOffice_popis.KomercijalnoNarudzbenicaBrDok == null ? false : true;
            dataGridView1.Columns["PorucenaKolicina"].ReadOnly = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? false : true;
            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan)
                dataGridView1.Columns["PorucenaKolicina"].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;

            dataGridView1.Columns["JM"].Width = 50;
            dataGridView1.Columns["JM"].HeaderText = "JM";
            dataGridView1.Columns["JM"].ReadOnly = true;

            dataGridView1.Columns["Cena"].Width = 80;
            dataGridView1.Columns["Cena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Cena"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["Cena"].HeaderText = "(5) Cena na dan " + _Komercijalno_popis.Datum.ToString("dd.MM.yyyy");
            dataGridView1.Columns["Cena"].ReadOnly = true;

            dataGridView1.Columns["VrednostPR"].Width = 130;
            dataGridView1.Columns["VrednostPR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["VrednostPR"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["VrednostPR"].DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;
            dataGridView1.Columns["VrednostPR"].HeaderText = "(6) Vrednost Porucene Robe [1 * 4]";
            dataGridView1.Columns["VrednostPR"].ReadOnly = true;
            dataGridView1.Columns["VrednostPR"].Visible = _Komercijalno_narudzbenica == null ? false : true;

            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].Width = 80;
            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].HeaderText = "(7) Stanje na dan " + _Komercijalno_popis.Datum.ToString("dd.MM.yyyy");
            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].ReadOnly = true;
            dataGridView1.Columns["StanjeNaDanPopisaKomercijalno"].Visible = Program.TrenutniKorisnik.ImaPravo(700801) ? true : false;

            dataGridView1.Columns["Razlika"].Width = 80;
            dataGridView1.Columns["Razlika"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Razlika"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["Razlika"].HeaderText = "(8) Razlika [1 - 7]";
            dataGridView1.Columns["Razlika"].ReadOnly = true;
            dataGridView1.Columns["Razlika"].Visible = Program.TrenutniKorisnik.ImaPravo(700802) ? true : false;

            dataGridView1.Columns["VrednostRazlike"].Width = 130;
            dataGridView1.Columns["VrednostRazlike"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["VrednostRazlike"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["VrednostRazlike"].HeaderText = "(9) Vrednost Razlike [5 * 8]";
            dataGridView1.Columns["VrednostRazlike"].ReadOnly = true;
            dataGridView1.Columns["VrednostRazlike"].Visible = Program.TrenutniKorisnik.ImaPravo(700803) ? true : false;

            foreach (DataColumn col in (dataGridView1.DataSource as DataTable).Columns)
                comboBox1.Items.Add(col.ColumnName);

            comboBox1.SelectedItem = "Naziv";

        }
        private void ColorDGV()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700803))
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (Convert.ToDouble(row.Cells["Razlika"].Value) > 0)
                        row.Cells["Razlika"].Style.ForeColor = System.Drawing.Color.Green;
                    else
                        row.Cells["Razlika"].Style.ForeColor = System.Drawing.Color.Red;
                }
                catch (Exception)
                {

                }
            }
        }
        private void ReloadUI()
        {
            analitikaStavke_gbx.Visible = Program.TrenutniKorisnik.ImaPravo(700810) ? LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu : false;
            dataGridView1.Height = panel2.Height - panel3.Height - 6;

            this.dokumentStatus_btn.Text = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? "Zakljucaj" : "Otkljucaj";
            this.BackColor = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            this.referent_txt.Text = _korisnici.Result.Where(x => x.ID == _TDOffice_popis.UserID).FirstOrDefault().Username;

            if (_TDOffice_popis.KomercijalnoPopisBrDok != null)
            {
                _Komercijalno_popis = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 7, (int)_TDOffice_popis.KomercijalnoPopisBrDok);
                if (_Komercijalno_popis != null)
                    this.popisKomercijalnoPopisDatum_txt.Text = _Komercijalno_popis.Datum.ToString("dd.MM.yyyy");
                else
                    this.popisKomercijalnoPopisDatum_txt.Text = "Greska";
            }

            if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok != null)
            {
                _Komercijalno_narudzbenica = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 33, (int)_TDOffice_popis.KomercijalnoNarudzbenicaBrDok);
                if (_Komercijalno_narudzbenica != null)
                    this.popisKomercijalnoNarudzbenicaDatum_txt.Text = _Komercijalno_narudzbenica.Datum.ToString("dd.MM.yyyy");
                else
                    this.popisKomercijalnoPopisDatum_txt.Text = "Greska";
            }

            this.napomena_txt.Text = _TDOffice_popis.Napomena.ToStringOrDefault();

            btn_Sacuvaj.BackColor = TDColor.ControlInactive;
            btn_OdbaciIzmene.BackColor = TDColor.ControlInactive;

            btn_Sacuvaj.Enabled = false;
            btn_OdbaciIzmene.Enabled = false;

            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Zakljucan)
            {
                btn_Sacuvaj.Hide();
                btn_OdbaciIzmene.Hide();
                napomena_txt.Enabled = false;
            }

            this.Text = "TDPopis - " + _TDOffice_popis.ID;
            this.dokument_gbx.Text = "Dokument Popisa";
            this.brojDokumenta_txt.Text = _TDOffice_popis.ID.ToString();
            this.datum_txt.Text = _TDOffice_popis.Datum.ToString("dd.MM.yyyy");
            this.komercijalnoPopis_txt.Text = _TDOffice_popis.KomercijalnoPopisBrDok == null ? "" : _TDOffice_popis.KomercijalnoPopisBrDok.ToString();
            this.komercijalnoNarudzbenica_txt.Text = _TDOffice_popis.KomercijalnoNarudzbenicaBrDok == null ? "" : _TDOffice_popis.KomercijalnoNarudzbenicaBrDok.ToString();
            Komercijalno.Magacin magacin = Komercijalno.Magacin.Get(DateTime.Now.Year, _TDOffice_popis.MagacinID);
            this.tdofficeMagacin_txt.Text = "[ " + magacin.ID + " ] " + magacin.Naziv;
            this.unosStavkeTipUnosa_cmb.Visible = _Komercijalno_narudzbenica == null ? false : true;
            this.stampajSpecijal_btn.Visible = _TDOffice_popis.SpecijalStampa >= 0 ? true : false;
            this.stampajSpecijal_btn.Text = $"Stampaj Specijal ({_TDOffice_popis.SpecijalStampa})";
        }
        private void NamestiUIPoZaduzenju()
        {
            if (_TDOffice_popis.ZaduzenjeBrDokKomercijalno == null)
            {
                this.kreirajDokumentRazduzenjaZaduzenja_btn.Enabled = _TDOffice_popis.Status == TDOffice.DokumentStatus.Zakljucan ? true : false;
            }
            else
            {
                this.kreirajDokumentRazduzenjaZaduzenja_btn.Enabled = false;
                this.kreirajDokumentRazduzenjaZaduzenja_btn.Text = _TDOffice_popis.ZaduzenjeBrDokKomercijalno.ToString();
            }
        }

        private void ReloadStavki()
        {
            _suspendValidation = true;

            double vrednostViska = 0;
            double vrednostManjka = 0;
            double vrednostZatecena = 0;

            _stavke = TDOffice.StavkaPopis.ListByDokumentAsync(_TDOffice_popis.ID);
            LoadKomercijalnoPopisStavke();
            LoadKomercijalnoNarudzbenicaStavke();

            if (currentDgvData == null)
            {
                currentDgvData = new DataTable();
                currentDgvData.Columns.Add("StavkaID", typeof(int));
                currentDgvData.Columns.Add("RobaID", typeof(int));
                currentDgvData.Columns.Add("KatBr", typeof(string));
                currentDgvData.Columns.Add("GrupaID", typeof(string));
                currentDgvData.Columns.Add("KatBrPro", typeof(string));
                currentDgvData.Columns.Add("Naziv", typeof(string));
                currentDgvData.Columns.Add("KomercijalnoPopisStavkaID", typeof(int));
                currentDgvData.Columns.Add("KomercijalnoPopisKolicina", typeof(double));
                currentDgvData.Columns.Add("KomercijalnoNarudzbenicaStavkaID", typeof(int));
                currentDgvData.Columns.Add("Kolicina", typeof(double));
                currentDgvData.Columns.Add("Skart", typeof(double));
                currentDgvData.Columns.Add("PorucenaKolicina", typeof(double));
                currentDgvData.Columns.Add("JM", typeof(string));
                currentDgvData.Columns.Add("Cena", typeof(double));
                currentDgvData.Columns.Add("VrednostPR", typeof(double));
                currentDgvData.Columns.Add("StanjeNaDanPopisaKomercijalno", typeof(double));
                currentDgvData.Columns.Add("Razlika", typeof(string));
                currentDgvData.Columns.Add("VrednostRazlike", typeof(double));
            }
            else
            {
                currentDgvData.Clear();
            }

            foreach (TDOffice.StavkaPopis s in _stavke.Result)
            {
                Komercijalno.Roba rob = _komercijalno_roba.Result.Where(x => x.ID == s.RobaID).FirstOrDefault();
                Komercijalno.Stavka komercijalnoPopisStavka = _komercijalno_popis_stavke == null ? null : _komercijalno_popis_stavke.Result.Where(x => x.RobaID == rob.ID).FirstOrDefault();
                Komercijalno.Stavka komercijalnoNarudzbenicaStavka = _komercijalno_narudzbenica_stavke == null ? null : _komercijalno_narudzbenica_stavke.Result.Where(x => x.RobaID == rob.ID).FirstOrDefault();

                DataRow dr = currentDgvData.NewRow();
                dr["StavkaID"] = s.ID;
                dr["RobaID"] = rob.ID;
                dr["KatBr"] = rob.KatBr;
                dr["GrupaID"] = rob.GrupaID;
                dr["KatBrPro"] = rob.KatBrPro;
                dr["Naziv"] = rob.Naziv;
                dr["KomercijalnoPopisStavkaID"] = komercijalnoPopisStavka == null ? -1 : komercijalnoPopisStavka.StavkaID;
                dr["KomercijalnoPopisKolicina"] = komercijalnoPopisStavka == null ? -1 : komercijalnoPopisStavka.Kolicina;
                dr["KomercijalnoNarudzbenicaStavkaID"] = komercijalnoNarudzbenicaStavka == null ? -1 : komercijalnoNarudzbenicaStavka.StavkaID;
                if(s.Kolicina != null)
                    dr["Kolicina"] = (double)s.Kolicina;
                dr["Skart"] = s.Skart;
                dr["PorucenaKolicina"] = s.Poruceno;
                dr["JM"] = rob.JM;
                dr["Cena"] = s.ProdajnaCena;
                dr["VrednostPR"] = komercijalnoPopisStavka == null ? -999999 : komercijalnoPopisStavka.Kolicina * s.ProdajnaCena;
                double stanjeNaDanPopisaKomercijalno = _Komercijalno_popis == null ? -999999 : Komercijalno.Procedure.StanjeDoDatuma(_Komercijalno_popis.Datum, _TDOffice_popis.MagacinID, rob.ID);
                dr["StanjeNaDanPopisaKomercijalno"] = stanjeNaDanPopisaKomercijalno;
                double razlika = komercijalnoPopisStavka == null ? -999999 : komercijalnoPopisStavka.Kolicina - stanjeNaDanPopisaKomercijalno;
                dr["Razlika"] = razlika.ToString("#,##0.00");
                dr["VrednostRazlike"] = razlika * s.ProdajnaCena;

                if (razlika > 0)
                    vrednostViska += razlika * s.ProdajnaCena;
                else
                    vrednostManjka += razlika * s.ProdajnaCena;

                vrednostZatecena += s.Kolicina == null ? 0 : (double)s.Kolicina * s.ProdajnaCena;

                currentDgvData.Rows.Add(dr);
            }

            dataGridView1.DataSource = currentDgvData;

            brojSlogova_lbl.Text = "Slogova: " + currentDgvData.Rows.Count;

            visak_txt.Text = vrednostViska.ToString("#,##0.00 RSD");
            manjak_txt.Text = vrednostManjka.ToString("#,##0.00 RSD");
            tb_ZatecenaVrednost.Text = vrednostZatecena.ToString("#,##0.00 RSD");

            _suspendValidation = false;
        }
        private void LoadKomercijalnoPopisStavke()
        {
            if (_Komercijalno_popis != null)
                _komercijalno_popis_stavke = Komercijalno.Stavka.ListByDokumentAsync(DateTime.Now.Year, _Komercijalno_popis.VrDok, _Komercijalno_popis.BrDok);
        }
        private void LoadKomercijalnoNarudzbenicaStavke()
        {
            if (_Komercijalno_narudzbenica != null)
                _komercijalno_narudzbenica_stavke = Komercijalno.Stavka.ListByDokumentAsync(DateTime.Now.Year, _Komercijalno_narudzbenica.VrDok, _Komercijalno_narudzbenica.BrDok);
        }

        private void LoadUkupniIzlazSvihStavkiRelativno()
        {
            try
            {
                _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice = new List<Tuple<int, double>>();

                int workers = Math.Min(10, dataGridView1.Rows.Count);
                int done = 0;
                int segment = dataGridView1.Rows.Count > 0 ? dataGridView1.Rows.Count / workers : 0;
                for (int i = 0; i < workers; i++)
                {
                    if (this == null || this.IsDisposed)
                        return;
                    ManualResetEventSlim workerSetupMRE = new ManualResetEventSlim(false);
                    Thread t = new Thread(() =>
                    {
                        int ti = i;
                        int startIndex = segment * i;
                        int endIndex = segment * (i + 1);
                        workerSetupMRE.Set();
                        for (int y = startIndex; y < (ti == (workers - 1) ? dataGridView1.Rows.Count : endIndex); y++)
                        {
                            if (this == null || this.IsDisposed)
                                return;
                            int robaID = Convert.ToInt32(dataGridView1.Rows[y].Cells["RobaID"].Value);
                            _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice.Add(new Tuple<int, double>(robaID, GetUkupniIzlazStavkeRelativno(robaID)));
                        }
                        done++;
                    });

                    t.IsBackground = true;
                    t.Start();

                    workerSetupMRE.Wait();
                }

                while (done < workers)
                    Thread.Sleep(100);

                _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice_loaded = true;
                this.Invoke((MethodInvoker)delegate
                {
                    analitikaPojedinacneStavkeIzlazRelativan_txt.BackColor = Control.DefaultBackColor;
                });
            }
            catch (InvalidOperationException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LoadUkupniIzlazSvihStavkiRelativnoAsync()
        {
            Thread t1 = new Thread(() =>
            {
                LoadUkupniIzlazSvihStavkiRelativno();
            });
            t1.IsBackground = true;
            t1.Start();
        }
        private void LoadUkupniUlazSvihStavkiRelativno()
        {
            try
            {
                _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice = new List<Tuple<int, double>>();
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (this == null || this.IsDisposed)
                        return;
                    int robaID = Convert.ToInt32(r.Cells["RobaID"].Value);
                    _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice.Add(new Tuple<int, double>(robaID, GetUkupniUlazStavkeRelativno(robaID)));
                }
                _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice_loaded = true;
                this.Invoke((MethodInvoker)delegate
                {
                    analitikaPojedinacneStavkeUlazRelativan_txt.BackColor = Control.DefaultBackColor;
                });
            }
            catch (InvalidOperationException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LoadUkupniUlazSvihStavkiRelativnoAsync()
        {
            Thread t1 = new Thread(() =>
            {
                LoadUkupniUlazSvihStavkiRelativno();
            });
            t1.IsBackground = true;
            t1.Start();
        }
        private double GetUkupniIzlazStavkeRelativno(int robaID)
        {
            if (_Komercijalno_popis == null)
                return -1;

            double ukupniIzlaz = 0;
            foreach (Komercijalno.Dokument d in _komercijalno_izlazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice.Result)
                ukupniIzlaz += _komercijalno_stavka_za_magacin.Result.Where(x => x.RobaID == robaID && x.VrDok == d.VrDok && x.BrDok == d.BrDok).Sum(y => y.Kolicina);

            return ukupniIzlaz;
        }
        private double GetUkupniUlazStavkeRelativno(int robaID)
        {
            if (_Komercijalno_popis == null)
                return -1;

            double ukupniUlaz = 0;
            foreach (Komercijalno.Dokument d in _komercijalno_ulazni_dokumenti_izmedju_datuma_popisa_komercijalno_tdoffice.Result)
                ukupniUlaz += _komercijalno_stavka_za_magacin.Result.Where(x => x.RobaID == robaID && x.VrDok == d.VrDok && x.BrDok == d.BrDok).Sum(y => y.Kolicina);

            return ukupniUlaz;
        }
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_izborRobePrikazan)
                return;

            if (_TDOffice_popis.Status != TDOffice.DokumentStatus.Otkljucan)
                return;

            if (_ir != null)
                return;

            Thread izborRobeThread = new Thread(() =>
            {
                using (_ir = new IzborRobe(_TDOffice_popis.MagacinID))
                {
                    _ir.DozvoliMultiSelect = true;
                    _ir.OnRobaClickHandler += DodajRobuUPopis;
                    _ir.TopMost = true;
                    _ir.ShowDialog();
                }

                _ir = null;
            });
            izborRobeThread.IsBackground = true;
            izborRobeThread.Start();
        }
        public void DodajRobuUPopis(Komercijalno.RobaUMagacinu[] robaArray)
        {
            _suspendValidation = true;
            double? kolicinaa = null;

            if (robaArray.Count() == 1)
            {
                string val = InputBox.Show("Unos kolicine", "Unesite popisanu kolicinu!");

                if (string.IsNullOrEmpty(val))
                {
                    MessageBox.Show("Neispravna kolicina!");
                    return;
                }

                try
                {
                    kolicinaa = double.Parse(val.Replace(',', '.'));
                }
                catch (Exception)
                {
                    MessageBox.Show("Neispravna kolicina!");
                    return;
                }
            }

            FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]);
            FbConnection con1 = new FbConnection(TDOffice.TDOffice.connectionString);
            con1.Open();
            con.Open();

            int robaN = 0;

            bool singeStavka = robaArray.Count() == 1;

            // Skidam svu robu koja vec postoji u dokumentu iz liste kako ne bih dodavao duplikate
            for (int i = robaArray.Length - 1; i >= 0; i--)
            {
                if (dataGridView1.DataSource != null)
                {
                    foreach (DataRow dr in (dataGridView1.DataSource as DataTable).Rows)
                    {
                        if (Convert.ToInt32(dr["RobaID"]) == robaArray[i].RobaID)
                        {
                            if (singeStavka)
                                MessageBox.Show("Stavka vec postoji u dokumentu!");

                            robaArray[i] = null;
                            break;
                        }
                    }
                }
            }

            List<Komercijalno.RobaUMagacinu> workingRoba = robaArray.Where(x => x != null).ToList();

            if (workingRoba.Count() == 0)
                return;

            Task<List<Komercijalno.RobaUMagacinu>> robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(_TDOffice_popis.MagacinID);

            Parallel.ForEach(workingRoba, roba =>
            {
                List<Komercijalno.Dokument> sviOtkljucaniIzlazniDokumentiZaMagacin = _komercijalno_dokumenti_za_magacin.Result.Where(x => (izlazniVrDok.Contains(x.VrDok) || ulazniVrDok.Contains(x.VrDok)) && x.Flag == 0 && x.KodDok == 0).ToList();

                //skidam vrdok 18
                sviOtkljucaniIzlazniDokumentiZaMagacin.RemoveAll(x => x.VrDok == 18 || x.VrDok == 0);
                List<Komercijalno.Stavka> komercijalneStavkeZaRobaID = _komercijalno_stavka_za_magacin.Result.Where(x => x.RobaID == roba.RobaID).ToList();

                foreach (Komercijalno.Dokument dok in sviOtkljucaniIzlazniDokumentiZaMagacin)
                {
                    if (komercijalneStavkeZaRobaID.Where(x => x.VrDok != 0 && x.VrDok == dok.VrDok && x.BrDok == dok.BrDok).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Stavka postoji u nekom dokumentu koji nije zakljucan! - VrDok: " + dok.VrDok + " - BrDok: " + dok.BrDok);
                        return;
                    }
                }

                double? insert_kolicina = null;
                double insert_poruceno = 0;

                if (_Komercijalno_narudzbenica != null && unosStavkeTipUnosa_cmb.Checked)
                {
                    insert_kolicina = null;
                    insert_poruceno = kolicinaa == null ? 0 : (double)kolicinaa;
                }
                else
                {
                    insert_kolicina = kolicinaa;
                    insert_poruceno = 0;
                }
                int newStavkaID = TDOffice.StavkaPopis.Insert(con1, _TDOffice_popis.ID, roba.RobaID, roba.ProdajnaCena, insert_kolicina, insert_poruceno, 0);

                if (newStavkaID <= 0)
                {
                    MessageBox.Show("Doslo je do greske prilikom kreiranja stavke!");
                    return;
                }

                if (_TDOffice_popis.KomercijalnoPopisBrDok != null)
                    if (Komercijalno.Stavka.Insert(con, _Komercijalno_popis, _komercijalno_roba.Result.Where(x => x.ID == roba.RobaID).FirstOrDefault(),
                        robaUMagacinu.Result.Where(x => x.RobaID == roba.RobaID).FirstOrDefault(),
                        (insert_kolicina == null ? 0 : (double)insert_kolicina) + GetUkupniIzlazStavkeRelativno(roba.RobaID) - GetUkupniUlazStavkeRelativno(roba.RobaID), 0) == -1)
                        MessageBox.Show("Doslo je do greske prilikom kreiranje stavke u komercijalnom popisu!");

                if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok != null)
                    if (Komercijalno.Stavka.Insert(con, _Komercijalno_narudzbenica, _komercijalno_roba.Result.Where(x => x.ID == roba.RobaID).FirstOrDefault(),
                        robaUMagacinu.Result.Where(x => x.RobaID == roba.RobaID).FirstOrDefault(),
                        0, 0) == -1)
                        MessageBox.Show("Doslo je do greske prilikom kreiranje stavke u komercijalnoj narudbenici!");

                _ukupni_izlaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice.Add(new Tuple<int, double>(roba.RobaID, GetUkupniIzlazStavkeRelativno(roba.RobaID)));
                _ukupni_ulaz_stavki_komercijalno_relativno_datumima_komercijalno_tdoffice.Add(new Tuple<int, double>(roba.RobaID, GetUkupniUlazStavkeRelativno(roba.RobaID)));

                robaN++;
                this.Invoke((MethodInvoker)delegate
                {
                    currentLoading_lbl.Text = robaN + " / " + workingRoba.Count();
                });

            });
            con.Close();
            con1.Close();

            this.Invoke((MethodInvoker)delegate
            {
                ReloadStavki();
                ColorDGV();
            });
        }
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.ClearSelection();
            if (e.RowIndex >= 0)
                dataGridView1.Rows[e.RowIndex].Selected = true;

            if (_fmKarticaRobe != null && _fmKarticaRobe.Visible)
            {
                int robaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RobaID"].Value);
                _fmKarticaRobe.UcitajKarticu(robaID, _TDOffice_popis.MagacinID);
            }
        }
        private void obrisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_TDOffice_popis.Status != TDOffice.DokumentStatus.Otkljucan)
                return;

            int stavkaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StavkaID"].Value);
            int komercijalnoPopisStavkaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["KomercijalnoPopisStavkaID"].Value);
            int komercijalnoNarudzbenicaStavkaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["KomercijalnoNarudzbenicaStavkaID"].Value);

            TDOffice.StavkaPopis.Remove(stavkaID);
            Komercijalno.Stavka.Remove(DateTime.Now.Year, komercijalnoPopisStavkaID);
            Komercijalno.Stavka.Remove(DateTime.Now.Year, komercijalnoNarudzbenicaStavkaID);

            ReloadStavki();
        }
        private void dokumentStatus_btn_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? 700002 : 700003))
            {
                TDOffice.Pravo.NematePravoObavestenje(_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? 700002 : 700003);
                return;
            }

            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan)
            {
                if (MessageBox.Show("Da li sigurno zelite zakljucati dokument?", "Zakljucati?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            else
            {
                if (_TDOffice_popis.ZaduzenjeBrDokKomercijalno != null)
                {
                    MessageBox.Show("Po ovom dokumentu je kreiran dokument zaduzenja/radzuzenja te se ne moze menjati!");
                    return;
                }
            }

            _TDOffice_popis.Status = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? TDOffice.DokumentStatus.Zakljucan : TDOffice.DokumentStatus.Otkljucan;
            _TDOffice_popis.Update();

            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Zakljucan)
                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                {
                    Datum = DateTime.Now,
                    Naslov = "TDOffice > Popis " + _TDOffice_popis.ID + " > Zakljucan",
                    Posiljalac = Program.TrenutniKorisnik.ID,
                    Primalac = 1,
                    Status = TDOffice.PorukaTip.Standard,
                    Tag = new TDOffice.PorukaAdditionalInfo(),
                    Tekst = "Zakljucao sam TDOffice Popis broj " + _TDOffice_popis.ID
                });

            this.dokumentStatus_btn.Text = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? "Zakljucaj" : "Otkljucaj";
            this.BackColor = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            if (_TDOffice_popis.Status == TDOffice.DokumentStatus.Zakljucan)
            {
                btn_Sacuvaj.Hide();
                btn_OdbaciIzmene.Hide();
                napomena_txt.Enabled = false;
            }
            else
            {
                btn_Sacuvaj.Show();
                btn_OdbaciIzmene.Show();
                napomena_txt.Enabled = true;
            }

            dataGridView1.Columns["Kolicina"].ReadOnly = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? false : true;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.BackColor = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? System.Drawing.Color.Yellow : System.Drawing.Color.White;

            dataGridView1.Columns["PorucenaKolicina"].ReadOnly = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? false : true;
            dataGridView1.Columns["PorucenaKolicina"].DefaultCellStyle.BackColor = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan ? System.Drawing.Color.Yellow : System.Drawing.Color.White;

            NamestiUIPoZaduzenju();
        }
        private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(500001))
            {
                TDOffice.Pravo.NematePravoObavestenje(500001);
                return;
            }
            int robaID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            if (_fmKarticaRobe == null)
            {
                using (FloatBox fb = new FloatBox("Ucitavam stavke i dokumente...."))
                {
                    fb.Show();
                    _karticaRobeInitializedMRE.Wait();
                    fb.Hide();
                }
            }

            if (_fmKarticaRobe == null || _fmKarticaRobe.IsDisposed)
                _fmKarticaRobe = new _7_fm_Komercijalno_Roba_Kartica();

            _fmKarticaRobe.UcitajKarticu(robaID, _TDOffice_popis.MagacinID);
            _fmKarticaRobe.TopMost = true;
            _fmKarticaRobe.Show();
        }
        private void komentar_btn_Click(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_TDOffice_popis.Komentar))
            {
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _TDOffice_popis.Komentar = noviKomentar;
                    _TDOffice_popis.Update();

                    if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok != null)
                        Komercijalno.Dokument.SetKomentar(_TDOffice_popis.Datum.Year, 33, (int)_TDOffice_popis.KomercijalnoNarudzbenicaBrDok, noviKomentar);

                    if (_TDOffice_popis.KomercijalnoPopisBrDok != null)
                        Komercijalno.Dokument.SetKomentar(_TDOffice_popis.Datum.Year, 7, (int)_TDOffice_popis.KomercijalnoPopisBrDok, noviKomentar);
                    
                    MessageBox.Show("Komentar uspesno sacuvan!");
                };
                kom.DozvoliEdit = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan;
                kom.ShowDialog();
            }
        }
        private void interniKomentar_btn_Click(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_TDOffice_popis.InterniKomentar))
            {
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _TDOffice_popis.InterniKomentar = noviKomentar;
                    _TDOffice_popis.Update();

                    if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok != null)
                        Komercijalno.Dokument.SetInterniKomentar(_TDOffice_popis.Datum.Year, 33, (int)_TDOffice_popis.KomercijalnoNarudzbenicaBrDok, noviKomentar);

                    if (_TDOffice_popis.KomercijalnoPopisBrDok != null)
                        Komercijalno.Dokument.SetInterniKomentar(_TDOffice_popis.Datum.Year, 7, (int)_TDOffice_popis.KomercijalnoPopisBrDok, noviKomentar);

                    MessageBox.Show("Interni komentar uspesno sacuvan!");
                };
                kom.DozvoliEdit = _TDOffice_popis.Status == TDOffice.DokumentStatus.Otkljucan;
                kom.ShowDialog();
            }
        }
        private void unosStavkeTipUnosa_cmb_CheckedChanged(object sender, EventArgs e)
        {
            LocalSettings.Settings.TDPopis_PrilikomUnosaStavkeUnosimNarucenuKolicinu = unosStavkeTipUnosa_cmb.Checked;
            LocalSettings.Update();
        }
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_suspendValidation || !_loaded)
                return;

            double newValue = 0;
            try
            {
                newValue = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
            catch (Exception)
            {
                newValue = 0;
            }

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Kolicina")
            {
                int stavkaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["StavkaID"].Value);

                var postojecaStavka = _stavke.Result.Where(x => x.ID == stavkaID).FirstOrDefault();
                if (postojecaStavka == null || postojecaStavka.Kolicina == newValue)
                    return;

                postojecaStavka.Kolicina = newValue;
                _stavke.Result.RemoveAll(x => x.ID == stavkaID);
                _stavke.Result.Add(postojecaStavka);

                int robaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value);
                postojecaStavka.Kolicina = newValue;
                postojecaStavka.Update();

                double newValueForKomercijalnoPopis = (GetUkupniIzlazStavkeRelativno(robaID) + newValue - GetUkupniUlazStavkeRelativno(robaID));

                if (_TDOffice_popis.KomercijalnoPopisBrDok != null && Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["KomercijalnoPopisStavkaID"].Value) != -1)
                {
                    int komercijalnoPopisStavkaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["KomercijalnoPopisStavkaID"].Value);

                    Komercijalno.Stavka s = Komercijalno.Stavka.Get(DateTime.Now.Year, komercijalnoPopisStavkaID);
                    s.Kolicina = newValueForKomercijalnoPopis;
                    s.Update(DateTime.Now.Year);

                    LoadKomercijalnoPopisStavke();
                    dataGridView1.Rows[e.RowIndex].Cells["KomercijalnoPopisKolicina"].Value = _komercijalno_popis_stavke.Result.Where(x => x.StavkaID == komercijalnoPopisStavkaID).FirstOrDefault().Kolicina;
                }

                double cenaStavkeNaDan = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Cena"].Value);
                double stanjeNaDanPopisaKomercijalno = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = newValueForKomercijalnoPopis - stanjeNaDanPopisaKomercijalno;
                dataGridView1.Rows[e.RowIndex].Cells["Razlika"].Value = razlika.ToString("#,##0.00");
                dataGridView1.Rows[e.RowIndex].Cells["VrednostRazlike"].Value = razlika * cenaStavkeNaDan;
                if (Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Razlika"].Value) > 0)
                    dataGridView1.Rows[e.RowIndex].Cells["Razlika"].Style.ForeColor = System.Drawing.Color.Green;
                else
                    dataGridView1.Rows[e.RowIndex].Cells["Razlika"].Style.ForeColor = System.Drawing.Color.Red;
                dataGridView1.EndEdit();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "PorucenaKolicina")
            {
                int stavkaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["StavkaID"].Value);

                var postojecaStavka = _stavke.Result.Where(x => x.ID == stavkaID).FirstOrDefault();
                if (postojecaStavka == null || postojecaStavka.Poruceno == newValue)
                    return;

                postojecaStavka.Poruceno = newValue;
                _stavke.Result.RemoveAll(x => x.ID == stavkaID);
                _stavke.Result.Add(postojecaStavka);

                int robaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value);

                TDOffice.StavkaPopis stavka = TDOffice.StavkaPopis.List().Where(x => x.ID == stavkaID).FirstOrDefault();

                stavka.Poruceno = newValue;
                stavka.Update();

                if (_TDOffice_popis.KomercijalnoNarudzbenicaBrDok != null && Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["KomercijalnoNarudzbenicaStavkaID"].Value) != -1)
                {
                    double cenaStavkeNaDan = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Cena"].Value);
                    int komercijalnoNarudzbenicaStavkaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["KomercijalnoNarudzbenicaStavkaID"].Value);

                    Komercijalno.Stavka s = Komercijalno.Stavka.Get(DateTime.Now.Year, komercijalnoNarudzbenicaStavkaID);
                    s.Kolicina = newValue;
                    s.Update(DateTime.Now.Year);

                    LoadKomercijalnoNarudzbenicaStavke();
                    dataGridView1.Rows[e.RowIndex].Cells["PorucenaKolicina"].Value = _komercijalno_narudzbenica_stavke.Result.Where(x => x.StavkaID == komercijalnoNarudzbenicaStavkaID).FirstOrDefault().Kolicina;
                    dataGridView1.Rows[e.RowIndex].Cells["VrednostPR"].Value = cenaStavkeNaDan * newValue;
                }
                dataGridView1.EndEdit();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Skart")
            {
                int stavkaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["StavkaID"].Value);

                var postojecaStavka = _stavke.Result.Where(x => x.ID == stavkaID).FirstOrDefault();
                if (postojecaStavka == null || postojecaStavka.Skart == newValue)
                    return;

                postojecaStavka.Skart = newValue;
                _stavke.Result.RemoveAll(x => x.ID == stavkaID);
                _stavke.Result.Add(postojecaStavka);

                int robaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value);

                TDOffice.StavkaPopis stavka = TDOffice.StavkaPopis.BufferedList(TimeSpan.FromHours(1)).Where(x => x.ID == stavkaID).FirstOrDefault();

                stavka.Skart = newValue;
                stavka.Update();
                ReloadStavki();
            }
        }
        private void dataGridView1_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
        }
        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ColorDGV();
        }
        private void obrisiSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_TDOffice_popis.Status != TDOffice.DokumentStatus.Otkljucan)
                return;

            if (MessageBox.Show("Da li sigurno zelite obrisati sve stavke iz dokumenta?", "Brisanje svih stavki", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
            {
                con.Open();
                TDOffice.StavkaPopis.RemoveAllFromDocument(con, _TDOffice_popis.ID);
            }
            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                if (_Komercijalno_popis != null)
                    Komercijalno.Stavka.RemoveAllFromDocument(con, 7, _Komercijalno_popis.BrDok);

                if (_Komercijalno_narudzbenica != null)
                    Komercijalno.Stavka.RemoveAllFromDocument(con, 33, _Komercijalno_narudzbenica.BrDok);
            }
            ReloadStavki();
        }
        private void stampajPopisnuListu_btn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("Kolicina", typeof(string));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dr = dt.NewRow();
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                Komercijalno.Roba kr = Komercijalno.Roba.Get(DateTime.Now.Year, robaID);
                dr["KatBr"] = kr.KatBr;
                dr["Naziv"] = kr.Naziv;
                dr["Kolicina"] = "";
                dt.Rows.Add(dr);
            }

            Document d = dt.ToPdf("TDOffice_v2 - Popisna Lista", "Popis br. " + _TDOffice_popis.ID.ToString(), new List<Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>>()
            {
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("KatBr", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Naziv", "10cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Left),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Kolicina", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center)
            });

            d.Render();
        }
        private void _7_fm_TDPopis_Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_fmKarticaRobe != null)
                _fmKarticaRobe.Dispose();

            if (_lagerListaForm != null)
                _lagerListaForm.Dispose();
        }
        private void stampajSpecijal_btn_Click(object sender, EventArgs e)
        {
            switch (_TDOffice_popis.SpecijalStampa)
            {
                case 0:
                    svihRazlikaToolStripMenuItem3.PerformClick();
                    break;
                case 1:
                    viskoviToolStripMenuItem.PerformClick();
                    break;
                case 2:
                    manjkovaToolStripMenuItem1.PerformClick();
                    break;
                default:
                    MessageBox.Show("Nije dodeljena!");
                    break;
            }
        }
        private void kreirajDokumentRazduzenjaZaduzenja_btn_Click(object sender, EventArgs e)
        {
            if (_TDOffice_popis.Status != TDOffice.DokumentStatus.Zakljucan)
            {
                MessageBox.Show("Dokument mora biti zakljucan!");
                return;
            }

            if (!Program.TrenutniKorisnik.ImaPravo(700809))
            {
                TDOffice.Pravo.NematePravoObavestenje(700809);
                return;
            }

            List<Tuple<int, double>> razlike = new List<Tuple<int, double>>();

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                double razlika = Convert.ToDouble(r.Cells["Razlika"].Value);
                if (razlika != 0)
                {
                    int robaID = Convert.ToInt32(r.Cells["RobaID"].Value);
                    razlike.Add(new Tuple<int, double>(robaID, razlika));
                }
            }

            if (razlike.Count == 0)
            {
                MessageBox.Show("Ne postoji ni jedna stavka sa razlikom da bi se kreirao dokument!");
                return;
            }

            int brDokZaduzenja = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 16, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, _TDOffice_popis.MagacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);

            if (brDokZaduzenja <= 0)
            {
                MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta zaduzenja u komercijalnom poslovanju!");
                return;
            }

            foreach (Tuple<int, double> t in razlike)
            {
                Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 16, brDokZaduzenja), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                    Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == _TDOffice_popis.MagacinID && x.RobaID == t.Item1).FirstOrDefault(),
                    t.Item2, 0);
            }

            _TDOffice_popis.ZaduzenjeBrDokKomercijalno = brDokZaduzenja;
            _TDOffice_popis.Update();

            NamestiUIPoZaduzenju();

            MessageBox.Show("Kreiran novi dokument zaduzenja broj: " + brDokZaduzenja);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                FilterEnter();
                dataGridView1.Focus();
            }
        }
        private void FilterEnter()
        {
            dataGridView1.ClearSelection();
            string kolona = comboBox1.SelectedItem.ToString();
            string input = textBox1.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Naziv"];
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
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells["Naziv"];
                    return;
                }
            }
        }

        private void kontrolaLagera_btn_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            foreach(DataGridViewRow r in dataGridView1.Rows)
               list.Add(Convert.ToInt32(r.Cells["RobaID"].Value));

            using (fm_KontrolaLagera_Index kl = new fm_KontrolaLagera_Index(_TDOffice_popis.MagacinID, list))
                if(!kl.IsDisposed)
                    kl.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }
        private void svihRazlikaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700805))
            {
                TDOffice.Pravo.NematePravoObavestenje(700805);
                return;
            }

            if (_stavke == null || _stavke.Result.Count == 0)
            {
                MessageBox.Show("Ovaj popis je prazan!");
                return;
            }

            if (MessageBox.Show("Da li sigurno zelite generisati novi popis za stavke kojima stanje nije tacno?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<int> robaIDZaNoviPopis = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                double newValueForKomercijalnoPopis = (GetUkupniIzlazStavkeRelativno(robaID) + popisanaKolicina - GetUkupniUlazStavkeRelativno(robaID));
                double stanjeNaDanPopisaKomercijalno = Convert.ToInt32(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = newValueForKomercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                if (razlika != 0)
                    robaIDZaNoviPopis.Add(robaID);
            }

            if (robaIDZaNoviPopis.Count == 0)
            {
                MessageBox.Show("U ovom popisu ne postoje razlike!");
                return;
            }

            // KReiram popis

            int magacinID = _TDOffice_popis.MagacinID;
            int tip = 1;
            int vreme = 1;


            DateTime komPopisDate = DateTime.Now;
            if (vreme == 0)
                komPopisDate = DateTime.Now.AddDays(-1);
            else
            {
                DateTime prvaNedeljaUNazad = DateTime.Now;

                while (prvaNedeljaUNazad.DayOfWeek != DayOfWeek.Sunday)
                    prvaNedeljaUNazad = prvaNedeljaUNazad.AddDays(-1);
                komPopisDate = prvaNedeljaUNazad;
            }
            int newID = TDOffice.DokumentPopis.Insert(Program.TrenutniKorisnik.ID, magacinID, 0, null, null, (TDOffice.PopisType)tip, null, null);
            TDOffice.DokumentPopis noviPopis = TDOffice.DokumentPopis.Get(newID);

            int komPopis = Komercijalno.Dokument.Insert(DateTime.Now.Year, 7, "TDOffice_v2 " + newID.ToString(), null, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
            Komercijalno.Dokument komDokPop = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, komPopis);
            komDokPop.Datum = komPopisDate;
            komDokPop.Update();

            if ((TDOffice.PopisType)tip == TDOffice.PopisType.PopisZaNabavku)
            {
                int komNar = Komercijalno.Dokument.Insert(DateTime.Now.Year, 33, "TDOffice_v2 " + newID.ToString(), Program.TrenutniKorisnik.Tag.narudbenicaPPID, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
                noviPopis.KomercijalnoNarudzbenicaBrDok = komNar;
            }

            noviPopis.KomercijalnoPopisBrDok = komPopis;

            noviPopis.Update();

            using (_7_fm_TDPopis_Index pi = new _7_fm_TDPopis_Index(noviPopis))
            {

                Task.Run(() =>
                {
                    pi.DodajRobuUPopis(Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(5)).Where(x => robaIDZaNoviPopis.Contains(x.RobaID)).ToArray());
                });

                pi.ShowDialog();
            }
        }
        private void svihRazlikaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Program.TrenutniKorisnik.ImaPravo(700806) && _TDOffice_popis.SpecijalStampa != 0)
                {
                    TDOffice.Pravo.NematePravoObavestenje(700806);
                    return;
                }
                List<int> robaIDZaNoviPopis = new List<int>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                    double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                    double komercijalnoPopis = Convert.ToDouble(row.Cells["KomercijalnoPopisKolicina"].Value);
                    double stanjeNaDanPopisaKomercijalno = Convert.ToDouble(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                    double razlika = komercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                    if (razlika != 0)
                        robaIDZaNoviPopis.Add(robaID);
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("KatBr", typeof(string));
                dt.Columns.Add("Naziv", typeof(string));
                dt.Columns.Add("Kolicina", typeof(string));

                foreach (var stavka in _stavke.Result.Where(x => robaIDZaNoviPopis.Contains(x.RobaID)))
                {
                    DataRow dr = dt.NewRow();
                    Komercijalno.Roba kr = Komercijalno.Roba.Get(DateTime.Now.Year, stavka.RobaID);
                    dr["KatBr"] = kr.KatBr;
                    dr["Naziv"] = kr.Naziv;
                    dr["Kolicina"] = "";
                    dt.Rows.Add(dr);
                }

                MessageBox.Show("Kreiram PDF");
                Document d = dt.ToPdf("TDOffice_v2 - Popisna Lista Svih Razlika", "Popis br. " + _TDOffice_popis.ID.ToString(), new List<Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>>()
            {
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("KatBr", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Naziv", "10cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Left),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Kolicina", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center)
            });

                MessageBox.Show("Kraj!");
                d.Render();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void viskoviToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700807) && _TDOffice_popis.SpecijalStampa != 1)
            {
                TDOffice.Pravo.NematePravoObavestenje(700807);
                return;
            }

            List<int> robaIDZaNoviPopis = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                double komercijalnoPopis = Convert.ToDouble(row.Cells["KomercijalnoPopisKolicina"].Value);
                double stanjeNaDanPopisaKomercijalno = Convert.ToDouble(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = komercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                if (razlika > 0)
                    robaIDZaNoviPopis.Add(robaID);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("Kolicina", typeof(string));

            foreach (var stavka in _stavke.Result.Where(x => robaIDZaNoviPopis.Contains(x.RobaID)))
            {
                DataRow dr = dt.NewRow();
                Komercijalno.Roba kr = Komercijalno.Roba.Get(DateTime.Now.Year, stavka.RobaID);
                dr["KatBr"] = kr.KatBr;
                dr["Naziv"] = kr.Naziv;
                dr["Kolicina"] = "";
                dt.Rows.Add(dr);
            }

            Document d = dt.ToPdf("TDOffice_v2 - Popisna Lista", "Popis br. " + _TDOffice_popis.ID.ToString(), new List<Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>>()
            {
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("KatBr", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Naziv", "10cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Left),
                new Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>("Kolicina", "5cm", MigraDoc.DocumentObjectModel.ParagraphAlignment.Center)
            });

            d.Render();
        }
        private void manjkovaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700807) && _TDOffice_popis.SpecijalStampa != 2)
            {
                TDOffice.Pravo.NematePravoObavestenje(700807);
                return;
            }

            List<int> robaIDZaNoviPopis = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                double komercijalnoPopis = Convert.ToDouble(row.Cells["KomercijalnoPopisKolicina"].Value);
                double stanjeNaDanPopisaKomercijalno = Convert.ToDouble(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = komercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                if (razlika < 0)
                    robaIDZaNoviPopis.Add(robaID);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("Kolicina", typeof(string));

            foreach (var stavka in _stavke.Result.Where(x => robaIDZaNoviPopis.Contains(x.RobaID)))
            {
                DataRow dr = dt.NewRow();
                Komercijalno.Roba kr = Komercijalno.Roba.Get(DateTime.Now.Year, stavka.RobaID);
                dr["KatBr"] = kr.KatBr;
                dr["Naziv"] = kr.Naziv;
                dr["Kolicina"] = "";
                dt.Rows.Add(dr);
            }

            Document d = dt.ToPdf("TDOffice_v2 - Popisna Lista", "Popis br. " + _TDOffice_popis.ID.ToString(), new List<Tuple<string, MigraDoc.DocumentObjectModel.Unit, MigraDoc.DocumentObjectModel.ParagraphAlignment>>()
            {
                new Tuple<string, Unit, ParagraphAlignment>("KatBr", "5cm", ParagraphAlignment.Center),
                new Tuple<string, Unit, ParagraphAlignment>("Naziv", "10cm", ParagraphAlignment.Left),
                new Tuple<string, Unit, ParagraphAlignment>("Kolicina", "5cm", ParagraphAlignment.Center)
            });

            d.Render();
        }
        private void dodeliSpecijalnuStampuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700808))
            {
                TDOffice.Pravo.NematePravoObavestenje(700808);
                return;
            }
            using (InputBox ib = new InputBox("Specijalna Stampa", "Unesite ID specijalne stampe"))
            {
                ib.ShowDialog();
                try
                {
                    int idSpecijalneStampe = Convert.ToInt32(ib.returnData);

                    if (idSpecijalneStampe < 0 || idSpecijalneStampe > 2)
                    {
                        MessageBox.Show("Neispravan ID specijalne stampe!");
                        return;
                    }

                    _TDOffice_popis.SpecijalStampa = idSpecijalneStampe;
                    _TDOffice_popis.Update();
                    this.stampajSpecijal_btn.Visible = _TDOffice_popis.SpecijalStampa >= 0 ? true : false;
                    this.stampajSpecijal_btn.Text = $"Stampaj Specijal ({_TDOffice_popis.SpecijalStampa})";
                    MessageBox.Show("Specijalna stampa uspesno dodeljena!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Neispravan ID specijalne stampe!");
                }
            }
        }
        private void ukloniSpecijalnuStampuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700808))
            {
                TDOffice.Pravo.NematePravoObavestenje(700808);
                return;
            }

            _TDOffice_popis.SpecijalStampa = -1;
            _TDOffice_popis.Update();
            this.stampajSpecijal_btn.Visible = _TDOffice_popis.SpecijalStampa >= 0 ? true : false;
            this.stampajSpecijal_btn.Text = $"Stampaj Specijal ({_TDOffice_popis.SpecijalStampa})";
            MessageBox.Show("Specijalna stampa uspesno uklonjena!");
        }
        private void detaljnaAnalizaToggleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700810))
            {
                TDOffice.Pravo.NematePravoObavestenje(700810);
                return;
            }
            LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu = !LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu;
            LocalSettings.Update();
            detaljnaAnalizaToggleToolStripMenuItem1.Text = LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu ? "Sakrij detaljnu analizu" : "Prikazi detaljnu analizu";
            analitikaStavke_gbx.Visible = LocalSettings.Settings.TDPopis_PrikaziDetaljnuAnalizu;
            dataGridView1.Height = panel2.Height - panel3.Height - 6;
        }
        private void logToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join("\n", _log));
        }

        private void manjkovaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700805))
            {
                TDOffice.Pravo.NematePravoObavestenje(700805);
                return;
            }

            if (_stavke == null || _stavke.Result.Count == 0)
            {
                MessageBox.Show("Ovaj popis je prazan!");
                return;
            }

            if (MessageBox.Show("Da li sigurno zelite generisati novi popis za stavke kojima stanje nije tacno?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<int> robaIDZaNoviPopis = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                double newValueForKomercijalnoPopis = (GetUkupniIzlazStavkeRelativno(robaID) + popisanaKolicina - GetUkupniUlazStavkeRelativno(robaID));
                double stanjeNaDanPopisaKomercijalno = Convert.ToInt32(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = newValueForKomercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                if (razlika < 0)
                    robaIDZaNoviPopis.Add(robaID);
            }

            if (robaIDZaNoviPopis.Count == 0)
            {
                MessageBox.Show("U ovom popisu ne postoje razlike!");
                return;
            }

            // KReiram popis

            int magacinID = _TDOffice_popis.MagacinID;
            int tip = 1;
            int vreme = 1;


            DateTime komPopisDate = DateTime.Now;
            if (vreme == 0)
                komPopisDate = DateTime.Now.AddDays(-1);
            else
            {
                DateTime prvaNedeljaUNazad = DateTime.Now;

                while (prvaNedeljaUNazad.DayOfWeek != DayOfWeek.Sunday)
                    prvaNedeljaUNazad = prvaNedeljaUNazad.AddDays(-1);
                komPopisDate = prvaNedeljaUNazad;
            }
            int newID = TDOffice.DokumentPopis.Insert(Program.TrenutniKorisnik.ID, magacinID, 0, null, null, (TDOffice.PopisType)tip, null, null);
            TDOffice.DokumentPopis noviPopis = TDOffice.DokumentPopis.Get(newID);

            int komPopis = Komercijalno.Dokument.Insert(DateTime.Now.Year, 7, "TDOffice_v2 " + newID.ToString(), null, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
            Komercijalno.Dokument komDokPop = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, komPopis);
            komDokPop.Datum = komPopisDate;
            komDokPop.Update();

            if ((TDOffice.PopisType)tip == TDOffice.PopisType.PopisZaNabavku)
            {
                int komNar = Komercijalno.Dokument.Insert(DateTime.Now.Year, 33, "TDOffice_v2 " + newID.ToString(), Program.TrenutniKorisnik.Tag.narudbenicaPPID, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
                noviPopis.KomercijalnoNarudzbenicaBrDok = komNar;
            }

            noviPopis.KomercijalnoPopisBrDok = komPopis;

            noviPopis.Update();

            using (_7_fm_TDPopis_Index pi = new _7_fm_TDPopis_Index(noviPopis))
            {

                Task.Run(() =>
                {
                    pi.DodajRobuUPopis(Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(5)).Where(x => robaIDZaNoviPopis.Contains(x.RobaID)).ToArray());
                });

                pi.ShowDialog();
            }
        }

        private void viskovaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700805))
            {
                TDOffice.Pravo.NematePravoObavestenje(700805);
                return;
            }

            if (_stavke == null || _stavke.Result.Count == 0)
            {
                MessageBox.Show("Ovaj popis je prazan!");
                return;
            }

            if (MessageBox.Show("Da li sigurno zelite generisati novi popis za stavke kojima stanje nije tacno?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<int> robaIDZaNoviPopis = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double popisanaKolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                double newValueForKomercijalnoPopis = (GetUkupniIzlazStavkeRelativno(robaID) + popisanaKolicina - GetUkupniUlazStavkeRelativno(robaID));
                double stanjeNaDanPopisaKomercijalno = Convert.ToInt32(row.Cells["StanjeNaDanPopisaKomercijalno"].Value);
                double razlika = newValueForKomercijalnoPopis - stanjeNaDanPopisaKomercijalno;

                if (razlika > 0)
                    robaIDZaNoviPopis.Add(robaID);
            }

            if (robaIDZaNoviPopis.Count == 0)
            {
                MessageBox.Show("U ovom popisu ne postoje razlike!");
                return;
            }

            // KReiram popis

            int magacinID = _TDOffice_popis.MagacinID;
            int tip = 1;
            int vreme = 1;


            DateTime komPopisDate = DateTime.Now;
            if (vreme == 0)
                komPopisDate = DateTime.Now.AddDays(-1);
            else
            {
                DateTime prvaNedeljaUNazad = DateTime.Now;

                while (prvaNedeljaUNazad.DayOfWeek != DayOfWeek.Sunday)
                    prvaNedeljaUNazad = prvaNedeljaUNazad.AddDays(-1);
                komPopisDate = prvaNedeljaUNazad;
            }
            int newID = TDOffice.DokumentPopis.Insert(Program.TrenutniKorisnik.ID, magacinID, 0, null, null, (TDOffice.PopisType)tip, null, null);
            TDOffice.DokumentPopis noviPopis = TDOffice.DokumentPopis.Get(newID);

            int komPopis = Komercijalno.Dokument.Insert(DateTime.Now.Year, 7, "TDOffice_v2 " + newID.ToString(), null, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
            Komercijalno.Dokument komDokPop = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, komPopis);
            komDokPop.Datum = komPopisDate;
            komDokPop.Update();

            if ((TDOffice.PopisType)tip == TDOffice.PopisType.PopisZaNabavku)
            {
                int komNar = Komercijalno.Dokument.Insert(DateTime.Now.Year, 33, "TDOffice_v2 " + newID.ToString(), Program.TrenutniKorisnik.Tag.narudbenicaPPID, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
                noviPopis.KomercijalnoNarudzbenicaBrDok = komNar;
            }

            noviPopis.KomercijalnoPopisBrDok = komPopis;

            noviPopis.Update();

            using (_7_fm_TDPopis_Index pi = new _7_fm_TDPopis_Index(noviPopis))
            {

                Task.Run(() =>
                {
                    pi.DodajRobuUPopis(Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(5)).Where(x => robaIDZaNoviPopis.Contains(x.RobaID)).ToArray());
                });

                pi.ShowDialog();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        #region Dozvoljava samo decimal separator i brojeve u datagridview-u
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.KeyDown += new KeyEventHandler(dataGridViewTextBox_KeyDown);
            e.Control.KeyDown += new KeyEventHandler(dataGridViewTextBox_KeyDown);

            tb.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
            e.Control.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
        }

        private void dataGridViewTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Back &&
                e.KeyCode != Keys.Enter &&
                e.KeyCode != Keys.Delete &&
                e.KeyCode != Keys.Left &&
                e.KeyCode != Keys.Right &&
                e.KeyCode != Keys.Up &&
                e.KeyCode != Keys.Down)
                if (!e.KeyCode.IsNumber() && e.KeyCode != Keys.OemPeriod && e.KeyCode != Keys.Oemcomma)
                    e.SuppressKeyPress = true;

        }

        private void dataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (new char[] { ',', '.' }.Contains(e.KeyChar))
            {
                string currVal = (sender as DataGridViewTextBoxEditingControl).EditingControlFormattedValue.ToStringOrDefault();
                if (e.KeyChar != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0])
                    e.Handled = true;

                if (string.IsNullOrWhiteSpace(currVal) || currVal.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]))
                    e.Handled = true;
            }
        }
        #endregion

        private void zabeleziTrenutnoStanjePopisaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700811))
            {
                TDOffice.Pravo.NematePravoObavestenje(700811);
                return;
            }
            DataTable dt = (dataGridView1.DataSource as DataTable).Copy();

            _TDOffice_popis.SetZabelezenoStanjePopisa(dt);
            _TDOffice_popis.Update();

            MessageBox.Show("Trenutno stanje popisa uspesno zabelezeno!");
        }

        private void ukloniZabelezenoStanjePopisaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700812))
            {
                TDOffice.Pravo.NematePravoObavestenje(700812);
                return;
            }
            _TDOffice_popis.SetZabelezenoStanjePopisa(null);
            _TDOffice_popis.Update();

            MessageBox.Show("Uklonjeno zabelezeno stanje popisa!");
        }

        private void prikaziZabelezenoStanjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(700813))
            {
                TDOffice.Pravo.NematePravoObavestenje(700813);
                return;
            }
            if (_TDOffice_popis.ZabelezenoStanjePopisa == null || _TDOffice_popis.ZabelezenoStanjePopisa.Rows.Count == 0)
            {
                MessageBox.Show("Stanje nije zabelezeno!");
                return;
            }
            using (DataGridViewSelectBox box = new DataGridViewSelectBox(_TDOffice_popis.ZabelezenoStanjePopisa))
            {
                box.RowHeaderVisible = false;
                box.TopMost = true;
                box.TopLevel = true;
                box.ShowDialog();
            }
        }

        private void btn_Sacuvaj_Click(object sender, EventArgs e)
        {
            _TDOffice_popis.Napomena = napomena_txt.Text;
            _TDOffice_popis.Update();

            btn_Sacuvaj.BackColor = TDColor.ControlInactive;
            btn_OdbaciIzmene.BackColor = TDColor.ControlInactive;

            btn_Sacuvaj.Enabled = false;
            btn_OdbaciIzmene.Enabled = false;
        }

        private void btn_OdbaciIzmene_Click(object sender, EventArgs e)
        {
            napomena_txt.Text = _TDOffice_popis.Napomena;

            btn_Sacuvaj.BackColor = TDColor.ControlInactive;
            btn_OdbaciIzmene.BackColor = TDColor.ControlInactive;

            btn_Sacuvaj.Enabled = false;
            btn_OdbaciIzmene.Enabled = false;
        }

        private void napomena_txt_KeyDown(object sender, KeyEventArgs e)
        {
            btn_Sacuvaj.BackColor = TDColor.ControlaActive;
            btn_OdbaciIzmene.BackColor = TDColor.ControlaActive;

            btn_Sacuvaj.Enabled = true;
            btn_OdbaciIzmene.Enabled = true;
        }

        private void hELPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }

        private void dokumentomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_TDOffice_popis.Status != TDOffice.DokumentStatus.Zakljucan)
            {
                MessageBox.Show("Dokument mora biti zakljucan!");
                return;
            }

            if (!Program.TrenutniKorisnik.ImaPravo(700809))
            {
                TDOffice.Pravo.NematePravoObavestenje(700809);
                return;
            }

            List<Tuple<int, double>> razlike = new List<Tuple<int, double>>();

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                double razlika = Convert.ToDouble(r.Cells["Razlika"].Value);
                if (razlika != 0)
                {
                    int robaID = Convert.ToInt32(r.Cells["RobaID"].Value);
                    razlike.Add(new Tuple<int, double>(robaID, razlika));
                }
            }

            if (razlike.Count == 0)
            {
                MessageBox.Show("Ne postoji ni jedna stavka sa razlikom da bi se kreirao dokument!");
                return;
            }

            int brDokZaduzenja = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 16, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, _TDOffice_popis.MagacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);

            if (brDokZaduzenja <= 0)
            {
                MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta zaduzenja u komercijalnom poslovanju!");
                return;
            }

            foreach (Tuple<int, double> t in razlike)
            {
                Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 16, brDokZaduzenja), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                    Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == _TDOffice_popis.MagacinID && x.RobaID == t.Item1).FirstOrDefault(),
                    t.Item2, 0);
            }

            _TDOffice_popis.ZaduzenjeBrDokKomercijalno = brDokZaduzenja;
            _TDOffice_popis.Update();

            NamestiUIPoZaduzenju();

            MessageBox.Show("Kreiran novi dokument zaduzenja broj: " + brDokZaduzenja);
        }

        private void dokumentomInternaOtpremnicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<fm_TDPopis_SvediStanjeInternomOtpremnicom>().FirstOrDefault() != null)
                return;
            DataTable dt = new DataTable();
            dt = (DataTable)dataGridView1.DataSource;
            Task.Run(() =>
            {
                using (fm_TDPopis_SvediStanjeInternomOtpremnicom pss = new fm_TDPopis_SvediStanjeInternomOtpremnicom(dt, _TDOffice_popis))
                    if (!pss.IsDisposed)
                        pss.ShowDialog();
            });
        }

        private void lagerListaPrikazanihStavkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<fm_TDPopis_SvediStanjeInternomOtpremnicom>().FirstOrDefault() != null)
                return;

            if (_lagerListaForm == null)
                _lagerListaForm = new fm_LagerListaStavki_Index(_stavke.Result.Select(x => x.RobaID).ToArray(), _TDOffice_popis.MagacinID);

            _lagerListaForm.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (_lagerListaForm == null || !_lagerListaForm.Visible)
                return;

            int robaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value);
            _lagerListaForm.PozicionirajNaRobu(robaID);
        }
    }
}
