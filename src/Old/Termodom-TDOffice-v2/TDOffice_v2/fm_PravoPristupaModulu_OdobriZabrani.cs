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
    public partial class fm_PravoPristupaModulu_OdobriZabrani : Form
    {
        private TDOffice.Poruka _poruka;
        public fm_PravoPristupaModulu_OdobriZabrani(TDOffice.Poruka poruka)
        {
            InitializeComponent();
            _poruka = poruka;
        }

        private void PravoPristupaModulu(int statusPoruke)
        {
            TDOffice.Pravo lp = TDOffice.Pravo.List($"USER_ID = {_poruka.Posiljalac} AND MODUL_ID = {Convert.ToInt32(_poruka.Tag.AdditionalInfo)}").FirstOrDefault();
            if (lp == null)
                lp = TDOffice.Pravo.Get(TDOffice.Pravo.Insert(Convert.ToInt32(_poruka.Tag.AdditionalInfo), statusPoruke, _poruka.Posiljalac));

            lp.Value = statusPoruke;
            lp.Update();

            string naslovPoruke = "";
            string tekstPoruke = "";
            if (statusPoruke == 1)
            {
                naslovPoruke = "Odobreno pravo pristupa modula " + Convert.ToInt32(_poruka.Tag.AdditionalInfo);
                tekstPoruke = "Odobreno ti je trazeno pravo pristupa modulu " + Convert.ToInt32(_poruka.Tag.AdditionalInfo);
            }
            else
            if (statusPoruke == -1)
            {
                naslovPoruke = "Zauvek zabranjeno pravo pristupa modula " + Convert.ToInt32(_poruka.Tag.AdditionalInfo);
                tekstPoruke = "Zauvek ti je zabranjeno pravo pristupa modulu " + Convert.ToInt32(_poruka.Tag.AdditionalInfo);
            }

            //Slanje poruke
            TDOffice.Poruka.Insert(new TDOffice.Poruka()
            {
                Datum = DateTime.Now,
                Naslov = naslovPoruke,
                Posiljalac = Program.TrenutniKorisnik.ID,
                Primalac = _poruka.Posiljalac,
                Status = TDOffice.PorukaTip.Standard,
                Tag = new TDOffice.PorukaAdditionalInfo(),
                Tekst = tekstPoruke
            });
            _poruka.Arhivirana = true;
            _poruka.Update();

            Task.Run(() =>
            {
                //Pronaci sve poruke gde je Posiljalac PorukaPosiljalac i Tag.AdditionalInfo = ModulID i promeniti status u Sakrivena
                int modulID = Convert.ToInt32(_poruka.Tag.AdditionalInfo);
                List<TDOffice.User> u = TDOffice.User.List().Where(x => x.Tag.PrimaObavestenja[TDOffice.User.TipAutomatskogObavestenja.PravoPristupaModulu] == true).ToList();
                List<TDOffice.Poruka> poruke = TDOffice.Poruka.List("POSILJALAC = " + Program.TrenutniKorisnik.ID);

                if (poruke != null && poruke.Count > 0)
                {
                    foreach (TDOffice.Poruka ps in poruke.Where(x => x.Tag != null && x.Tag.Action == TDOffice.PorukaAction.PravoPristupaModulu))
                    {
                        if (ps.Tag.AdditionalInfo == null)
                            continue;
                        if (Convert.ToInt32(ps.Tag.AdditionalInfo) == modulID)
                        {
                            ps.Arhivirana = true;
                            ps.Update();
                        }
                    }
                }
            });
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PravoPristupaModulu(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PravoPristupaModulu(-1);
        }

        private void fm_PravoPristupaModulu_Load(object sender, EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
        }

        private void odustani_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
