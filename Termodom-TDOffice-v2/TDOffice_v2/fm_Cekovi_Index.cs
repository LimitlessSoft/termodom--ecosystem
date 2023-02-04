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
    public partial class fm_Cekovi_Index : Form
    {
        private TDOffice.Cek _cek { get; set; }
        public fm_Cekovi_Index(int cekID)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136109))
            {
                TDOffice.Pravo.NematePravoObavestenje(136109);
                this.Close();
                return;
            }
            InitializeComponent();
            _cek = TDOffice.Cek.Get(cekID);

            if(_cek == null)
            {
                MessageBox.Show("Cek nije pronadjen!");
                this.Close();
                return;
            }

            datumUnosa_dtp.Enabled = false;
            datumValute_dtp.Enabled = false;
            banka_cmb.Enabled = false;
            magacin_cmb.Enabled = false;
            brojCeka_txt.Enabled = false;
            trGradjana_txt.Enabled = false;
            vrednost_txt.Enabled = false;

            magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            banka_cmb.DataSource = Komercijalno.Banka.List(DateTime.Now.Year);
            banka_cmb.DisplayMember = "Naziv";
            banka_cmb.ValueMember = "BankaID";

            button1.Enabled = false;
        }

        private void fm_Cekovi_Index_Load(object sender, EventArgs e)
        {
            datumUnosa_dtp.Value = _cek.Datum;
            datumValute_dtp.Value = _cek.DatumValute;
            magacin_cmb.SelectedValue = _cek.MagacinID;
            banka_cmb.SelectedValue = _cek.PodnosilacBanka;
            trGradjana_txt.Text = _cek.TRGradjana;
            brojCeka_txt.Text = _cek.BrojCeka;
            vrednost_txt.Text = _cek.Vrednost.ToString();

            datumUnosa_dtp.Enabled = true;
            datumValute_dtp.Enabled = true;
            banka_cmb.Enabled = true;
            magacin_cmb.Enabled = true;
            brojCeka_txt.Enabled = true;
            trGradjana_txt.Enabled = true;
            vrednost_txt.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trGradjana_txt.Text) ||
                string.IsNullOrWhiteSpace(brojCeka_txt.Text))
            {
                MessageBox.Show("Neispravno popunjena polja!");
                return;
            }

            try
            {
                Convert.ToInt32(vrednost_txt.Text);
            }
            catch
            {
                MessageBox.Show("Neispravna vrednost ceka!");
                return;
            }

            _cek.Update();

            MessageBox.Show("Cek uspesno sacuvan!");
            button1.Enabled = false;
        }

        private void trGradjana_txt_TextChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.TRGradjana = trGradjana_txt.Text;
        }

        private void datumValute_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.DatumValute = datumValute_dtp.Value;
        }

        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.MagacinID = (int)magacin_cmb.SelectedValue;
        }

        private void banka_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.PodnosilacBanka = (int)banka_cmb.SelectedValue;
        }

        private void datumUnosa_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.Datum = datumUnosa_dtp.Value;
        }

        private void brojCeka_txt_TextChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.BrojCeka = brojCeka_txt.Text;
        }

        private void vrednost_txt_TextChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Enabled)
                return;

            button1.Enabled = true;
            _cek.Vrednost = Convert.ToDouble(vrednost_txt.Text);
        }
    }
}
