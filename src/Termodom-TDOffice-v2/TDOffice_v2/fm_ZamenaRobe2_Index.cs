using FirebirdSql.Data.FirebirdClient;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_ZamenaRobe2_Index : Form
    {
        private Task<List<Komercijalno.Roba>> _roba { get; set; } = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
        private TDOffice.DokumentZamenaRobe _zamenaRobe { get; set; } = null;
        private Komercijalno.Dokument _MPRacun { get; set; } = null;
        private List<Komercijalno.Stavka> _stavkeMPRacuna { get; set; } = null;
        private List<Komercijalno.RobaUMagacinu> _robaUMagacinu { get; set; } = null;
        private Task<List<Komercijalno.Magacin>> _magacini { get; set; } = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<List<TDOffice.User>> _users { get; set; } = TDOffice.User.ListAsync();
        private double _vracaSeSumNeto { get => _vracaSeSum * ((100 - (double)trosakZamene_nud.Value) / 100); }
        private double _vracaSeSum { get; set; }
        private double _uzimaSeSum { get; set; }
        private Task<IzborRobe> _izborRobe { get; set; } = null;
        private Task<fm_Help> _helpFrom { get; set; }
        public fm_ZamenaRobe2_Index(int brojZamene)
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul._1325_fm_ZamenaRobe_Index);
            UcitajZamenuRobe(brojZamene);
        }

        private void fm_ZamenaRobe2_Index_Load(object sender, EventArgs e)
        {
        }

        private void UcitajZamenuRobe(int brojZamene)
        {
            _zamenaRobe = TDOffice.DokumentZamenaRobe.Get(brojZamene);
            PrilagodiUI();
            brojDokumenta_txt.Text = _zamenaRobe.ID.ToString();
            datum_txt.Text = _zamenaRobe.Datum.ToString("dd.MM.yyyy (HH:mm)");
            TDOffice.User user = _users.Result.FirstOrDefault(x => x.ID == _zamenaRobe.UserID);
            referent_txt.Text = user == null ? "Unknown user" : user.Username;
            Komercijalno.Magacin mag = _magacini.Result.FirstOrDefault(x => x.ID == _zamenaRobe.MagacinID);
            tdofficeMagacin_txt.Text = mag == null ? "Unknown" : mag.Naziv;
            mpRacun_txt.Text = _zamenaRobe.MPRacun.ToStringOrDefault();
            _izborRobe = Task.Run(() =>
            {
                IzborRobe ir = new IzborRobe(_zamenaRobe.MagacinID);
                ir.DisposeOnClose = false;
                ir.OnRobaClickHandler += (Komercijalno.RobaUMagacinu[] rums) =>
                {
                    if (_zamenaRobe.Tag.UzimaSe.Any(x => x.Item1 == rums[0].RobaID))
                    {
                        MessageBox.Show("Stavka se vec nalazi u 'Uzima se'!");
                        return;
                    }

                    Task<double> minKolicinaRobeUKartici = Task.Run<double>(() =>
                    {
                        using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[_MPRacun.Datum.Year]))
                        {
                            con.Open();
                            Komercijalno.Procedure.SrediKarticu(con, _MPRacun.MagacinID, rums[0].RobaID, new DateTime(DateTime.Now.Year, 1, 1));
                            #region
                            DateTime maxDatumDokumentaZaRobuIDUnutarMagacina = new DateTime(DateTime.Now.Year, 1, 1);
                            /// Prvo selektujem maximalni dokument za datu stavku
                            /// za slucaj da stavka nema stanje NAKON racuna u koji
                            /// pokusavam da je insertujem. Ako nema dokument u tom slucaju
                            /// ce mi vratiti COALESCE 0, a treba da mi vrati stanje iz 
                            /// robe u magacinu
                            using(FbCommand cmd = new FbCommand(@"SELECT MAX(DOKUMENT.DATUM) FROM DOKUMENT
                                LEFT OUTER JOIN STAVKA ON DOKUMENT.VRDOK = STAVKA.VRDOK AND DOKUMENT.BRDOK = STAVKA.BRDOK
                                WHERE DOKUMENT.MAGACINID = @MAG AND STAVKA.ROBAID = @RID", con))
                            {
                                cmd.Parameters.AddWithValue("@MAG", _MPRacun.MagacinID);
                                cmd.Parameters.AddWithValue("@RID", rums[0].RobaID);

                                using (FbDataReader dr = cmd.ExecuteReader())
                                    if (dr.Read())
                                        if (!(dr[0] is DBNull))
                                            maxDatumDokumentaZaRobuIDUnutarMagacina = Convert.ToDateTime(dr[0]);
                            }
                            #endregion

                            if (maxDatumDokumentaZaRobuIDUnutarMagacina < _MPRacun.Datum)
                            {
                                Komercijalno.RobaUMagacinu rum = _robaUMagacinu.FirstOrDefault(x => x.RobaID == rums[0].RobaID);
                                return rum == null ? 0 : rum.Stanje;
                            }
                            else
                            {
                                using (FbCommand cmd = new FbCommand(@"
                                    SELECT COALESCE(MIN(STAVKA.TREN_STANJE), 0) FROM STAVKA
                                    LEFT OUTER JOIN DOKUMENT ON STAVKA.VRDOK = DOKUMENT.VRDOK AND STAVKA.BRDOK = DOKUMENT.BRDOK
                                    WHERE DOKUMENT.MAGACINID = @MID AND DOKUMENT.DATUM >= @DAT AND ROBAID = @RID", con))
                                {
                                    cmd.Parameters.AddWithValue("@MID", _MPRacun.MagacinID);
                                    cmd.Parameters.AddWithValue("@RID", rums[0].RobaID);
                                    cmd.Parameters.AddWithValue("@DAT", _MPRacun.Datum);

                                    using (FbDataReader dr = cmd.ExecuteReader())
                                        if (dr.Read())
                                            return Convert.ToDouble(dr[0]);

                                    throw new Exception("Greska prilikom hvatanja minimalnog stanja iz kartice robe!");
                                }
                            }
                        }
                    });

                    string rd;
                    using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu koju zelite uzeti!"))
                    {
                        ib.ShowDialog();
                        rd = ib.returnData;
                    }

                    double kolicinaKojaSeUzima;
                    if(!double.TryParse(rd, out kolicinaKojaSeUzima))
                    {
                        MessageBox.Show("Neispravna kolicina koja se uzima!");
                        return;
                    }

                    if (kolicinaKojaSeUzima <= 0)
                    {
                        MessageBox.Show("Kolicina koja se uzima mora biti veca od 0!");
                        return;
                    }

                    if(minKolicinaRobeUKartici.Result < kolicinaKojaSeUzima)
                    {
                        MessageBox.Show($"Kolicina bi odvela magacin u minus u nekom trenutku tokom godine!" +
                            $"Maksimalna kolicina za unos: {minKolicinaRobeUKartici.Result.ToString("#,##0.00")}");
                        return;
                    }

                    double razlika = _vracaSeSumNeto - (_uzimaSeSum + (rums[0].ProdajnaCena * kolicinaKojaSeUzima));
                    if (razlika < 0)
                    {
                        double prvaNizaKolicinaKojuMoguUnetiUDokument = Math.Round(((_vracaSeSumNeto - _uzimaSeSum) / rums[0].ProdajnaCena), 2);
                        MessageBox.Show($"Ne mogu uneti stavku, vrednost robe koja se uzima ce premasiti vrednost vracene robe za {Math.Abs(razlika).ToString("#,##0.00")}!");
                        MessageBox.Show($"Najveca validna kolicina za unos je {prvaNizaKolicinaKojuMoguUnetiUDokument}");
                        return;
                    }

                    _zamenaRobe.Tag.UzimaSe.Add(new Tuple<int, double>(rums[0].RobaID, kolicinaKojaSeUzima));
                    _zamenaRobe.Update();
                    this.Invoke((MethodInvoker) delegate
                    {
                        OsveziUzimaSeDGV();
                    });
                };
                return ir;
            });
            _MPRacun = null;
            _stavkeMPRacuna = null;
            _robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(_zamenaRobe.MagacinID);
            if(_zamenaRobe.MPRacun != null)
                UcitajMPRacun((int)_zamenaRobe.MPRacun);
            OsveziVracaSeDGV();
            OsveziUzimaSeDGV();
        }

        private void PrilagodiUI()
        {
            this.info_gb.Enabled = _zamenaRobe.Status == TDOffice.DokumentStatus.Otkljucan;
            this.dokumentStatus_btn.Text = _zamenaRobe.Status == TDOffice.DokumentStatus.Zakljucan ? "Otkljucaj" : "Zakljucaj";
            this.BackColor = _zamenaRobe.Status == TDOffice.DokumentStatus.Zakljucan ? _zamenaRobe.Realizovana ? Color.Purple : Color.Red : Color.Green;
        }
        private void UcitajMPRacun(int mpRacun)
        {
            _zamenaRobe.MPRacun = mpRacun;
            _zamenaRobe.Update();
            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                Komercijalno.Dokument dok = Komercijalno.Dokument.Get(con, 15, mpRacun);

                if (dok == null)
                {
                    MessageBox.Show("MP Racun nije pronadjen!");
                    return;
                }

                if(dok.Flag != 1)
                {
                    MessageBox.Show("MP Racun nije zakljucan!");
                    return;
                }

                if(dok.MagacinID != _zamenaRobe.MagacinID)
                {
                    MessageBox.Show("MP Racun ne pripada magacinu zamene robe!");
                    return;
                }

                _MPRacun = dok;
                mpracunDatum_txt.Text = dok.Datum.ToString("dd.MM.yyyy");
                _stavkeMPRacuna = _zamenaRobe.Realizovana ? _zamenaRobe.Tag.StaroStanjeStavkiDokumenta : Komercijalno.Stavka.ListByDokument(con, 15, mpRacun);

                DataTable dt = new DataTable();
                dt.Columns.Add("RobaID", typeof(int));
                dt.Columns.Add("Proizvod", typeof(string));
                dt.Columns.Add("JM", typeof(string));
                dt.Columns.Add("Kolicina", typeof(string));

                foreach(var s in _stavkeMPRacuna)
                {
                    Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == s.RobaID);

                    DataRow dr = dt.NewRow();
                    dr["RobaID"] = s.RobaID;
                    dr["Proizvod"] = r == null ? "Undefined" : r.Naziv;
                    dr["JM"] = r == null ? "Undefined" : r.Naziv;
                    dr["Kolicina"] = s.Kolicina;
                    dt.Rows.Add(dr);
                }

                mpRacun_dgv.DataSource = dt;
                mpRacun_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                _zamenaRobe.Update();
            }
        }

        private void ucitajMPRacun_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_zamenaRobe.Status != TDOffice.DokumentStatus.Otkljucan)
                {
                    MessageBox.Show("Zamena mora biti otkljucana!");
                    return;
                }

                int brojMpRacuna = Convert.ToInt32(mpRacun_txt.Text);
                UcitajMPRacun(brojMpRacuna);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OsveziVracaSeDGV()
        {
            _vracaSeSum = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("Proizvod", typeof(string));
            dt.Columns.Add("Kolicina", typeof(double));
            dt.Columns.Add("Rabat", typeof(double));
            dt.Columns.Add("Prodajna Cena Na Dan Racuna", typeof(double));
            dt.Columns.Add("Prodajna Cena Racun Sa Popustom", typeof(double));
            dt.Columns.Add("Prodajna Cena Danas", typeof(double));
            dt.Columns.Add("Prihvacena Prodajna Cena", typeof(double));
            dt.Columns.Add("Vrednost", typeof(double));

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                foreach (Tuple<int, double> r in _zamenaRobe.Tag.VracaSe)
                {
                    Komercijalno.Roba roba = _roba.Result.FirstOrDefault(x => x.ID == r.Item1);
                    Komercijalno.RobaUMagacinu rum = _robaUMagacinu.FirstOrDefault(x => x.RobaID == r.Item1);
                    Komercijalno.Stavka stavka = _stavkeMPRacuna.FirstOrDefault(x => x.RobaID == r.Item1);

                    double prodajnaCenaDanas = rum == null ? double.MaxValue : rum.ProdajnaCena;
                    double prodajnaCenaNaDanMPRacuna = Komercijalno.Procedure.ProdajnaCenaNaDan(con, _MPRacun.MagacinID, r.Item1, _MPRacun.Datum);

                    DataRow dr = dt.NewRow();
                    dr["RobaID"] = r.Item1;
                    dr["Proizvod"] = roba == null ? "Undefined" : roba.Naziv;
                    double prodajnaCenaMPRacunaSaPopustom = prodajnaCenaNaDanMPRacuna * ((100 - stavka.Rabat) / 100);
                    double manjaProdajnaCena = prodajnaCenaMPRacunaSaPopustom > prodajnaCenaDanas ? prodajnaCenaDanas : prodajnaCenaMPRacunaSaPopustom;
                    dr["Rabat"] = stavka.Rabat;
                    dr["Prodajna Cena Na Dan Racuna"] = prodajnaCenaNaDanMPRacuna;
                    dr["Prodajna Cena Racun Sa Popustom"] = prodajnaCenaMPRacunaSaPopustom;
                    dr["Prodajna Cena Danas"] = prodajnaCenaDanas;
                    dr["Prihvacena Prodajna Cena"] = manjaProdajnaCena;
                    dr["Kolicina"] = r.Item2;
                    dr["Vrednost"] = manjaProdajnaCena * r.Item2;
                    dt.Rows.Add(dr);
                    _vracaSeSum += r.Item2 * manjaProdajnaCena;
                }
            }

            vracaSe_dgv.DataSource = dt;
            vracaSe_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            vracaSe_dgv.Columns["Prodajna Cena Na Dan Racuna"].DefaultCellStyle.Format = "#,##0.00";
            vracaSe_dgv.Columns["Prodajna Cena Racun Sa Popustom"].DefaultCellStyle.Format = "#,##0.00";
            vracaSe_dgv.Columns["Prodajna Cena Danas"].DefaultCellStyle.Format = "#,##0.00";
            vracaSe_dgv.Columns["Prihvacena Prodajna Cena"].DefaultCellStyle.Format = "#,##0.00";
            vracaSe_dgv.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00";
            vracaSe_dgv.Columns["Rabat"].DefaultCellStyle.Format = "#,##0.00";
            OsveziZbirove();
        }
        private void OsveziUzimaSeDGV()
        {
            _uzimaSeSum = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("Proizvod", typeof(string));
            dt.Columns.Add("Prodajna Cena", typeof(double));
            dt.Columns.Add("Kolicina", typeof(double));

            foreach (Tuple<int, double> r in _zamenaRobe.Tag.UzimaSe)
            {
                Komercijalno.Roba roba = _roba.Result.FirstOrDefault(x => x.ID == r.Item1);
                Komercijalno.RobaUMagacinu rum = _robaUMagacinu.FirstOrDefault(x => x.RobaID == r.Item1);

                double prodajnaCenaDanas = rum == null ? double.MaxValue : rum.ProdajnaCena;

                DataRow dr = dt.NewRow();
                dr["RobaID"] = r.Item1;
                dr["Proizvod"] = roba == null ? "Undefined" : roba.Naziv;
                dr["Prodajna Cena"] = prodajnaCenaDanas;
                dr["Kolicina"] = r.Item2;
                dt.Rows.Add(dr);
                _uzimaSeSum += r.Item2 * prodajnaCenaDanas;
            }

            uzimaSe_dgv.DataSource = dt;
            uzimaSe_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            OsveziZbirove();
        }
        private void OsveziZbirove()
        {
            uzimaSeVrednost_txt.Text = _uzimaSeSum.ToString("#,##0.00");
            vracaSeVrednost_txt.Text = $"(-{trosakZamene_nud.Value}%) {_vracaSeSumNeto.ToString("#,##0.00")}";
        }
        private void mpRacun_dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _zamenaRobe.Status != TDOffice.DokumentStatus.Otkljucan || _zamenaRobe.Realizovana)
                return;

            int robaID = Convert.ToInt32(mpRacun_dgv.Rows[e.RowIndex].Cells["RobaID"].Value);

            if (_zamenaRobe.Tag.VracaSe.Any(x => x.Item1 == robaID))
            {
                MessageBox.Show("Proizvod se vec nalazi u 'Vraca se'!");
                return;
            }

            string rd;
            using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu koju zelite vratiti!"))
            {
                ib.ShowDialog();
                rd = ib.returnData;
            }
            double kolicinaKojaSeVraca;

            if(!double.TryParse(rd, out kolicinaKojaSeVraca))
            {
                MessageBox.Show("Neispravna kolicina koja se vraca!");
                return;
            }

            if (kolicinaKojaSeVraca <= 0)
            {
                MessageBox.Show("Kolicina koja se vraca mora biti veca od 0!");
                return;
            }
            if(kolicinaKojaSeVraca > _stavkeMPRacuna.First(x => x.RobaID == robaID).Kolicina)
            {
                MessageBox.Show("Kolicina koja se vraca ne moze biti veca od kolicine iz MP racuna!");
                return;
            }

            _zamenaRobe.Tag.VracaSe.Add(new Tuple<int, double>(robaID, kolicinaKojaSeVraca));
            OsveziVracaSeDGV();
            _zamenaRobe.Update();
        }

        private void uzimaSe_dgv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_zamenaRobe.Status != TDOffice.DokumentStatus.Otkljucan || _zamenaRobe.Realizovana)
                return;

            _izborRobe.Result.Show();
        }

        private void trosakZamene_nud_ValueChanged(object sender, EventArgs e)
        {

            _zamenaRobe.Update();
            OsveziVracaSeDGV();
            OsveziUzimaSeDGV();
        }

        private void izvrsiZamenu_btn_Click(object sender, EventArgs e)
        {
            if(_zamenaRobe.Status != TDOffice.DokumentStatus.Zakljucan)
            {
                MessageBox.Show("Zamena mora biti zakljucana!");
                return;
            }

            if(_zamenaRobe.Realizovana)
            {
                MessageBox.Show("Zamena robe je vec realizovana!");
                return;
            }

            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                _zamenaRobe.Tag.StaroStanjeStavkiDokumenta = _stavkeMPRacuna;
                Komercijalno.Procedure.PresaberiDokument(con, 15, _MPRacun.BrDok);

                _MPRacun = Komercijalno.Dokument.Get(con, 15, _MPRacun.BrDok);

                foreach (Tuple<int, double> v in _zamenaRobe.Tag.VracaSe)
                {
                    Komercijalno.Stavka st = _stavkeMPRacuna.First(x => x.RobaID == v.Item1);
                    if(st == null)
                    {
                        MessageBox.Show($"Stavka sa robom id {v.Item1} ne postoji u mp racunu!");
                        return;
                    }
                    st.Kolicina -= v.Item2;
                    st.Update(con);
                    Komercijalno.Procedure.SrediKarticu(con, _MPRacun.MagacinID, st.RobaID, new DateTime(_MPRacun.Datum.Year, 1, 1));
                }

                foreach(Tuple<int, double> u in _zamenaRobe.Tag.UzimaSe)
                {
                    Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == u.Item1);

                    if(r == null)
                    {
                        MessageBox.Show("Doslo je do greske u nalasku robe prilikom insertovanja robe koja se uzima!");
                        return;
                    }

                    Komercijalno.RobaUMagacinu rum = _robaUMagacinu.FirstOrDefault(x => x.RobaID == u.Item1);

                    if (rum == null)
                    {
                        MessageBox.Show("Doslo je do greske u nalasku robe u magacinu prilikom insertovanja robe koja se uzima!");
                        return;
                    }

                    Komercijalno.Stavka.Insert(con, _MPRacun, r, rum, u.Item2, 0);
                    Komercijalno.Procedure.SrediKarticu(con, _MPRacun.MagacinID, r.ID, new DateTime(_MPRacun.Datum.Year, 1, 1));
                }
                Komercijalno.Procedure.PresaberiDokument(con, 15, _MPRacun.BrDok);
                Komercijalno.Dokument noviMP = Komercijalno.Dokument.Get(con, 15, _MPRacun.BrDok);
                double trosakZamene = (_MPRacun.Potrazuje - noviMP.Potrazuje);
                Komercijalno.Procedure.NapraviUslugu(15, _MPRacun.BrDok, 6635, Math.Round(trosakZamene, 2, MidpointRounding.AwayFromZero), 1, 0);
                Komercijalno.Procedure.PresaberiDokument(con, 15, _MPRacun.BrDok);
                _zamenaRobe.Realizovana = true;
                _zamenaRobe.Update();
                PrilagodiUI();
                MessageBox.Show("Uspesno izvrsena zamena! Razlika: " + trosakZamene.ToString("#,##0.00"));
            }
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }

        private void interniKomentar_btn_Click(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_zamenaRobe.InterniKomentar))
            {
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _zamenaRobe.InterniKomentar = noviKomentar;
                    _zamenaRobe.Update();

                    MessageBox.Show("Dokument sacuvan!");
                };
                kom.ShowDialog();
            }
        }

        private void komentar_btn_Click(object sender, EventArgs e)
        {
            using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_zamenaRobe.Komentar))
            {
                kom.KomentarSacuvan += (noviKomentar) =>
                {
                    _zamenaRobe.Komentar = noviKomentar;
                    _zamenaRobe.Update();

                    MessageBox.Show("Dokument sacuvan!");
                };
                kom.ShowDialog();
            }
        }

        private void dokumentStatus_btn_Click(object sender, EventArgs e)
        {
            if(_zamenaRobe.Status == TDOffice.DokumentStatus.Otkljucan)
            {
                if(!Program.TrenutniKorisnik.ImaPravo(132502))
                {
                    TDOffice.Pravo.NematePravoObavestenje(132502);
                    return;
                }
                if (MessageBox.Show("Da li sigurno zelite zakljucati ovu zamenu robe?", "Zakljucavanje zamene robe", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            else
            {
                if (!Program.TrenutniKorisnik.ImaPravo(132503))
                {
                    TDOffice.Pravo.NematePravoObavestenje(132503);
                    return;
                }
                if (_zamenaRobe.Realizovana)
                {
                    MessageBox.Show("Ne mozete otkljucati realizovanu zamenu robe!");
                    return;
                }
                if (MessageBox.Show("Da li sigurno zelite otkljucati ovu zamenu robe?", "Otkljucavanje zamene robe", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            _zamenaRobe.Status = _zamenaRobe.Status == TDOffice.DokumentStatus.Otkljucan ? TDOffice.DokumentStatus.Zakljucan : TDOffice.DokumentStatus.Otkljucan;
            _zamenaRobe.Update();
            PrilagodiUI();
        }

        private void blankoFizickiPovracaj_btn_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "TDOffice_v2 - Zamena Robe";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont h1 = new XFont("Verdena", 20, XFontStyle.Bold);
            XFont h2 = new XFont("Verdena", 16, XFontStyle.Bold);
            XFont h3 = new XFont("Verdena", 12, XFontStyle.Regular);

            XBrush bb = XBrushes.Black;

            double marginLeft = 10;
            double top = 30;
            double tableLineHeight = 30;
            double kolicinaCellWidth = 200;
            double jmCellWidth = 40;

            gfx.DrawString("Fizicki povracaj robe", h1, bb, new XPoint(marginLeft, top));
            top += 30;
            gfx.DrawString("Datum: " + DateTime.Now.ToString("dd.MM.yyyy"), h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("Zamena robe: " + _zamenaRobe.ID, h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("Referent: " + _users.Result.Where(x => x.ID == _zamenaRobe.UserID).FirstOrDefault().Username, h2, bb, new XPoint(marginLeft, top));
            top += 50;

            gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
            gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(marginLeft, top + tableLineHeight));
            gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft, top), new XPoint(page.Width - marginLeft, top + tableLineHeight));
            gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top), new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top + tableLineHeight));
            gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - jmCellWidth, top), new XPoint(page.Width - marginLeft - jmCellWidth, top + tableLineHeight));
            gfx.DrawString("Naziv artikla", h1, bb, new XPoint(marginLeft * 2, top + 22));
            gfx.DrawString("Kolicina", h1, bb, new XPoint(page.Width - kolicinaCellWidth - marginLeft * 4, top + 22));
            gfx.DrawString("JM", h1, bb, new XPoint(page.Width - jmCellWidth - 5, top + 22));
            top += tableLineHeight;

            for (int i = 0; i < 10; i++)
            {
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft, top), new XPoint(page.Width - marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top), new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - jmCellWidth, top), new XPoint(page.Width - marginLeft - jmCellWidth, top + tableLineHeight));
                top += tableLineHeight;
            }
            gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
            top += tableLineHeight;

            top += 100;

            double centerX = page.Width / 2;
            gfx.DrawLine(XPens.Black, new XPoint(centerX - 100, top), new XPoint(centerX + 100, top));
            gfx.DrawString("Magacioner", h1, bb, new XPoint(centerX - 50, top + 22));

            document.Save(System.IO.Path.GetTempPath() + "/_temp_ZamenaRobe.pdf");
            Process.Start(System.IO.Path.GetTempPath() + "/_temp_ZamenaRobe.pdf");
        }

        private void stampaj_btn_Click(object sender, EventArgs e)
        {
            if (!_zamenaRobe.Realizovana)
            {
                MessageBox.Show("Morate izvrsiti akciju zamene robe!");
                return;
            }
            PdfDocument document = new PdfDocument();
            document.Info.Title = "TDOffice_v2 - Zamena Robe";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont h1 = new XFont("Verdena", 20, XFontStyle.Bold);
            XFont h2 = new XFont("Verdena", 16, XFontStyle.Bold);
            XFont h3 = new XFont("Verdena", 12, XFontStyle.Regular);
            XFont hs = new XFont("Verdena", 8, XFontStyle.Regular);

            XBrush bb = XBrushes.Black;

            double marginLeft = 10;
            double top = 30;
            double tableLineHeight = 30;
            double kolicinaCellWidth = 200;
            double jmCellWidth = 40;

            gfx.DrawString("TERMODOM D.O.O.", h1, bb, new XPoint(marginLeft, top));
            top += 30;
            gfx.DrawString("Zrenjaninski Put 84g,", h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("11000 Beograd", h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("PIB: 100005295", h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("064 108 39 32", h2, bb, new XPoint(marginLeft, top));
            top += 50;
            gfx.DrawString("Referent zamene: " + _users.Result.Where(x => x.ID == _zamenaRobe.UserID).FirstOrDefault().Username, h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("Datum zamene: " + _zamenaRobe.Datum.ToString("dd.MM.yyyy"), h2, bb, new XPoint(marginLeft, top));
            top += 20;
            gfx.DrawString("Zamena izvrsena po MP racunu: " + _zamenaRobe.MPRacun.ToString(), h2, bb, new XPoint(marginLeft, top));
            if (_zamenaRobe.NovokreiraniMPRacun != null)
            {
                top += 16;
                gfx.DrawString("Uzeta roba ima vecu vrednost od vracene s toga je za razliku kreiran MP racun broj: " + _zamenaRobe.NovokreiraniMPRacun.ToString(), h3, bb, new XPoint(marginLeft, top));
            }
            top += 30;

            tableLineHeight = 16;
            kolicinaCellWidth = 100;

            gfx.DrawString("Roba koje je vracena", h2, bb, new XPoint(marginLeft, top));
            top += 10;
            foreach (var stavka in _zamenaRobe.Tag.VracaSe)
            {
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft, top), new XPoint(page.Width - marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top), new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - jmCellWidth, top), new XPoint(page.Width - marginLeft - jmCellWidth, top + tableLineHeight));
                gfx.DrawString(Komercijalno.Roba.Get(DateTime.Now.Year, stavka.Item1).Naziv, h3, bb, new XPoint(marginLeft * 2, top + 12));
                gfx.DrawString(stavka.Item2.ToString(), h3, bb, new XPoint(page.Width - kolicinaCellWidth, top + 12));
                gfx.DrawString(Komercijalno.Roba.Get(DateTime.Now.Year, stavka.Item1).JM, h3, bb, new XPoint(page.Width - jmCellWidth - 5, top + 12));
                top += tableLineHeight;
            }
            gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
            top += 50;
            gfx.DrawString("Roba koje se uzima", h2, bb, new XPoint(marginLeft, top));
            top += 10;
            foreach (var stavka in _zamenaRobe.Tag.UzimaSe)
            {
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
                gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft, top), new XPoint(page.Width - marginLeft, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top), new XPoint(page.Width - marginLeft - kolicinaCellWidth - jmCellWidth, top + tableLineHeight));
                gfx.DrawLine(XPens.Black, new XPoint(page.Width - marginLeft - jmCellWidth, top), new XPoint(page.Width - marginLeft - jmCellWidth, top + tableLineHeight));
                gfx.DrawString(Komercijalno.Roba.Get(DateTime.Now.Year, stavka.Item1).Naziv, h3, bb, new XPoint(marginLeft * 2, top + 12));
                gfx.DrawString(stavka.Item2.ToString(), h3, bb, new XPoint(page.Width - kolicinaCellWidth, top + 12));
                gfx.DrawString(Komercijalno.Roba.Get(DateTime.Now.Year, stavka.Item1).JM, h3, bb, new XPoint(page.Width - jmCellWidth - 5, top + 12));
                top += tableLineHeight;
            }

            gfx.DrawLine(XPens.Black, new XPoint(marginLeft, top), new XPoint(page.Width - marginLeft, top));
            top += tableLineHeight;

            top += 30;

            double centerX = page.Width / 2;
            gfx.DrawLine(XPens.Black, new XPoint(centerX - 100, top), new XPoint(centerX + 100, top));
            gfx.DrawString("Ime i Prezime kupca", hs, bb, new XPoint(centerX - 32, top + 8));

            top += 30;

            gfx.DrawLine(XPens.Black, new XPoint(centerX - 100, top), new XPoint(centerX + 100, top));
            gfx.DrawString("Broj licne karte kupca", hs, bb, new XPoint(centerX - 35, top + 8));

            top += 70;

            gfx.DrawLine(XPens.Black, new XPoint(centerX - 100, top), new XPoint(centerX + 100, top));
            gfx.DrawString("Potpis kupca", hs, bb, new XPoint(centerX - 20, top + 8));

            top += 100;

            gfx.DrawLine(XPens.Black, new XPoint(centerX - 100, top), new XPoint(centerX + 100, top));
            gfx.DrawString("Potpis referenta", h3, bb, new XPoint(centerX - 45, top + 16));

            document.Save(System.IO.Path.GetTempPath() + "/_temp_ZamenaRobe.pdf");
            Process.Start(System.IO.Path.GetTempPath() + "/_temp_ZamenaRobe.pdf");
        }

        private void trosakZamene_nud_Validating(object sender, CancelEventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(132504))
            {
                TDOffice.Pravo.NematePravoObavestenje(132504);
                e.Cancel = true;
            }
        }

        private void obrisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vracaSe_dgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Morate selektovati neku stavku!");
                return;
            }

            int robaID = Convert.ToInt32(vracaSe_dgv.Rows[vracaSe_dgv.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            _zamenaRobe.Tag.VracaSe.RemoveAll(x => x.Item1 == robaID);
            _zamenaRobe.Update();
            OsveziVracaSeDGV();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (uzimaSe_dgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Morate selektovati neku stavku!");
                return;
            }

            int robaID = Convert.ToInt32(uzimaSe_dgv.Rows[uzimaSe_dgv.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            _zamenaRobe.Tag.UzimaSe.RemoveAll(x => x.Item1 == robaID);
            _zamenaRobe.Update();
            OsveziUzimaSeDGV();
        }

        private void karticaRobeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(mpRacun_dgv.Rows[mpRacun_dgv.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            using (_7_fm_Komercijalno_Roba_Kartica k = new _7_fm_Komercijalno_Roba_Kartica(robaID, _zamenaRobe.MagacinID))
                if (!k.IsDisposed)
                    k.ShowDialog();
        }

        private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(vracaSe_dgv.Rows[vracaSe_dgv.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            using (_7_fm_Komercijalno_Roba_Kartica k = new _7_fm_Komercijalno_Roba_Kartica(robaID, _zamenaRobe.MagacinID))
                if(!k.IsDisposed)
                    k.ShowDialog();
        }

        private void karticaRobeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(uzimaSe_dgv.Rows[uzimaSe_dgv.SelectedCells[0].RowIndex].Cells["RobaID"].Value);

            using (_7_fm_Komercijalno_Roba_Kartica k = new _7_fm_Komercijalno_Roba_Kartica(robaID, _zamenaRobe.MagacinID))
                if (!k.IsDisposed)
                    k.ShowDialog();
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            UcitajZamenuRobe(_zamenaRobe.ID);
        }
    }
}
