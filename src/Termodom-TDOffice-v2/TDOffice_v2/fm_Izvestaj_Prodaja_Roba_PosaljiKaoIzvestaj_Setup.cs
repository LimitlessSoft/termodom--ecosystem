using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Concurrent;
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
    public partial class fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup : Form
    {
        private DataTable _data = new DataTable();

        private DataTable _dataIzvestaj = new DataTable();

        private Models.Periodi _periodi;
        private Task<List<TDOffice.User>> _tdOfficeUsers { get; set; } = TDOffice.User.ListAsync();
        private List<int> _magaciniUIzvestaju { get; set; } = new List<int>();
        private string _tipIzvestaja { get; set; }
        private Task<fm_Help> _help { get; set; }
        private List<int> _listaRobe { get; set; } = new List<int>();
        private Task<List<Komercijalno.Roba>> _komercijalnoRoba { get; set; } = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
        private Task<List<Komercijalno.RobaUMagacinu>> _komercijalnoRobaUMagacinu50 { get; set; } = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(50);
        private Task<List<Komercijalno.Tarife>> _tarife { get; set; } = Task.Run(() => { return Komercijalno.Tarife.List(); });

        public fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup(DataTable data, Models.Periodi periodi, string tipIzv,List<int> listaRobe)
        {
            InitializeComponent();
            _data = data;
            _periodi = periodi;
            _tipIzvestaja = tipIzv;

            _help = this.InitializeHelpModulAsync(Modul.fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup);

            _magaciniUIzvestaju = _data.AsEnumerable().Select(x => Convert.ToInt32(x["MAGACINID"])).Distinct().ToList();
            _listaRobe = _data.AsEnumerable().Select(x => Convert.ToInt32(x["ROBAID"])).Distinct().ToList();

            korisnicimaMagacinaSamoSvojePodatke_rb.Checked = true;
            PodesiCLB();
        }

        private void fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup_Load(object sender, EventArgs e)
        {
        }

        private void posalji_btn_Click(object sender, EventArgs e)
        {
            if(korisnicima_clb.CheckedItems.Count == 0)
            {
                MessageBox.Show("Morate izabrati barem nekog primaoca!");
                return;
            }

            List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

            foreach (TDOffice.User u in korisnicima_clb.CheckedItems.OfType<TDOffice.User>())
            {
                DataTable izvestaj = _data.Copy(); // nema reperne

                if (korisnicimaMagacinaSamoSvojePodatke_rb.Checked)
                    izvestaj = _data.AsEnumerable().Where(x => Convert.ToInt32(x["MAGACINID"]) == _clanoviMagacina.FirstOrDefault(y => y.KorisnikID == u.ID).MagacinID).CopyToDataTable();

                if (!uIzvestajUbaciVrednosti_cb.Checked)
                    foreach(int godina in _periodi.Godine)
                        izvestaj.Columns.Remove("vGOD" + godina);

                if (!uIzvestajUbaciKolicine_btn.Checked)
                    foreach (int godina in _periodi.Godine)
                        izvestaj.Columns.Remove("kGOD" + godina);
                string naslov = "Izvestaj Prodaja Robe";
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
                        Action = TDOffice.PorukaAction.IzvestajProdajeRobe,
                        AdditionalInfo = new Tuple<DataTable, Models.Periodi, string>(izvestaj, _periodi, _tipIzvestaja)
                    },
                    Tekst = string.Join(Environment.NewLine, komentar_rtb.Text, "Izvestaj Prometa Robe. Pogledaj prilog gore desno!")
                });
            }

            MessageBox.Show("Izvestaj uspesno poslat!");  
        }

        private void PodesiCLB()
        {
            List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

            korisnicima_clb.DataSource = korisnicimaMagacinaCeoIzvestaj_rb.Checked || izvestajKakavVIdim_rb.Checked ?
                    _tdOfficeUsers.Result : _tdOfficeUsers.Result.Where(x => _clanoviMagacina.Any(y => y.KorisnikID == x.ID && _magaciniUIzvestaju.Contains(y.MagacinID))).ToList();
            
            korisnicima_clb.DisplayMember = "Username";
            korisnicima_clb.ValueMember = "ID";

            foreach(int mag in _magaciniUIzvestaju)
            {
                if(_clanoviMagacina.Count(x => x.MagacinID == mag) == 0)
                {
                    Task.Run(() =>
                    {
                        MessageBox.Show("Magacin " + mag + " nema ni jednog clana!");
                    });
                }
            }
        }

        private void korisnicimaMagacinaSamoSvojePodatke_rb_CheckedChanged(object sender, EventArgs e)
        {
            PodesiCLB();
        }

        private void korisnicimaMagacinaCeoIzvestaj_rb_CheckedChanged(object sender, EventArgs e)
        {
            PodesiCLB();
        }

        private void izvestajKakavVIdim_rb_CheckedChanged(object sender, EventArgs e)
        {
            PodesiCLB();
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _help.Result.ShowDialog();
        }

        private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < korisnicima_clb.Items.Count; i++)
                korisnicima_clb.SetItemChecked(i, true);
        }

        private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < korisnicima_clb.Items.Count; i++)
                korisnicima_clb.SetItemChecked(i, false);
        }

        private void btn_ReperniMagacini_Click(object sender, EventArgs e)
        {
            List<Komercijalno.Magacin> magacini = Komercijalno.Magacin.ListAsync().Result;
            List<Tuple<int, string>> dict = new List<Tuple<int, string>>();
            posalji_btn.Enabled = false;

            foreach (Komercijalno.Magacin m in magacini)
                dict.Add(new Tuple<int, string>(m.ID, m.Naziv));

            using (Input_CheckedListBox cb = new Input_CheckedListBox())
            {
                cb.DataSource = dict;
                cb.CheckOnClick = true;

                cb.ShowDialog();

                if (cb.CheckedValues.Count <= 0)
                {
                    posalji_btn.Enabled = true;
                    MessageBox.Show("Niste izabrali ni jedan magacin za REPER!");
                    return;
                }

                List<int> CheckedValues = cb.CheckedValues;

                Task.Run(() =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "Ucitavanje...";
                        posalji_btn.Enabled = false;
                        korisnicimaMagacina_gb.Enabled = false;
                        groupBox1.Enabled = false;
                        korisnicima_clb.Enabled = false;
                        btn_ReperniMagacini.Enabled = false;
                    });

                    DataTable reperniRezultati = _tipIzvestaja == "RSD" ?
                        Models.Izvestaj.ProdajaRobePoRealnimCenama(_periodi, _magaciniUIzvestaju, _listaRobe.Count > 0 ? _listaRobe : null, CheckedValues) :
                        Models.Izvestaj.ProdajaRobePoPoslednjimCenama(_periodi, _magaciniUIzvestaju, _listaRobe.Count > 0 ? _listaRobe : null, CheckedValues);
                    for (int i = _data.Rows.Count - 1; i >= 0; i--)
                        if (Convert.ToInt32(_data.Rows[i]["MAGACINID"]) == -8)
                            _data.Rows.RemoveAt(i);

                    _data = reperniRezultati;

                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = $"";
                        posalji_btn.Enabled = true;
                        korisnicimaMagacina_gb.Enabled = true;
                        groupBox1.Enabled = true;
                        korisnicima_clb.Enabled = true;
                        btn_ReperniMagacini.Enabled = true;
                    });
                });
            }
        }
    }
}
