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
    public partial class _1337_fm_Korisnici_Novi : Form
    {
        public _1337_fm_Korisnici_Novi()
        {
            InitializeComponent();

            List<TDOffice.Grad> gradovi = TDOffice.Grad.List();

            grad_cmb.DisplayMember = "Naziv";
            grad_cmb.ValueMember = "ID";
            grad_cmb.DataSource = gradovi;
        }

        private void _1337_fm_Korisnici_Novi_Load(object sender, EventArgs e)
        {

        }

        private void kreiraj_btn_Click(object sender, EventArgs e)
        {
            int gradID = Convert.ToInt32(grad_cmb.SelectedValue);

            if(gradID <= 0)
            {
                MessageBox.Show("Neispravan grad");
                return;
            }
            if (string.IsNullOrWhiteSpace(korisnickoIme_txt.Text))
            {
                MessageBox.Show("Neispravno korisnicko ime!");
                return;
            }
            if (string.IsNullOrWhiteSpace(sifra_txt.Text))
            {
                MessageBox.Show("Neispravno korisnicko ime!");
                return;
            }

            TDOffice.User.Insert(korisnickoIme_txt.Text, sifra_txt.Text, gradID);

            MessageBox.Show("Novi korisnik upsesno kreiran!");
        }
    }
}
