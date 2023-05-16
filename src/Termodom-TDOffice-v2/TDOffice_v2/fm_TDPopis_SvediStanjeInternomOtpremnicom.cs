using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_TDPopis_SvediStanjeInternomOtpremnicom : Form
    {
        private List<Komercijalno.Magacin> magacini = new List<Komercijalno.Magacin>();
        private DataTable _dataTable { get; set; }
        private TDOffice.DokumentPopis _TDOffice_popis { get; set; }
        private Task<List<Komercijalno.Roba>> _komercijalno_roba { get; set; }
        private Task<List<Komercijalno.RobaUMagacinu>> _komercijalno_robaUMagacinu { get; set; }
        public fm_TDPopis_SvediStanjeInternomOtpremnicom(DataTable dt, TDOffice.DokumentPopis dp)
        {
            InitializeComponent();
            magacini = Komercijalno.Magacin.ListAsync().Result;
            magacini.Add(new Komercijalno.Magacin()
            {
                ID = -1,
                Naziv = "Svi magacini"
            });

            cmb_IzMagacinaManjkovi.DataSource = magacini.OrderBy(x => x.ID).ToList();
            cmb_IzMagacinaManjkovi.DisplayMember = "Naziv";
            cmb_IzMagacinaManjkovi.ValueMember = "ID";

            cmb_UMagacinManjkovi.DataSource = magacini.OrderBy(x => x.ID).ToList();
            cmb_UMagacinManjkovi.DisplayMember = "Naziv";
            cmb_UMagacinManjkovi.ValueMember = "ID";

            cmb_IzMagacinaViskovi.DataSource = magacini.OrderBy(x => x.ID).ToList();
            cmb_IzMagacinaViskovi.DisplayMember = "Naziv";
            cmb_IzMagacinaViskovi.ValueMember = "ID";

            cmb_UMagacinViskovi.DataSource = magacini.OrderBy(x => x.ID).ToList();
            cmb_UMagacinViskovi.DisplayMember = "Naziv";
            cmb_UMagacinViskovi.ValueMember = "ID";
            _dataTable = dt;
            _TDOffice_popis = dp;
        }

        private void fm_TDPopis_SvediStanjeInternomOtpremnicom_Load(object sender, EventArgs e)
        {

        }

        private void btn_Kreiraj_Click(object sender, EventArgs e)
        {
            int IzMagacinaManjkovi = Convert.ToInt32(cmb_IzMagacinaManjkovi.SelectedValue);
            int IzMagacinaViskovi = Convert.ToInt32(cmb_IzMagacinaViskovi.SelectedValue);
            int UMagacinManjkovi = Convert.ToInt32(cmb_UMagacinManjkovi.SelectedValue);
            int UmagacinViskovi = Convert.ToInt32(cmb_UMagacinViskovi.SelectedValue);
            _komercijalno_roba = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
            
            if (cb_IstaOtpremnicaZaViskoveiManjkove.Checked == true)
            {
                if((IzMagacinaViskovi < 0) || (UmagacinViskovi < 0))
                {
                    MessageBox.Show("Izaberite magacin!!!");
                    return;
                }
                if ((IzMagacinaViskovi - UmagacinViskovi) != 0)
                {
                    MessageBox.Show("Moraju biti selektovani isti magacini kao polazni i dolazni");
                    return;
                }
                
                List<Tuple<int, double>> razlike = new List<Tuple<int, double>>();
                foreach (DataRow r in _dataTable.Rows)
                {
                    double razlika = Convert.ToDouble(r["Razlika"]);
                    if (razlika != 0)
                    {
                        int robaID = Convert.ToInt32(r["RobaID"]);
                        razlike.Add(new Tuple<int, double>(robaID, razlika));
                    }
                }
                if (razlike.Count == 0)
                {
                    MessageBox.Show("Ne postoji ni jedna stavka sa razlikom da bi se kreirao dokument!");
                    return;
                }
                int brDokOtpremnice = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 19, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, IzMagacinaViskovi, Program.TrenutniKorisnik.KomercijalnoUserID, UmagacinViskovi);
                
                if (brDokOtpremnice <= 0)
                {
                    MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta otpremnice u komercijalnom poslovanju!");
                    return;
                }

                _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(IzMagacinaViskovi);
                foreach (Tuple<int, double> t in razlike)
                {
                    Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremnice), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                        Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == IzMagacinaViskovi && x.RobaID == t.Item1).FirstOrDefault(),
                        t.Item2, 0);
                }
                //Interna MP kalkulacija
                int brDokInternaMPKalkilacija = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 18, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, UmagacinViskovi, Program.TrenutniKorisnik.KomercijalnoUserID, IzMagacinaViskovi);
                
                if (brDokInternaMPKalkilacija <= 0)
                {
                    MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta Interna MP kalkulacija u komercijalnom poslovanju!");
                    return;
                }
                foreach (Tuple<int, double> t in razlike)
                {
                    Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacija), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                        Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == UmagacinViskovi && x.RobaID == t.Item1).FirstOrDefault(),
                        t.Item2, 0);
                }
                // Na Otpremnici izmena VRDOKOUT: 18 BRDOKOUT: brDokInternaMPKalkilacija
                Komercijalno.Dokument dokOtpremnica = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremnice);
                dokOtpremnica.VrDokOut = 18;
                dokOtpremnica.BrDokOut = brDokInternaMPKalkilacija;
                dokOtpremnica.Update();
                // Na Internoj MP Kalkulaciji
                Komercijalno.Dokument dokInternaMPKalkulacija = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacija);
                dokInternaMPKalkulacija.VrDokIn = 19;
                dokInternaMPKalkulacija.BrDokIn = brDokOtpremnice;
                dokInternaMPKalkulacija.Update();
                MessageBox.Show("Kreiran novi dokumenti Otpremnica broj: " + brDokOtpremnice.ToString() +" i novi dokument Interna MP kalkulacoija broj: " + brDokInternaMPKalkilacija.ToString());
            }
            else
            {
                if ((IzMagacinaViskovi < 0) || (UmagacinViskovi < 0) || (IzMagacinaManjkovi < 0) || (UMagacinManjkovi < 0))
                {
                    MessageBox.Show("Izaberite magacin!!!");
                    return;
                }
                List<Tuple<int, double>> razlikeManjkovi = new List<Tuple<int, double>>();
                List<Tuple<int, double>> razlikeViskovi = new List<Tuple<int, double>>();
                foreach (DataRow r in _dataTable.Rows)
                {
                    double razlika = Convert.ToDouble(r["Razlika"]);
                    if (razlika > 0)
                    {
                        int robaID = Convert.ToInt32(r["RobaID"]);
                        razlikeViskovi.Add(new Tuple<int, double>(robaID, razlika));
                    } 
                    else
                    if (razlika < 0)
                    {
                        int robaID = Convert.ToInt32(r["RobaID"]);
                        razlikeViskovi.Add(new Tuple<int, double>(robaID, razlika));
                    }
                }

                if ((razlikeViskovi.Count == 0) && (razlikeManjkovi.Count == 0))
                {
                    MessageBox.Show("Ne postoji ni jedna stavka sa razlikom da bi se kreirao dokument!");
                    return;
                }

                if (razlikeViskovi.Count > 0)
                {
                    int brDokOtpremniceViskova = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 19, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, IzMagacinaViskovi, Program.TrenutniKorisnik.KomercijalnoUserID, UmagacinViskovi);
                    
                    if (brDokOtpremniceViskova <= 0)
                    {
                        MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta otpremnice u komercijalnom poslovanju!");
                        return;
                    }
                    _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(IzMagacinaViskovi);
                    foreach (Tuple<int, double> t in razlikeViskovi)
                    {
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremniceViskova), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                            Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == IzMagacinaViskovi && x.RobaID == t.Item1).FirstOrDefault(),
                            t.Item2, 0);
                    }

                    //Interna MP kalkulacija
                    int brDokInternaMPKalkilacijaViskova = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 18, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, UmagacinViskovi, Program.TrenutniKorisnik.KomercijalnoUserID, IzMagacinaViskovi);

                    if (brDokInternaMPKalkilacijaViskova <= 0)
                    {
                        MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta Interna MP kalkulacija u komercijalnom poslovanju!");
                        return;
                    }
                    _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(UmagacinViskovi);
                    foreach (Tuple<int, double> t in razlikeViskovi)
                    {
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacijaViskova), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                            Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == UmagacinViskovi && x.RobaID == t.Item1).FirstOrDefault(),
                            t.Item2, 0);
                    }
                    // Na Otpremnici izmena VRDOKOUT: 18 BRDOKOUT: brDokInternaMPKalkilacijaViskova
                    Komercijalno.Dokument dokOtpremnicaViskova = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremniceViskova);
                    dokOtpremnicaViskova.VrDokOut = 18;
                    dokOtpremnicaViskova.BrDokOut = brDokInternaMPKalkilacijaViskova;
                    dokOtpremnicaViskova.Update();
                    // Na Internoj MP Kalkulaciji
                    Komercijalno.Dokument dokInternaMPKalkulacijaViskova = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacijaViskova);
                    dokInternaMPKalkulacijaViskova.VrDokIn = 19;
                    dokInternaMPKalkulacijaViskova.BrDokIn = brDokOtpremniceViskova;
                    dokInternaMPKalkulacijaViskova.Update();
                    MessageBox.Show("Kreiran novi dokumenti Otpremnica broj: " + brDokOtpremniceViskova.ToString() + " i novi dokument Interna MP kalkulacija broj: " + brDokInternaMPKalkilacijaViskova.ToString());
                }

                if (razlikeManjkovi.Count > 0)
                {
                    int brDokOtpremniceManjkova = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 19, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, IzMagacinaManjkovi, Program.TrenutniKorisnik.KomercijalnoUserID, UMagacinManjkovi);

                    if (brDokOtpremniceManjkova <= 0)
                    {
                        MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta otpremnice u komercijalnom poslovanju!");
                        return;
                    }
                    _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(IzMagacinaManjkovi);
                    foreach (Tuple<int, double> t in razlikeManjkovi)
                    {
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremniceManjkova), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                            Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == IzMagacinaManjkovi && x.RobaID == t.Item1).FirstOrDefault(),
                            t.Item2, 0);
                    }
                    //Interna MP kalkulacija
                    int brDokInternaMPKalkilacijaManjkova = Komercijalno.Dokument.Insert(_TDOffice_popis.Datum.Year, 18, "TD " + _TDOffice_popis.ID, null, "TDOffice_v2 - " + _TDOffice_popis.ID, 1, UMagacinManjkovi, Program.TrenutniKorisnik.KomercijalnoUserID, IzMagacinaManjkovi);

                    if (brDokInternaMPKalkilacijaManjkova <= 0)
                    {
                        MessageBox.Show("Doslo je do greske prilikmo kreiranja dokumenta Interna MP kalkulacija u komercijalnom poslovanju!");
                        return;
                    }
                    _komercijalno_robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(UMagacinManjkovi);
                    foreach (Tuple<int, double> t in razlikeManjkovi)
                    {
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacijaManjkova), _komercijalno_roba.Result.Where(x => x.ID == t.Item1).FirstOrDefault(),
                            Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(10)).Where(x => x.MagacinID == UMagacinManjkovi && x.RobaID == t.Item1).FirstOrDefault(),
                            t.Item2, 0);
                    }
                    // Na Otpremnici izmena VRDOKOUT: 18 BRDOKOUT: brDokInternaMPKalkilacijaManjkova
                    Komercijalno.Dokument dokOtpremnicaManjkova = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 19, brDokOtpremniceManjkova);
                    dokOtpremnicaManjkova.VrDokOut = 18;
                    dokOtpremnicaManjkova.BrDokOut = brDokInternaMPKalkilacijaManjkova;
                    dokOtpremnicaManjkova.Update();
                    // Na Internoj MP Kalkulaciji
                    Komercijalno.Dokument dokInternaMPKalkulacijaManjkova = Komercijalno.Dokument.Get(_TDOffice_popis.Datum.Year, 18, brDokInternaMPKalkilacijaManjkova);
                    dokInternaMPKalkulacijaManjkova.VrDokIn = 19;
                    dokInternaMPKalkulacijaManjkova.BrDokIn = brDokOtpremniceManjkova;
                    dokInternaMPKalkulacijaManjkova.Update();
                    MessageBox.Show("Kreiran novi dokumenti Otpremnica broj: " + brDokOtpremniceManjkova.ToString() + " i novi dokument Interna MP kalkulacija broj: " + brDokInternaMPKalkilacijaManjkova.ToString());
                }


            }
        }

        private void cb_IstaOtpremnicaZaViskoveiManjkove_CheckedChanged(object sender, EventArgs e)
        {
            gb_SveManjkoveOtpremnicom.Visible = !cb_IstaOtpremnicaZaViskoveiManjkove.Checked;
            gb_SveViskoveOtpremnicom.Text = cb_IstaOtpremnicaZaViskoveiManjkove.Checked ? "Sredi sve viskove i manjkove otpremnicom" : "Sredi sve viskove otpremnicom";
        }
    }
}
