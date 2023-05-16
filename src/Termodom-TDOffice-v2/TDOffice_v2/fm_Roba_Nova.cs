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
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_Roba_Nova : Form
    {
        //private Task<Termodom.Data.Entities.Komercijalno.TarifaDictionary> _tarife { get; set; } = TarifeManager.DictionaryAsync();
        private List<Tarife> _tarife { get; set; }
        private List<Komercijalno.Proizvodjac> _proizvodjaci { get; set; }
        private List<Komercijalno.Grupa> _grupe { get; set; }
        private List<Komercijalno.PodGrupa> _podGrupe { get; set; }
        private List<Komercijalno.JedMere> _jedMere { get; set; }
        private List<Komercijalno.Roba> _roba { get; set; }

        private FbConnection _conKomercijalno { get; set; }
        public fm_Roba_Nova()
        {
            InitializeComponent();
            if (!Program.TrenutniKorisnik.ImaPravo(133510))
            {
                TDOffice.Pravo.NematePravoObavestenje(133510);
                this.Close();
                return;
            }
            _conKomercijalno = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]);
            _conKomercijalno.Open();
            UcitajListURobe();

            _tarife = Komercijalno.Tarife.List(_conKomercijalno);
            pdvTarifa_cmb.DisplayMember = "Naziv";
            pdvTarifa_cmb.ValueMember = "TarifaID";
            pdvTarifa_cmb.DataSource = _tarife;

            _proizvodjaci = Komercijalno.Proizvodjac.List(_conKomercijalno);
            proizvodjac_cmb.DisplayMember = "Naziv";
            proizvodjac_cmb.ValueMember = "ProID";
            proizvodjac_cmb.DataSource = _proizvodjaci;

            _grupe = Komercijalno.Grupa.List(_conKomercijalno);
            grupa_cmb.DisplayMember = "Naziv";
            grupa_cmb.ValueMember = "GrupaID";
            grupa_cmb.DataSource = _grupe;

            _podGrupe = Komercijalno.PodGrupa.List(_conKomercijalno);
            podgrupa_cmb.DisplayMember = "Naziv";
            podgrupa_cmb.ValueMember = "PodGrupaID";
            podgrupa_cmb.DataSource = _podGrupe;

            _jedMere = Komercijalno.JedMere.List(_conKomercijalno);
            jm_cmb.ValueMember = "JM";
            jm_cmb.DisplayMember = "Naziv";
            jm_cmb.DataSource = _jedMere;

            transportnoPakovanjeJM_cmb.ValueMember = "JM";
            transportnoPakovanjeJM_cmb.DisplayMember = "Naziv";
            transportnoPakovanjeJM_cmb.DataSource = new List<Komercijalno.JedMere>(_jedMere);
            
        }

        private void fm_Roba_Nova_Load(object sender, EventArgs e)
        {

        }

        private void ResetUI()
        {
            katBrPro_txt.Text = "";
            naziv_txt.Text = "";
        }
        private void UcitajListURobe()
        {
            _roba = Komercijalno.Roba.List(_conKomercijalno);

            List<string> skraceniKataloski = _roba.Select(x => x.KatBr.Substring(0, 4)).ToList();
            skraceniKataloski.Sort((a, b) => a.CompareTo(b));
            string poslednji = skraceniKataloski[skraceniKataloski.Count - 1];
            string sledeci = GetSledeciKataloskiBroj(poslednji);
            katBr_txt.Text = sledeci;
        }

        private string GetSledeciKataloskiBroj(string poslednjiKataloskiBroj)
        {
            int poslednjiN = Convert.ToInt32(poslednjiKataloskiBroj.Substring(1, 3));
            int sledeciN = poslednjiN == 999 ? 1 : poslednjiN + 1;
            string poslednjiS = poslednjiKataloskiBroj[0].ToString();
            string sledeciS = sledeciN == 1 ? (poslednjiS[0] + 1).ToString() : poslednjiS;
            return sledeciS + sledeciN.ToString("000");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (transportnoPakovanje_cb.Checked)
            {
                transportnoPakovanje_gb.Enabled = true;
            }
            else
            {
                transportnoPakovanje_gb.Enabled = false;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string katBrPro = katBrPro_txt.Text;
            string naziv = naziv_txt.Text;
            int vrsta = vrsta_cmb.SelectedIndex;
            string katBr = katBr_txt.Text;
            if (string.IsNullOrWhiteSpace(katBr))
            {
                MessageBox.Show("Neispravan kataloski broj!");
                return;
            }
            if (katBr.Length > 25)
            {
                MessageBox.Show("Kataloski broj moze imati najvise 25 karaktera!");
                return;
            }
            if (string.IsNullOrWhiteSpace(katBrPro))
            {
                MessageBox.Show("Neispravan kataloski broj proizvodjaca!");
                return;
            }
            if (katBrPro.Length > 25)
            {
                MessageBox.Show("Kataloski broj proizvodjaca moze imati najvise 25 karaktera!");
                return;
            }
            if (string.IsNullOrWhiteSpace(naziv))
            {
                MessageBox.Show("Neispravan naziv!");
                return;
            }
            if (naziv.Length > 25)
            {
                MessageBox.Show("Naziv moze imati najvise 50 karaktera!");
                return;
            }
            if (vrsta == 0)
            {
                MessageBox.Show("Morate izabrati vrstu objekta (gore desno)!");
                return;
            }
            string grupaID = grupa_cmb.SelectedValue.ToString();
            int podgrupaID = Convert.ToInt32(podgrupa_cmb.SelectedValue);
            string proizvodjacID = proizvodjac_cmb.SelectedValue.ToString();
            string jm = jm_cmb.SelectedValue.ToString();
            string tarifa = pdvTarifa_cmb.SelectedValue.ToString();
            string trPakJM = transportnoPakovanje_cb.Checked ? transportnoPakovanjeJM_cmb.SelectedValue.ToString() : null;
            double? trPakKol = transportnoPakovanje_cb.Checked ? (double?)Convert.ToDouble(transportnoPakovanjeKolicina_txt.Text) : null;
            var res = await TDBrain_v3.PostAsync("/komercijalno/roba/insert", new Dictionary<string, string>()
            {
                { "katBr", katBr },
                { "katBrPro", katBrPro },
                { "naziv", naziv },
                { "vrsta", vrsta .ToString() },
                { "grupaID", grupaID },
                { "podgrupaID", podgrupaID.ToString() },
                { "proizvodjacID", proizvodjacID },
                { "jm", jm },
                { "tarifaID", tarifa },
                { "trPakJM", trPakJM },
                { "trPakKolicina", trPakKol == null ? "" : trPakKol.ToString() }
            });
            switch((int)res.StatusCode)
            {
                case 201:
                    UcitajListURobe();
                    ResetUI();
                    MessageBox.Show("Roba uspesno kreirana!");
                    break;
                case 400:
                    MessageBox.Show(await res.Content.ReadAsStringAsync());
                    break;
                case 500:
                    MessageBox.Show("Greska na API-ju!");
                    break;
                default:
                    MessageBox.Show("Neobradjen status kod: " + res.StatusCode);
                    break;
            }
        }
    }
}
