using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using System.Threading.Tasks;

namespace TDOffice_v2
{
    public partial class _1348_fm_Partner_Index : Form
    {
        // Koristicemo ovu formu i za novog partnera i za postojecek (izmene)
        // Ako koristimo prvi konstruktor bez parter ID-a znaci da je to novi parnter
        // Ako korististimo drugi konstruktor znaci da gledamo partnera koji vec postoji i njega menjamo
        // U oba slucaja koristimo objekat _parner, a u akcijama cemo prepoznati da li je novi ili postojeci po ID-u
        private Komercijalno.Partner _partner;  

        private Komercijalno.TekuciRacun _tekuciRacun { get; set; }
        private TDOffice.PartnerKomercijalno _tdofficePartnerKomercijalno { get; set; }
        private Task<fm_Help> _helpFrom { get; set; }

        private bool _loaded = false;

        // Ovde u ovom konstruktoru kreiramo novog praznog partnera sto znaci da je ID == 0
        public _1348_fm_Partner_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Partner_Index);
            SetUI();

            this.Location = new System.Drawing.Point(10 + (System.DateTime.Now.Millisecond / 5), 10 + (System.DateTime.Now.Millisecond / 5));
            this.Text = "Novi kupac";

            _partner = new Partner();
            _tekuciRacun = new TekuciRacun();   

            clbSpecijalniCenovnici.Enabled = false;
            clbSpecijalniCenovniciNacinUplate.Enabled = false;
            specijalniCenovnikOtherUslov_cmb.Enabled = false;
            specijalniCenovnikOtherModifikator_txt.Enabled = false;
        }
        // Ovde hvatamo postojeceg partnera sto znaci da ID != 9
        public _1348_fm_Partner_Index(int partnerID)
        {
            InitializeComponent();
            
            SetUI();
            this.Location = new System.Drawing.Point(10 + (System.DateTime.Now.Millisecond / 5), 10 + (System.DateTime.Now.Millisecond / 5));
            _partner = Komercijalno.Partner.GetAsync(partnerID).Result;
            _tekuciRacun = Komercijalno.TekuciRacun.Get(DateTime.Now.Year, partnerID);
            _tdofficePartnerKomercijalno = TDOffice.PartnerKomercijalno.Get(partnerID);
            PopulateData();
            if (_tdofficePartnerKomercijalno != null && _tdofficePartnerKomercijalno.SpecijalniCenovnikPars != null &&
                _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList != null)
            {
                for(int i = 0; i < clbSpecijalniCenovnici.Items.Count; i++)
                {
                    TDOffice.SpecijalniCenovnik sc = clbSpecijalniCenovnici.Items[i] as TDOffice.SpecijalniCenovnik;
                    if (_tdofficePartnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList.Contains(sc.ID))
                        clbSpecijalniCenovnici.SetItemChecked(i, true);
                }
            }
            if (_tdofficePartnerKomercijalno != null && _tdofficePartnerKomercijalno.SpecijalniCenovnikPars != null &&
                _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.NaciniUplateZaKojiVazeSpecijalniCenovnici != null)
            {
                for (int i = 0; i < clbSpecijalniCenovniciNacinUplate.Items.Count; i++)
                {
                    Tuple<int, string> sc = clbSpecijalniCenovniciNacinUplate.Items[i] as Tuple<int, string>;
                    if (_tdofficePartnerKomercijalno.SpecijalniCenovnikPars.NaciniUplateZaKojiVazeSpecijalniCenovnici.Contains(sc.Item1))
                        clbSpecijalniCenovniciNacinUplate.SetItemChecked(i, true);
                }
            }
            clbSpecijalniCenovnici.Enabled = true;
            clbSpecijalniCenovniciNacinUplate.Enabled = true;

            //this.Text = "ovde treba uzeti naziv partnera";
            this.Text = "< " + _partner.Naziv + " >";
        }
        private Task<List<Komercijalno.Opstina>> _opstina = Task.Run(() =>
        {
            List<Komercijalno.Opstina> opstina = Komercijalno.Opstina.List();
            opstina.Add(new Komercijalno.Opstina()
            {
                ID = -1,
                Naziv = "Sve opstine"
            });

            return opstina.OrderByDescending(x => x.ID).ToList();
        });
        private Task<List<Komercijalno.Mesta>> _mesta = Task.Run(() =>
        {
            List<Komercijalno.Mesta> mesta = Komercijalno.Mesta.List();
           
            //return mesta.OrderByDescending(x => x.MestoID).ToList();
            return mesta.OrderBy(x => x.Naziv).ToList();
        });
        private Task<List<Komercijalno.Zemlja>> _zemlja = Task.Run(() =>
        {
            List<Komercijalno.Zemlja> zemlja = Komercijalno.Zemlja.List();
            zemlja.Add(new Komercijalno.Zemlja()
            {
                DrzavaID = -1,
                Naziv = "<Sve zemlje>"
            });

            return zemlja.OrderByDescending(x => x.DrzavaID).ToList();
        });
        private void PopulateData()
        {
            try
            {
                tbNaziv.Text = _partner.Naziv;
                tbAdresa.Text = _partner.Adresa;
                tbPosta.Text = _partner.Posta;
                cmbMesto.SelectedValue = _partner.MestoID;
                cmbOpstina.SelectedValue = _partner.OpstinaID;
                cmbDrzava.SelectedValue = _partner.DrzavaID;
                tbTelefoni.Text = _partner.Telefon;
                tbMobTel.Text = _partner.Mobilni;
                tbFax.Text = _partner.Fax;
                tbEmail.Text = _partner.Email;
                tbKontakt.Text = _partner.Kontakt;
                tbPib.Text = _partner.PIB;
                tbMaticniBroj.Text = _partner.MBroj;
                cbAktivan.Checked = _partner.Aktivan == 0 ? false : true;
                cbSistemPDV.Checked = _partner.PDVO == 0 ? false : true;
                string k;
                k = _partner.Kategorija.ToString();
                Partner.NapuniCheckedListBox(ref this.clbKategorije, k);
                tbTekuciRacun.Text = _tekuciRacun == null ? "" : _tekuciRacun.Racun;
                cmbBanka.SelectedValue = _tekuciRacun == null ? -1 : _tekuciRacun.BankaID;
                specijalniCenovnikOtherUslov_cmb.SelectedValue = _tdofficePartnerKomercijalno == null ? 2 : _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.OtherTip;
                specijalniCenovnikOtherModifikator_txt.Text = _tdofficePartnerKomercijalno == null ? "0" : _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.OtherModifikator.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void _1348_fm_Partner_Index_Load(object sender, EventArgs e)
        {
            _loaded = true;
        }
        private void SetUI()
        {
            try
            {
                clbKategorije.DataSource = Komercijalno.PPKategorije.List();
                clbKategorije.DisplayMember = "KatNaziv";
                clbKategorije.ValueMember = "KatID";

                clbSpecijalniCenovnici.DataSource = TDOffice.SpecijalniCenovnik.List();
                clbSpecijalniCenovnici.DisplayMember = "Naziv";
                clbSpecijalniCenovnici.ValueMember = "ID";

                specijalniCenovnikOtherUslov_cmb.DataSource = fm_SpecijalniCenovnik_Index._uslovTipovi;
                specijalniCenovnikOtherUslov_cmb.DisplayMember = "Item2";
                specijalniCenovnikOtherUslov_cmb.ValueMember = "Item1";

                clbSpecijalniCenovniciNacinUplate.DataSource = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(1, "Virmanom"),
                new Tuple<int, string>(5, "Gotovinom"),
                new Tuple<int, string>(11, "Karticom")
            };
                clbSpecijalniCenovniciNacinUplate.DisplayMember = "Item2";
                clbSpecijalniCenovniciNacinUplate.ValueMember = "Item1";

                cmbOpstina.DataSource = _opstina.Result;
                cmbOpstina.DisplayMember = "Naziv";
                cmbOpstina.ValueMember = "ID";

                cmbMesto.DataSource = _mesta.Result;
                cmbMesto.DisplayMember = "Naziv";
                cmbMesto.ValueMember = "MestoID";
                cmbMesto.SelectedValue = "-1";

                cmbDrzava.DataSource = _zemlja.Result;
                cmbDrzava.DisplayMember = "Naziv";
                cmbDrzava.ValueMember = "DrzavaID";
                cmbDrzava.SelectedValue = -1;

                cmbBanka.DataSource = Komercijalno.Banka.List(DateTime.Now.Year);
                cmbBanka.DisplayMember = "Naziv";
                cmbBanka.ValueMember = "BankaID";
                cmbBanka.SelectedValue = 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public Int64 stepenrobakategorije(int b)
        {
            Int64 strbk;
            if (b > 0)
            {
                strbk = 1;
                for (int j = 1; j <= b; j++)
                {
                    strbk = strbk * 2;
                }
            }
            else
            {
                strbk = 1;  
            }
            return Math.Abs(strbk); 
        }
        // Ukoliko je _partner.ID == 0 znaci datreba da kreiramo novog
        // Ukoliko je _partner.ID != 0 znaci da treba da updateujemo partnera
        private async void btnSacuvaj_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Int64 kat = 0;
                if (clbKategorije.CheckedItems.Count < 4)
                {
                    MessageBox.Show("Niste popunili kategorije partnera!");
                    return;
                }
                else
                {
                    for (int i = 0; i < clbKategorije.Items.Count - 1; i++)
                    {
                        if (clbKategorije.GetItemChecked(i))
                        {
                            kat = kat + stepenrobakategorije(i);
                        }
                    }
                }
                if (cmbMesto.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Niste izabrali mesto!");
                    return;
                }
                if(string.IsNullOrWhiteSpace(tbMaticniBroj.Text))
                {
                    MessageBox.Show("Morate uneti matican broj!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Morate uneti email!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbKontakt.Text))
                {
                    MessageBox.Show("Morate uneti ime kontakt osobe!");
                    return;
                }
                if (specijalniCenovnikOtherUslov_cmb.SelectedIndex == -1)
                {
                    MessageBox.Show("Neispravan tip uslova nedefinisane robe u specijalnom cenovniku!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(specijalniCenovnikOtherModifikator_txt.Text))
                {
                    MessageBox.Show("Neispravna vrednost modifikatora nedefinisane robe u specijalnom cenovniku!");
                    return;
                }

                double specijalniCenovnikOtherModifikator = 0;
                try
                {
                    specijalniCenovnikOtherModifikator = Convert.ToInt32(specijalniCenovnikOtherModifikator_txt.Text);
                }
                catch
                {
                    MessageBox.Show("Neispravna vrednost modifikatora nedefinisane robe u specijalnom cenovniku!");
                    return;
                }

                _partner.Kategorija = kat;
                _partner.Valuta = "DIN";
                _partner.RefID = Program.TrenutniKorisnik.ID;
                _partner.DozvoljeniMinus = 0;
                _partner.ImaUgovor = 0;
                _partner.VrstaCenovnika = 0;
                _partner.DrzavljanstvoID = 0;
                _partner.ZanimanjeID = 0;
                _partner.WEB_Status = 0;
                _partner.GPPID = 0;
                _partner.Cene_od_grupe = 0;
                _partner.VPCID = 1;
                _partner.PROCPC = 0;
                //_partner.MestoID = "1";

                if (_partner.PPID == 0)
                {
                    if (await Partner.GetAsync(_partner.PIB) != null)
                    {
                        MessageBox.Show("Partner sa datim pibom vec postoji!");
                        return;
                    }
                    if (_tekuciRacun.Racun == null)
                    {
                        _tekuciRacun.Racun = "";
                        if (MessageBox.Show("Niste uneli Racun. Da li zelite da unesete novog partnera bez Racuna?", "Novi partner", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        {
                            _tekuciRacun.Racun = null;
                            return;
                        }
                    }
                    int NewPPID;
                    NewPPID = await Komercijalno.Partner.Insert(_partner.Naziv, _partner.Adresa, _partner.Posta, _partner.Telefon, _partner.Mobilni, _partner.Fax,
                        _partner.Email, _partner.Kontakt, _partner.PIB, _partner.Kategorija, 1, _partner.MestoID, _partner.MBroj, _partner.OpstinaID,
                        _partner.DrzavaID, _partner.RefID, _partner.PDVO, _partner.NazivZaStampu, _partner.Valuta, _partner.DozvoljeniMinus, _partner.ImaUgovor,
                        _partner.VrstaCenovnika, _partner.DrzavljanstvoID, _partner.ZanimanjeID, _partner.WEB_Status, _partner.GPPID,
                        _partner.Cene_od_grupe, _partner.VPCID, _partner.PROCPC);

                    Komercijalno.TekuciRacun.Insert(DateTime.Now.Year, NewPPID, _tekuciRacun.Racun, _tekuciRacun.BankaID, _partner.Valuta, 0, null);
                    string naslov = "Novi partner";
                    string tekst = "Keiran je novi partner <" + _partner.Naziv + ">\n PPID <" + NewPPID.ToString() + ">";

                    TDOffice.Poruka.Insert(new TDOffice.Poruka()
                    {
                        Datum = DateTime.Now,
                        Naslov = naslov,
                        Posiljalac = Program.TrenutniKorisnik.ID,
                        Primalac = Program.TrenutniKorisnik.ID,
                        Status = TDOffice.PorukaTip.Standard,
                        Tag = new TDOffice.PorukaAdditionalInfo()
                        {
                            Action = TDOffice.PorukaAction.NoviPartner,
                            AdditionalInfo = NewPPID
                        },
                        Tekst = tekst
                    });
                    MessageBox.Show("Partner uspesno kreiran");
                }
                else
                {
                    _partner.Update();
                    if(_tekuciRacun != null)
                        _tekuciRacun.Update(DateTime.Now.Year, _tekuciRacun.PPID);

                    if(_tdofficePartnerKomercijalno == null)
                    {
                        TDOffice.PartnerKomercijalno.Insert(_partner.PPID, new TDOffice.PartnerKomercijalno.SpecijalniCenovnikDTO());
                        _tdofficePartnerKomercijalno = TDOffice.PartnerKomercijalno.Get(_partner.PPID);
                    }

                    _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.OtherModifikator = specijalniCenovnikOtherModifikator;
                    _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.OtherTip = Convert.ToInt32(specijalniCenovnikOtherUslov_cmb.SelectedValue);
                    _tdofficePartnerKomercijalno.Update();
                    MessageBox.Show("Uspesno izvrsene izmene");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void tbPosta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!_loaded)
                return;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            _partner.Posta = tbPosta.Text;
        }
        private void tbTelefoni_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            if (System.Text.RegularExpressions.Regex.IsMatch(tbTelefoni.Text, "[^0-9,;, ]"))
            {
                tbTelefoni.Text = tbTelefoni.Text.Remove(tbTelefoni.Text.Length - 1);
                tbTelefoni.Select(tbTelefoni.Text.Length, tbTelefoni.Text.Length);
            }
            _partner.Telefon = tbTelefoni.Text;
        }
        private void tbMobTel_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            if (System.Text.RegularExpressions.Regex.IsMatch(tbMobTel.Text, "[^0-9,;, ]"))
            {
                tbMobTel.Text = tbMobTel.Text.Remove(tbMobTel.Text.Length - 1);
                tbMobTel.Select(tbMobTel.Text.Length, tbMobTel.Text.Length);
            }
            _partner.Mobilni = tbMobTel.Text;
        }
        private void tbFax_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            if (System.Text.RegularExpressions.Regex.IsMatch(tbFax.Text, "[^0-9,;, ]"))
            {
                tbFax.Text = tbFax.Text.Remove(tbFax.Text.Length - 1);
                tbFax.Select(tbFax.Text.Length, tbFax.Text.Length);
            }
            _partner.Fax = tbFax.Text;
        }
        private void tbNaziv_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.Naziv = tbNaziv.Text;
            _partner.NazivZaStampu = tbNaziv.Text;
        }
        private void tbAdresa_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.Adresa = tbAdresa.Text;
        }
        private void tbPosta_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.Posta = tbPosta.Text;
        }
        private void cmbMesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.MestoID = cmbMesto.SelectedValue.ToString();
        }
        private void cmbOpstina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.OpstinaID = Convert.ToInt32(cmbOpstina.ValueMember);
        }
        private void cmbDrzava_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.DrzavaID = 1; // Convert.ToInt32(cmbDrzava.SelectedValue);
        }
        private void tbEmail_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.Email = tbEmail.Text;
        }
        private void tbKontakt_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.Kontakt = tbKontakt.Text;
        }
        private void tbPib_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.PIB = tbPib.Text;
        }
        private void tbMaticniBroj_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _partner.MBroj = tbMaticniBroj.Text;
        }
        private void cbAktivan_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            if (cbAktivan.Checked)
            {
                _partner.Aktivan = 1;
            }

            else
            {
                _partner.Aktivan = 0;
            }
                
        }
        private void cbSistemPDV_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            if (cbSistemPDV.Checked)
            {
                _partner.PDVO = 1;
            }

            else
            {
                _partner.PDVO = 0;
            }
            
        }

        private void cmbBanka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;
            _tekuciRacun.BankaID = Convert.ToInt32(cmbBanka.SelectedValue);
        }

        private void tbTekuciRacun_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _tekuciRacun.Racun = tbTekuciRacun.Text;
        }

        

        private void cmbMesto_Validated(object sender, EventArgs e)
        {
            string stmesto = cmbMesto.Text;
            List<Mesta> list = cmbMesto.DataSource as List<Mesta>;

            foreach(Mesta m in list)
            {
                if (m.Naziv.ToUpper() == stmesto.ToUpper())
                {
                    cmbMesto.SelectedValue = m.MestoID;
                    return;
                }
            }

            cmbMesto.SelectedValue = "-1";
            MessageBox.Show("Mesto " + stmesto + " ne postoji u bazi");
        }

        private void cmbBanka_Validated(object sender, EventArgs e)
        {
            string stBanka = cmbBanka.Text;
            List<Banka> list = cmbBanka.DataSource as List<Banka>;

            foreach (Banka m in list)
            {
                if (m.Naziv.ToUpper() == stBanka.ToUpper())
                {
                    cmbBanka.SelectedValue = m.BankaID;
                    return;
                }
            }

            cmbBanka.SelectedValue = "-1";
            MessageBox.Show("Banka " + stBanka + " ne postoji u bazi");
        }

        private void clbSpecijalniCenovnici_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            List<int> scs = new List<int>();
            foreach(TDOffice.SpecijalniCenovnik sc in clbSpecijalniCenovnici.CheckedItems)
            {
                scs.Add(sc.ID);
            }
            if(_tdofficePartnerKomercijalno == null)
            {
                TDOffice.PartnerKomercijalno.Insert(_partner.PPID, new TDOffice.PartnerKomercijalno.SpecijalniCenovnikDTO());
                _tdofficePartnerKomercijalno = TDOffice.PartnerKomercijalno.Get(_partner.PPID);
            }

            if(_tdofficePartnerKomercijalno.SpecijalniCenovnikPars == null)
            {
                _tdofficePartnerKomercijalno.SpecijalniCenovnikPars = new TDOffice.PartnerKomercijalno.SpecijalniCenovnikDTO()
                {
                    SpecijalniCenovnikList = scs
                };
                return;
            }

            _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList = scs;
        }

        private void clbSpecijalniCenovniciNacinUplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            List<int> scs = new List<int>();
            foreach(Tuple<int, string> tp in clbSpecijalniCenovniciNacinUplate.CheckedItems)
            {
                scs.Add(tp.Item1);
            }
            if (_tdofficePartnerKomercijalno == null)
            {
                TDOffice.PartnerKomercijalno.Insert(_partner.PPID, new TDOffice.PartnerKomercijalno.SpecijalniCenovnikDTO());
                _tdofficePartnerKomercijalno = TDOffice.PartnerKomercijalno.Get(_partner.PPID);
            }

            if (_tdofficePartnerKomercijalno.SpecijalniCenovnikPars == null)
            {
                _tdofficePartnerKomercijalno.SpecijalniCenovnikPars = new TDOffice.PartnerKomercijalno.SpecijalniCenovnikDTO()
                {
                    NaciniUplateZaKojiVazeSpecijalniCenovnici = scs
                };
                return;
            }

            _tdofficePartnerKomercijalno.SpecijalniCenovnikPars.NaciniUplateZaKojiVazeSpecijalniCenovnici = scs;
        }
    }
}
