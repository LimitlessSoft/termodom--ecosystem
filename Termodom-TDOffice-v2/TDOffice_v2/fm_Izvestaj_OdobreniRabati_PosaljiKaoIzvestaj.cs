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
    public partial class fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj : Form
    {
        private DataTable _data = new DataTable();
        private Task<List<TDOffice.User>> _tdOfficeUsers { get; set; } = TDOffice.User.ListAsync();
        private List<int> _magaciniUIzvestaju { get; set; } = new List<int>();
        private Task<fm_Help> _help { get; set; }
        public fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj(DataTable data)
        {
            InitializeComponent();
            _help = this.InitializeHelpModulAsync(Modul.fm_Izvestaj_OdobreniRabati_PosaljiKaoIzvestaj);
            _data = data;
            _magaciniUIzvestaju = _data.AsEnumerable().Where(x => int.TryParse(x["MAGACINID"].ToString(), out _)).Select(x => Convert.ToInt32(x["MAGACINID"])).Distinct().ToList();
            PodesiCLB();
        }
        private void PodesiCLB()
        {
            List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

            korisnicima_clb.DataSource = korisnicimaMagacinaCeoIzvestaj_rb.Checked || izvestajKakavVIdim_rb.Checked ?
                    _tdOfficeUsers.Result : _tdOfficeUsers.Result.Where(x => _clanoviMagacina.Any(y => y.KorisnikID == x.ID && _magaciniUIzvestaju.Contains(y.MagacinID))).ToList();

            korisnicima_clb.DisplayMember = "Username";
            korisnicima_clb.ValueMember = "ID";

            foreach (int mag in _magaciniUIzvestaju)
            {
                if (_clanoviMagacina.Count(x => x.MagacinID == mag) == 0)
                {
                    Task.Run(() =>
                    {
                        MessageBox.Show("Magacin " + mag + " nema ni jednog clana!");
                    });
                }
            }
        }

        private void posalji_btn_Click(object sender, EventArgs e)
        {
            if (korisnicima_clb.CheckedItems.Count == 0)
            {
                MessageBox.Show("Morate izabrati barem nekog primaoca!");
                return;
            }
            List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

            foreach (TDOffice.User u in korisnicima_clb.CheckedItems.OfType<TDOffice.User>())
            {
                DataTable izvestaj = _data.Copy();

                if (korisnicimaMagacinaSamoSvojePodatke_rb.Checked)
                    izvestaj = _data.AsEnumerable().Where(x => Convert.ToInt32(x["MAGACINID"]) == _clanoviMagacina.FirstOrDefault(y => y.KorisnikID == u.ID).MagacinID).CopyToDataTable();
                else if (korisnicimaMagacinaCeoIzvestaj_rb.Checked)
                    izvestaj = _data.Copy();
                string naslov = "Izvestaj Odobreni rabat";
                if (tb_NaslovIzvestaja.TextLength > 0)
                    naslov = tb_NaslovIzvestaja.Text;

                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                {
                    Datum = DateTime.Now,
                    Naslov = naslov,
                    Posiljalac = Program.TrenutniKorisnik.ID,
                    Primalac = u.ID,
                    Status = TDOffice.PorukaTip.Standard,
                    Tag = new TDOffice.PorukaAdditionalInfo()
                    {
                        Action = TDOffice.PorukaAction.OdobreniRabatMagacina,
                        AdditionalInfo = izvestaj
                    },
                    Tekst = string.Join(Environment.NewLine, komentar_rtb.Text, "Izvestaj Prometa Magacina. Pogledaj prilog gore desno!")
                });

            }

            MessageBox.Show("Izvestaj uspesno poslat!");
        }

        private void korisnicimaMagacinaCeoIzvestaj_rb_CheckedChanged(object sender, EventArgs e)
        {
            PodesiCLB();
        }

        private void korisnicimaMagacinaSamoSvojePodatke_rb_CheckedChanged(object sender, EventArgs e)
        {
            PodesiCLB();
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _help.Result.ShowDialog();
        }
    }
}
