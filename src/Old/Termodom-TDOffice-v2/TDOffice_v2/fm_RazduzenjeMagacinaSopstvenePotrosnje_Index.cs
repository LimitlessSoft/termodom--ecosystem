using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;

namespace TDOffice_v2
{
    public partial class fm_RazduzenjeMagacinaSopstvenePotrosnje_Index : Form
    {
        private class StavkaUslugeDto
        {
            public int RobaId { get; set; }
            public double Kolicina { get; set; }
            public double CenaBezPdv { get; set; }
            public string Naziv { get; set; }
            public string Jm { get; set; }
        }
        private Task<Termodom.Data.Entities.Komercijalno.Dokument> _izvornaInternaKalkulacijaTask { get; set; }
        private fm_IzborUsluga_Index _izborUsluge { get; set; }
        private const int _izvorVrDok = 26;
        /// <summary>
        /// robaId, kolicina, cena
        /// </summary>
        private Dictionary<int, StavkaUslugeDto> _stavkeUsluge { get; set; } = new Dictionary<int, StavkaUslugeDto>();

        public fm_RazduzenjeMagacinaSopstvenePotrosnje_Index()
        {
            InitializeComponent();
        }

        private void fm_RazduzenjeMagacinaSopstvenePotrosnje_Index_Load(object sender, EventArgs e)
        {
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            ToggleUi(false);
            try
            {
                _izborUsluge = new fm_IzborUsluga_Index();
                _izborUsluge.DisposeOnClose = false;
                _izborUsluge.PotvrdaUsluge += async (robaId, kolicina, cena, naziv, jm) =>
                {
                    if (_stavkeUsluge.ContainsKey(robaId))
                    {
                        MessageBox.Show("Ova usluga vec postoji!");
                        return;
                    }

                    _stavkeUsluge.Add(robaId, new StavkaUslugeDto()
                    {
                        CenaBezPdv = cena,
                        Jm = jm,
                        Kolicina = kolicina,
                        Naziv = naziv,
                        RobaId = robaId
                    });

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Naziv", typeof(string));
                    dt.Columns.Add("Kolicina", typeof(double));
                    dt.Columns.Add("Jm", typeof(string));
                    dt.Columns.Add("Cena bez pdv", typeof(double));

                    foreach (var stavka in _stavkeUsluge.Values)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Naziv"] = stavka.Naziv;
                        dr["Kolicina"] = stavka.Kolicina;
                        dr["Jm"] = stavka.Jm;
                        dr["Cena bez pdv"] = stavka.CenaBezPdv;
                        dt.Rows.Add(dr);
                    }

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AllowUserToResizeRows = false;
                    dataGridView1.ReadOnly = true;

                    preostalaVrednostRobe_txt.Text = (await GetPreostaluVrednostRobeZaKojuTrebaNapravitiUslugeAsync()).ToString("#,##0.00 RSD");
                };

                var firme = await FirmaManager.DictionaryAsync();

                izvornaBaza_cmb.DataSource = firme.Values.ToList();
                izvornaBaza_cmb.DisplayMember = "Naziv";
                izvornaBaza_cmb.ValueMember = "GlavniMagacin";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Doslo je do greske!");
                MessageBox.Show(ex.ToString());
            }

            ToggleUi(true);
        }

        private void ucitaj_btn_Click(object sender, EventArgs e)
        {
            _ = UcitajIzvornuInternuKalkulacijeAsync();
        }

        private async Task UcitajIzvornuInternuKalkulacijeAsync()
        {
            ToggleUi(false);
            try
            {
                ResetUI();
                int izvorKalkulacijaBrDok;

                if (!int.TryParse(izvor_txt.Text, out izvorKalkulacijaBrDok))
                {
                    MessageBox.Show("Neispravan broj dokumenta kalkulacije!");
                    ToggleUi(true);
                    return;
                }

                int izvornaBazaId = Convert.ToInt32(izvornaBaza_cmb.SelectedValue);
                _izvornaInternaKalkulacijaTask = DokumentManager.GetAsync(izvornaBazaId, _izvorVrDok, izvorKalkulacijaBrDok, DateTime.Now.Year);
                var izvornaInternaKalkulacija = await _izvornaInternaKalkulacijaTask;
                if (izvornaInternaKalkulacija == null)
                {
                    MessageBox.Show("Kalkulacija sa datim brojem nije pronadjena u izabranoj bazi!");
                    ToggleUi(true);
                    return;
                }

                if (izvornaInternaKalkulacija.Flag != 1)
                {
                    MessageBox.Show("Kalkulacija mora biti zakljucana!");
                    _izvornaInternaKalkulacijaTask = null;
                    ToggleUi(true);
                    return;
                }

                if(izvornaInternaKalkulacija.VrDokOut.HasValue)
                {
                    MessageBox.Show("Kalkulacija je vec razduzena (vrDokOut), ne mozete ponovo razduziti po njoj!");
                    _izvornaInternaKalkulacijaTask = null;
                    ToggleUi(true);
                    return;
                }

                var firme = await FirmaManager.DictionaryAsync();
                var magacinRazduzenja = firme.Values.First(x => x.GlavniMagacin == izvornaBazaId).MagacinRazduzenja;
                if (izvornaInternaKalkulacija.MagacinID != magacinRazduzenja)
                {
                    MessageBox.Show($"Magacin razduzenaj za izabranu firmu je {magacinRazduzenja}. Ova kalkulacija ne pripada datom magacinu!");
                    _izvornaInternaKalkulacijaTask = null;
                    ToggleUi(true);
                    return;
                }

                _stavkeUsluge.Clear();
                dataGridView1.DataSource = null;
                preostalaVrednostRobe_txt.Text = (await GetPreostaluVrednostRobeZaKojuTrebaNapravitiUslugeAsync()).ToString("#,##0.00 RSD");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Doslo je do greske!");
                MessageBox.Show(ex.ToString());
            }
            ToggleUi(true);
        }

        private void ResetUI()
        {
            preostalaVrednostRobe_txt.Text = "";
        }

        private async Task<double> GetPreostaluVrednostRobeZaKojuTrebaNapravitiUslugeAsync()
        {
            var izvornaInternaKalkulacija = await _izvornaInternaKalkulacijaTask;
            int izvornaBazaId = Convert.ToInt32(izvornaBaza_cmb.SelectedValue);
            await DokumentManager.PresaberiAsync(izvornaBazaId, _izvorVrDok, izvornaInternaKalkulacija.BrDok, DateTime.Now.Year);

            _izvornaInternaKalkulacijaTask = DokumentManager.GetAsync(izvornaBazaId, _izvorVrDok, izvornaInternaKalkulacija.BrDok, DateTime.Now.Year);
            izvornaInternaKalkulacija = await _izvornaInternaKalkulacijaTask;
            if (izvornaInternaKalkulacija == null)
                MessageBox.Show("Kalkulacija sa datim brojem nije pronadjena u izabranoj bazi!");

            var vrednostUnetihUsluga = _stavkeUsluge.Values.Sum(x => x.CenaBezPdv);
            return izvornaInternaKalkulacija.Potrazuje - vrednostUnetihUsluga;
        }

        private void ToggleUi(bool state)
        {
            foreach (Control control in this.Controls)
                control.Enabled = state;

            ToggleUfuUi(_izvornaInternaKalkulacijaTask != null && _izvornaInternaKalkulacijaTask.Result != null);
        }

        private void ToggleUfuUi(bool state)
        {
            ufu_gb.Enabled = state;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _izborUsluge.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ = RazduziMagacinAsync();
        }

        private async Task RazduziMagacinAsync()
        {
            if ((await GetPreostaluVrednostRobeZaKojuTrebaNapravitiUslugeAsync()) != 0)
            {
                MessageBox.Show("Vrednost usluga i robe izvorne kalkulacije moraju biti isti!");
                return;
            }

            ToggleUi(false);
            try
            {
                int izvornaBazaId = Convert.ToInt32(izvornaBaza_cmb.SelectedValue);
                var izvornaInternaKalkulacija = await _izvornaInternaKalkulacijaTask;

                if (izvornaInternaKalkulacija.Potrazuje == 0)
                {
                    MessageBox.Show("Kalkulacija nema vrednost!");
                    return;
                }

                var povezanaInternaOtpremnica = await DokumentManager.GetAsync(izvornaBazaId, (int)izvornaInternaKalkulacija.VrDokIn, (int)izvornaInternaKalkulacija.BrDokIn, izvornaInternaKalkulacija.Datum.Year);

                var firme = await FirmaManager.DictionaryAsync();
                var magacinRazduzenja = firme.Values.First(x => x.GlavniMagacin == izvornaBazaId).MagacinRazduzenja;
                var novokreiraniDokument28BrDok = await DokumentManager.InsertAsync(new DokumentManager.DokumentInsertRequestBody()
                {
                    BazaId = magacinRazduzenja,
                    GodinaBaze = DateTime.Now.Year,
                    DozvoliDaljeIzmeneUKomercijalnom = true,
                    InterniBroj = $"M{povezanaInternaOtpremnica.MagacinID}",
                    KomercijalnoKorisnikId = Program.TrenutniKorisnik.KomercijalnoUserID,
                    MagacinId = magacinRazduzenja,
                    NuId = 1,
                    PPID = firme.Values.First(x => x.GlavniMagacin == izvornaBazaId).PPID,
                    VrDok = 28
                });
                var novokreiraniDokument28 = await DokumentManager.GetAsync(magacinRazduzenja, 28, novokreiraniDokument28BrDok, DateTime.Now.Year);

                var stavkeIzvora = await StavkaManager.DictionaryAsync(izvornaBazaId, izvornaInternaKalkulacija.VrDok, izvornaInternaKalkulacija.BrDok, izvornaInternaKalkulacija.Datum.Year);

                foreach (var stavka in stavkeIzvora.Values)
                {
                    await StavkaManager.InsertAsync(new StavkaManager.StavkaInsertRequestBody()
                    {
                        BazaId = magacinRazduzenja,
                        GodinaBaze = DateTime.Now.Year,
                        BrDok = novokreiraniDokument28BrDok,
                        Kolicina = stavka.Kolicina,
                        ProdajnaCenaBezPdv = null,
                        Rabat = 0,
                        RobaId = stavka.RobaID,
                        VrDok = 28
                    });
                }

                var novokreiraniDokument6BrDok = await DokumentManager.InsertAsync(new DokumentManager.DokumentInsertRequestBody()
                {
                    BazaId = povezanaInternaOtpremnica.MagacinID,
                    GodinaBaze = DateTime.Now.Year,
                    DozvoliDaljeIzmeneUKomercijalnom = true,
                    InterniBroj = $"M{povezanaInternaOtpremnica.MagacinID}",
                    KomercijalnoKorisnikId = Program.TrenutniKorisnik.KomercijalnoUserID,
                    MagacinId = povezanaInternaOtpremnica.MagacinID,
                    NuId = 1,
                    PPID = firme.Values.First(x => x.GlavniMagacin == izvornaBazaId).PPID,
                    VrDok = 6
                });

                foreach (int robaId in _stavkeUsluge.Keys)
                {
                    await StavkaManager.NapraviUsluguAsync(new StavkaManager.NapraviUsluguRequestBody()
                    {
                        BazaId = izvornaInternaKalkulacija.MagacinID,
                        GodinaBaze = DateTime.Now.Year,
                        BrDok = novokreiraniDokument6BrDok,
                        Kolicina = _stavkeUsluge[robaId].Kolicina,
                        CenaBezPdv = _stavkeUsluge[robaId].CenaBezPdv,
                        Rabat = 0,
                        RobaId = robaId,
                        VrDok = 6
                    });
                }

                MessageBox.Show("Gotvo!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            ToggleUi(true);
        }
    }
}
