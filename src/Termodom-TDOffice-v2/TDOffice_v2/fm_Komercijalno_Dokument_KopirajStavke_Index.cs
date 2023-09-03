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
using TDOffice_v2.Komercijalno;
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_Komercijalno_Dokument_KopirajStavke_Index : Form
    {
        private int[] VPDokumenti = { 0, 1, 13, 26 };
        private Task<VrstaDokDictionary> _vrsteDokumenta { get; set; }
        private Task<List<DistinctConnectionInfo>> _distinctPutanjeDoBaza { get; set; }

        public fm_Komercijalno_Dokument_KopirajStavke_Index()
        {
            InitializeComponent();
        }

        private void fm_Komercijalno_Dokument_KopirajStavke_Index_Load(object sender, EventArgs e)
        {
            _vrsteDokumenta = VrstaDokManager.DictionaryAsync();
            _distinctPutanjeDoBaza = BazaManager.DistinctConnectionInfoListAsync();

            _vrsteDokumenta.ContinueWith(async (prev) =>
            {
                List<VrstaDok> vrsteDokumenata = (await _vrsteDokumenta).Values.ToList();
                this.Invoke((MethodInvoker)delegate
                {
                    izVrdok_cmb.DataSource = new List<VrstaDok>(vrsteDokumenata);
                    izVrdok_cmb.DisplayMember = "NazivDok";
                    izVrdok_cmb.ValueMember = "VrDok";

                    uVrDok_cmb.DataSource = new List<VrstaDok>(vrsteDokumenata);
                    uVrDok_cmb.DisplayMember = "NazivDok";
                    uVrDok_cmb.ValueMember = "VrDok";
                });
            });

            _distinctPutanjeDoBaza.ContinueWith(async (prev) =>
            {
                List<Tuple<string, string>> list = new List<Tuple<string, string>>();

                foreach (DistinctConnectionInfo csi in await _distinctPutanjeDoBaza)
                {
                    string[] putanjaParts = csi.PutanjaDoBaze.Split("/");
                    list.Add(new Tuple<string, string>(csi.PutanjaDoBaze, $"{csi.Godina} - {putanjaParts[putanjaParts.Length - 1]}"));
                }

                this.Invoke((MethodInvoker)delegate
                {
                    izGodine_cmb.DataSource = new List<Tuple<string, string>>(list);
                    izGodine_cmb.DisplayMember = "Item2";
                    izGodine_cmb.ValueMember = "Item1";

                    uGodinu_cmb.DataSource = new List<Tuple<string, string>>(list);
                    uGodinu_cmb.DisplayMember = "Item2";
                    uGodinu_cmb.ValueMember = "Item1";
                });
            });
        }

        private void kopiraj_btn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Task.Run(() =>
            {
                this.Invoke((MethodInvoker) delegate
                {
                    status_lbl.Text = "Proveravam unos...";
                });
                int izvor_godina = 0;
                int izvor_vrDok = 0;
                int izvor_brDok = 0;

                int destinacija_godina = 0;
                int destinacija_vrDok = 0;
                int destinacija_brDok = 0;
                string izvorniString = "";
                string destinacioniString = "";
                try
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                        izvorniString = $"data source=192.168.0.12; initial catalog = {izGodine_cmb.SelectedValue.ToString()}; user=SYSDBA; password=masterkey";
                        destinacioniString = $"data source=192.168.0.12; initial catalog = {uGodinu_cmb.SelectedValue.ToString()}; user=SYSDBA; password=masterkey";
                        //izvor_godina = Convert.ToInt32(izGodine_cmb.SelectedValue);
                        izvor_vrDok = Convert.ToInt32(izVrdok_cmb.SelectedValue);
                        izvor_brDok = Convert.ToInt32(izBrDok_txt.Text);

                        //destinacija_godina = Convert.ToInt32(uGodinu_cmb.SelectedValue);
                        destinacija_vrDok = Convert.ToInt32(uVrDok_cmb.SelectedValue);
                        destinacija_brDok = Convert.ToInt32(uBrDok_txt.Text);
                    });
                }
                catch
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "";
                        this.Enabled = true;
                    });
                    MessageBox.Show("Niste dobro popunili sva polja!");
                    return;
                }

                this.Invoke((MethodInvoker) delegate
                {
                    status_lbl.Text = "Pripremam dokumente...";
                });

                Komercijalno.Dokument izvorniDokument;
                Komercijalno.Dokument destinacioniDokument;

                using(FbConnection con = new FbConnection(izvorniString))
                //using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[izvor_godina]))
                {
                    con.Open();
                    izvorniDokument = Komercijalno.Dokument.Get(con, izvor_vrDok, izvor_brDok);
                }

                if(izvorniDokument == null)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "";
                        this.Enabled = true;
                    });
                    MessageBox.Show("Izvorni dokument ne postoji!");
                    return;
                }

                using(FbConnection con = new FbConnection(destinacioniString))
                //using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[destinacija_godina]))
                {
                    con.Open();
                    destinacioniDokument = Komercijalno.Dokument.Get(con, destinacija_vrDok, destinacija_brDok);
                }

                if (destinacioniDokument == null)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "";
                        this.Enabled = true;
                    });
                    MessageBox.Show("Destinacioni dokument ne postoji!");
                    return;
                }

                if (destinactioniDokumentMoraBitiOtkljucan_cb.Checked && destinacioniDokument.Flag != 0)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "";
                        this.Enabled = true;
                    });
                    MessageBox.Show("Destinacioni dokument mora biti otkljucan!");
                    return;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    status_lbl.Text = "Pripremam stavke izvornog dokumenta...";
                });

                List<Komercijalno.Stavka> izvorneStavke = new List<Komercijalno.Stavka>();

                using(FbConnection con = new FbConnection(izvorniString))
                //using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[izvor_godina]))
                {
                    con.Open();
                    izvorneStavke = Komercijalno.Stavka.ListByDokument(con, izvor_vrDok, izvor_brDok);
                }

                if(izvorneStavke.Count == 0)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = "";
                        this.Enabled = true;
                    });
                    MessageBox.Show("Izvorni dokument je prazan!");
                    return;
                }

                using(FbConnection con = new FbConnection(destinacioniString))
                //using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[destinacija_godina]))
                {
                    con.Open();

                    if(destinacioniDokumentMoraBitiPrazan_cb.Checked)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            status_lbl.Text = $"Proveravam da li je destinacioni dokument prazan!";
                        });

                        if (Komercijalno.Stavka.ListByDokument(con, destinacija_vrDok, destinacija_brDok).Count > 0)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                status_lbl.Text = "";
                                this.Enabled = true;
                            });
                            MessageBox.Show("Destinacioni dokument mora biti prazan!");
                            return;
                        }
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = $"Ucitavam sifarnik robe...";
                    });
                    List<Komercijalno.Roba> robaKomercijalno = Komercijalno.Roba.List(con);

                    this.Invoke((MethodInvoker)delegate
                    {
                        status_lbl.Text = $"Ucitavam robu magacina iz destinacionog dokumenta ({destinacioniDokument.MagacinID})";
                    });
                    List<Komercijalno.RobaUMagacinu> robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(con, destinacioniDokument.MagacinID);

                    int i = 0;
                    int max = izvorneStavke.Count;

                    List<Komercijalno.Tarife> tarife = Komercijalno.Tarife.List(con);
                    List<Komercijalno.Stavka> postojeceStavkePrePocetkaInserta = Komercijalno.Stavka.ListByDokument(con, destinacioniDokument.VrDok, destinacioniDokument.BrDok);
                    foreach (Komercijalno.Stavka s in izvorneStavke)
                    {
                        i++;
                        this.Invoke((MethodInvoker) delegate
                        {
                            status_lbl.Text = $"Kopiram stavke {i} / {max}";
                        });

                        if(postojeceStavkePrePocetkaInserta.Count(x => x.RobaID == s.RobaID) > 0)
                            if(zaobidjiDuplikat_rb.Checked)
                                continue;

                        int newStavkaID = Komercijalno.Stavka.Insert(con, destinacioniDokument, robaKomercijalno.FirstOrDefault(x => x.ID == s.RobaID), robaUMagacinu.FirstOrDefault(x => x.RobaID == s.RobaID), s.Kolicina, s.Rabat);

                        if(nabavneCeneOstaviKaoUIzvornomDokumentu_cb.Checked)
                        {
                            Komercijalno.Stavka novaStavka = Komercijalno.Stavka.Get(con, newStavkaID);
                            novaStavka.NabavnaCena = s.NabavnaCena;
                            novaStavka.Update(con);
                        }

                        if (prodajneCeneOstaviKaoUIzvornomDokumentu_cb.Checked)
                        {
                            Komercijalno.Stavka novaStavka = Komercijalno.Stavka.Get(con, newStavkaID);
                            double prodajnaCenaBP = VPDokumenti.Contains(izvorniDokument.VrDok) ? s.ProdajnaCena : s.ProdCenaBP;
                            novaStavka.ProdajnaCena = VPDokumenti.Contains(destinacioniDokument.VrDok) ? prodajnaCenaBP : s.ProdajnaCena;
                            novaStavka.ProdCenaBP = s.ProdCenaBP;
                            novaStavka.Korekcija = s.Korekcija;
                            novaStavka.Update(con);
                        }
                    }
                }
                this.Invoke((MethodInvoker) delegate
                {
                    PoveziDokumente(izvorniDokument, destinacioniDokument, izvorniString, destinacioniString);
                    status_lbl.Text = $"Gotovo!";
                    this.Enabled = true;
                    MessageBox.Show("Gotovo!");
                });
            });
        }

        private void PoveziDokumente(Komercijalno.Dokument izvorniDokument, Komercijalno.Dokument destinacioniDokument, string izvorniString, string destinactioniString)
        {
            if (MessageBox.Show("Da li zelite povezati ove dokumente?", "Povezi dokumente?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (izvorniDokument.VrDokOut.HasValue)
                    if (MessageBox.Show("Izvorni dokument vec ima vrednost u polju 'VrDokOut'. Povezivanje dokumenata ce obrisati staru vrednost. Da li zelite nastaviti?", "Oprez!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                if(destinacioniDokument.VrDokIn.HasValue)
                    if (MessageBox.Show("Destinacioni dokument vec ima vrednost u polju 'VrDokIn'. Povezivanje dokumenata ce obrisati staru vrednost. Da li zelite nastaviti?", "Oprez!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                izvorniDokument.VrDokOut = destinacioniDokument.VrDok;
                izvorniDokument.BrDokOut = destinacioniDokument.BrDok;

                using(FbConnection con = new FbConnection(izvorniString))
                {
                    con.Open();
                    izvorniDokument.Update(con);
                }

                destinacioniDokument.VrDokIn = izvorniDokument.VrDok;
                destinacioniDokument.BrDokIn = izvorniDokument.BrDok;

                using (FbConnection con = new FbConnection(destinactioniString))
                {
                    con.Open();
                    destinacioniDokument.Update(con);
                }
            }
        }

        private void destinacioniDokumentMoraBitiPrazan_cb_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible = !destinacioniDokumentMoraBitiPrazan_cb.Checked;
        }
    }
}
