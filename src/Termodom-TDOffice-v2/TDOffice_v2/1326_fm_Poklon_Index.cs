using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class _1326_fm_Poklon_Index : Form
    {
        private static List<Poklon> pokloni = TDOffice.Config<List<Poklon>>.Get(TDOffice.ConfigParameter.Pokloni).Tag;
        private Task<fm_Help> _helpFrom { get; set; }

        private List<Komercijalno.Roba> _roba = null;
        private List<Komercijalno.RobaUMagacinu> _robaUMagacnu = null;
        private ManualResetEventSlim _robaMRE = new ManualResetEventSlim(false);
        private ManualResetEventSlim _robaUMagacinuMRE = new ManualResetEventSlim(false);

        public class Poklon
        {
            public int ID { get; set; }
            public int RobaID { get; set; }
            public string Naziv { get; set; }
            public double VrednostMPRacuna { get; set; }
            public int Kolicina { get; set; }

            public Poklon()
            {

            }

            public Poklon(int id, int robaID, string naziv, double vrednostMPRacuna, int kolicina)
            {
                this.ID = id;
                this.RobaID = robaID;
                this.Naziv = naziv;
                this.VrednostMPRacuna = vrednostMPRacuna;
                this.Kolicina = kolicina;
            }
        }
        public _1326_fm_Poklon_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Poklon_Index);
        }

        private void _1326_fm_Poklon_Index_Load(object sender, EventArgs e)
        {
            UcitajPoklone();

            Thread t1 = new Thread(() =>
            {
                _roba = Komercijalno.Roba.List(DateTime.Now.Year);
                _robaMRE.Set();

                _robaUMagacnu = Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromHours(60));
                _robaUMagacinuMRE.Set();
            });
            t1.IsBackground = true;
            t1.Start();
        }

        private void UcitajPoklone()
        {
            comboBox1.DataSource = pokloni;
            comboBox1.DisplayMember = "Naziv";
            comboBox1.ValueMember = "ID";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _robaMRE.Wait();
            Poklon poklon = pokloni.Where(x => x.ID == (int)comboBox1.SelectedValue).FirstOrDefault();

            if(poklon.ID == 0)
            {
                MessageBox.Show("Niste izabrali poklon!");
                return;
            }

            int mpRacun = -1;

            try
            {
                mpRacun = Convert.ToInt32(textBox1.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Neispravan MP racun");
                return;
            }

            Komercijalno.Dokument dokument = Komercijalno.Dokument.Get(DateTime.Now.Year, 15, mpRacun);

            if (dokument.Flag != 0)
            {
                MessageBox.Show("Racun mora biti otkljucan!");
                return;
            }

            if(dokument.Potrazuje < poklon.VrednostMPRacuna)
            {
                MessageBox.Show("Vrednost MP Racuna mora biti veca od " + poklon.VrednostMPRacuna.ToString("#,##0.00 RSD"));
                return;
            }

            List<Komercijalno.Stavka> stavkeRacuna = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 15, mpRacun);

            if(stavkeRacuna.Any(x => pokloni.Any(y => y.RobaID == x.RobaID)))
            {
                MessageBox.Show("Racun vec sadrzi poklon!");
                return;
            }

            Komercijalno.Stavka.Insert(DateTime.Now.Year, dokument,
                _roba.Where(x => x.ID == poklon.RobaID).FirstOrDefault(),
                _robaUMagacnu.Where(x => x.RobaID == poklon.RobaID && x.MagacinID == dokument.MagacinID).FirstOrDefault(),
                poklon.Kolicina,
                99.9);

            MessageBox.Show("Gotovo");
        }

    }
}
