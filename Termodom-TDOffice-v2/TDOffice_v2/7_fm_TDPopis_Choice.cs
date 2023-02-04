using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class _7_fm_TDPopis_Choice : Form
    {
        public delegate void PopisChoiceResponse(PopisChoiceResponseArgs popis);

        public PopisChoiceResponse OnPopisChoiceResponse;

        public _7_fm_TDPopis_Choice(PopisChoiceResponse response)
        {
            InitializeComponent();
            OnPopisChoiceResponse = response;
            
            List<Komercijalno.Magacin> magacini = Komercijalno.Magacin.ListAsync().Result;
            magacini.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "<izaberi magacain>" });
            magacini.Sort((x, y) => x.ID.CompareTo(y.ID));
            magacin_cmb.DataSource = magacini;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(700004) ? true : false;
            magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
            magacin_cmb.SelectedIndex = 0;

            this.tip_cmb.SelectedIndex = 0;
            this.vreme_cmb.SelectedIndex = 0;
            this.magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.vreme_cmb.SelectedIndex < 1) { MessageBox.Show("Niste izabrali koji popis Jucerasnji ili Nedeljni"); return; }
            if (this.tip_cmb.SelectedIndex < 1) { MessageBox.Show("Niste izabrali tip popisa"); return; }
            if (this.magacin_cmb.SelectedIndex < 1) { MessageBox.Show("Niste izabrali magacin"); return; }

            int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
            int tip = tip_cmb.SelectedIndex - 1;
            int vreme = vreme_cmb.SelectedIndex;

            DateTime komPopisDate = DateTime.Now;
            if(vreme == 1)
                komPopisDate = DateTime.Now.AddDays(-1);
            else
            {
                DateTime prvaNedeljaUNazad = DateTime.Now;

                while (prvaNedeljaUNazad.DayOfWeek != DayOfWeek.Sunday)
                    prvaNedeljaUNazad = prvaNedeljaUNazad.AddDays(-1);
                komPopisDate = prvaNedeljaUNazad;
            }

            int newPopisID = TDOffice.DokumentPopis.Insert(Program.TrenutniKorisnik.ID, magacinID, 0, null, null, (TDOffice.PopisType)tip, null, null);

            int komPopis = Komercijalno.Dokument.Insert(DateTime.Now.Year, 7, "TD2 " + newPopisID.ToString(), null, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
            TDOffice.DokumentPopis popis = TDOffice.DokumentPopis.Get(newPopisID);

            Komercijalno.Dokument komDok = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, komPopis);
            komDok.Datum = komPopisDate;
            komDok.Update();

            if (popis.Tip == TDOffice.PopisType.PopisZaNabavku)
            {
                int komNar = Komercijalno.Dokument.Insert(DateTime.Now.Year, 33, "TD2 " + newPopisID.ToString(), Program.TrenutniKorisnik.Tag.narudbenicaPPID, "TDOffice_v2", 1, magacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
                popis.KomercijalnoNarudzbenicaBrDok = komNar;
            }

            popis.KomercijalnoPopisBrDok = komPopis;

            popis.Update();
            this.Close();

            OnPopisChoiceResponse(new PopisChoiceResponseArgs() { popis = popis });
        }
    }

    public class PopisChoiceResponseArgs
    {
        public TDOffice.DokumentPopis popis { get; set; }
    }
}
