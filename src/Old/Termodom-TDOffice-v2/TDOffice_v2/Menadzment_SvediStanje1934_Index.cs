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
    public partial class Menadzment_SvediStanje1934_Index : Form
    {
        private static int intOtpMagacin12 = 41;
        private TDOffice.Config<List<int>> razduzeni34 = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.SvedenoStanje1934);

        public Menadzment_SvediStanje1934_Index()
        {
            InitializeComponent();
            if (razduzeni34.Tag == null)
                razduzeni34.Tag = new List<int>();
        }

        private void Menadzment_SvediStanje1934_Index_Load(object sender, EventArgs e)
        {
            List<Komercijalno.Magacin> magacini = Komercijalno.Magacin.ListAsync().Result;
            magacini.RemoveAll(x => x.ID < 12 || x.ID > 13);
            comboBox1.DataSource = magacini;
            comboBox1.DisplayMember = "Naziv";
            comboBox1.ValueMember = "ID";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int magacinID = Convert.ToInt32(comboBox1.SelectedValue);

            if(magacinID != 12)
            {
                MessageBox.Show("Magacin nije ispravan!");
                return;
            }

            Komercijalno.Dokument intOtp = Komercijalno.Dokument.Get(DateTime.Now.Year, 19, intOtpMagacin12);
            List<Komercijalno.Stavka> stavkeOtpremnice = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 19, intOtpMagacin12);
            List<Komercijalno.Roba> komercijalnoRoba = Komercijalno.Roba.List(DateTime.Now.Year);
            List<Komercijalno.RobaUMagacinu> komercijalnoRobaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(12);

            List<Komercijalno.Dokument> dokumenti = Komercijalno.Dokument.ListByVrDok(DateTime.Now.Year, 34);
            dokumenti.RemoveAll(x => razduzeni34.Tag.Contains(x.BrDok) || x.MagacinID != magacinID);

            if(dokumenti.Count == 0)
            {
                MessageBox.Show("Svi dokumenti su vec svedeni!");
                return;
            }

            foreach(Komercijalno.Dokument dok in dokumenti)
            {
                List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, dok.VrDok, dok.BrDok);

                foreach(Komercijalno.Stavka s in stavke)
                {
                    if(stavkeOtpremnice.Any(x => x.RobaID == s.RobaID))
                    {
                        // stavka postoji, povecavam stanje
                        s.Kolicina += stavkeOtpremnice.FirstOrDefault(x => x.RobaID == s.RobaID).Kolicina;
                        s.Update(DateTime.Now.Year);
                    }
                    else
                    {
                        // Stavka ne postoji, insertujem
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, intOtp,
                            komercijalnoRoba.FirstOrDefault(x => x.ID == s.RobaID),
                            komercijalnoRobaUMagacinu.FirstOrDefault(x => x.RobaID == s.RobaID),
                            s.Kolicina,
                            0);
                    }
                }

                razduzeni34.Tag.Add(dok.BrDok);
                razduzeni34.UpdateOrInsert();
            }

            MessageBox.Show("Gotovo!");
        }
    }
}
