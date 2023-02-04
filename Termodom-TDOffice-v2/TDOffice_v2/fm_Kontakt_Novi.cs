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
    public partial class fm_Kontakt_Novi : Form
    {
        private Task<List<TDOffice.Partner>> _partneri = TDOffice.Partner.ListAsync();

        public fm_Kontakt_Novi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(ime_txt.Text))
            {
                MessageBox.Show("Morate uneti ime kontakta!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Morate uneti mobilni telefon kontakta!");
                return;
            }
            if (ime_txt.Text.Length > 64)
            {
                MessageBox.Show("Maksimalna duzina imena kontakta je 64 karaktera!");
                return;
            }
            if(textBox1.Text.Length > 32)
            {
                MessageBox.Show("Maksimalna duzina mobilnog telefona je 32 karaktera!");
                return;
            }

            string validMobileNumber = MobileNumber.Collate(textBox1.Text);

            TDOffice.Partner postojeciPartner = _partneri.Result.FirstOrDefault(x => x.Mobilni.Contains(validMobileNumber));
            if(postojeciPartner != null)
            {
                MessageBox.Show($"Mobilni vec postoji u partneru [({postojeciPartner.ID}) {postojeciPartner.Naziv}]");
                return;
            }

            try
            {
                TDOffice.Partner.Insert(ime_txt.Text, validMobileNumber, null, 0, null, null, null, null, null);
                MessageBox.Show("Kontakt uspesno kreiran!");
                this.Close();
            }
            catch(Exceptions.FailedDatabaseInsertException)
            {
                MessageBox.Show("Doslo je do greske prilikom insertovanja kontakta u bazu!");
            }
        }
    }
}
