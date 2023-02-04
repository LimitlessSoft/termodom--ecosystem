using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
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

namespace TDOffice_v2
{
    public partial class fm_Roba_Index : Form
    {
        private FbConnection _conKomercijalno { get; set; }
        private List<Komercijalno.Tarife> _tarife { get; set; }
        private List<Komercijalno.Proizvodjac> _proizvodjaci { get; set; }
        private List<Komercijalno.Grupa> _grupe { get; set; }
        private List<Komercijalno.PodGrupa> _podGrupe { get; set; }
        private List<Komercijalno.JedMere> _jedMere { get; set; }
        public fm_Roba_Index(int robaID)
        {
            InitializeComponent();
            if (!Program.TrenutniKorisnik.ImaPravo(133520))
            {
                TDOffice.Pravo.NematePravoObavestenje(133520);
                this.Close();
                return;
            }
            panel1.Enabled = false;
            _ = UcitajRobuAsync(robaID);
            _conKomercijalno = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]);
            _conKomercijalno.Open();

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

        private void fm_Roba_Index_Load(object sender, EventArgs e)
        {

        }

        private async Task UcitajRobuAsync(int robaID)
        {
            var res = await TDBrain_v3.GetAsync("/komercijalno/roba/get?robaid=" + robaID);

            if ((int)res.StatusCode == 200)
            {
                DTO.TDBrain_v3.Komercijalno.RobaDTO roba = JsonConvert.DeserializeObject<DTO.TDBrain_v3.Komercijalno.RobaDTO>(await res.Content.ReadAsStringAsync());

                robaID_txt.Text = roba.ID.ToString();
                katBr_txt.Text = roba.KatBr;
                katBrPro_txt.Text = roba.KatBrPro;
                naziv_txt.Text = roba.Naziv;
                proizvodjac_cmb.SelectedValue = roba.proID;
                grupa_cmb.SelectedValue = roba.GrupaID;
                podgrupa_cmb.SelectedValue = roba.Podgrupa;
                jm_cmb.SelectedValue = roba.JM;
                pdvTarifa_cmb.SelectedValue = roba.TarifaID;
                if(roba.TrKol != null)
                {
                    transportnoPakovanje_cb.Checked = true;
                    transportnoPakovanjeJM_cmb.SelectedValue = roba.TrPak;
                    transportnoPakovanjeKolicina_txt.Text = roba.TrKol.ToString();
                }
                else
                {
                    transportnoPakovanje_cb.Checked = false;
                }
                panel1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Greska prilikom ucitavanja robe!");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(133521))
            {
                TDOffice.Pravo.NematePravoObavestenje(133521);
                return;
            }
            string katBrPro = katBrPro_txt.Text;
            string naziv = naziv_txt.Text;
            int vrsta = 1;
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
            panel1.Enabled = false;
            string grupaID = grupa_cmb.SelectedValue.ToString();
            int podgrupaID = Convert.ToInt32(podgrupa_cmb.SelectedValue);
            string proizvodjacID = proizvodjac_cmb.SelectedValue.ToString();
            string jm = jm_cmb.SelectedValue.ToString();
            string tarifa = pdvTarifa_cmb.SelectedValue.ToString();
            string trPakJM = transportnoPakovanje_cb.Checked ? transportnoPakovanjeJM_cmb.SelectedValue.ToString() : null;
            double? trPakKol = transportnoPakovanje_cb.Checked ? (double?)Convert.ToDouble(transportnoPakovanjeKolicina_txt.Text) : null;
            var res = await TDBrain_v3.PostAsync("/komercijalno/roba/update", new Dictionary<string, string>()
            {
                { "robaID", robaID_txt.Text },
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

            if(res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Detalji robe uspesno azurirani!");
            }
            else if((int)res.StatusCode == 400)
            {
                MessageBox.Show("400");
                MessageBox.Show(await res.Content.ReadAsStringAsync());
            }
            else
            {
                MessageBox.Show(res.StatusCode.ToString());

                if (res.Content.Headers.ContentLength > 0)
                    MessageBox.Show(await res.Content.ReadAsStringAsync());
            }
            panel1.Enabled = true;
        }

        private void transportnoPakovanje_cb_CheckedChanged(object sender, EventArgs e)
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

        private void transportnoPakovanjeJM_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
