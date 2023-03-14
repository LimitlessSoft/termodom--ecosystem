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
    public partial class fm_Web_UredjivanjeCena_Item : Form
    {
        private TDOffice.WebUredjivanjeCenaStavka _trenutnaWebStavka { get; set; } = null;
        private Task<List<Komercijalno.Roba>> _komercijalnoRoba { get; set; } = Komercijalno.Roba.ListAsync(DateTime.Now.Year);

        private bool _ucitavanje = true;

        public EventHandler<TDOffice.WebUredjivanjeCenaStavka> ItemUpdated;

        public fm_Web_UredjivanjeCena_Item()
        {
            InitializeComponent();
        }
        private void fm_Web_UredjivanjeCena_Item_Load(object sender, EventArgs e)
        {
            uslovi_cmb.DataSource = new List<Tuple<int, string>>() {
                new Tuple<int, string>(0, "Nedefinisano"),
                new Tuple<int, string>(1, "Poslednja Nabavna Cena +/- %"),
                new Tuple<int, string>(2, "Fiksna Cena"),
                new Tuple<int, string>(3, "Komercijalno Prodajna Cena +/- %"),
                new Tuple<int, string>(4, "Referentni Proizvod")
            };
            uslovi_cmb.DisplayMember = "Item2";
            uslovi_cmb.ValueMember = "Item1";

            magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            _komercijalnoRoba.Wait();
            _komercijalnoRoba.Result.Add(new Komercijalno.Roba() { ID = -1, Naziv = " <<< Izaberi Robu >>>" });
            _komercijalnoRoba.Result.Sort((x, y) => x.ID.CompareTo(y.ID));

            referentiProizvod_cmb.DataSource = _komercijalnoRoba.Result;
            referentiProizvod_cmb.DisplayMember = "Naziv";
            referentiProizvod_cmb.ValueMember = "ID";

            _ucitavanje = false;
        }
        private void fm_Web_UredjivanjeCena_Item_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void UcitajStavku(int stavkaID, DateTime analizaOd, DateTime analizaDo)
        {
            _ucitavanje = true;

            _trenutnaWebStavka = TDOffice.WebUredjivanjeCenaStavka.Get(stavkaID);
            Komercijalno.Roba kRoba = Komercijalno.Roba.Get(DateTime.Now.Year, _trenutnaWebStavka.RobaID);
            robaID_txt.Text = _trenutnaWebStavka.RobaID.ToString();
            nazivArtikla_txt.Text = kRoba.Naziv;
            magacin_cmb.SelectedValue = _trenutnaWebStavka.MagacinID;
            uslovi_cmb.SelectedValue = (int)_trenutnaWebStavka.Uslov;
            modifikator_txt.Text = _trenutnaWebStavka.Modifikator.ToString();
            referentiProizvod_cmb.SelectedValue = _trenutnaWebStavka.ReferentniProizvod == null ? -1 : _trenutnaWebStavka.ReferentniProizvod;
            referentiProizvod_cmb.Visible = _trenutnaWebStavka.Uslov == TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod;

            odDatuma_dtp.Value = analizaOd;
            doDatuma_dtp.Value = analizaDo;

            btn_OdbaciIzmene.Enabled = false;
            btn_Sacuvaj.Enabled = false;
            btn_OdbaciIzmene.BackColor = Color.Gray;
            btn_Sacuvaj.BackColor = Color.Gray;

            _ucitavanje = false;
        }

        private void uslovi_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ucitavanje)
                return;

            var usl = (TDOffice.Enums.WebUredjivanjeCenaUslov)Convert.ToInt32(uslovi_cmb.SelectedValue);
            _trenutnaWebStavka.Uslov = usl;

            if (usl != TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod)
                _trenutnaWebStavka.ReferentniProizvod = null;

            referentiProizvod_cmb.Visible = usl == TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod;

            btn_OdbaciIzmene.Enabled = true;
            btn_Sacuvaj.Enabled = true;
            btn_OdbaciIzmene.BackColor = Control.DefaultBackColor;
            btn_Sacuvaj.BackColor = Control.DefaultBackColor;
        }
        private void modifikator_txt_TextChanged(object sender, EventArgs e)
        {
            if (_ucitavanje)
                return;

            try
            {
                _trenutnaWebStavka.Modifikator = Convert.ToDouble(modifikator_txt.Text);
                btn_OdbaciIzmene.Enabled = true;
                btn_Sacuvaj.Enabled = true;
                btn_OdbaciIzmene.BackColor = Control.DefaultBackColor;
                btn_Sacuvaj.BackColor = Control.DefaultBackColor;
            }
            catch
            {
                MessageBox.Show("Neispravna vrednost modifikatora!");
            }
        }
        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ucitavanje)
                return;

            _trenutnaWebStavka.MagacinID = Convert.ToInt32(uslovi_cmb.SelectedValue);

            btn_OdbaciIzmene.Enabled = true;
            btn_Sacuvaj.Enabled = true;
            btn_OdbaciIzmene.BackColor = Control.DefaultBackColor;
            btn_Sacuvaj.BackColor = Control.DefaultBackColor;
        }
        private void referentiProizvod_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ucitavanje)
                return;

            _trenutnaWebStavka.ReferentniProizvod = Convert.ToInt32(referentiProizvod_cmb.SelectedValue);

            btn_OdbaciIzmene.Enabled = true;
            btn_Sacuvaj.Enabled = true;
            btn_OdbaciIzmene.BackColor = Control.DefaultBackColor;
            btn_Sacuvaj.BackColor = Control.DefaultBackColor;
        }

        private void btn_Sacuvaj_Click(object sender, EventArgs e)
        {
            _trenutnaWebStavka.Update();
            ItemUpdated(null, _trenutnaWebStavka);
            UcitajStavku(_trenutnaWebStavka.RobaID, odDatuma_dtp.Value, doDatuma_dtp.Value);
        }
        private void btn_OdbaciIzmene_Click(object sender, EventArgs e)
        {
            UcitajStavku(_trenutnaWebStavka.RobaID, odDatuma_dtp.Value, doDatuma_dtp.Value);
        }
    }
}
