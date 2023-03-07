using FirebirdSql.Data.FirebirdClient;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.PDFSharpARExtensions;

namespace TDOffice_v2
{
    public partial class _1344_fm_SpecifikacijaNovca_Index : Form
    {
        private TDOffice.SpecifikacijaNovca _specifikacija { get; set; }
        private List<TDOffice.Cek> _cekovi { get; set; }
        private Task<fm_Help> _helpFrom { get; set; }
        private double _prometNaDan { get; set; }
        private double _gotovinomNaDan { get; set; }
        private double _virmanomNaDan { get; set; }
        private Task _UISet { get; set; }
        private Task<Termodom.Data.Entities.Komercijalno.DokumentDictionary> _MPRacuniMagacina { get; set; }

        public int MagacinID
        {
            get
            {
                return Convert.ToInt32(magacin_cmb.SelectedValue);
            }
            set
            {
                _UISet.ContinueWith((prev) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        magacin_cmb.SelectedValue = value;
                    });
                });
            }
        }
        public DateTime Datum
        {
            get
            {
                return odDatuma_dtp.Value;
            }
            set
            {
                odDatuma_dtp.Value = value;
            }
        }

        private Task<List<Komercijalno.Dokument>> _PovratniceKupca { get; set; }

        public _1344_fm_SpecifikacijaNovca_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.SpecifikacijaNovca_Index);
            _UISet = SetUIAsync();
        }

        private void _1344_fm_SpecifikacijaNovca_Index_Load(object sender, EventArgs e)
        {
            Datum = DateTime.Now;
        }

        private async Task SetUIAsync()
        {
            await Task.Run(() =>
            {
                modNavigacijeSpecifikacije_btn.Tag = true;
                modNavigacijeSpecifikacije_btn.BackColor = System.Drawing.Color.Orange;
                modNavigacijeSpecifikacije_btn.Enabled = Program.TrenutniKorisnik.ImaPravo(134404);

                magacin_cmb.DataSource = Komercijalno.Magacin.DictionaryAsync().GetAwaiter().GetResult().Values.ToList();
                magacin_cmb.DisplayMember = "Naziv";
                magacin_cmb.ValueMember = "ID";

                magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(134404) ? true : false;
                MagacinID = Program.TrenutniKorisnik.MagacinID;
            });
        }

        private async Task UcitajSpecifikaciju()
        {
            FbConnection tdOfficeConnection = new FbConnection(TDOffice.TDOffice.connectionString);
            tdOfficeConnection.Open();

            FbConnection komercijalnoConnection = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[Datum.Year]);
            komercijalnoConnection.Open();

            _specifikacija = TDOffice.SpecifikacijaNovca.Get(tdOfficeConnection, MagacinID, Datum);

            if (_specifikacija == null)
            {
                TDOffice.SpecifikacijaNovca.Insert(tdOfficeConnection, MagacinID, new TDOffice.SpecifikacijaNovca.Detalji(), Datum);
                _specifikacija = TDOffice.SpecifikacijaNovca.Get(tdOfficeConnection, MagacinID, Datum);
            }

            if (_specifikacija.Tag == null)
                _specifikacija.Tag = new TDOffice.SpecifikacijaNovca.Detalji();

            if (_specifikacija.Tag.StorniraniMPRacuni == null)
                _specifikacija.Tag.StorniraniMPRacuni = new List<int>();

            if((await Komercijalno.Dokument.ListAsync(MagacinID, new int[] { 15, 22 }, _specifikacija.Datum.Date, _specifikacija.Datum.Date, 0)).Any(x => x.Duguje != 0))
            {
                label47.Visible = true;
            }
            else
            {
                label47.Visible = false;
            }

            //if (Komercijalno.Dokument.List(komercijalnoConnection, $@"(VRDOK = 15 OR VRDOK = 22) AND MAGACINID = {MagacinID}
            //    AND DATUM = '{new DateTime(_specifikacija.Datum.Year, _specifikacija.Datum.Month, _specifikacija.Datum.Day, 0, 0, 1).ToString("dd.MM.yyyy")}'
            //    AND DATUM <= '{new DateTime(_specifikacija.Datum.Year, _specifikacija.Datum.Month, _specifikacija.Datum.Day, 23, 59, 59).ToString("dd.MM.yyyy")}'")
            //    .Any(x => x.Placen == 0 && x.Duguje != 0))
            //{
            //    label47.Visible = true;
            //}
            //else
            //{
            //    label47.Visible = false;
            //}

            _MPRacuniMagacina = Komercijalno.Dokument.ListAsync(MagacinID, new int[] { 15 }, _specifikacija.Datum.Date, _specifikacija.Datum.Date, 0);

            await _MPRacuniMagacina;

            int magacinID = MagacinID;
            _PovratniceKupca = UcitajPovratniceKupcaAsync(magacinID);

            await _PovratniceKupca;

            _cekovi = TDOffice.Cek.ListByDatum(tdOfficeConnection, odDatuma_dtp.Value)
                .Where(x => x.MagacinID == _specifikacija.MagacinID && x.Zaduzio == null)
                .ToList();
            specifikacijaCekovi_txt.BackColor = _cekovi.Count(x => x.Status == TDOffice.Enums.CekStatus.Nerealizovan) > 0 ?
                System.Drawing.Color.Red : System.Drawing.Color.LightGreen;

            _prometNaDan = Komercijalno.Magacin.Promet(_specifikacija.MagacinID, odDatuma_dtp.Value).Item2;
            _gotovinomNaDan = Komercijalno.Magacin.Promet(_specifikacija.MagacinID, odDatuma_dtp.Value, Komercijalno.NacinUplate.Gotovina).Item2;
            _virmanomNaDan = Komercijalno.Magacin.Promet(_specifikacija.MagacinID, odDatuma_dtp.Value, Komercijalno.NacinUplate.Virman).Item2;
            gotovina5000_txt.Text = _specifikacija.Tag.Novcanice5000.ToString();
            gotovina5000Sum_txt.Text = (_specifikacija.Tag.Novcanice5000 * 5000).ToString("#,##0.00 RSD");

            gotovina2000_txt.Text = _specifikacija.Tag.Novcanice2000.ToString();
            gotovina2000Sum_txt.Text = (_specifikacija.Tag.Novcanice2000 * 2000).ToString("#,##0.00 RSD");

            gotovina1000_txt.Text = _specifikacija.Tag.Novcanice1000.ToString();
            gotovina1000Sum_txt.Text = (_specifikacija.Tag.Novcanice1000 * 1000).ToString("#,##0.00 RSD");

            gotovina500_txt.Text = _specifikacija.Tag.Novcanice500.ToString();
            gotovina500Sum_txt.Text = (_specifikacija.Tag.Novcanice500 * 500).ToString("#,##0.00 RSD");

            gotovina200_txt.Text = _specifikacija.Tag.Novcanice200.ToString();
            gotovina200Sum_txt.Text = (_specifikacija.Tag.Novcanice200 * 200).ToString("#,##0.00 RSD");

            gotovina100_txt.Text = _specifikacija.Tag.Novcanice100.ToString();
            gotovina100Sum_txt.Text = (_specifikacija.Tag.Novcanice100 * 100).ToString("#,##0.00 RSD");

            gotovina50_txt.Text = _specifikacija.Tag.Novcanice50.ToString();
            gotovina50Sum_txt.Text = (_specifikacija.Tag.Novcanice50 * 50).ToString("#,##0.00 RSD");

            gotovina20_txt.Text = _specifikacija.Tag.Novcanice20.ToString();
            gotovina20Sum_txt.Text = (_specifikacija.Tag.Novcanice20 * 20).ToString("#,##0.00 RSD");

            gotovina10_txt.Text = _specifikacija.Tag.Novcanice10.ToString();
            gotovina10Sum_txt.Text = (_specifikacija.Tag.Novcanice10 * 10).ToString("#,##0.00 RSD");

            gotovina5_txt.Text = _specifikacija.Tag.Novcanice5.ToString();
            gotovina5Sum_txt.Text = (_specifikacija.Tag.Novcanice5 * 5).ToString("#,##0.00 RSD");

            gotovina2_txt.Text = _specifikacija.Tag.Novcanice2.ToString();
            gotovina2Sum_txt.Text = (_specifikacija.Tag.Novcanice2 * 2).ToString("#,##0.00 RSD");

            gotovina1_txt.Text = _specifikacija.Tag.Novcanice1.ToString();
            gotovina1Sum_txt.Text = (_specifikacija.Tag.Novcanice1).ToString("#,##0.00 RSD");

            specifikacijaKartica_txt.Text = _specifikacija.Tag.Kartice.ToString();
            specifikacijaCekovi_txt.Text = _cekovi.Sum(x => x.Vrednost).ToString("#,##0.00 RSD");
            specifikacijaPapiri_txt.Text = _specifikacija.Tag.Papiri.ToString();
            specifikacijaTroskovi_txt.Text = _specifikacija.Tag.Troskovi.ToString();
            specifikacijaVozaciDuguju_txt.Text = _specifikacija.Tag.VozaciDuguju.ToString();
            tb_KodSase.Text = _specifikacija.Tag.KodSase.ToString();

            eur1_txt.Text = _specifikacija.Tag.Kurs1.Kolicina.ToString();
            eur1Kurs_txt.Text = _specifikacija.Tag.Kurs1.Kurs.ToString();
            eur1Sum_txt.Text = _specifikacija.Tag.Kurs1.Vrednost().ToString("#,##0.00 RSD");

            eur2_txt.Text = _specifikacija.Tag.Kurs2.Kolicina.ToString();
            eur2Kurs_txt.Text = _specifikacija.Tag.Kurs2.Kurs.ToString();
            eur2Sum_txt.Text = _specifikacija.Tag.Kurs2.Vrednost().ToString("#,##0.00 RSD");

            double vrednostStornoGotovinskihRacuna = 0;
            double vrednostStornoVirmanskihRacuna = 0;
            double vrednostNefiskalizovanihGotovinskihRacuna = 0;
            double vrednostNefiskalizovanihVirmanskihRacuna = 0;
            double vrednostStornoGotovinskihRacunaKase = 0;
            double vrednostStornoVirmanskihRacunaKase = 0;
            double vrednostRealizvoanihStorniranihRacuna = 0;

            if (_specifikacija.Tag != null && _specifikacija.Tag.StorniraniMPRacuni != null)
            {
                foreach(int brDok in _specifikacija.Tag.StorniraniMPRacuni)
                {
                    Komercijalno.Dokument dok = _MPRacuniMagacina.Result.FirstOrDefault(X => X.BrDok == brDok);

                    if (dok.KodDok == 0)
                    {
                        if (dok.Placen == 0)
                        {
                            if (dok.NUID == Komercijalno.NacinUplate.Virman)
                                vrednostNefiskalizovanihVirmanskihRacuna += dok.Potrazuje;
                            else
                                vrednostNefiskalizovanihGotovinskihRacuna += dok.Potrazuje;
                        }
                        else
                        {
                            if (dok.NUID == Komercijalno.NacinUplate.Virman)
                            {
                                vrednostStornoVirmanskihRacuna += dok.Potrazuje;
                                vrednostStornoVirmanskihRacunaKase += dok.Potrazuje;
                            }
                            else
                            {
                                vrednostStornoGotovinskihRacuna += dok.Potrazuje;
                                vrednostStornoGotovinskihRacunaKase += dok.Potrazuje;
                            }
                        }
                    }
                    else
                    {
                        vrednostRealizvoanihStorniranihRacuna += dok.Potrazuje;
                    }
                }

                tb_Gotovina.Text = vrednostStornoGotovinskihRacuna.ToString("#,##0.00 RSD");
                tb_Virman.Text = vrednostStornoVirmanskihRacuna.ToString("#,##0.00 RSD");
                racunarStornirani_txt.Text = vrednostRealizvoanihStorniranihRacuna.ToString("#,##0.00 RSD");

                tb_NeFiskGotovina.Text = vrednostNefiskalizovanihGotovinskihRacuna.ToString("#,##0.00 RSD");
                tb_NeFiskGotovina.BackColor = vrednostNefiskalizovanihGotovinskihRacuna != 0 ? System.Drawing.Color.Red : Control.DefaultBackColor;

                tb_NeFiskVirman.Text = vrednostNefiskalizovanihVirmanskihRacuna.ToString("#,##0.00 RSD");
                tb_NeFiskVirman.BackColor = vrednostNefiskalizovanihVirmanskihRacuna != 0 ? System.Drawing.Color.Red : Control.DefaultBackColor;

                tb_NiVirmanski.Text = vrednostStornoVirmanskihRacunaKase.ToString("#,##0.00 RSD");
                tb_NiGotovinski.Text = vrednostStornoGotovinskihRacunaKase.ToString("#,##0.00 RSD");

            }

            double vrednostStorniranihDuploFiskalizovanih = 0;

            if (_specifikacija.Tag != null && _specifikacija.Tag.StornoKasaDuplo != null)
            {
                vrednostStorniranihDuploFiskalizovanih = _MPRacuniMagacina.Result.Where(x => _specifikacija.Tag.StornoKasaDuplo.Contains(x.BrDok)).Sum(x => x.Potrazuje);
            }

            racunarGotovinski_txt.Text = _gotovinomNaDan.ToString("#,##0.00 RSD");
            racunarVirmanom_txt.Text = _virmanomNaDan.ToString("#,##0.00 RSD");
            racunarOstalo_txt.Text = (_prometNaDan - _virmanomNaDan - _gotovinomNaDan).ToString("#,##0.00 RSD");
            gotovineRacunar_txt.Text = (_prometNaDan - _PovratniceKupca.Result.Where(x => x.NUID == Komercijalno.NacinUplate.Gotovina).Sum(x => x.Potrazuje) - _virmanomNaDan - (vrednostStornoGotovinskihRacuna + vrednostStornoVirmanskihRacuna + vrednostNefiskalizovanihGotovinskihRacuna + vrednostNefiskalizovanihVirmanskihRacuna)).ToString("#,##0.00 RSD");
            zbirRacunar_txt.Text = _prometNaDan.ToString("#,##0.00 RSD");

            Komercijalno.Magacin magacin = Komercijalno.Magacin.Get(DateTime.Now.Year, MagacinID);
            double ukupanFiskalizovanPometProdaja = 0;
            double ukupanFiskalizovanRefund = 0;

            if (magacin.PFRID != null)
            {
                string jid = Komercijalno.PFRS.List(komercijalnoConnection).FirstOrDefault(x => x.PFRID == magacin.PFRID).JID;
                List<TDOffice.FiskalniRacun> fiskalniRacuni = TDOffice.FiskalniRacun.List(tdOfficeConnection, $"SIGNED_BY = '{jid}'")
                    .Where(x => x.SDCTime_ServerTimeZone.Date == _specifikacija.Datum).ToList();

                ukupanFiskalizovanPometProdaja = fiskalniRacuni
                    .Where(x =>
                        x.InvoiceType == "Normal" &&
                        x.TransactionType == "Sale")
                    .Sum(x => x.TotalAmount);
                ukupanFiskalizovanRefund = fiskalniRacuni
                    .Where(x =>
                        x.InvoiceType == "Normal" &&
                        x.TransactionType == "Refund")
                    .Sum(x => x.TotalAmount);

                dnevniIzvesta_txt.Text = ukupanFiskalizovanPometProdaja.ToString("#,##0.00 RSD");
                fiskalizovanePovratnice_txt.Text = ukupanFiskalizovanRefund.ToString("#,##0.00 RSD");
            }
            else
            {
                dnevniIzvesta_txt.Text = "NO PFR";
                fiskalizovanePovratnice_txt.Text = "NO PFR";
            }

            beleska_rtb.Text = _specifikacija.Tag.Beleksa;

            dnevniIzvesta_txt.ForeColor = System.Drawing.Color.White;
            dnevniIzvesta_txt.BackColor =
                Math.Abs(_prometNaDan - ukupanFiskalizovanPometProdaja) < 0.01 ?
                    System.Drawing.Color.Green :
                    System.Drawing.Color.Red;

            fiskalizovanePovratnice_txt.ForeColor = System.Drawing.Color.White;
            fiskalizovanePovratnice_txt.BackColor =
                Math.Abs(_PovratniceKupca.Result.Sum(x => x.Potrazuje) - ukupanFiskalizovanRefund) < 0.01 ? 
                System.Drawing.Color.Green :
                System.Drawing.Color.Red;

            PreracunajRazlikuSpecifikacije();

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
            DateTime spec = new DateTime(_specifikacija.Datum.Year, _specifikacija.Datum.Month, _specifikacija.Datum.Day, 0, 0, 1);
            tb_BrojSpecifikacije.Text = _specifikacija.ID.ToString();

            if (Program.TrenutniKorisnik.ImaPravo(134409) || now == spec)
            {
                beleska_rtb.Enabled = true;
                eur1Kurs_txt.Enabled = true;
                eur1_txt.Enabled = true;
                eur2Kurs_txt.Enabled = true;
                eur2_txt.Enabled = true;
                gotovina5000_txt.Enabled = true;
                gotovina2000_txt.Enabled = true;
                gotovina1000_txt.Enabled = true;
                gotovina500_txt.Enabled = true;
                gotovina200_txt.Enabled = true;
                gotovina100_txt.Enabled = true;
                gotovina50_txt.Enabled = true;
                gotovina20_txt.Enabled = true;
                gotovina10_txt.Enabled = true;
                gotovina5_txt.Enabled = true;
                gotovina2_txt.Enabled = true;
                gotovina1_txt.Enabled = true;
                specifikacijaKartica_txt.Enabled = true;
                specifikacijaCekovi_txt.Enabled = true;
                specifikacijaPapiri_txt.Enabled = true;
                specifikacijaTroskovi_txt.Enabled = true;
                specifikacijaVozaciDuguju_txt.Enabled = true;
                tb_KodSase.Enabled = true;
            }
            else
            {
                beleska_rtb.Enabled = false;
                eur1Kurs_txt.Enabled = false;
                eur1_txt.Enabled = false;
                eur2Kurs_txt.Enabled = false;
                eur2_txt.Enabled = false;
                gotovina5000_txt.Enabled = false;
                gotovina2000_txt.Enabled = false;
                gotovina1000_txt.Enabled = false;
                gotovina500_txt.Enabled = false;
                gotovina200_txt.Enabled = false;
                gotovina100_txt.Enabled = false;
                gotovina50_txt.Enabled = false;
                gotovina20_txt.Enabled = false;
                gotovina10_txt.Enabled = false;
                gotovina5_txt.Enabled = false;
                gotovina2_txt.Enabled = false;
                gotovina1_txt.Enabled = false;
                specifikacijaKartica_txt.Enabled = false;
                specifikacijaCekovi_txt.Enabled = false;
                specifikacijaPapiri_txt.Enabled = false;
                specifikacijaTroskovi_txt.Enabled = false;
                specifikacijaVozaciDuguju_txt.Enabled = false;
                tb_KodSase.Enabled = false;
            }

            tdOfficeConnection.Close();
            tdOfficeConnection.Dispose();

            komercijalnoConnection.Close();
            komercijalnoConnection.Dispose();
        }
        private async Task<List<Komercijalno.Dokument>> UcitajPovratniceKupcaAsync(int magacinID)
        {
            var doks = await Task.Run(() =>
            {
                return Komercijalno.Dokument.List("VRDOK = 22 AND MAGACINID = " + magacinID + " AND DATUM = '" + _specifikacija.Datum.ToString("dd.MM.yyyy") + "'");
            });
            gotovinskePovratnice_txt.Text = doks.Where(x => x.NUID == Komercijalno.NacinUplate.Gotovina).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD");
            virmanskePovratnice_txt.Text = doks.Where(x => x.NUID == Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD");
            ostalePovratnice_txt.Text = doks.Where(x => x.NUID != Komercijalno.NacinUplate.Virman && x.NUID != Komercijalno.NacinUplate.Gotovina).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD");
            return doks;
        }
        private void PreracunajRazlikuSpecifikacije()
        {
            List<Komercijalno.NacinUplate> prihvaceniNaciniUplate = new List<Komercijalno.NacinUplate>();
            prihvaceniNaciniUplate.Add(Komercijalno.NacinUplate.Odlozeo);
            prihvaceniNaciniUplate.Add(Komercijalno.NacinUplate.Kartica);

            #region Improve
            List<Komercijalno.NacinUplate> naciniUplate = new List<Komercijalno.NacinUplate>()
            {
                Komercijalno.NacinUplate.Odlozeo,
                Komercijalno.NacinUplate.Kartica
            };
            #endregion

            bool DaLiJeDokumentPrihvacenNacinomUplate = prihvaceniNaciniUplate.Contains(Komercijalno.NacinUplate.Gotovina);

            double vrednostStorniranihRacuna = 0;

            if(_specifikacija != null && _specifikacija.Tag != null && _specifikacija.Tag.StorniraniMPRacuni != null)
                vrednostStorniranihRacuna = _MPRacuniMagacina.Result.Where(x => _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok) && x.NUID != Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje);
            double specifikacijaNovcaRazlika = 
                (_specifikacija.Sum() + _cekovi.Sum(x => x.Vrednost) -
                (_prometNaDan - _virmanomNaDan) + 
                vrednostStorniranihRacuna + _PovratniceKupca.Result.Where(x => x.NUID != Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje));

            specifikacijaNovcaRazlika_txt.Text = specifikacijaNovcaRazlika.ToString("#,##0.00 RSD");

            if(specifikacijaNovcaRazlika > 5)
            {
                specifikacijaNovcaRazlika_txt.BackColor = System.Drawing.Color.Pink;
                specifikacijaNovcaRazlika_txt.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (specifikacijaNovcaRazlika < -5)
            {
                specifikacijaNovcaRazlika_txt.BackColor = System.Drawing.Color.Pink;
                specifikacijaNovcaRazlika_txt.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else
            {
                specifikacijaNovcaRazlika_txt.BackColor = System.Drawing.Color.Green;
                specifikacijaNovcaRazlika_txt.ForeColor = System.Drawing.Color.White;
            }

            ukupnoGotovine_txt.Text = (_specifikacija.Tag.Zbir() -
                _specifikacija.Tag.Troskovi -
                _specifikacija.Tag.VozaciDuguju -
                _specifikacija.Tag.KodSase -
                _specifikacija.Tag.Kartice -
                _specifikacija.Tag.Papiri).ToString("#,##0.00 RSD");
        }

        private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
        }
        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UcitajSpecifikaciju();
        }
        private void gotovina5000_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina5000Sum_txt.Text = (Convert.ToInt32(gotovina5000_txt.Text) * 5000).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice5000 = Convert.ToInt32(gotovina5000_txt.Text);
            }
            catch
            {
                gotovina5000Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice5000 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina2000_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina2000Sum_txt.Text = (Convert.ToInt32(gotovina2000_txt.Text) * 2000).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice2000 = Convert.ToInt32(gotovina2000_txt.Text);
            }
            catch (Exception)
            {
                gotovina2000Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice2000 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina1000_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina1000Sum_txt.Text = (Convert.ToInt32(gotovina1000_txt.Text) * 1000).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice1000 = Convert.ToInt32(gotovina1000_txt.Text);
            }
            catch (Exception)
            {
                gotovina1000Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice1000 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina500_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina500Sum_txt.Text = (Convert.ToInt32(gotovina500_txt.Text) * 500).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice500 = Convert.ToInt32(gotovina500_txt.Text);
            }
            catch (Exception)
            {
                gotovina500Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice500 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina200_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina200Sum_txt.Text = (Convert.ToInt32(gotovina200_txt.Text) * 200).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice200 = Convert.ToInt32(gotovina200_txt.Text);
            }
            catch (Exception)
            {
                gotovina200Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice200 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina100_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina100Sum_txt.Text = (Convert.ToInt32(gotovina100_txt.Text) * 100).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice100 = Convert.ToInt32(gotovina100_txt.Text);
            }
            catch (Exception)
            {
                gotovina100Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice100 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina50_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina50Sum_txt.Text = (Convert.ToInt32(gotovina50_txt.Text) * 50).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice50 = Convert.ToInt32(gotovina50_txt.Text);
            }
            catch (Exception)
            {
                gotovina50Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice50 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina20_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina20Sum_txt.Text = (Convert.ToInt32(gotovina20_txt.Text) * 20).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice20 = Convert.ToInt32(gotovina20_txt.Text);
            }
            catch (Exception)
            {
                gotovina20Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice20 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void gotovina10_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina10Sum_txt.Text = (Convert.ToInt32(gotovina10_txt.Text) * 10).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice10 = Convert.ToInt32(gotovina10_txt.Text);
            }
            catch (Exception)
            {
                gotovina10Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice10 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private async void specifikacijaKartica_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                specifikacijaKartica_txt.Text = Convert.ToDouble(specifikacijaKartica_txt.Text).ToString();
                _specifikacija.Tag.Kartice = Convert.ToDouble(specifikacijaKartica_txt.Text);
                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.Kartice = 0;
                await UcitajSpecifikaciju();
            }
        }
        private void specifikacijaCekovi_txt_TextChanged(object sender, EventArgs e)
        {
        }
        private async void specifikacijaPapiri_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                specifikacijaPapiri_txt.Text = Convert.ToDouble(specifikacijaPapiri_txt.Text).ToString();
                _specifikacija.Tag.Papiri = Convert.ToDouble(specifikacijaPapiri_txt.Text);
                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.Papiri = 0;
                await UcitajSpecifikaciju();
            }
        }
        private async void specifikacijaTroskovi_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                specifikacijaTroskovi_txt.Text = Convert.ToDouble(specifikacijaTroskovi_txt.Text).ToString();
                _specifikacija.Tag.Troskovi = Convert.ToDouble(specifikacijaTroskovi_txt.Text);
                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.Troskovi = 0;
                await UcitajSpecifikaciju();
            }
        }
        private async void eur1Kurs_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                _specifikacija.Tag.Kurs1.Kurs = Convert.ToDouble(eur1Kurs_txt.Text);

                eur1_txt.Text = _specifikacija.Tag.Kurs1.Kolicina.ToString();
                eur1Kurs_txt.Text = _specifikacija.Tag.Kurs1.Kurs.ToString();
                eur1Sum_txt.Text = _specifikacija.Tag.Kurs1.Vrednost().ToString("#,##0.00 RSD");

                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch(Exception)
            {
                _specifikacija.Tag.Kurs1.Kurs = 0;
                await UcitajSpecifikaciju();
            }
        }
        private async void eur2Kurs_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                _specifikacija.Tag.Kurs2.Kurs = Convert.ToDouble(eur2Kurs_txt.Text);

                eur2_txt.Text = _specifikacija.Tag.Kurs2.Kolicina.ToString();
                eur2Kurs_txt.Text = _specifikacija.Tag.Kurs2.Kurs.ToString();
                eur2Sum_txt.Text = _specifikacija.Tag.Kurs2.Vrednost().ToString("#,##0.00 RSD");

                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.Kurs2.Kurs = 0;
                await UcitajSpecifikaciju();
            }
        }
        private void eur1_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                _specifikacija.Tag.Kurs1.Kolicina = Convert.ToDouble(eur1_txt.Text);
            }
            catch (Exception)
            {
                _specifikacija.Tag.Kurs1.Kolicina = 0;
            }

            eur1_txt.Text = _specifikacija.Tag.Kurs1.Kolicina.ToString();
            eur1Kurs_txt.Text = _specifikacija.Tag.Kurs1.Kurs.ToString();
            eur1Sum_txt.Text = _specifikacija.Tag.Kurs1.Vrednost().ToString("#,##0.00 RSD");

            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void eur2_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                _specifikacija.Tag.Kurs2.Kolicina = Convert.ToDouble(eur2_txt.Text);
            }
            catch (Exception)
            {
                _specifikacija.Tag.Kurs2.Kolicina = 0;
            }

            eur2_txt.Text = _specifikacija.Tag.Kurs2.Kolicina.ToString();
            eur2Kurs_txt.Text = _specifikacija.Tag.Kurs2.Kurs.ToString();
            eur2Sum_txt.Text = _specifikacija.Tag.Kurs2.Vrednost().ToString("#,##0.00 RSD");

            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }
        private void beleska_rtb_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            _specifikacija.Tag.Beleksa = beleska_rtb.Text;
            _specifikacija.Update();
        }
        private void dnevniIzvesta_txt_TextChanged(object sender, EventArgs e)
        {
        }

        private void stampaj_btn_Click(object sender, EventArgs e)
        {
            string filePath = System.IO.Path.GetTempPath();
            Unit rowHeight = 10;
            Document document = new Document();
            DocumentRenderer renderer = new DocumentRenderer(document);
            document.Info.Title = "Specifikacija Novca";
            document.Info.Author = "TDOffice_v2";
            
            Section section = document.AddSection();
            section.PageSetup.TopMargin = 15;
            section.PageSetup.LeftMargin = 15;

            Paragraph p = section.AddParagraph("Specifikacija Novca");
            p.Format.Font.Size = 18;

            p = section.AddParagraph(Komercijalno.Magacin.Get(_specifikacija.Datum.Year, _specifikacija.MagacinID).Naziv);
            p.Format.Font.Size = 12;

            p = section.AddParagraph("Datum specifikacije: " + _specifikacija.Datum.ToString("dd.MM.yyyy"));
            p.Format.Font.Size = 12;

            p = section.AddParagraph("Datum stampe: " + DateTime.Now.ToString("dd.MM.yyyy") + "  Stampao: [" + Program.TrenutniKorisnik.Username+"]");
            p.Format.Font.Size = 8;

            section.AddParagraph();

            #region Racunar Table
            Table table = section.AddTable();

            table.Style = "Table";
            table.Borders.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(0, 0, 0);
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            Column column = table.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Inicijalizacija zaglavlja
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Height = rowHeight;
            row.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(245, 245, 245);

            row.Cells[0].AddParagraph("Racunar");
            row.Cells[0].MergeRight = 1;

            table.SetEdge(0, 0, 2, 1, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75, MigraDoc.DocumentObjectModel.Color.Empty);

            Row r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("Gotovinski Racuni: " + _gotovinomNaDan.ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("Storno Gotovinski: " + _MPRacuniMagacina.Result.Where(x => x.NUID == Komercijalno.NacinUplate.Gotovina && _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok)).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("Virmanski Racuni: " + _virmanomNaDan.ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("Storno Virmanski: " + _MPRacuniMagacina.Result.Where(x => x.NUID == Komercijalno.NacinUplate.Virman && _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok)).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("Ostali racuni: " + (_prometNaDan - _virmanomNaDan - _gotovinomNaDan).ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("Storno Ostalo: " + _MPRacuniMagacina.Result.Where(x => !new List<int>() { 1, 5 }.Contains((int)x.NUID) && _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok)).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Format.Font.Bold = true;
            r.Cells[0].AddParagraph("Ukupno racunar: " + _prometNaDan.ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("");
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("Gotovinske povratnice: " + _PovratniceKupca.Result.Where(x => x.NUID == Komercijalno.NacinUplate.Gotovina).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("Virmanske povratnice: " + _PovratniceKupca.Result.Where(x => x.NUID == Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);
            #endregion
            section.AddParagraph();
            #region Kasa Table
            table = section.AddTable();

            table.Style = "Table";
            table.Borders.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(0, 0, 0);
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            column = table.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Inicijalizacija zaglavlja
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Height = rowHeight;
            row.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(245, 245, 245);

            row.Cells[0].AddParagraph("Fiskalna Kasa");
            row.Cells[0].MergeRight = 1;

            table.SetEdge(0, 0, 2, 1, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75, MigraDoc.DocumentObjectModel.Color.Empty);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("Dnevni Izvestaj: " + _specifikacija.Tag.DnevniIzvestaj.ToString("#,##0.00 RSD"));
            r.Cells[1].AddParagraph("Storno Bez Duplo Fisk: " + _MPRacuniMagacina.Result.Where(x => x.Placen == 1 && _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok)).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Left;
            r.Cells[0].AddParagraph("");
            r.Cells[1].AddParagraph("Duplo fiskalizovani racuni: " + _MPRacuniMagacina.Result.Where(x => _specifikacija.Tag.StornoKasaDuplo.Contains(x.BrDok)).Sum(x => x.Potrazuje).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);
            #endregion
            section.AddParagraph();
            #region Specifikacija
            table = section.AddTable();

            table.Style = "Table";
            table.Borders.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(0, 0, 0);
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Inicijalizacija zaglavlja
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Height = rowHeight;
            row.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(245, 245, 245);

            row.Cells[0].AddParagraph("Stanje Blagajne");
            row.Cells[0].MergeRight = 4;

            table.SetEdge(0, 0, 2, 1, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75, MigraDoc.DocumentObjectModel.Color.Empty);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("5.000 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice5000.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((5000 * _specifikacija.Tag.Novcanice5000).ToString(" = #,##0.00 RSD"));
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Kartice:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_specifikacija.Tag.Kartice.ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("2.000 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice2000.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((2000 * _specifikacija.Tag.Novcanice2000).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Cekovi:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_cekovi.Sum(x => x.Vrednost).ToString("#,##0.00 RSD"));

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("1.000 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice1000.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((1000 * _specifikacija.Tag.Novcanice1000).ToString(" = #,##0.00 RSD"));
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Papiri:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_specifikacija.Tag.Papiri.ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("500 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice500.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((500 * _specifikacija.Tag.Novcanice500).ToString(" = #,##0.00 RSD"));
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Troskovi:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_specifikacija.Tag.Troskovi.ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("200 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice200.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((200 * _specifikacija.Tag.Novcanice200).ToString(" = #,##0.00 RSD"));
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Vozaci Duguju:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_specifikacija.Tag.VozaciDuguju.ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("100 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice100.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((100 * _specifikacija.Tag.Novcanice100).ToString(" = #,##0.00 RSD"));
            r.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            r.Cells[3].AddParagraph("Kod Sase:");
            r.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[4].AddParagraph(_specifikacija.Tag.KodSase.ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("50 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice50.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((50 * _specifikacija.Tag.Novcanice50).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("20 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice20.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((20 * _specifikacija.Tag.Novcanice20).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("10 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice10.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((10 * _specifikacija.Tag.Novcanice10).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("5 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice5.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((5 * _specifikacija.Tag.Novcanice5).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("2 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice2.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((2 * _specifikacija.Tag.Novcanice2).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("1 x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Novcanice1.ToString());
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((_specifikacija.Tag.Novcanice1).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph(_specifikacija.Tag.Kurs1.Kolicina + " kurs x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Kurs1.Kurs.ToString("0 RSD"));
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((_specifikacija.Tag.Kurs1.Kolicina * _specifikacija.Tag.Kurs1.Kurs).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph(_specifikacija.Tag.Kurs2.Kolicina + " kurs x ");
            r.Cells[1].AddParagraph(_specifikacija.Tag.Kurs2.Kurs.ToString("0 RSD"));
            r.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            r.Cells[2].AddParagraph((_specifikacija.Tag.Kurs2.Kolicina * _specifikacija.Tag.Kurs2.Kurs).ToString(" = #,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.Cells[0].AddParagraph("Ukupno gotovine: ");
            r.Cells[0].MergeRight = 1;
            r.Cells[2].AddParagraph((_specifikacija.Tag.Zbir() -
                _specifikacija.Tag.Troskovi -
                _specifikacija.Tag.VozaciDuguju -
                _specifikacija.Tag.KodSase -
                _specifikacija.Tag.Kartice -
                _specifikacija.Tag.Papiri).ToString("#,##0.00 RSD"));
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Font.Size = 12;
            r.Format.Font.Bold = true;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.VerticalAlignment = VerticalAlignment.Center;
            r.Cells[0].AddParagraph(_specifikacija.Sum().ToString("Ukupno Stanje Blagajne = #,##0.00 RSD"));
            r.Cells[0].MergeRight = 4;
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);

            r = table.AddRow();
            r.Height = rowHeight;
            r.Format.Font.Size = 12;
            r.Format.Font.Bold = true;
            r.Format.Alignment = ParagraphAlignment.Center;
            r.VerticalAlignment = VerticalAlignment.Center;
            double trebaBlagajne = _prometNaDan - _virmanomNaDan;
            //double razlika = _specifikacija.Sum() - trebaBlagajne;
            
            double vrednostStorniranihRacuna = 0;

            if (_specifikacija != null && _specifikacija.Tag != null && _specifikacija.Tag.StorniraniMPRacuni != null)
                vrednostStorniranihRacuna = _MPRacuniMagacina.Result.Where(x => _specifikacija.Tag.StorniraniMPRacuni.Contains(x.BrDok) && x.NUID != Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje);
            double specifikacijaNovcaRazlika = (_specifikacija.Sum() + _cekovi.Sum(x => x.Vrednost) - (_prometNaDan - _virmanomNaDan) + vrednostStorniranihRacuna + _PovratniceKupca.Result.Where(x => x.NUID != Komercijalno.NacinUplate.Virman).Sum(x => x.Potrazuje));
            double razlika = specifikacijaNovcaRazlika;
            r.Cells[0].AddParagraph(Math.Abs(razlika).ToString((razlika == 0 ? "Sve se poklapa!" : razlika > 0 ? "Visak: " : "Manjak: ") + " #,##0.00 RSD"));
            r.Cells[0].MergeRight = 4;
            table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75);
            #endregion

            //document.Render();
            string strNameFile = "Izvestaj";
            //string strNameFileNew = $"{strNameFile}-SpecifikacijaNovca-{_specifikacija.Datum.ToString("dd.MM.yyyy")}";
            string strNameFileNew = "Izvestaj-SpecifikacijaNovca";
            renderer.PrepareDocument();
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            renderer.RenderPage(gfx, 1, PageRenderOptions.All);
            pdf.Save(filePath+"\\"+ strNameFileNew + ".pdf");
            var pr = new Process();
            pr.StartInfo = new ProcessStartInfo(filePath + "\\" + strNameFileNew + ".pdf")
            {
                UseShellExecute = true
            };
            pr.Start();
            
        }
        private async void tb_PretragaPoBrojuSpecifikacije_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                int IDSP = 0;
                try
                {
                    IDSP = Convert.ToInt32(this.tb_PretragaPoBrojuSpecifikacije.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Neispravan broj specifikacije " + tb_PretragaPoBrojuSpecifikacije.Text);
                    tb_PretragaPoBrojuSpecifikacije.Focus();
                    tb_PretragaPoBrojuSpecifikacije.SelectAll();
                }
                await UcitajSpecifikaciju();
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            using (fm_SpecifikacijaNovca_Storno sr = new fm_SpecifikacijaNovca_Storno(_specifikacija, fm_SpecifikacijaNovca_Storno.StornoTip.Standard))
            {
                sr.Text = "Specifikacija novca <<Storno racuni>>";
                sr.ShowDialog();
                await UcitajSpecifikaciju();
            }
        }
        private async void btn_DuploFiskalizovan_Click(object sender, EventArgs e)
        {
            using (fm_SpecifikacijaNovca_Storno sr = new fm_SpecifikacijaNovca_Storno(_specifikacija, fm_SpecifikacijaNovca_Storno.StornoTip.KasaDuplo))
            {
                sr.Text = "Specifikacija novca <<Kasa DuploFiskalizovani>>";
                sr.ShowDialog();
                await UcitajSpecifikaciju();
            }
        }

        private async void btn_NiRacuni_Click(object sender, EventArgs e)
        {
            using (fm_SpecifikacijaNovca_Storno sr = new fm_SpecifikacijaNovca_Storno(_specifikacija, fm_SpecifikacijaNovca_Storno.StornoTip.Standard))
            {
                sr.Text = "Specifikacija novca <<NI Racuni>>";
                sr.ShowDialog();
                await UcitajSpecifikaciju();
            }
        }

        private async void specifikacijaVozaciDuguju_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                specifikacijaVozaciDuguju_txt.Text = Convert.ToDouble(specifikacijaVozaciDuguju_txt.Text).ToString();
                _specifikacija.Tag.VozaciDuguju = Convert.ToDouble(specifikacijaVozaciDuguju_txt.Text);
                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.VozaciDuguju = 0;
                await UcitajSpecifikaciju();
            }
        }

        private void btn_BeleskaKartice_Click(object sender, EventArgs e)
        {
            string beleska;
            if (_specifikacija.Tag.KarticeBeleksa != null)
            { 
                beleska = _specifikacija.Tag.KarticeBeleksa; 
            }
                
            else
                beleska = "";

            using (fm_InputBeleske ib = new fm_InputBeleske("Beleska <<Kartice>>", "Unesite belesku za kartice",beleska))
            {
                
                ib.ShowDialog();
                beleska = ib.returnData;
                _specifikacija.Tag.KarticeBeleksa = beleska;
                _specifikacija.Update();
            }

        }

        private async void btn_BeleskaCekovi_Click(object sender, EventArgs e)
        {
            using (fm_Cekovi_List cl = new fm_Cekovi_List())
            {
                if (cl.IsDisposed)
                    return;

                cl.SetMagacin(_specifikacija.MagacinID);
                cl.ShowDialog();
            }
            await UcitajSpecifikaciju();
        }

        private void btn_BeleskaPapiri_Click(object sender, EventArgs e)
        {
            string beleska;
            if (_specifikacija.Tag.PapiriBeleksa != null)
            {
                beleska = _specifikacija.Tag.PapiriBeleksa;
            }

            else
                beleska = "";
            using (fm_InputBeleske ib = new fm_InputBeleske("Beleska <<Papiri>>", "Unesite belesku za papire", beleska))
            {

                ib.ShowDialog();
                beleska = ib.returnData;
                _specifikacija.Tag.PapiriBeleksa = beleska;
                _specifikacija.Update();
            }
        }

        private void btn_BeleskaTroskovi_Click(object sender, EventArgs e)
        {
            string beleska;
            if (_specifikacija.Tag.TroskoviBeleksa != null)
            {
                beleska = _specifikacija.Tag.TroskoviBeleksa;
            }

            else
                beleska = "";
            using (fm_InputBeleske ib = new fm_InputBeleske("Beleska <<Troskovi>>", "Unesite belesku za troskove", beleska))
            {

                ib.ShowDialog();
                beleska = ib.returnData;
                _specifikacija.Tag.TroskoviBeleksa = beleska;
                _specifikacija.Update();
            }
        }

        private void btn_BeleskaVozaciDuguju_Click(object sender, EventArgs e)
        {
            string beleska;
            if (_specifikacija.Tag.VozaciDugujuBeleksa != null)
            {
                beleska = _specifikacija.Tag.VozaciDugujuBeleksa;
            }

            else
                beleska = "";
            using (fm_InputBeleske ib = new fm_InputBeleske("Beleska <<Vozaci duguju>>", "Unesite belesku za Vozaci duguju", beleska))
            {

                ib.ShowDialog();
                beleska = ib.returnData;
                _specifikacija.Tag.VozaciDugujuBeleksa = beleska;
                _specifikacija.Update();
            }
        }

        private async void tb_KodSase_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            string t = (sender as TextBox).Text;
            if (t.Length == 0)
                (sender as TextBox).Text = "0";
            else if (t[t.Length - 1] == '.')
                return;

            try
            {
                tb_KodSase.Text = Convert.ToDouble(tb_KodSase.Text).ToString();
                _specifikacija.Tag.KodSase = Convert.ToDouble(tb_KodSase.Text);
                _specifikacija.Update();
                PreracunajRazlikuSpecifikacije();
            }
            catch (Exception)
            {
                _specifikacija.Tag.KodSase = 0;
                await UcitajSpecifikaciju();
            }
        }

        private void btn_KodSase_Click(object sender, EventArgs e)
        {
            string beleska;
            if (_specifikacija.Tag.KodSaseBeleksa != null)
            {
                beleska = _specifikacija.Tag.KodSaseBeleksa;
            }

            else
                beleska = "";
            using (fm_InputBeleske ib = new fm_InputBeleske("Beleska <<Kod Sase>>", "Unesite belesku za Kod Sase", beleska))
            {

                ib.ShowDialog();
                beleska = ib.returnData;
                _specifikacija.Tag.KodSaseBeleksa = beleska;
                _specifikacija.Update();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await UcitajSpecifikaciju();
        }

        private void eur1Kurs_txt_Enter(object sender, EventArgs e)
        {
            // Kick off SelectAll asynchronously so that it occurs after Click
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }

        private void eur2Kurs_txt_Enter(object sender, EventArgs e)
        {
            // Kick off SelectAll asynchronously so that it occurs after Click
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }

        private void specifikacijaKartica_txt_Enter(object sender, EventArgs e)
        {
            // Kick off SelectAll asynchronously so that it occurs after Click
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }

        private async void odDatuma_dtp_CloseUp(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            await UcitajSpecifikaciju();
        }

        private void gotovina5_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina5Sum_txt.Text = (Convert.ToInt32(gotovina5_txt.Text) * 5).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice5 = Convert.ToInt32(gotovina5_txt.Text);
            }
            catch (Exception)
            {
                gotovina5Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice5 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }

        private void gotovina2_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina2Sum_txt.Text = (Convert.ToInt32(gotovina2_txt.Text) * 2).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice2 = Convert.ToInt32(gotovina2_txt.Text);
            }
            catch (Exception)
            {
                gotovina2Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice2 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }

        private void gotovina1_txt_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            try
            {
                gotovina1Sum_txt.Text = (Convert.ToInt32(gotovina1_txt.Text)).ToString("#,##0.00 RSD");
                _specifikacija.Tag.Novcanice1 = Convert.ToInt32(gotovina1_txt.Text);
            }
            catch (Exception)
            {
                gotovina1Sum_txt.Text = "0.00 RSD";
                _specifikacija.Tag.Novcanice1 = 0;
            }
            _specifikacija.Update();
            PreracunajRazlikuSpecifikacije();
        }

        private void prethodnaSpecifikacija_btn_Click(object sender, EventArgs e)
        {
            bool magacinMode = Convert.ToBoolean(modNavigacijeSpecifikacije_btn.Tag);

            if(magacinMode)
            {
                odDatuma_dtp.Value = odDatuma_dtp.Value.AddDays(-1);
                button3.PerformClick();
            }
            else
            {
                var spec = TDOffice.SpecifikacijaNovca.Get(_specifikacija.ID - 1);
                if(spec == null)
                {
                    MessageBox.Show("Ne postoji specifikacija pre ove!");
                    return;
                }

                MagacinID = spec.MagacinID;
                odDatuma_dtp.Value = spec.Datum;
                button3.PerformClick();
            }
        }

        private void sledecaSpecifikacija_btn_Click(object sender, EventArgs e)
        {
            bool magacinMode = Convert.ToBoolean(modNavigacijeSpecifikacije_btn.Tag);

            if (magacinMode)
            {
                odDatuma_dtp.Value = odDatuma_dtp.Value.AddDays(1);
                button3.PerformClick();
            }
            else
            {
                var spec = TDOffice.SpecifikacijaNovca.Get(_specifikacija.ID + 11);
                if (spec == null)
                {
                    MessageBox.Show("Ne postoji specifikacija pre ove!");
                    return;
                }

                MagacinID = spec.MagacinID;
                odDatuma_dtp.Value = spec.Datum;
                button3.PerformClick();
            }
        }

        private void modNavigacijeSpecifikacije_btn_Click(object sender, EventArgs e)
        {
            bool current = Convert.ToBoolean(modNavigacijeSpecifikacije_btn.Tag);
            current = !current;

            modNavigacijeSpecifikacije_btn.Tag = current;
            modNavigacijeSpecifikacije_btn.BackColor = current ? System.Drawing.Color.Orange : Control.DefaultBackColor;
        }

        private void fiskalizovaniRacuniInfo_btn_Click(object sender, EventArgs e)
        {
            Komercijalno.Magacin magacin = Komercijalno.Magacin.Get(DateTime.Now.Year, MagacinID);
            if (magacin.PFRID != null)
            {
                string jid = Komercijalno.PFRS.List(DateTime.Now.Year).FirstOrDefault(x => x.PFRID == magacin.PFRID).JID;
                List<TDOffice.FiskalniRacun> fiskalniRacuni = TDOffice.FiskalniRacun.List($"SIGNED_BY = '{jid}'")
                    .Where(x => x.SDCTime_ServerTimeZone.Date == _specifikacija.Datum && x.TransactionType == "Sale").ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("SignedBy", typeof(string));
                dt.Columns.Add("InvoiceCounter", typeof(string));
                dt.Columns.Add("VremeFiskalizacije", typeof(DateTime));
                dt.Columns.Add("Cashier", typeof(string));
                dt.Columns.Add("BuyerTin", typeof(string));
                dt.Columns.Add("TotalAmount", typeof(double));
                dt.Columns.Add("InvoiceType", typeof(string));
                dt.Columns.Add("TransactionType", typeof(string));
                dt.Columns.Add("InvoiceNumber", typeof(string));
                dt.Columns.Add("TIN", typeof(string));
                dt.Columns.Add("VrDok", typeof(int));
                dt.Columns.Add("BrDok", typeof(int));
                dt.Columns.Add("PDV", typeof(double));
                dt.Columns.Add("TaxItems", typeof(string));

                Task<List<Komercijalno.DokumentFisk>> _dokumentFisk = Komercijalno.DokumentFisk.ListAsync(DateTime.Now.Year);
                List<TDOffice.FiskalniRacun_TaxItem> _fiskalniRacuniTaxItems = TDOffice.FiskalniRacun_TaxItem.List();

                foreach (TDOffice.FiskalniRacun fr in fiskalniRacuni)
                {
                    List<TDOffice.FiskalniRacun_TaxItem> taxItems = _fiskalniRacuniTaxItems.Where(x => x.InvoiceNumber == fr.InvoiceNumber).ToList();
                    Komercijalno.DokumentFisk df = _dokumentFisk.Result.FirstOrDefault(x => x.ReferentDocumentNumber == fr.InvoiceNumber);

                    DataRow dr = dt.NewRow();
                    dr["SignedBy"] = fr.SignedBy;
                    dr["InvoiceCounter"] = fr.InvoiceCounter;
                    dr["VremeFiskalizacije"] = fr.SDCTime_ServerTimeZone;
                    dr["Cashier"] = fr.Cashier;
                    dr["BuyerTin"] = fr.BuyerTin;
                    dr["TotalAmount"] = fr.TotalAmount;
                    dr["InvoiceType"] = fr.InvoiceType;
                    dr["TransactionType"] = fr.TransactionType;
                    dr["InvoiceNumber"] = fr.InvoiceNumber;
                    dr["TIN"] = fr.TIN;
                    dr["VrDok"] = df != null ? df.VrDok : -1;
                    dr["BrDok"] = df != null ? df.BrDok : -1;
                    dr["PDV"] = taxItems.Sum(x => x.Amount);
                    dr["TaxItems"] = JsonConvert.SerializeObject(taxItems);
                    dt.Rows.Add(dr);
                }

                using (DataGridViewSelectBox dgvsb = new DataGridViewSelectBox(dt))
                    dgvsb.ShowDialog();
            }
            else
            {
                MessageBox.Show("Greska!");
            }
        }

        private void fizkalizovanePovratniceInfo_btn_Click(object sender, EventArgs e)
        {
            Komercijalno.Magacin magacin = Komercijalno.Magacin.Get(DateTime.Now.Year, MagacinID);
            if (magacin.PFRID != null)
            {
                string jid = Komercijalno.PFRS.List(DateTime.Now.Year).FirstOrDefault(x => x.PFRID == magacin.PFRID).JID;
                List<TDOffice.FiskalniRacun> fiskalniRacuni = TDOffice.FiskalniRacun.List($"SIGNED_BY = '{jid}'")
                    .Where(x => x.SDCTime_ServerTimeZone.Date == _specifikacija.Datum && x.TransactionType == "Refund").ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("SignedBy", typeof(string));
                dt.Columns.Add("InvoiceCounter", typeof(string));
                dt.Columns.Add("VremeFiskalizacije", typeof(DateTime));
                dt.Columns.Add("Cashier", typeof(string));
                dt.Columns.Add("BuyerTin", typeof(string));
                dt.Columns.Add("TotalAmount", typeof(double));
                dt.Columns.Add("InvoiceType", typeof(string));
                dt.Columns.Add("TransactionType", typeof(string));
                dt.Columns.Add("InvoiceNumber", typeof(string));
                dt.Columns.Add("TIN", typeof(string));
                dt.Columns.Add("VrDok", typeof(int));
                dt.Columns.Add("BrDok", typeof(int));
                dt.Columns.Add("PDV", typeof(double));
                dt.Columns.Add("TaxItems", typeof(string));

                Task<List<Komercijalno.DokumentFisk>> _dokumentFisk = Komercijalno.DokumentFisk.ListAsync(DateTime.Now.Year);
                List<TDOffice.FiskalniRacun_TaxItem> _fiskalniRacuniTaxItems = TDOffice.FiskalniRacun_TaxItem.List();

                foreach (TDOffice.FiskalniRacun fr in fiskalniRacuni)
                {
                    List<TDOffice.FiskalniRacun_TaxItem> taxItems = _fiskalniRacuniTaxItems.Where(x => x.InvoiceNumber == fr.InvoiceNumber).ToList();
                    Komercijalno.DokumentFisk df = _dokumentFisk.Result.FirstOrDefault(x => x.ReferentDocumentNumber == fr.InvoiceNumber);

                    DataRow dr = dt.NewRow();
                    dr["SignedBy"] = fr.SignedBy;
                    dr["InvoiceCounter"] = fr.InvoiceCounter;
                    dr["VremeFiskalizacije"] = fr.SDCTime_ServerTimeZone;
                    dr["Cashier"] = fr.Cashier;
                    dr["BuyerTin"] = fr.BuyerTin;
                    dr["TotalAmount"] = fr.TotalAmount;
                    dr["InvoiceType"] = fr.InvoiceType;
                    dr["TransactionType"] = fr.TransactionType;
                    dr["InvoiceNumber"] = fr.InvoiceNumber;
                    dr["TIN"] = fr.TIN;
                    dr["VrDok"] = df != null ? df.VrDok : -1;
                    dr["BrDok"] = df != null ? df.BrDok : -1;
                    dr["PDV"] = taxItems.Sum(x => x.Amount);
                    dr["TaxItems"] = JsonConvert.SerializeObject(taxItems);
                    dt.Rows.Add(dr);
                }

                using (DataGridViewSelectBox dgvsb = new DataGridViewSelectBox(dt))
                    dgvsb.ShowDialog();
            }
            else
            {
                MessageBox.Show("Greska!");
            }
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }
    }
}
