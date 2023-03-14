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
    public partial class fm_Magacin_Index : Form
    {
        private Task<Komercijalno.Magacin> _komercijalnoMagacin { get; set; }
        private TDOffice.Magacin _tdMagacin { get; set; }
        private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<List<Config.Zaposleni>> _configZaposleni { get; set; } = Config.Zaposleni.ListAsync();
        private Task<List<TDOffice.User>> _tdOfficeKorisnici { get; set; } = TDOffice.User.ListAsync();
        private Task<fm_Help> _helpFrom { get; set; }

        public fm_Magacin_Index(int magacinID)
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Magacin_Index);
            _komercijalnoMagacin = Komercijalno.Magacin.GetAsync(DateTime.Now.Year, magacinID);
            _tdMagacin = TDOffice.Magacin.Get(_komercijalnoMagacin.Result.ID);

            if (_tdMagacin == null)
            {
                TDOffice.Magacin.Insert(_komercijalnoMagacin.Result.ID, _komercijalnoMagacin.Result.ID, 50, 1, "NN", "NN"); // Poslednja 3 parametra treba izmeniti na korisnikov unos
                _tdMagacin = TDOffice.Magacin.Get(_komercijalnoMagacin.Result.ID);
            }
        }

        private void fm_Magacin_Index_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                _tdOfficeKorisnici.Wait();

                List<TDOffice.User> vlasnikCmbData = new List<TDOffice.User>(_tdOfficeKorisnici.Result);
                vlasnikCmbData.Add(new TDOffice.User() { ID = -1, Username = " < nema vlasnika >" });
                vlasnikCmbData.Sort((x, y) => x.ID.CompareTo(y.ID));

                List<TDOffice.User> noviKorisnikCmbData = new List<TDOffice.User>(_tdOfficeKorisnici.Result);
                noviKorisnikCmbData.Add(new TDOffice.User() { ID = -1, Username = " < izaberi korisnika >" });
                noviKorisnikCmbData.Sort((x, y) => x.ID.CompareTo(y.ID));

                this.Invoke((MethodInvoker)delegate
                {
                    TDOffice.MagacinClan vlasnik = TDOffice.MagacinClan.ListByMagacinID(_tdMagacin.ID).FirstOrDefault(x => x.Tip == TDOffice.Enums.MagacinClanTip.Vlasnik);
                    vlasnik_cmb.DataSource = vlasnikCmbData;
                    vlasnik_cmb.DisplayMember = "Username";
                    vlasnik_cmb.ValueMember = "ID";
                    vlasnik_cmb.SelectedValue = vlasnik == null ? -1 : vlasnik.KorisnikID;

                    noviKorisnik_cmb.DataSource = noviKorisnikCmbData;
                    noviKorisnik_cmb.DisplayMember = "Username";
                    noviKorisnik_cmb.ValueMember = "ID";

                    vlasnik_cmb.Enabled = true;
                    noviKorisnik_cmb.Enabled = true;
                });
            });
            Task.Run(() =>
            {
                Task.WaitAll(_komercijalnoMagacini);

                this.Invoke((MethodInvoker)delegate
                {
                    nadredjeniMagacin_cmb.DisplayMember = "Naziv";
                    nadredjeniMagacin_cmb.ValueMember = "ID";
                    nadredjeniMagacin_cmb.DataSource = new List<Komercijalno.Magacin>(_komercijalnoMagacini.Result);

                    magacinRazduzenja_cmb.DisplayMember = "Naziv";
                    magacinRazduzenja_cmb.ValueMember = "ID";
                    magacinRazduzenja_cmb.DataSource = new List<Komercijalno.Magacin>(_komercijalnoMagacini.Result);

                    magacinRazduzenja_cmb.SelectedValue = _tdMagacin.MagacinRazduzenja;
                    nadredjeniMagacin_cmb.SelectedValue = _tdMagacin.NadredjeniMagacin;

                    nadredjeniMagacin_cmb.Enabled = true;
                    magacinRazduzenja_cmb.Enabled = true;
                });
            });

            UcitajKorisnikeMagacinaAsync();
        }

        private Task UcitajKorisnikeMagacinaAsync()
        {
            return Task.Run(() =>
            {
                List<TDOffice.MagacinClan> mcs = TDOffice.MagacinClan.ListByMagacinID(_tdMagacin.ID);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Korisnik", typeof(string));
                dt.Columns.Add("KomercijalnoID", typeof(int));
                dt.Columns.Add("KomercijalnoIme", typeof(string));

                foreach (TDOffice.MagacinClan mc in mcs)
                {
                    if (mc.Tip == TDOffice.Enums.MagacinClanTip.Vlasnik)
                        continue;

                    TDOffice.User tdUser = _tdOfficeKorisnici.Result.FirstOrDefault(x => x.ID == mc.KorisnikID);
                    Config.Zaposleni configZap = tdUser.KomercijalnoUserID != null ? _configZaposleni.Result.FirstOrDefault(x => x.ZapID == tdUser.KomercijalnoUserID) : new Config.Zaposleni() { ZapID = -1, Naziv = "NIJE DEFINISAN" };

                    DataRow dr = dt.NewRow();
                    dr["ID"] = mc.KorisnikID;
                    dr["Korisnik"] = tdUser.Username;
                    dr["KomercijalnoID"] = configZap.ZapID;
                    dr["KomercijalnoIme"] = configZap.Naziv;
                    dt.Rows.Add(dr);
                }

                this.Invoke((MethodInvoker) delegate
                {
                    korisnici_dgv.DataSource = dt;
                    korisnici_dgv.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    korisnici_dgv.Columns["Korisnik"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    korisnici_dgv.Columns["KomercijalnoID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    korisnici_dgv.Columns["KomercijalnoIme"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                });
            });
        }
        private void vlasnik_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!vlasnik_cmb.Enabled)
                return;

            int korisnikID = (int)vlasnik_cmb.SelectedValue;

            if (korisnikID > 0)
                if (TDOffice.MagacinClan.ListByKorisnikID(korisnikID).Count > 0)
                {
                    UcitajKorisnikeMagacinaAsync();
                    MessageBox.Show("Zaposleni je vec vlasnik / korisnik nekog magacina!");
                    return;
                }
            List<TDOffice.MagacinClan> mcs = TDOffice.MagacinClan.ListByMagacinID(_tdMagacin.ID);

            TDOffice.MagacinClan magacinClan = mcs.Where(x => x.Tip == TDOffice.Enums.MagacinClanTip.Vlasnik).FirstOrDefault();
            if (magacinClan != null)
                TDOffice.MagacinClan.Delete(magacinClan.ID);

            if (korisnikID > 0)
                TDOffice.MagacinClan.Insert(_tdMagacin.ID, korisnikID, TDOffice.Enums.MagacinClanTip.Vlasnik);

            MessageBox.Show("Vlasnik magacina uspesno azuriran!");
        }

        private void noviKorisnik_btn_Click(object sender, EventArgs e)
        {
            if (!noviKorisnik_cmb.Enabled)
                return;
            
            int korisnikID = (int)noviKorisnik_cmb.SelectedValue;
            List<TDOffice.MagacinClan> mcs = TDOffice.MagacinClan.ListByMagacinID(_tdMagacin.ID);

            if (mcs.Count(x => x.KorisnikID == korisnikID) > 0)
            {
                MessageBox.Show("Zaposleni je vec korisnik / vlasnik ovog magacina!");
                return;
            }
            mcs = TDOffice.MagacinClan.ListByKorisnikID(korisnikID);

            if (mcs.Count > 0)
            {
                MessageBox.Show("Zaposleni je vec korisnik ili vlasnik nekog drugog magacina!");
                return;
            }

            TDOffice.MagacinClan.Insert(_tdMagacin.ID, korisnikID, TDOffice.Enums.MagacinClanTip.Clan);
            UcitajKorisnikeMagacinaAsync();
            MessageBox.Show("Zaposleni je uspesno dodat kao korisnik magacina!");
        }

        private void ukloniKorisnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (korisnici_dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Niste selektovali ni jedan red!");
                return;
            }

            int korisnikID = Convert.ToInt32(korisnici_dgv.SelectedRows[0].Cells["ID"].Value);

            TDOffice.MagacinClan.Delete(TDOffice.MagacinClan.ListByKorisnikID(korisnikID)[0].ID);

            UcitajKorisnikeMagacinaAsync();
            MessageBox.Show("Korisnik uspesno uklonjen!");
        }

        private void nadredjeniMagacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!nadredjeniMagacin_cmb.Enabled)
                return;

            _tdMagacin.NadredjeniMagacin = Convert.ToInt32(nadredjeniMagacin_cmb.SelectedValue);
            _tdMagacin.Update();

            MessageBox.Show("Nadredjeni magacin uspesno sacuvan!");
        }

        private void magacinRazduzenja_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!magacinRazduzenja_cmb.Enabled)
                return;

            _tdMagacin.MagacinRazduzenja = Convert.ToInt32(magacinRazduzenja_cmb.SelectedValue);
            _tdMagacin.Update();

            MessageBox.Show("Magacin razduzenja uspesno sacuvan!");
        }
    }
}
