using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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
    public partial class _1301_fm_Poruka_Index : Form
    {
        /// <summary>
        /// Lista id-eva poruka nad kojima je pritisnuto dugme "sakrij poruku".
        /// Koristi se da loop za prikazivanje poruka moze da obrati paznju kako ne bi ponovo prikazao ove poruke.
        /// </summary>
        public static List<int> IDeviSakrivenihPoruka { get; set; } = new List<int>();

        public int PorukaID
        {
            get
            {
                if (_poruka == null)
                    throw new NullReferenceException("Objekat TDOffice.Poruka koji je vezan za ovu formu je null!");

                return _poruka.ID;
            }
        }
        private bool _loaded = false;
        private TDOffice.Poruka _poruka;
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        public bool ForceClose { get; set; } = false;

        public _1301_fm_Poruka_Index(TDOffice.Poruka poruka)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this._poruka = poruka;
            this.TopMost = true;
            SetUI();
        }
        private void _1301_fm_Poruka_Index_Load(object sender, EventArgs e)
        {
            if (_poruka.Status == TDOffice.PorukaTip.Sticky)
            {
                _poruka.DatumCitanja = DateTime.Now;
                _poruka.Update();
            }

            _loaded = true;
        }
        private void _1301_fm_Poruka_Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((_poruka.Status == TDOffice.PorukaTip.Sticky && !ForceClose) || _poruka.Status == TDOffice.PorukaTip.Expanding)
                e.Cancel = true;
        }
        private void SetUI()
        {
            this.idPoruke_lbl.Text = _poruka.ID.ToString();

            this.Size = new Size(800, 150);

            this.panel1.Size = new Size(800, 33);
            this.panel1.BackColor = _poruka.Status == TDOffice.PorukaTip.Sticky ? Color.Purple : _poruka.Status == TDOffice.PorukaTip.Expanding ? Color.Red : this.panel1.BackColor;

            PozicionirajPoruku();

            TDOffice.User posiljalac = _korisnici.Result.Where(x => x.ID == _poruka.Posiljalac).FirstOrDefault();
            posiljalac_txt.Text = posiljalac == null ? "Sistemska Poruka" : posiljalac.Username;
            datum_txt.Text = _poruka.Datum.ToString("dd.MM.yyyy [ HH:mm:ss ]");
            tekst_rtb.Text = _poruka.Tekst;
            status_lbl.Text = _poruka.DatumCitanja == null ? "Poslata" : "Procitana";
            status_lbl.ForeColor = _poruka.DatumCitanja == null ? Color.Orange : Color.Green;
            this.naslov_txt.Text = _poruka.Status == TDOffice.PorukaTip.Sticky ? "Sticky: " + _poruka.Naslov : _poruka.Naslov;
            this.Text = "Poruka - " + _poruka.Naslov;
            status_lbl.Visible = _poruka.Primalac == Program.TrenutniKorisnik.ID ? false : true;
            
            if (_poruka.Posiljalac == Program.TrenutniKorisnik.ID)
                arhiviraj_btn.Visible = false;
            
            if (_poruka.Posiljalac == _poruka.Primalac)
                arhiviraj_btn.Visible = true;

            if (_poruka.DatumCitanja != null)
                    ProsiriPoruku();
                else
                    arhiviraj_btn.Visible = false;


            this.label4.ForeColor = _poruka.Tag != null && !string.IsNullOrWhiteSpace(_poruka.Tag.Beleska) ? Color.Orange : Color.Black;

            if (_poruka.Tag != null)
            {
                switch (_poruka.Tag.Action)
                {
                    case TDOffice.PorukaAction.NULL:
                        action_btn.Visible = false;
                        break;
                    case TDOffice.PorukaAction.NoviTDOfficePopis:
                        action_btn.Visible = true;
                        action_btn.Text = "Otvori Popis";
                        break;
                    case TDOffice.PorukaAction.NoviPartner:
                        action_btn.Visible = true;
                        action_btn.Text = "Prikazi partnera";
                        break;
                    case TDOffice.PorukaAction.DataTableAttachment:
                        action_btn.Visible = true;
                        action_btn.Text = "Prikazi prilog";
                        break;
                    case TDOffice.PorukaAction.OdgovorNaStickyPoruku:
                        action_btn.Visible = true;
                        action_btn.Text = "Obelezi zadak kao izvrsen";
                        break;
                    case TDOffice.PorukaAction.RealizovanaZamenaRobe:
                        action_btn.Visible = true;
                        action_btn.Text = "Idi na zamenu robe br: " + _poruka.Tag.AdditionalInfo;
                        break;
                    case TDOffice.PorukaAction.IzvestajProdajeRobe:
                    case TDOffice.PorukaAction.IzvestajProdajeRobeZbirnoPoMagacinima:
                    case TDOffice.PorukaAction.IzvestajPrometaMagacina:
                    case TDOffice.PorukaAction.OdobreniRabatMagacina:
                        action_btn.Visible = true;
                        action_btn.Text = "Prikazi izvestaj";
                        break;
                    case TDOffice.PorukaAction.RealizovanoRazduzenjeMagacina:
                        action_btn.Visible = true;
                        action_btn.Text = "Idi na razduzenje magacina br: " + _poruka.Tag.AdditionalInfo;
                        break;
                    case TDOffice.PorukaAction.PravoPristupaModulu:
                        action_btn.Visible = true;
                        action_btn.Text = "Pokreni akciju ";// + _poruka.Tag.AdditionalInfo;
                        break;
                }
            }

            if (_poruka.Paket != null)
            {
                List<TDOffice.Poruka> porukeCC = TDOffice.Poruka.List($"PAKET = {(int)_poruka.Paket} AND TIPPRIMAOCA = {(int)TDOffice.Enums.PorukaTipPrimaoca.CC}");
                string stCC = "CC: ";
                foreach (TDOffice.Poruka pcc in porukeCC)
                {
                    TDOffice.User primalac = _korisnici.Result.Where(x => x.ID == pcc.Primalac).FirstOrDefault();
                    if (primalac.ID != _poruka.Posiljalac)
                    {
                        stCC += primalac.Username + ", ";
                    }

                }
                txt_CC.Text = stCC;
                toolTip1.SetToolTip(txt_CC, stCC);
            }

            if (_poruka.Status == TDOffice.PorukaTip.Sticky)
            {
                minimiziraj_btn.Visible = false;
                arhiviraj_btn.Visible = false;
                btnProsledi.Visible = false;
            }
            else if(_poruka.Status == TDOffice.PorukaTip.Expanding)
            {
                ProsiriPoruku();
                minimiziraj_btn.Visible = false;
                arhiviraj_btn.Visible = false;
                zatvori_btn.Visible = false;
                btnProsledi.Visible = false;
                this.TopMost = true;
                this.TopLevel = true;
                double days = (DateTime.Now.Date - _poruka.Datum.Date).TotalDays;

                Rectangle currentScreen = Screen.FromControl(this).Bounds;
                this.Size = new Size((int)Math.Min(currentScreen.Width, 800 * Math.Pow(1.2, days)), (int)Math.Min(currentScreen.Height, 400 * Math.Pow(1.2, days)));
                if (this.Size.Width == currentScreen.Width)
                    this.Location = new Point(0, 0);
                polozaj_cmb.Visible = false;
                int oc = Math.Max(0, Convert.ToInt32(220 - Math.Min(240, Math.Pow(1.5, Math.Pow(days + 1, 2.2)))));
                this.BackColor = Color.FromArgb(255, oc, oc);
            }

            if((_poruka.Status == TDOffice.PorukaTip.Expanding || _poruka.Status == TDOffice.PorukaTip.Sticky) && _poruka.Tag.Action != TDOffice.PorukaAction.OdgovorNaStickyPoruku)
            {
                action_btn.Visible = true;
                action_btn.Text = "Izvesti o izvrsenju";
            }

            polozaj_cmb.Items.Add("Standard");
            polozaj_cmb.Items.Add("Prioritet 1");
            polozaj_cmb.Items.Add("Prioritet 2");
            polozaj_cmb.Items.Add("Prioritet 3");
            polozaj_cmb.SelectedIndex = _poruka.Tag.PolozajPoruke;
        }

        private void prikaziPoruku_btn_Click(object sender, EventArgs e)
        {
            if (Program.TrenutniKorisnik.ID == _poruka.Primalac)
            {
                _poruka.DatumCitanja = DateTime.Now;
                _poruka.Update();
                arhiviraj_btn.Visible = true;
            }
            else arhiviraj_btn.Visible = false;
            ProsiriPoruku();
            
        }

        private void PozicionirajPoruku()
        {
            Screen current = Screen.FromHandle(this.Handle);
            int pivotX = _poruka.Tag.PolozajPoruke % 2 == 0 ? 0 : current.Bounds.Width / 2;
            int pivotY = _poruka.Tag.PolozajPoruke > 1 ? current.Bounds.Height / 2 : 0;
            this.Location = new System.Drawing.Point(pivotX + (System.DateTime.Now.Millisecond / 50), pivotY + (System.DateTime.Now.Millisecond / 50));
        }
        public void ProsiriPoruku()
        {
            prikazi_btn.Visible = false;
            status_lbl.Text = _poruka.DatumCitanja == null ? "Poslata" : "Procitana";
            status_lbl.ForeColor = _poruka.DatumCitanja == null ? Color.Orange : Color.Green;
            this.Size = new Size(756, 400);
            btnOdgovor.Visible = true;
            btnProsledi.Visible = _poruka.Status == TDOffice.PorukaTip.Sticky ? false : true;
        }

        private void zatvori_btn_Click(object sender, EventArgs e)
        {
            ForceClose = true;
            this.Close();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnOdgovor_Click(object sender, EventArgs e)
        {
            int idPoruke = _poruka.ID;

            var trenutnaPoruka = TDOffice.Poruka.Get(idPoruke);
            TDOffice.User trenutnaPorukaKorisnik = _korisnici.Result.FirstOrDefault(x => x.ID == trenutnaPoruka.Posiljalac);
            string oldText = trenutnaPoruka.Tekst;
            trenutnaPoruka.Tekst = "\n\n========================\n";
            trenutnaPoruka.Tekst += trenutnaPoruka.Datum.ToString("[ dd.MM.yyyy ]\n");
            trenutnaPoruka.Tekst += trenutnaPorukaKorisnik == null ? "nepoznati korisnik" : trenutnaPorukaKorisnik.Username + " je napisao:\n";
            trenutnaPoruka.Tekst += "========================\n";
            trenutnaPoruka.Tekst += oldText;
            trenutnaPoruka.Primalac = trenutnaPoruka.Posiljalac;
            trenutnaPoruka.Posiljalac = Program.TrenutniKorisnik.ID;

            using (_1301_fm_Poruka_Nova nova = new _1301_fm_Poruka_Nova(trenutnaPoruka))
            {
                nova.TopMost = true;
                nova.IsNaslovReadOnly = true;
                nova.VidljivostPorukeIzbor = false;
                nova.ShowDialog();
            }

            this.Close();
        }

        private void btnProsledi_Click(object sender, EventArgs e)
        {
            int idPoruke = _poruka.ID;

            TDOffice.Poruka poruka = TDOffice.Poruka.Get(idPoruke); // Trenutno prikazanu poruku
            TDOffice.Poruka novaPoruka = new TDOffice.Poruka();
            novaPoruka.Tekst = poruka.Tekst;
            novaPoruka.Naslov = "Prosledjeno: " + poruka.Naslov;
            novaPoruka.Primalac = 0;
            novaPoruka.Posiljalac = Program.TrenutniKorisnik.ID;
            novaPoruka.Tag = poruka.Tag;

            _1301_fm_Poruka_Nova nova = new _1301_fm_Poruka_Nova(novaPoruka);
                nova.Show();
            
        }

        private void action_btn_Click(object sender, EventArgs e)
        {
            switch (_poruka.Tag.Action)
            {
                case TDOffice.PorukaAction.NULL:
                    if (_poruka.Status == TDOffice.PorukaTip.Expanding || _poruka.Status == TDOffice.PorukaTip.Sticky)
                    {
                        int idPoruke = _poruka.ID;

                        var trenutnaPoruka = TDOffice.Poruka.Get(idPoruke);
                        TDOffice.User trenutnaPorukaKorisnik = _korisnici.Result.Where(x => x.ID == trenutnaPoruka.Posiljalac).FirstOrDefault();
                        string oldText = trenutnaPoruka.Tekst;
                        trenutnaPoruka.Tekst = "\n\n========================\n";
                        trenutnaPoruka.Tekst += trenutnaPoruka.Datum.ToString("[ dd.MM.yyyy ]\n");
                        trenutnaPoruka.Tekst += trenutnaPorukaKorisnik == null ? "nepoznati korisnik" : trenutnaPorukaKorisnik.Username + " je napisao:\n";
                        trenutnaPoruka.Tekst += "========================\n";
                        trenutnaPoruka.Tekst += oldText;
                        trenutnaPoruka.Primalac = trenutnaPoruka.Posiljalac;
                        trenutnaPoruka.Posiljalac = Program.TrenutniKorisnik.ID;

                        using (_1301_fm_Poruka_Nova nova = new _1301_fm_Poruka_Nova(trenutnaPoruka))
                        {
                            nova.AdditionalInfo = _poruka.ID;
                            nova.Akcija = TDOffice.PorukaAction.OdgovorNaStickyPoruku;
                            nova.TopMost = true;
                            nova.IsNaslovReadOnly = true;
                            nova.VidljivostPorukeIzbor = false;
                            nova.ShowDialog();
                        }
                    }
                    break;
                case TDOffice.PorukaAction.NoviTDOfficePopis:
                    this.TopMost = false;
                    this.TopLevel = false;

                    using (_7_fm_TDPopis_Index pi = new _7_fm_TDPopis_Index(TDOffice.DokumentPopis.Get(Convert.ToInt32(_poruka.Tag.AdditionalInfo))))
                        pi.ShowDialog();

                    this.TopMost = true;
                    this.TopLevel = true;
                    break;
                case TDOffice.PorukaAction.NoviPartner:
                    using (_1348_fm_Partner_Index ppnew = new _1348_fm_Partner_Index(Convert.ToInt32(_poruka.Tag.AdditionalInfo)))
                        ppnew.ShowDialog();
                    break;
                case TDOffice.PorukaAction.OdgovorNaStickyPoruku:
                    TDOffice.Poruka porukaPosiljaoca = TDOffice.Poruka.Get(Convert.ToInt32(_poruka.Tag.AdditionalInfo));
                    porukaPosiljaoca.Arhivirana = true;
                    porukaPosiljaoca.Update();
                    MessageBox.Show("Zadatak obelezen kao izvrsen!");
                    _poruka.Arhivirana = true;
                    _poruka.Update();
                    this.Close();
                    break;
                case TDOffice.PorukaAction.RealizovanaZamenaRobe:
                    using (fm_ZamenaRobe2_Index zi = new fm_ZamenaRobe2_Index(Convert.ToInt32(_poruka.Tag.AdditionalInfo)))
                        zi.ShowDialog();
                    break;
                case TDOffice.PorukaAction.RealizovanoRazduzenjeMagacina:
                    using (_1336_fm_RazduzenjeMagacina_Index zi = new _1336_fm_RazduzenjeMagacina_Index(TDOffice.DokumentRazduzenjaMagacina.Get(Convert.ToInt32(_poruka.Tag.AdditionalInfo))))
                        zi.ShowDialog();
                    break;
                case TDOffice.PorukaAction.DataTableAttachment:
                    string js = JsonConvert.SerializeObject(_poruka.Tag.AdditionalInfo);
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(js);
                    using (DataGridViewSelectBox box = new DataGridViewSelectBox(dt))
                    {
                        box.RowHeaderVisible = false;
                        box.TopMost = true;
                        box.TopLevel = true;
                        box.ShowDialog();
                    }
                    break;
                case TDOffice.PorukaAction.IzvestajProdajeRobe:
                    IEnumerable<object> obj = _poruka.Tag.AdditionalInfo as IEnumerable<object>;
                    DataTable dtt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject((obj.First() as IEnumerable<object>).First()));
                    Models.Periodi periodi = JsonConvert.DeserializeObject<Models.Periodi>(JsonConvert.SerializeObject((obj.ElementAt(1) as IEnumerable<object>).First()));
                    string tipizv = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject((obj.ElementAt(2) as IEnumerable<object>).FirstOrDefault()));
                    List<int> nList = new List<int>();
                    using (fm_Izvestaj_Prodaja_Roba_Index ind = new fm_Izvestaj_Prodaja_Roba_Index(dtt, periodi, tipizv, nList, nList))
                    {
                        ind.TopMost = true;
                        ind.TopLevel = true;
                        ind.DozvoliSlanjeIzvestaja = false;
                        ind.ShowDialog();
                    }
                    break;
                case TDOffice.PorukaAction.IzvestajProdajeRobeZbirnoPoMagacinima:
                    IEnumerable<object> obj1 = _poruka.Tag.AdditionalInfo as IEnumerable<object>;
                    DataTable dtt1 = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject((obj1.First() as IEnumerable<object>).First()));
                    Models.Periodi periodi1 = JsonConvert.DeserializeObject<Models.Periodi>(JsonConvert.SerializeObject((obj1.ElementAt(1) as IEnumerable<object>).First()));
                    List<int> nList1 = new List<int>();
                    string st ="";

                    using (fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima ind = new fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima(dtt1, periodi1, st, nList1))
                    {
                        ind.DozvoliSlanjeIzvestaja = false;
                        ind.TopMost = true;
                        ind.TopLevel = true;
                        ind.ShowDialog();
                    }
                    break;
                case TDOffice.PorukaAction.IzvestajPrometaMagacina:
                    //IEnumerable<object> obj2 = _poruka.Tag.AdditionalInfo as IEnumerable<object>;
                    //DataTable dtt2 = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject((obj2.First() as IEnumerable<object>).First()));
                    string js2 = JsonConvert.SerializeObject(_poruka.Tag.AdditionalInfo);
                    DataTable dtt2 = JsonConvert.DeserializeObject<DataTable>(js2);
                    using (fm_PrikazIzvestajPrometMagacina ind = new fm_PrikazIzvestajPrometMagacina(dtt2))
                    {
                        ind.TopMost = true;
                        ind.TopLevel = true;
                        ind.ShowDialog();
                    }
                    break;
                case TDOffice.PorukaAction.OdobreniRabatMagacina:
                    string js3 = JsonConvert.SerializeObject(_poruka.Tag.AdditionalInfo);
                    DataTable dtt3 = JsonConvert.DeserializeObject<DataTable>(js3);
                    using (fm_PrikazIzvestajPrometMagacina ind = new fm_PrikazIzvestajPrometMagacina(dtt3))
                    {
                        ind.TopMost = true;
                        ind.TopLevel = true;
                        ind.Text = "Odobreni rabat magacina";
                        ind.ShowDialog();
                    }
                    break;
                case TDOffice.PorukaAction.PravoPristupaModulu:
                    using (fm_PravoPristupaModulu_OdobriZabrani ppm = new fm_PravoPristupaModulu_OdobriZabrani(_poruka))
                    {
                        ppm.ShowDialog();
                        ppm.BringToFront();
                    }
                        
                    //TDOffice.Pravo lp = TDOffice.Pravo.List().Where(x => x.ModulID == Convert.ToInt32(_poruka.Tag.AdditionalInfo) && x.UserID == _poruka.Posiljalac).FirstOrDefault();
                    //if (lp == null)
                    //{
                    //    TDOffice.Pravo.Insert(Convert.ToInt32(_poruka.Tag.AdditionalInfo), 1, _poruka.Posiljalac);
                    //}
                    //else
                    //{
                    //    TDOffice.Pravo ps = TDOffice.Pravo.Get(lp.ID);
                    //    ps.Value = 1;
                    //    ps.Update();
                    //}

                    ////Slanje poruke
                    //TDOffice.Poruka.Insert(new TDOffice.Poruka()
                    //{
                    //    Datum = DateTime.Now,
                    //    Naslov = "Odobreno pravo pristupa modula " + Convert.ToInt32(_poruka.Tag.AdditionalInfo),
                    //    Posiljalac = Program.TrenutniKorisnik.ID,
                    //    Primalac = _poruka.Posiljalac,
                    //    Status = TDOffice.PorukaTip.Standard,
                    //    Tag = new TDOffice.PorukaAdditionalInfo(),
                    //    Tekst = "Odobreno ti je trazeno pravo pristupa modulu " + Convert.ToInt32(_poruka.Tag.AdditionalInfo)
                    //});
                    //_poruka.Arhivirana = true;
                    //_poruka.Update();
                    ////Pronaci sve poruke gde je Posiljalac PorukaPosiljalac i Tag.AdditionalInfo = ModulID i promeniti status u Sakrivena
                    //int modulID = Convert.ToInt32(_poruka.Tag.AdditionalInfo);
                    //List<TDOffice.User> u = TDOffice.User.List().Where(x => x.Tag.PrimaObavestenja[TDOffice.User.TipAutomatskogObavestenja.PravoPristupaModulu] == true).ToList();
                    //List<TDOffice.Poruka> poruke = TDOffice.Poruka.ListByPosiljalac(_poruka.Posiljalac);

                    //if (poruke != null && poruke.Count > 0)
                    //{
                    //    foreach (TDOffice.Poruka ps in poruke.Where(x => x.Tag != null && x.Tag.Action == TDOffice.PorukaAction.PravoPristupaModulu))
                    //    {
                    //        if (ps.Tag.AdditionalInfo == null)
                    //            continue;
                    //        if (Convert.ToInt32(ps.Tag.AdditionalInfo) == modulID)
                    //        {
                    //            ps.Arhivirana = true;
                    //            ps.Update();
                    //        }
                    //    }
                    //}
                    this.Close();
                    break;
                case TDOffice.PorukaAction.BonusZakljucavanje:
                    int id = Convert.ToInt32(_poruka.Tag.AdditionalInfo);
                    int brBonusa = 0;
                    using (InputBox ib = new InputBox("Reset bonusa", "Unesite broj bonusa"))
                    {
                        ib.ShowDialog();

                        if (ib.DialogResult != DialogResult.OK)
                            return;

                        try
                        {
                            brBonusa = Convert.ToInt32(ib.returnData);
                        }
                        catch
                        {
                            MessageBox.Show("Neispravan broj bonusa!");
                            return;
                        }
                    }
                    if(brBonusa > 0)
                    {
                        TDOffice.User uu = TDOffice.User.Get(id);
                        uu.BonusZakljucavanjaCount = brBonusa;
                        uu.Update();
                    }
                    else
                    {
                        MessageBox.Show("Pogresan broj bonusa. Mora biti veci od 0");
                    }

                    break;
            }
        }

        private void arhiviraj_btn_Click(object sender, EventArgs e)
        {
            if (_poruka.DatumCitanja != null)
            {
                _poruka.Arhivirana = true;
                _poruka.Update();
            }

            this.Close();
        }

        private void minimiziraj_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            using (_1301_fm_Poruka_Beleska fb = new _1301_fm_Poruka_Beleska(_poruka))
            {
                fb.ShowDialog();
                _poruka = fb.poruka;
            }

            this.label4.ForeColor = _poruka.Tag != null && !string.IsNullOrWhiteSpace(_poruka.Tag.Beleska) ? Color.Orange : Color.Black;
        }

        private void polozaj_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            _poruka.Tag.PolozajPoruke = polozaj_cmb.SelectedIndex;
            _poruka.Update();
            PozicionirajPoruku();
        }
    }
}
