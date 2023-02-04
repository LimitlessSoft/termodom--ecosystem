using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class Menadzment_Fakturisi0634_Index : Form
    {
        private TDOffice.Dokument0634 _dokument { get; set; }
        private Task<TDOffice.Config<Dictionary<int, Dictionary<int, double>>>> _lager = TDOffice.Config<Dictionary<int, Dictionary<int, double>>>.GetAsync(TDOffice.ConfigParameter.Lager0634);
        private Task<List<Komercijalno.Roba>> _robaKomercijalno = Task.Run(() =>
        {
            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return Komercijalno.Roba.List(con);
            }
        });
        private Task<List<Komercijalno.RobaUMagacinu>> _robaUMagacinuKomercijalno = Task.Run(() =>
        {
            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return Komercijalno.RobaUMagacinu.ListByMagacinID(con, 50);
            }
        });
        private Task<List<Komercijalno.Magacin>> _magaciniKomercijalno = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);

        private Task<Dictionary<int, double>> _nabavneCene;


        public Menadzment_Fakturisi0634_Index(int brDok)
        {
            InitializeComponent();
            _dokument = TDOffice.Dokument0634.Get(brDok);
        }

        private void Menadzment_Fakturisi0634_Index_Load(object sender, EventArgs e)
        {
            NamestiUI();
            _nabavneCene = UcitajNabavneCeneAsync();
            UcitajStavke();
        }

        private Task<Dictionary<int, double>> UcitajNabavneCeneAsync()
        {
            return Task.Run(() =>
            {
                Dictionary<int, double> dict = new Dictionary<int, double>();
                List<int> keys = _lager.Result.Tag[_dokument.MagacinID].Keys.ToList();

                List<Komercijalno.Dokument> dokumentiNabavke = Komercijalno.Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");
                List<Komercijalno.Stavka> stavkeNabavke = Komercijalno.Stavka.List(DateTime.Now.Year, $"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

                foreach (int key in keys)
                {
                    try
                    {
                        dict[key] = Komercijalno.Komercijalno.GetRealnaNabavnaCena(key, DateTime.Now, dokumentiNabavke, stavkeNabavke);
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.Message.ToLower().Contains("invoke") || ex.Message.ToLower().Contains("disposed"))
                            return null;

                        Task.Run(() =>
                        {
                            MessageBox.Show(ex.ToString());
                        });
                    }
                    catch (Exception ex)
                    {
                        Task.Run(() =>
                        {
                            MessageBox.Show(ex.ToString());
                        });
                    }
                }
                return dict;
            });
        }
        private void NamestiUI()
        {
            this.BackColor = _dokument.Status == 0 ? Color.Green : Color.Red;
            this.status_btn.BackgroundImage = _dokument.Status == 0 && _dokument.KomercijalnoFaktura == null ? global::TDOffice_v2.Properties.Resources.key_green : global::TDOffice_v2.Properties.Resources.key_red;
            brDokKomProracun_txt.Text = _dokument.KomercijalnoFaktura.ToStringOrDefault();
            obrisiToolStripMenuItem.Enabled = _dokument.Status == 0;
            obrisiSveStavkeToolStripMenuItem.Enabled = _dokument.Status == 0;
            brojDokumenta_txt.Text = _dokument.ID.ToString();
            datum_txt.Text = _dokument.Datum.ToString("dd.MM.yyyy");
            magacin_txt.Text = _magaciniKomercijalno.Result.FirstOrDefault(x => x.ID == _dokument.MagacinID).Naziv;
            poveziSaKomercijalim_btn.Enabled = _dokument.Status == 1 && _dokument.KomercijalnoFaktura == null;
        }

        private void UcitajStavke()
        {
            List<TDOffice.Stavka0634> stavke = TDOffice.Stavka0634.List("BRDOK = " + _dokument.ID);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Proizvod", typeof(string));
            dt.Columns.Add("Kolicina", typeof(double));
            dt.Columns.Add("JM", typeof(string));

            foreach(TDOffice.Stavka0634 s in stavke)
            {
                Komercijalno.Roba r = _robaKomercijalno.Result.FirstOrDefault(x => x.ID == s.RobaID);
                DataRow dr = dt.NewRow();
                dr["ID"] = s.ID;
                dr["RobaID"] = s.RobaID;
                dr["KatBr"] = r == null ? "GRESKA" : r.KatBr;
                dr["Proizvod"] = r == null ? "GRESKA" : r.Naziv;
                dr["Kolicina"] = s.Kolicina;
                dr["JM"] = r == null ? "GRESKA" : r.JM;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Task.Run(() =>
            {
                using (Menadzment_Lager0634_Index l = new Menadzment_Lager0634_Index())
                {
                    l.Shown += (ev, args) =>
                    {
                        l.MagacinID = _dokument.MagacinID;
                        l.IzborRobe = true;
                    };
                    l.OnRobaSelect += (robaID, kolicina) =>
                    {
                        if(TDOffice.Stavka0634.List("BRDOK = " + _dokument.ID).FirstOrDefault(x => x.RobaID == robaID) != null)
                        {
                            MessageBox.Show("Proizvod vec postoji u dokumentu!");
                            return;
                        }

                        TDOffice.Stavka0634.Insert(_dokument.ID, robaID, kolicina);
                        _lager.Result.Tag[_dokument.MagacinID][robaID] -= kolicina;
                        _lager.Result.UpdateOrInsert();

                        this.Invoke((MethodInvoker) delegate
                        {
                            UcitajStavke();
                        });
                    };
                    l.ShowDialog();
                }
            });
        }

        private void napuni_btn_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                double ciljNabavneVrednosti = 0;
                using(InputBox ib = new InputBox("Cilj nabavne vrednosti", "Unesite ciljanu nabavnu vrednost"))
                {
                    ib.ShowDialog();

                    if (!double.TryParse(ib.returnData, out ciljNabavneVrednosti))
                    {
                        MessageBox.Show("Neispravna nabavna vrednost!");
                        return;
                    }
                }

                if(ciljNabavneVrednosti <= 0)
                {
                    MessageBox.Show("Cilj nabavne vrednosti mora biti veca od 0!");
                    return;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    this.Enabled = false;
                });
                while (!_nabavneCene.IsCompleted)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        napuni_btn.BackColor = Random.LightColor();
                    });
                    Thread.Sleep(500);
                }

                double trenutnaNabavna = 0;

                List<int> keys = _lager.Result.Tag[_dokument.MagacinID].Keys.ToList();

                bool br = false;
                while (trenutnaNabavna < ciljNabavneVrednosti)
                {
                    int k = Random.Next(0, keys.Count - 1);
                    double kolicina = _lager.Result.Tag[_dokument.MagacinID][keys[k]];
                    double nabavnaCena = _nabavneCene.Result[keys[k]];

                    if (nabavnaCena <= 0)
                        continue;

                    if (TDOffice.Stavka0634.List("BRDOK = " + _dokument.ID).FirstOrDefault(x => x.RobaID == keys[k]) != null)
                        continue;

                    if (trenutnaNabavna + (nabavnaCena * kolicina) > ciljNabavneVrednosti)
                    {
                        br = true;
                        double razlika = ciljNabavneVrednosti - trenutnaNabavna;

                        kolicina = Convert.ToInt32(razlika / nabavnaCena);
                    }

                    TDOffice.Stavka0634.Insert(_dokument.ID, keys[k], kolicina);
                    _lager.Result.Tag[_dokument.MagacinID][keys[k]] -= kolicina;
                    _lager.Result.UpdateOrInsert();

                    trenutnaNabavna += kolicina * nabavnaCena;

                    if (br || trenutnaNabavna > ciljNabavneVrednosti)
                        break;

                    this.Invoke((MethodInvoker) delegate
                    {
                        napuni_btn.BackColor = Random.LightColor();
                    });
                }
                this.Invoke((MethodInvoker)delegate
                {
                    UcitajStavke();
                    napuni_btn.BackColor = Control.DefaultBackColor;
                    this.Enabled = true;
                    MessageBox.Show("Gotovo!");
                });
            });
        }
        private void poveziSaKomercijalim_btn_Click(object sender, EventArgs e)
        {
            int brFaktureKom = 0;
            using (InputBox ib = new InputBox("Faktura", "Unesite broj fakture"))
            {
                ib.ShowDialog();

                if(!Int32.TryParse(ib.returnData, out brFaktureKom))
                {
                    MessageBox.Show("Neispravan broj fakture!");
                    return;
                }
            }
            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                Komercijalno.Dokument dok = Komercijalno.Dokument.Get(con, 13, brFaktureKom);

                if(dok == null)
                {
                    MessageBox.Show("Faktura nije pronadjena!");
                    return;
                }

                if(dok.Flag != 0)
                {
                    MessageBox.Show("Faktura mora biti otkljucana!");
                    return;
                }

                List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 13, brFaktureKom);

                if(stavke.Count != 0)
                {
                    MessageBox.Show("Faktura mora biti prazna!");
                    return;
                }

                foreach (TDOffice.Stavka0634 s in TDOffice.Stavka0634.List("BRDOK = " + _dokument.ID).Where(x => x.Kolicina > 0))
                    Komercijalno.Stavka.Insert(con, dok, _robaKomercijalno.Result.FirstOrDefault(x => x.ID == s.RobaID), _robaUMagacinuKomercijalno.Result.FirstOrDefault(x => x.RobaID == s.RobaID), s.Kolicina, 0);

                MessageBox.Show("Gotovo!");
            }
        }

        private void status_btn_Click(object sender, EventArgs e)
        {
            _dokument.Status = _dokument.Status == 0 ? 1 : 0;
            _dokument.Update();
            NamestiUI();
        }

        private void navigacijaSledeci_btn_Click(object sender, EventArgs e)
        {
            TDOffice.Dokument0634 dok = TDOffice.Dokument0634.Get(_dokument.ID + 1);
            if(dok == null)
            {
                MessageBox.Show("Ne postoji!");
                return;
            }

            _dokument = dok;
            NamestiUI();
            UcitajStavke();
        }

        private void navigacijaPrethodni_btn_Click(object sender, EventArgs e)
        {
            TDOffice.Dokument0634 dok = TDOffice.Dokument0634.Get(_dokument.ID - 1);
            if (dok == null)
            {
                MessageBox.Show("Ne postoji!");
                return;
            }

            _dokument = dok;
            NamestiUI();
            UcitajStavke();
        }

        private void obrisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowID = dataGridView1.SelectedCells[0].RowIndex;
            int idStavke = Convert.ToInt32(dataGridView1.Rows[rowID].Cells["ID"].Value);
            int robaID  = Convert.ToInt32(dataGridView1.Rows[rowID].Cells["RobaID"].Value);
            double kolicina = Convert.ToDouble(dataGridView1.Rows[rowID].Cells["Kolicina"].Value);
            TDOffice.Stavka0634.Delete(idStavke);
            _lager.Result.Tag[_dokument.MagacinID][robaID] += kolicina;
            _lager.Result.UpdateOrInsert();
            dataGridView1.Rows.RemoveAt(rowID);
        }

        private void obrisiSveStavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Da li zelite ukloniti sve stavke iz dokumenta?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int idStavke = Convert.ToInt32(row.Cells["ID"].Value);
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);
                double kolicina = Convert.ToDouble(row.Cells["Kolicina"].Value);
                TDOffice.Stavka0634.Delete(idStavke);
                _lager.Result.Tag[_dokument.MagacinID][robaID] += kolicina;
                _lager.Result.UpdateOrInsert();
            }
            UcitajStavke();
        }
    }
}
