using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class _1350_fm_Zakljucaj500_Index : Form
    {
        private double tolerancija = 500;
        private Task<fm_Help> _helpFrom { get; set; }

        public _1350_fm_Zakljucaj500_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Zakljucaj500_Index);
        }
        private void _1350_fm_Zakljucaj500_Index_Load_1(object sender, EventArgs e)
        {
            _ = SetUIAsync();
        }

        private async Task SetUIAsync()
        {
            Termodom.Data.Entities.Komercijalno.VrstaDokDictionary vrsteDokumenta = await Komercijalno.VrstaDokManager.DictionaryAsync();
            List<VrstaDok> vrdokList = vrsteDokumenta.Values.Where(x => x.VrDok == 13 || x.VrDok == 15).ToList();
            vrdokList.Add(new VrstaDok() { VrDok = -1, NazivDok = " < vrsta dokumenta > " });
            vrdokList.Sort((x, y) => x.VrDok.CompareTo(y.VrDok));

            cmb_VrstaDokumeta.DataSource = vrdokList;
            cmb_VrstaDokumeta.DisplayMember = "NazivDok";
            cmb_VrstaDokumeta.ValueMember = "VrDok";
        }

        private async void btn_Zakljucaj_Click(object sender, EventArgs e)
        {
            int vrDok = Convert.ToInt32(cmb_VrstaDokumeta.SelectedValue);
            int brDok = Convert.ToInt32(tb_BrojDokumenta.Text);

            Komercijalno.Dokument dokument = Komercijalno.Dokument.Get(DateTime.Now.Year, vrDok, brDok);

            if (dokument == null) { MessageBox.Show("Dokument nije pronadjen"); return; }
            if (dokument.Flag == 1) { MessageBox.Show("Dokument je zakljucan"); return; }
            if(dokument.NUID != Komercijalno.NacinUplate.Virman) { MessageBox.Show($"Dokument nije virmanski!"); }
            if(dokument.PPID <= 0) { MessageBox.Show($"Dokument nema dodeljenog partnera!"); return; }

            //Uslov za zakljucavanje partnera Vrednost (duguje - potrazuje) partnera veca od unete tolerancije
            Komercijalno.Partner partner = await Komercijalno.Partner.GetAsync((int)dokument.PPID);
            double dugovanje = partner.Duguje - partner.Potrazuje;
            if (dugovanje < tolerancija)
            {
                //Zakljucavanje
                dokument.Flag = 1;
                dokument.Update();
                string naslov = "Zakljucavanje dokumenta <<ZAKLJUCAJ 500>>";
                string tekst = "<<Bonus zakljucavanje 500>','Dokument <" +brDok.ToString() + "> je zakljucan \n po sistemu bonus zakljucavanja <<Zakljucaj 500>> \n < "+ Program.TrenutniKorisnik.Username + " > \n PArtner :" + partner.Naziv;

                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                {
                    Datum = DateTime.Now,
                    Naslov = naslov,
                    Posiljalac = Program.TrenutniKorisnik.ID,
                    Primalac = Program.TrenutniKorisnik.ID,
                    Status = TDOffice.PorukaTip.Standard,
                    Tag = new TDOffice.PorukaAdditionalInfo(),
                    Tekst = tekst
                });
                MessageBox.Show("Zakljucali ste dokument broj " + brDok.ToString());

            }
            else
            {
                MessageBox.Show("Dokument broj " + brDok.ToString() + " nije zakljucan \n Partner duguje " + dugovanje + " RSD! ");
            }
        }
        private void tb_BrojDokumenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyCode.IsNumber() && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
