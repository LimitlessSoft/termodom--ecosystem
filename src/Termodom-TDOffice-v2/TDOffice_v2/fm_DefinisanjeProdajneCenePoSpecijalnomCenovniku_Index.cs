using FirebirdSql.Data.FirebirdClient;
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
    public partial class fm_DefinisanjeProdajneCenePoSpecijalnomCenovniku_Index : Form
    {
        private Task<List<Komercijalno.Dokument>> _dokumentiNabavke { get; set; } = Task.Run(() => { return Komercijalno.Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)"); });
        private Task<List<Komercijalno.Stavka>> _stavkeNabavke { get; set; } = Task.Run(() => { return Komercijalno.Stavka.List(DateTime.Now.Year, $"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)"); });
        public fm_DefinisanjeProdajneCenePoSpecijalnomCenovniku_Index()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(137010))
            {
                TDOffice.Pravo.NematePravoObavestenje(137010);
                this.Close();
                return;
            }

            InitializeComponent();
        }

        private void fm_DefinisanjeProdajneCenePoSpecijalnomCenovniku_Index_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int brDok = 0;
            try
            {
                brDok = Convert.ToInt32(brojDokumenta_txt.Text);
            }
            catch
            {
                MessageBox.Show("Neispravan broj dokumenta!");
                return;
            }
            MessageBox.Show(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]);
            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                Komercijalno.Dokument dokument = Komercijalno.Dokument.Get(con, brDok > 1200000 ? 15 : 32, brDok);

                if (dokument == null)
                {
                    MessageBox.Show("Dokument sa datim brojem nije pronadjen (ni MP Racun ni Proracun)!");
                    return;
                }

                if (dokument.PPID == null)
                {
                    MessageBox.Show("Dokument mora imati nekog partnera!");
                    return;
                }

                List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.List(con, $"VRDOK = {dokument.VrDok} AND BRDOK = {dokument.BrDok}");
                if (stavke == null || stavke.Count == 0)
                {
                    MessageBox.Show("Dokument nema stavke!");
                    return;
                }

                TDOffice.PartnerKomercijalno partnerKomercijalno = TDOffice.PartnerKomercijalno.Get((int)dokument.PPID);
                if (partnerKomercijalno == null ||
                    partnerKomercijalno.SpecijalniCenovnikPars == null ||
                    partnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList == null ||
                    partnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList.Count == 0)
                {
                    MessageBox.Show("Partner nema definisane nikakve specijalne uslove!");
                    return;
                }

                if (partnerKomercijalno == null ||
                    partnerKomercijalno.SpecijalniCenovnikPars == null ||
                    partnerKomercijalno.SpecijalniCenovnikPars.NaciniUplateZaKojiVazeSpecijalniCenovnici == null ||
                    !partnerKomercijalno.SpecijalniCenovnikPars.NaciniUplateZaKojiVazeSpecijalniCenovnici.Contains((int)dokument.NUID))
                {
                    MessageBox.Show("Partner nema definisane uslove za zadati nacin placanja dokumenta!");
                    return;
                }

                List<TDOffice.SpecijalniCenovnik.Item> specijalniCenovniciItems = TDOffice.SpecijalniCenovnik.Item.List($"CENOVNIKID IN ({string.Join(", ", partnerKomercijalno.SpecijalniCenovnikPars.SpecijalniCenovnikList)})");

                foreach(Komercijalno.Stavka s in stavke)
                {
                    TDOffice.SpecijalniCenovnik.Item it = specijalniCenovniciItems.FirstOrDefault(x => x.RobaID == s.RobaID);
                    double zeljenaProdajna, rabat = 0;
                    if(it == null)
                    {
                        it = new TDOffice.SpecijalniCenovnik.Item()
                        {
                            MaxRabat = 10,
                            NabavnaCenaMargina = 0,
                            RobaID = s.RobaID,
                            UslovModifikator = partnerKomercijalno.SpecijalniCenovnikPars.OtherModifikator,
                            UslovTip = partnerKomercijalno.SpecijalniCenovnikPars.OtherTip
                        };
                        zeljenaProdajna = Math.Round(fm_SpecijalniCenovnik_Index.IzracunajSpecijalnuCenu(con, it, dokument.MagacinID, _dokumentiNabavke.Result, _stavkeNabavke.Result), 4);
                        rabat = Math.Min(it.MaxRabat, (((zeljenaProdajna / s.ProdCenaBP) - 1) * -100));
                        s.Rabat = radioButton2.Checked ? Math.Floor(rabat) : Math.Round(rabat, 2);
                        s.Update(con);
                        continue;
                    }
                    zeljenaProdajna = fm_SpecijalniCenovnik_Index.IzracunajSpecijalnuCenu(con, it, dokument.MagacinID, _dokumentiNabavke.Result, _stavkeNabavke.Result);
                    rabat = Math.Min(it.MaxRabat, (((zeljenaProdajna / s.ProdCenaBP) - 1) * -100));
                    s.Rabat = radioButton2.Checked ? Math.Floor(rabat) : Math.Round(rabat, 2);
                    s.Update(con);
                }

                Komercijalno.Procedure.PresaberiDokument(dokument.VrDok, dokument.BrDok);

                MessageBox.Show("Gotovo!");
            }
        }
    }
}
