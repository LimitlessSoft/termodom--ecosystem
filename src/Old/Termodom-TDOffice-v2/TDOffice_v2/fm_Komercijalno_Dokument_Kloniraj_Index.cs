using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_Komercijalno_Dokument_Kloniraj_Index : Form
    {
        private Task<fm_Help> _helpFrom { get; set; }
        //_helpFrom = this.InitializeHelpModulAsync(Modul.);
        public fm_Komercijalno_Dokument_Kloniraj_Index()
        {
            InitializeComponent();

            List<int> baze = Komercijalno.Komercijalno.CONNECTION_STRING.Keys.ToList();
            baze.Sort((y, x) => x.CompareTo(y));

            cmb_IzvornaBaza.DataSource = new List<int>(baze);
            cmb_DestinacionaBaza.DataSource = new List<int>(baze);
        }

        private void btn_Kloniraj_Click(object sender, EventArgs e)
        {
            int godinaSource = Convert.ToInt32(cmb_IzvornaBaza.SelectedValue);
            int godinaDest = Convert.ToInt32(cmb_DestinacionaBaza.SelectedValue);
            int godinaTekuca = DateTime.Now.Year;

            if(string.IsNullOrWhiteSpace(txt_VrstaDokumenta.Text))
            {
                MessageBox.Show("Morate uneti vrstu dokumenta!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_BrDok.Text))
            {
                MessageBox.Show("Morate uneti broj dokumenta!");
                return;
            }

            int vrDok;
            int brDok;

            if(!Int32.TryParse(this.txt_VrstaDokumenta.Text, out vrDok) || vrDok <= 0)
            {
                MessageBox.Show("Neispravna vrsta dokumenta!");
                return;
            }

            if (!Int32.TryParse(this.txt_BrDok.Text, out brDok) || brDok <= 0)
            {
                MessageBox.Show("Neispravan broj dokumenta!");
                return;
            }

            Dokument izvorniDokument = null;
            List<Stavka> izvorneStavke = null;

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godinaSource]))
            {
                con.Open();
                izvorniDokument = Komercijalno.Dokument.Get(con, vrDok, brDok);

                // Provera da li postoji dokument
                if (izvorniDokument == null) { MessageBox.Show($"Dokument vrdok: { vrDok }, brdok: { brDok } nije pronadjen"); return; }

                izvorneStavke = Stavka.ListByDokument(con, vrDok, brDok);
            }


            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godinaDest]))
            {
                con.Open();
                int newBrDok = Komercijalno.Dokument.Insert(con, izvorniDokument.VrDok, izvorniDokument.IntBroj, izvorniDokument.PPID,
                    "Klon", Convert.ToInt32(izvorniDokument.NUID), izvorniDokument.MagacinID, izvorniDokument.RefID, izvorniDokument.MagID);

                Dokument destinacioniDokument = Dokument.Get(con, vrDok, newBrDok);
                
                //List<RobaUMagacinu> robaUMagacinu = RobaUMagacinu.ListByMagacinID(con, izvorniDokument.MagacinID);

                foreach (Stavka s in izvorneStavke)
                {
                    Roba r = Roba.Get(con, s.RobaID);
                    //RobaUMagacinu rum = robaUMagacinu.FirstOrDefault(x => x.RobaID == s.RobaID);
                    RobaUMagacinu rum = RobaUMagacinu.Get(con, izvorniDokument.MagacinID, s.RobaID);
                    if (r == null)
                    {
                        MessageBox.Show("U ovoj godini ne postoji roba u sifarniku sa IDem: " + s.RobaID);
                        continue;
                    }
                    if(rum == null)
                    {
                        MessageBox.Show("U ovoj godini ne postoji roba u magacinu sa IDem: " + s.RobaID);
                        continue;
                    }
                    Stavka.Insert(con, destinacioniDokument, r, rum, s.Kolicina, s.Rabat);
                }

                if(godinaTekuca != godinaDest)
                {
                    DateTime noviDatum = new DateTime(godinaDest, 12, 31);
                    destinacioniDokument.Datum = noviDatum;
                    destinacioniDokument.DatRoka = noviDatum;
                    destinacioniDokument.Update(con);
                }
            }

            MessageBox.Show("Gotovo!");
        }
    }
}
