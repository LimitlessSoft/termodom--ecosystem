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
using TDOffice_v2.EventHandlers;

namespace TDOffice_v2
{
    public partial class fm_UnesiPartnera_Index : Form
    {
        private FbConnection _con { get; set; }
        private fm_IzborPartnera _ip { get; set; }
        private Task<Dictionary<int, Komercijalno.Partner>> _partneri { get; set; }
        
        private int? _ppid { get; set; }

        public fm_UnesiPartnera_Index()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(137040))
            {
                TDOffice.Pravo.NematePravoObavestenje(137040);
                this.Close();
                return;
            }

            InitializeComponent();
            _con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]);
            _con.Open();
            button2.Enabled = false;
            _partneri = Task.Run(async () =>
            {
                Dictionary<int, Komercijalno.Partner> part = new Dictionary<int, Komercijalno.Partner>();
                List<Komercijalno.Partner> list = await Komercijalno.Partner.ListAsync();
                foreach (Komercijalno.Partner p in list)
                    part.Add(p.PPID, p);
                return part;
            });
            Task.Run(() =>
            {
                _ip = new fm_IzborPartnera();
                _ip.IzborPartneraSelect += IzborPartneraSelect;
                this.Invoke((MethodInvoker)delegate
                {
                    button2.Enabled = true;
                });
            });

            List<Tuple<int, string>> vrDoks = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(4, "Profaktura"),
                new Tuple<int, string>(13, "Faktura"),
                new Tuple<int, string>(15, "MP Racun"),
                new Tuple<int, string>(32, "Proracun")
            };
            comboBox1.DisplayMember = "Item2";
            comboBox1.ValueMember = "Item1";
            comboBox1.DataSource = vrDoks;
            comboBox1.SelectedIndex = 0;
        }

        ~fm_UnesiPartnera_Index()
        {
            _con.Close();
        }

        private void fm_UnesiPartnera_Index_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(_ppid == null)
            {
                MessageBox.Show("Nije izabran partner!");
                return;
            }
            int brDok;
            try
            {
                brDok = Convert.ToInt32(brojMpRacuna_txt.Text);
            }
            catch
            {
                MessageBox.Show("Neispravan broj dokumenta!");
                return;
            }
            int vrDok = Convert.ToInt32(comboBox1.SelectedValue);
            Komercijalno.Dokument dok = Komercijalno.Dokument.Get(_con, vrDok, brDok);

            if(dok == null)
            {
                MessageBox.Show("Dokument nije pronadjen!");
                return;
            }

            if(dok.PPID != null)
            {
                MessageBox.Show("Dokument ne sme vec imati partnera!");
                return;
            }

            if (dok.Flag != 1)
            {
                MessageBox.Show("Dokument mora biti zakljucan!");
                return;
            }

            if(!Program.TrenutniKorisnik.ImaPravo(137041) && (Program.TrenutniKorisnik.KomercijalnoUserID == null || dok.ZapID != Program.TrenutniKorisnik.KomercijalnoUserID))
            {
                MessageBox.Show("Dokument u kom zelite da promenite partnera nije vas dokument! Ukoliko je greska, " +
                    "proverite sa administratorom da li vam je nalog" +
                    "povezan sa Komercijalnim poslovanjem. Takodje postoji pravo koje zanemaruje ovu proveru pa mozete zatraziti njega!");
                return;
            }

            dok.PPID = _ppid;
            dok.Update(_con);
            Komercijalno.Procedure.PresaberiDokument(_con, vrDok, brDok);
            MessageBox.Show("Partner u dokumentu uspesno azuriran!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ip.ShowDialog();
        }

        private void IzborPartneraSelect(IzborPartneraSelectArgs args)
        {
            if(!_partneri.Result.ContainsKey(args.PPID))
            {
                MessageBox.Show($"Partner sa ID-em {args.PPID} nije pronadjen!");
            }
            else
            {
                button2.Text = _partneri.Result[args.PPID].Naziv;
                _ppid = args.PPID;
            }

            _ip.Hide();
        }
    }
}
