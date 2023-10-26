using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.TDOffice;

namespace TDOffice_v2
{
    public partial class _1332_fm_Proracun_Index : Form
    {
        private DokumentProracun _dokument = null;
        private Task<fm_Help> _helpFrom { get; set; }
        private Task<List<DokumentProracun.Stavka>> _stavke { get; set; }
        private Task<_7_fm_Komercijalno_Roba_Kartica> _karticaRobe { get; set; } = Task.Run(() =>
        {
            return new _7_fm_Komercijalno_Roba_Kartica();
        });
        private Task<Termodom.Data.Entities.Komercijalno.MagacinDictionary> _magacini { get; set; } = Komercijalno.MagacinManager.DictionaryAsync(DateTime.Now.Year);
        private Task<Termodom.Data.Entities.Komercijalno.PartnerDictionary> _partneri { get; set; } = Komercijalno.PartnerManager.DictionaryAsync(DateTime.Now.Year);
        private DataTable stavkeDT = null;
        private TDOffice.Config<int> _maksimalnaZastarelostProracunaPrilikomPretvaranjaUMPRacun { get; set; } = TDOffice.Config<int>.Get(5283);

        private bool _uiSetUpFinish { get; set; } = false;
        private bool _ppidCmbLoaded { get; set; } = false;

        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();

        private Task _proracunStatusLoop { get; set; }

        public _1332_fm_Proracun_Index(DokumentProracun dokument)
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Proracun_Index);
            _dokument = dokument;
        }
        private async void _1332_fm_PredlogProracuna_Index_LoadAsync(object sender, EventArgs e)
        {
            SetDGV();
            await SetupUIAsync();
            ReloadStavke();
            PopulateDGV();

            _proracunStatusLoop = Task.Run(() =>
            {
                while (!this.IsDisposed)
                {
                    if (_dokument.KomercijalnoProracunBroj != null)
                    {
                        Komercijalno.Dokument proracunDok = Komercijalno.Dokument.Get(_dokument.Datum.Year, 32, (int)_dokument.KomercijalnoProracunBroj);
                        if (proracunDok != null && proracunDok.Flag == 1)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                brDokKomProracun_txt.BackColor = Color.FromArgb(255, 192, 192);
                            });
                        }
                        else
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                brDokKomProracun_txt.BackColor = Color.FromArgb(192, 255, 192);
                            });
                        }
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            brDokKomProracun_txt.BackColor = Color.FromArgb(192, 255, 192);
                        });
                    }
                    Thread.Sleep(1000);
                }
            });
        }
        private async Task SetupUIAsync()
        {
            _uiSetUpFinish = false;

            List<Tuple<int, string>> listaNacinaUplate = new List<Tuple<int, string>>();
            foreach (Komercijalno.NacinUplate nu in (Komercijalno.NacinUplate[])Enum.GetValues(typeof(Komercijalno.NacinUplate)))
            {
                if (((int)nu == 1) || ((int)nu == 5))
                    listaNacinaUplate.Add(new Tuple<int, string>((int)nu, nu.ToString().DivideOnCapital()));
            }

            listaNacinaUplate.Add(new Tuple<int, string>((int)-1, (string)"<Izaberi nacin placanja>"));
            cmb_NacinPlacanja.DataSource = listaNacinaUplate;
            cmb_NacinPlacanja.DisplayMember = "Item2";
            cmb_NacinPlacanja.ValueMember = "Item1";
            cmb_NacinPlacanja.SelectedValue = -1;

            Termodom.Data.Entities.Komercijalno.MagacinDictionary magacini = await _magacini;
            magacin_cmb.DataSource = magacini.Values.ToList();
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";
            ppid_cmb.Text = "Ucitavanje...";
            _ = Task.Run(async () =>
            {
                _ppidCmbLoaded = false;

                List<Termodom.Data.Entities.Komercijalno.Partner> partneri = (await _partneri).Values.ToList();
                //ppid_cmb.Text = "Ucitavanje...";
                partneri.Add(new Termodom.Data.Entities.Komercijalno.Partner() { PPID = -1, Naziv = "< partner nije bitan >" });
                partneri.RemoveAll(x => string.IsNullOrWhiteSpace(x.Naziv) || x.PPID <= 0);
                partneri.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));

                this.Invoke((MethodInvoker)delegate
                {
                    ppid_cmb.DisplayMember = "Naziv";
                    ppid_cmb.ValueMember = "PPID";
                    ppid_cmb.DataSource = partneri;
                    ppid_cmb.SelectedValue = _dokument.PPID == null ? -1 : _dokument.PPID;
                    ppid_cmb.Enabled = _dokument.Status == DokumentStatus.Otkljucan;
                    _ppidCmbLoaded = true;
                });
            });

            brojDokumenta_txt.Text = _dokument.ID.ToString();
            referent_txt.Text = _korisnici.Result.Where(x => x.ID == _dokument.UserID).FirstOrDefault().Username;
            datum_txt.Text = _dokument.Datum.ToString();
            magacin_cmb.SelectedValue = _dokument.MagacinID;

            dataGridView1.Columns["Kolicina"].ReadOnly = _dokument.Status == DokumentStatus.Otkljucan ? false : true;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.BackColor = _dokument.Status == DokumentStatus.Otkljucan ? Color.LightYellow : Color.White;

            magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(133204) ? (_dokument.Status == DokumentStatus.Otkljucan ? true : false) : false;
            //ppid_cmb.Enabled = _dokument.Status == DokumentStatus.Otkljucan;
            dokumentStatus_btn.Enabled = _dokument.Status == DokumentStatus.Otkljucan ? Program.TrenutniKorisnik.ImaPravo(133202) : Program.TrenutniKorisnik.ImaPravo(133203);
            komentar_btn.Enabled = true;
            interniKomentar_btn.Enabled = true;
            cmb_NacinPlacanja.Enabled = _dokument.Status == DokumentStatus.Otkljucan;
            //ppid_cmb.Enabled = _dokument.Status == DokumentStatus.Otkljucan;

            klonirajUKomercijalno_btn.Visible = _dokument.Status == DokumentStatus.Zakljucan;

            if ((int)_dokument.NUID > 0)
                cmb_NacinPlacanja.SelectedValue = (int)_dokument.NUID;

            if (_dokument.KomercijalnoProracunBroj != null)
            {
                Komercijalno.Dokument proracunKomercijalno = Komercijalno.Dokument.Get(_dokument.Datum.Year, 32, (int)_dokument.KomercijalnoProracunBroj);

                brDokKomProracun_txt.Text = proracunKomercijalno.BrDok.ToString();
                brDokKomMPRacun_txt.Text = proracunKomercijalno.BrDokOut.ToStringOrDefault();
                this.dokumentStatus_btn.Visible = proracunKomercijalno.BrDokOut == null;
                klonirajUKomercijalno_btn.Visible = _dokument.Status == DokumentStatus.Zakljucan && proracunKomercijalno.BrDokOut == null;
            }

            pretvoriUMPRacun_btn.Visible = _dokument.KomercijalnoProracunBroj != null;

            this.dokumentStatus_btn.Text = _dokument.Status == DokumentStatus.Otkljucan ? "Zakljucaj" : "Otkljucaj";
            this.BackColor = _dokument.Status == DokumentStatus.Otkljucan ? Color.Green : _dokument.KomercijalnoProracunBroj == null ? Color.Red : Color.Purple;
            if (_dokument.Status != 0)
                this.contextMenuStrip1.Enabled = false;

            _uiSetUpFinish = true;
        }
        private void SetDGV()
        {
            stavkeDT = new DataTable();
            stavkeDT.Columns.Add("ID", typeof(int));
            stavkeDT.Columns.Add("RobaID", typeof(int));
            stavkeDT.Columns.Add("KatBr", typeof(string));
            stavkeDT.Columns.Add("KatBrPro", typeof(string));
            stavkeDT.Columns.Add("Proizvod", typeof(string));
            stavkeDT.Columns.Add("Kolicina", typeof(double));
            stavkeDT.Columns.Add("JM", typeof(string));
            stavkeDT.Columns.Add("Cena", typeof(double));
            stavkeDT.Columns.Add("Vrednost", typeof(double));

            dataGridView1.DataSource = stavkeDT;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["RobaID"].Visible = false;

            dataGridView1.Columns["KatBr"].Width = 100;
            dataGridView1.Columns["KatBr"].ReadOnly = true;

            dataGridView1.Columns["KatBrPro"].Width = 100;
            dataGridView1.Columns["KatBrPro"].ReadOnly = true;

            dataGridView1.Columns["Proizvod"].Width = 250;
            dataGridView1.Columns["Proizvod"].ReadOnly = true;

            dataGridView1.Columns["Kolicina"].Width = 50;
            dataGridView1.Columns["Kolicina"].ReadOnly = false;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.BackColor = Color.LightYellow;
            dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["JM"].Width = 50;
            dataGridView1.Columns["JM"].ReadOnly = true;
            dataGridView1.Columns["JM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns["Cena"].Width = 80;
            dataGridView1.Columns["Cena"].ReadOnly = false;
            dataGridView1.Columns["Cena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Cena"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["Vrednost"].Width = 100;
            dataGridView1.Columns["Vrednost"].ReadOnly = false;
            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00";
        }
        private void ReloadStavke()
        {
            _stavke = DokumentProracun.Stavka.ListByDokumentAsync(_dokument);
        }
        private void PopulateDGV()
        {
            DataTable tempTable = stavkeDT.Clone();

            foreach (DokumentProracun.Stavka s in _stavke.Result)
            {
                Komercijalno.Roba r = Komercijalno.Roba.Get(DateTime.Now.Year, s.RobaID);
                DataRow dr = tempTable.NewRow();
                dr["ID"] = s.ID;
                dr["RobaID"] = s.RobaID;
                dr["KatBr"] = r == null ? "Greska" : r.KatBr;
                dr["KatBrPro"] = r == null ? "Greska" : r.KatBrPro;
                dr["Proizvod"] = r == null ? "Greska" : r.Naziv;
                dr["Kolicina"] = s.Kolicina;
                dr["JM"] = r == null ? "Greska" : r.JM;
                dr["Cena"] = s.ProdajnaCenaBezPDV;
                dr["Vrednost"] = s.ProdajnaCenaBezPDV * s.Kolicina;
                tempTable.Rows.Add(dr);
            }
            stavkeDT = tempTable;
            dataGridView1.DataSource = stavkeDT;
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (_dokument.Status != 0)
                return;
            using (IzborRobe ir = new IzborRobe(_dokument.MagacinID))
            {
                ir.DozvoliMultiSelect = false;
                ir.OnRobaClickHandler += OnIzborRobeTrigger;
                ir.ShowDialog();
            }
        }
        private void OnIzborRobeTrigger(Komercijalno.RobaUMagacinu[] args)
        {
            Komercijalno.RobaUMagacinu r = args[0];
            using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu"))
            {
                ib.ShowDialog();

                try
                {
                    double kol = Convert.ToDouble(ib.returnData);

                    DokumentProracun.Stavka.Insert(_dokument, r.RobaID, kol, Komercijalno.Procedure.ProdajnaCenaNaDan(_dokument.MagacinID, r.RobaID, DateTime.Now));
                    ReloadStavke();
                    PopulateDGV();
                }
                catch
                {
                    MessageBox.Show("Neispravna kolicina!");
                }
            }
        }
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Kolicina")
            {
                int idStavke = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                double staraKolicina = _stavke.Result.Where(x => x.ID == idStavke).FirstOrDefault().Kolicina;
                double novaKolicina = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Kolicina"].Value);

                if (staraKolicina == novaKolicina)
                    return;

                double cena = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Cena"].Value);

                using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
                {
                    con.Open();
                    DokumentProracun.Stavka stavka = DokumentProracun.Stavka.Get(con, idStavke);
                    stavka.Kolicina = novaKolicina;
                    stavka.Update(con);
                }

                ReloadStavke();
                dataGridView1.Rows[e.RowIndex].Cells["Vrednost"].Value = cena * novaKolicina;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PretvoriUMPRacun();
        }

        private void klonirajUKomercijalno_btn_Click(object sender, EventArgs e)
        {
            KlonirajUProracun();
        }

        private void KlonirajUProracun()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(133205))
            {
                Pravo.NematePravoObavestenje(133205);
                return;
            }

            if (_dokument.Status == DokumentStatus.Otkljucan)
            {
                MessageBox.Show("Dokument mora biti zakljucan!");
                return;
            }
            if (Convert.ToInt32(cmb_NacinPlacanja.SelectedValue) == -1)
            {
                MessageBox.Show("Niste izabrali nacin placanja!");
                return;
            }


            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                int proracunKomercijalno = 0;

                MagacinClan mvl = MagacinClan.ListByMagacinID(_dokument.MagacinID)
                    .Where(x => x.Tip == TDOffice.Enums.MagacinClanTip.Vlasnik)
                    .FirstOrDefault();

                if (mvl == null)
                {
                    MessageBox.Show("Magacin " + _dokument.MagacinID + " nema dodeljenog vlasnika magacina!");
                    return;
                }

                //TDOffice.User mv = TDOffice.User.Get(mvl.KorisnikID);
                TDOffice.User mv = TDOffice.User.Get(Program.TrenutniKorisnik.ID);

                if (mv.KomercijalnoUserID == null)
                {
                    MessageBox.Show($"TDOffice korisniku [{mv.ID}]{mv.Username} nije povezan nalog sa komercijalnim poslovanjem!");
                    return;
                }

                if (_dokument.KomercijalnoProracunBroj == null)
                {
                    proracunKomercijalno = Komercijalno.Dokument.Insert(con,
                        32,
                        "TD " + _dokument.ID,
                        _dokument.PPID > 0 ? _dokument.PPID : null,
                        "Generisao TDOffice_v2 iz dokumenta proracun broj: " + _dokument.ID,
                        Convert.ToInt32(cmb_NacinPlacanja.SelectedValue),
                        _dokument.MagacinID,
                        mv.KomercijalnoUserID, 0, true);
                }
                else
                {
                    proracunKomercijalno = (int)_dokument.KomercijalnoProracunBroj;
                }
                Komercijalno.Dokument dokumentKomercijalno = Komercijalno.Dokument.Get(con, 32, proracunKomercijalno);
                dokumentKomercijalno.Flag = 0;
                dokumentKomercijalno.Update();

                // Brisem stavke iz proracuna za slucaj da je vec kreiran
                List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 32, proracunKomercijalno);
                foreach (Komercijalno.Stavka s in stavke)
                    Komercijalno.Stavka.Remove(con, s.StavkaID);
                // =========

                Komercijalno.Dokument.SetKomentar(dokumentKomercijalno.Datum.Year, 32, proracunKomercijalno, _dokument.Komentar);
                Komercijalno.Dokument.SetInterniKomentar(dokumentKomercijalno.Datum.Year, 32, proracunKomercijalno, _dokument.InterniKomentar);

                List<Komercijalno.Roba> robaKomercijalno = Komercijalno.Roba.List(con);
                List<Komercijalno.RobaUMagacinu> rumKomercijalno = Komercijalno.RobaUMagacinu.List(con);

                foreach (DokumentProracun.Stavka s in _stavke.Result)
                {
                    Komercijalno.Roba r = robaKomercijalno.Where(x => x.ID == s.RobaID).FirstOrDefault();
                    Komercijalno.RobaUMagacinu rum = rumKomercijalno.Where(x => x.RobaID == s.RobaID && x.MagacinID == _dokument.MagacinID).FirstOrDefault();
                    Komercijalno.Stavka.Insert(DateTime.Now.Year, dokumentKomercijalno, r, rum, s.Kolicina, 0, null);
                }

                _dokument.KomercijalnoProracunBroj = proracunKomercijalno;
                _dokument.Update();
                con.Close();
                brDokKomMPRacun_txt.Text = proracunKomercijalno.ToString();
                _ = SetupUIAsync().ContinueWith((prev) =>
                {
                    MessageBox.Show("Prebaceno u proracun " + proracunKomercijalno + "!");
                });
            }
        }
        private void PretvoriUMPRacun()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(133206))
            {
                Pravo.NematePravoObavestenje(133206);
                return;
            }

            if (_dokument.KomercijalnoProracunBroj == null)
            {
                MessageBox.Show("Dokument prvo mora biti kloniran u proracun!");
                return;
            }

            int maxd = Convert.ToInt32(_maksimalnaZastarelostProracunaPrilikomPretvaranjaUMPRacun.Tag);
            DateTime datumaZaUporedjivanje = DateTime.Now.AddDays(-maxd);

            if (_dokument.Datum < datumaZaUporedjivanje)
            {
                MessageBox.Show("Zastareo proracun!");
                return;
            }

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                Komercijalno.Dokument proracunKomercijalno = Komercijalno.Dokument.Get(con, 32, (int)_dokument.KomercijalnoProracunBroj);

                if (proracunKomercijalno.Flag != 1)
                {
                    MessageBox.Show("Proracun u komercijalnom nije zakljucan!");
                    return;
                }

                List<Komercijalno.Roba> robaKomercijalno = Komercijalno.Roba.List(con);
                List<Komercijalno.RobaUMagacinu> rumKomercijalno = Komercijalno.RobaUMagacinu.List(con).Where(x => x.MagacinID == _dokument.MagacinID).ToList();

                int mpRacunBroj = 0;
                Komercijalno.Dokument mpRacunKomercijalno = null;

                if (proracunKomercijalno.BrDokOut == null)
                {
                    mpRacunBroj = Komercijalno.Dokument.Insert(con,
                        15,
                        "TD " + _dokument.ID,
                        proracunKomercijalno.PPID,
                        "Generisao TDOffice_v2 iz dokumenta proracun broj: " + _dokument.ID,
                        (int)proracunKomercijalno.NUID,
                        proracunKomercijalno.MagacinID,
                        User.Get(_dokument.UserID).KomercijalnoUserID, null);

                    mpRacunKomercijalno = Komercijalno.Dokument.Get(con, 15, mpRacunBroj);
                    mpRacunKomercijalno.Flag = 0;
                    mpRacunKomercijalno.VrDokIn = 32;
                    mpRacunKomercijalno.BrDokIn = proracunKomercijalno.BrDok;
                    mpRacunKomercijalno.Update();
                }
                else
                {
                    mpRacunBroj = (int)proracunKomercijalno.BrDokOut;
                    mpRacunKomercijalno = Komercijalno.Dokument.Get(con, 15, mpRacunBroj);
                }

                if (mpRacunKomercijalno.Flag != 0)
                {
                    MessageBox.Show("MP Racun mora biti otkljucan kako bi proracun bio ponovo prebacen u njega!");
                    return;
                }

                // Brisem stavke iz proracuna za slucaj da je vec kreiran
                List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 15, mpRacunBroj);
                foreach (Komercijalno.Stavka s in stavke)
                    Komercijalno.Stavka.Remove(con, s.StavkaID);
                // =========

                Komercijalno.Dokument.SetKomentar(_dokument.Datum.Year, 15, mpRacunBroj, _dokument.Komentar);
                Komercijalno.Dokument.SetInterniKomentar(_dokument.Datum.Year, 15, mpRacunBroj, _dokument.InterniKomentar);
                Komercijalno.Stavka stavkaKomerc;
                Komercijalno.Stavka stavkaKomercProracun;
                List<string> robaKojeNemaNaStanju = new List<string>();

                foreach (DokumentProracun.Stavka s in _stavke.Result)
                {
                    Komercijalno.Roba r = robaKomercijalno.Where(x => x.ID == s.RobaID).FirstOrDefault();
                    Komercijalno.RobaUMagacinu rum = rumKomercijalno.Where(x => x.RobaID == s.RobaID && x.MagacinID == _dokument.MagacinID).FirstOrDefault();

                    if (rum.Stanje < s.Kolicina)
                    {
                        robaKojeNemaNaStanju.Add(rum.RobaID.ToString());
                        continue;
                    }
                    int novaStavkaID = Komercijalno.Stavka.Insert(con, mpRacunKomercijalno, r, rum, s.Kolicina, 0, null);//Ovo je ID stavke za koju treba proracunati rabat
                    stavkaKomerc = Komercijalno.Stavka.Get(DateTime.Now.Year, novaStavkaID);
                    stavkaKomercProracun = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 32, (int)_dokument.KomercijalnoProracunBroj).FirstOrDefault(x => x.RobaID == s.RobaID);
                    double KPC = stavkaKomerc.ProdajnaCena;
                    double TKPC = stavkaKomercProracun.ProdajnaCena;// s.ProdajnaCenaBezPDV; 
                    double preracunatirabat = ((KPC - TKPC) / KPC) * 100;
                    stavkaKomerc.Rabat = preracunatirabat;
                    stavkaKomerc.Update(DateTime.Now.Year);
                }

                proracunKomercijalno.VrDokOut = 15;
                proracunKomercijalno.BrDokOut = mpRacunBroj;
                proracunKomercijalno.Update();

                con.Close();

                SetupUIAsync().ContinueWith((prev) =>
                {
                    MessageBox.Show("Prebaceno u MP Racun " + mpRacunBroj + "!");
                    if (robaKojeNemaNaStanju.Count > 0)
                    {
                        var stRobaKojeNemaNaStanju = string.Join("," + Environment.NewLine, robaKojeNemaNaStanju);
                        MessageBox.Show("Na stanju nema robe: " + stRobaKojeNemaNaStanju);
                    }
                });

            }
        }
        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_uiSetUpFinish)
                return;

            _dokument.MagacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
            _dokument.Update();
        }
        private void ppid_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_ppidCmbLoaded)
                return;

            try
            {
                _dokument.PPID = Convert.ToInt32(ppid_cmb.SelectedValue);
            }
            catch
            {
                _dokument.PPID = null;
            }
            _dokument.Update();
        }
        private void obrisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Ivan");
            int idStavke = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
            {
                con.Open();
                DokumentProracun.Stavka stavka = DokumentProracun.Stavka.Get(con, idStavke);
                stavka.Delete(idStavke);
                stavka.Update(con);
            }

            ReloadStavke();
            PopulateDGV();
        }

        private void obrisiSveStavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Ivan");
            DokumentProracun.Stavka.DeleteAll(_dokument.ID);
            ReloadStavke();
            PopulateDGV();
        }

        private void navigacijaIdiNa_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                try
                {
                    int brojDok = Convert.ToInt32(navigacijaIdiNa_txt.Text);

                    OtvoriProracunAsync(brojDok);
                }
                catch
                {
                    MessageBox.Show("Neispravan broj dokumenta!");
                }
            }
        }

        private void navigacijaSledeci_btn_Click(object sender, EventArgs e)
        {
            OtvoriProracunAsync(_dokument.ID + 1);
        }

        private void navigacijaPrethodni_btn_Click(object sender, EventArgs e)
        {
            OtvoriProracunAsync(_dokument.ID - 1);
        }

        private void OtvoriProracunAsync(int brDok)
        {
            this.Enabled = false;
            Thread t1 = new Thread(() =>
            {
                DokumentProracun dp = DokumentProracun.Get(brDok);

                if (dp == null)
                {
                    MessageBox.Show("Ne postoji!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Enabled = true;
                    });
                    return;
                }

                using (_1332_fm_Proracun_Index pi = new _1332_fm_Proracun_Index(dp))
                {
                    pi.Shown += (object sender, EventArgs e) =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    };
                    pi.ShowDialog();
                }
            });
            t1.Start();
        }

        private void dokumentStatus_btn_Click_1(object sender, EventArgs e)
        {
            if (_dokument.Status == DokumentStatus.Otkljucan)
            {
                if ((int)cmb_NacinPlacanja.SelectedValue <= 0)
                {
                    MessageBox.Show("Morate izabrati nacin placanja!");
                    return;
                }
                if ((int)cmb_NacinPlacanja.SelectedValue == 1 && (int)ppid_cmb.SelectedValue <= 0)
                {
                    MessageBox.Show("Za ovaj nacin uplate morate izabrati partnera!");
                    return;
                }
                if (!Program.TrenutniKorisnik.ImaPravo(133202))
                {
                    Pravo.NematePravoObavestenje(133202);
                    return;
                }
                _dokument.Status = DokumentStatus.Zakljucan;
            }
            else
            {
                if (!Program.TrenutniKorisnik.ImaPravo(133203))
                {
                    Pravo.NematePravoObavestenje(133203);
                    return;
                }
                _dokument.Status = DokumentStatus.Otkljucan;
            }

            _dokument.Update();

            _ = SetupUIAsync();
        }

        private void interniKomentar_btn_Click_1(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_dokument.InterniKomentar))
            {
                kom.DozvoliEdit = _dokument.Status == DokumentStatus.Otkljucan;
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _dokument.InterniKomentar = noviKomentar;
                    _dokument.Update();

                    MessageBox.Show("Dokument sacuvan!");
                };
                kom.ShowDialog();
            }
        }

        private void komentar_btn_Click(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_dokument.Komentar))
            {
                kom.DozvoliEdit = _dokument.Status == DokumentStatus.Otkljucan;
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _dokument.Komentar = noviKomentar;
                    _dokument.Update();

                    MessageBox.Show("Dokument sacuvan!");
                };
                kom.ShowDialog();
            }
        }

        private void cmb_NacinPlacanja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_uiSetUpFinish)
                return;

            if ((Komercijalno.NacinUplate)Convert.ToInt32(cmb_NacinPlacanja.SelectedValue) == Komercijalno.NacinUplate.Virman & (int)ppid_cmb.SelectedValue <= 0)
            {
                MessageBox.Show("Za ovaj nacin uplate morate izabrati partnera!");
                cmb_NacinPlacanja.SelectedValue = (int)Komercijalno.NacinUplate.Gotovina;
                _dokument.NUID = Komercijalno.NacinUplate.Gotovina;
                _dokument.Update();
                return;
            }

            _dokument.NUID = (Komercijalno.NacinUplate)Convert.ToInt32(cmb_NacinPlacanja.SelectedValue);
            _dokument.Update();
        }

        private void podesiMaksimalnuZastarelostProracunaPrilikomPretvaranjaUMPRacunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Trenutna maksimalna zastarelost proracuna je {_maksimalnaZastarelostProracunaPrilikomPretvaranjaUMPRacun.Tag} dana, da li zelite da je promenite?", "Promena", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (InputBox ib = new InputBox("Nova vrednost zastarelosti proracuna...", "Unesite novu vrednost"))
            {
                ib.ShowDialog();

                int novaVrednost;
                if (string.IsNullOrWhiteSpace(ib.returnData) || !int.TryParse(ib.returnData, out novaVrednost))
                {
                    MessageBox.Show("Nepodrzana vrednost!");
                    return;
                }

                _maksimalnaZastarelostProracunaPrilikomPretvaranjaUMPRacun.Tag = novaVrednost;
                _maksimalnaZastarelostProracunaPrilikomPretvaranjaUMPRacun.UpdateOrInsert();

                MessageBox.Show($"Uspesno azurirana vrednost na {novaVrednost} dana!");
            }
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }

        private void kartcaRobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int robaId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value);
            _karticaRobe.Result.UcitajKarticu(robaId, _dokument.MagacinID);
            _karticaRobe.Result.Show();
        }

        private void istorijaNabavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jos uvke nije implementirano. Rucno selektujte filter 'Samo nabavka'");
            int robaId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value);
            _karticaRobe.Result.UcitajKarticu(robaId, _dokument.MagacinID);
            _karticaRobe.Result.Show();
        }
    }
}