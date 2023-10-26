using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Cekovi_Novi : Form
    {
        private static string _poslednjiTR { get; set; }
        private static string _poslednjiBrojCeka { get; set; }
        private static string _poslednjaVrednost { get; set; }
        public int MagacinID
        {
            get
            {
                return Convert.ToInt32(magacin_cmb.SelectedValue);
            }
            set
            {
                magacin_cmb.SelectedValue = value;
            }
        }
        public fm_Cekovi_Novi()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(136101))
            {
                Task.Run(() =>
                {
                    TDOffice.Pravo.NematePravoObavestenje(136101);
                });
                this.Close();
                return;
            }
            InitializeComponent();
            datumUnosa_dtp.Value = DateTime.Now;

            magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            banka_cmb.DataSource = Komercijalno.Banka.List(DateTime.Now.Year);
            banka_cmb.DisplayMember = "Naziv";
            banka_cmb.ValueMember = "BankaID";

            if(!Program.TrenutniKorisnik.ImaPravo(136108))
                datumUnosa_dtp.Enabled = false;

            magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;

            magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136102);

            brojCeka_txt.Text = _poslednjiBrojCeka;
            trGradjana_txt.Text = _poslednjiTR;
            vrednost_txt.Text = _poslednjaVrednost;
        }

        private void fm_Cekovi_Novi_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trGradjana_txt.Text) ||
                string.IsNullOrWhiteSpace(brojCeka_txt.Text))
            {
                MessageBox.Show("Neispravno popunjena polja!");
                return;
            }

            try
            {
                Convert.ToInt32(vrednost_txt.Text);
            }
            catch
            {
                MessageBox.Show("Neispravna vrednost ceka!");
                return;
            }

            TDOffice.Cek.Insert(datumUnosa_dtp.Value,
                TDOffice.Enums.CekStatus.Nerealizovan,
                (int)magacin_cmb.SelectedValue,
                (int)banka_cmb.SelectedValue,
                trGradjana_txt.Text,
                brojCeka_txt.Text,
                datumValute_dtp.Value,
                Convert.ToInt32(vrednost_txt.Text));

            _poslednjiTR = trGradjana_txt.Text;
            _poslednjaVrednost = vrednost_txt.Text;
            _poslednjiBrojCeka = brojCeka_txt.Text;

            MessageBox.Show("Cek uspesno unet!");
            this.Close();
        }
    }
}
