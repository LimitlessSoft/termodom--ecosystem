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
    public partial class fm_Cekovi_RazduziBox : Form
    {
        private TDOffice.Cek _cek { get; set; }

        public fm_Cekovi_RazduziBox(TDOffice.Cek cek)
        {
            InitializeComponent();

            _cek = cek;

            comboBox1.DataSource = Komercijalno.Magacin.ListAsync();
            comboBox1.DisplayMember = "Naziv";
            comboBox1.ValueMember = "ID";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(TDOffice.SpecifikacijaNovca.Get((int)comboBox1.SelectedValue, datumUnosa_dtp.Value) == null)
            {
                MessageBox.Show("Specifikacija novca datog magazina na izabranom datumu ne postoji!");
                return;
            }

            DateTime datum = datumUnosa_dtp.Value;
            int magacinID = (int)comboBox1.SelectedValue;
            _cek.Zaduzio = null;
            _cek.Datum = datum;
            _cek.MagacinID = magacinID;
            _cek.Status = TDOffice.Enums.CekStatus.Realizovan;
            _cek.Update();

            MessageBox.Show("Cek uspesno realizovan na datum " + datum.ToString("dd.MM.yyyy") + "!");

            this.Close();
        }
    }
}
