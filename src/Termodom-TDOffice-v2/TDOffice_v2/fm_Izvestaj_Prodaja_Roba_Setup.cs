using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Izvestaj_Prodaja_Roba_Setup : Form
    {
        private DateTimePicker dtp { get; set; }
        private Boolean _initializedtp { get; set; }
        private Task<fm_Help> _helpFrom { get; set; }

        private List<int> _listaRobe = new List<int>();
        private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } = Task.Run(() =>
        {
            return Komercijalno.Magacin.ListAsync().Result.Where(x => new int[] { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 }.Contains(x.ID)).ToList();
        });
        private Task<List<Komercijalno.Roba>> _komercijalnoRoba { get; set; } = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
        private Task<List<Komercijalno.RobaUMagacinu>> _komercijalnoRobaUMagacinu50 { get; set; } = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(50);
        private Task<List<Komercijalno.Tarife>> _tarife { get; set; } = Task.Run(() => { return Komercijalno.Tarife.List(); });
        private Task<fm_Help> _help { get; set; }

        public fm_Izvestaj_Prodaja_Roba_Setup()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(170000))
            {
                TDOffice.Pravo.NematePravoObavestenje(170000);
                this.Close();
                return;
            }

            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Prodaja_Roba_Setup);
            status_lbl.Text = "";

            _help = this.InitializeHelpModulAsync(Modul.fm_Izvestaj_Prodaja_Roba_Setup);
            #region GodineDGV
            DataTable dt = new DataTable();
            dt.Columns.Add("Godina", typeof(int));
            dt.Columns.Add("KoristiSe", typeof(bool));
            dt.Columns.Add("OdDatuma", typeof(DateTime));
            dt.Columns.Add("DoDatuma", typeof(DateTime));

            foreach(int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
            {
                DataRow dr = dt.NewRow();
                dr["Godina"] = godina;
                dr["KoristiSe"] = false;
                dr["OdDatuma"] = new DateTime(godina, 1, 1);
                dr["DoDatuma"] = new DateTime(godina, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day);
                dt.Rows.Add(dr);
            }

            godine_dgv.DataSource = dt;
            godine_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            godine_dgv.Sort(godine_dgv.Columns["Godina"], ListSortDirection.Descending);
            #endregion
            godine_panel.DesniKlik_DatumRangeDGV(null);
        }
        private void fm_Izvestaj_Prodaja_Roba_Setup_Load(object sender, EventArgs e)
        {
            NamestiMagacineCLBAsync();
        }
        private Task NamestiMagacineCLBAsync()
        {
            return Task.Run(() =>
            {
                _komercijalnoMagacini.Wait();

                this.Invoke((MethodInvoker)delegate
                {
                    magacini_clb.DataSource = _komercijalnoMagacini.Result;
                    magacini_clb.DisplayMember = "Naziv";
                    magacini_clb.ValueMember = "ID";
                });
            });
        }

        private void prikaziIzvestaj_btn_Click(object sender, EventArgs e)
        {
            prikaziIzvestaj_btn.Enabled = false;

            if (realneCene_rb.Checked)
                UictajPodatkeRealneCene();
            else
                UcitajPodatkePoslednjeCene();
        }

        private void UictajPodatkeRealneCene()
        {
            Task.Run(() =>
            {
                this.Invoke((MethodInvoker) delegate
                {
                    status_lbl.Text = "Ucitavanje...";
                    splitContainer1.Enabled = false;
                    splitContainer2.Enabled = false;
                });
                Models.Periodi periodi = new Models.Periodi();
                foreach (DataGridViewRow row in godine_dgv.Rows)
                    if (Convert.ToBoolean(row.Cells["KoristiSe"].Value))
                        periodi[Convert.ToInt32(row.Cells["Godina"].Value)] = new Models.Periodi.Period(Convert.ToDateTime(row.Cells["OdDatuma"].Value), Convert.ToDateTime(row.Cells["DoDatuma"].Value));

                List<int> magacini = magacini_clb.CheckedItems.OfType<Komercijalno.Magacin>().Select(x => x.ID).ToList();

                if (periodi.Count == 0)
                {
                    MessageBox.Show("Morate izabrati barem jednu godinu!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true ;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                if (magacini.Count == 0)
                {
                    MessageBox.Show("Morate izabrati barem jedan magacin!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                if (_listaRobe.Count > 0 && !listaRobe_cb.Checked)
                {
                    if (MessageBox.Show("Lista robe ima stavke ali nije cekirano da se koristi lista. Da li zelite nastaviti?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            prikaziIzvestaj_btn.Enabled = true;
                            splitContainer1.Enabled = true;
                            splitContainer2.Enabled = true;
                            status_lbl.Text = "";
                        });
                        return;
                    }
                }

                if (_listaRobe.Count == 0 && listaRobe_cb.Checked)
                {
                    MessageBox.Show("Obelezili ste da koristite listu robe ali ona je prazna!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                DataTable dt = Models.Izvestaj.ProdajaRobePoRealnimCenama(periodi, magacini, listaRobe_cb.Checked ? _listaRobe : null);

                this.Invoke((MethodInvoker)delegate
                {
                    status_lbl.Text = $"";
                    splitContainer1.Enabled = true;
                    splitContainer2.Enabled = true;
                });
                this.Invoke((MethodInvoker)delegate
                {
                    using (fm_Izvestaj_Prodaja_Roba_Index ind = new fm_Izvestaj_Prodaja_Roba_Index(dt, periodi, "RSD", _listaRobe, magacini))
                    {
                        /// Checkpoint koristim da bih znao da je korisnik izvrsio zdatak
                        /// Ako se zadrzao duze od 15 sekundi onda prihvatam kao da je izvrsio zadatak
                        /// U suprotnom ne prihvatam zadatak
                        DateTime checkPoint = DateTime.Now;
                        ind.ShowDialog();
                        Task.Run(() =>
                        {
                            if ((DateTime.Now - checkPoint).TotalSeconds > 15)
                            {
                                List<TDOffice.CheckListItem> korisnikZadaci = TDOffice.CheckListItem.ListByKorisnikID(Program.TrenutniKorisnik.ID);
                                TDOffice.CheckListItem item = korisnikZadaci.FirstOrDefault(x => x.Job == TDOffice.CheckList.Jobs.IzvestajProdajeRobe);
                                if (item != null)
                                {
                                    item.DatumIzvrsenja = checkPoint;
                                    item.Update();
                                }
                            }
                        });
                    }
                    prikaziIzvestaj_btn.Enabled = true;
                });
            });
        }
        private void UcitajPodatkePoslednjeCene()
        {
            Task.Run(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    status_lbl.Text = "Ucitavanje...";
                    splitContainer1.Enabled = false;
                    splitContainer2.Enabled = false;
                });
                Models.Periodi periodi = new Models.Periodi();
                foreach (DataGridViewRow row in godine_dgv.Rows)
                    if (Convert.ToBoolean(row.Cells["KoristiSe"].Value))
                        periodi[Convert.ToInt32(row.Cells["Godina"].Value)] = new Models.Periodi.Period(Convert.ToDateTime(row.Cells["OdDatuma"].Value), Convert.ToDateTime(row.Cells["DoDatuma"].Value));

                List<int> magacini = magacini_clb.CheckedItems.OfType<Komercijalno.Magacin>().Select(x => x.ID).ToList();

                if (periodi.Count == 0)
                {
                    MessageBox.Show("Morate izabrati barem jednu godinu!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                if (magacini.Count == 0)
                {
                    MessageBox.Show("Morate izabrati barem jedan magacin!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                if (_listaRobe.Count > 0 && !listaRobe_cb.Checked)
                {
                    if (MessageBox.Show("Lista robe ima stavke ali nije cekirano da se koristi lista. Da li zelite nastaviti?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            prikaziIzvestaj_btn.Enabled = true;
                            splitContainer1.Enabled = true;
                            splitContainer2.Enabled = true;
                            status_lbl.Text = "";
                        });
                        return;
                    }
                }

                if (_listaRobe.Count == 0 && listaRobe_cb.Checked)
                {
                    MessageBox.Show("Obelezili ste da koristite listu robe ali ona je prazna!");
                    this.Invoke((MethodInvoker)delegate
                    {
                        prikaziIzvestaj_btn.Enabled = true;
                        splitContainer1.Enabled = true;
                        splitContainer2.Enabled = true;
                        status_lbl.Text = "";
                    });
                    return;
                }

                DataTable dt = Models.Izvestaj.ProdajaRobePoPoslednjimCenama(periodi, magacini, listaRobe_cb.Checked ? _listaRobe : null);
                //DataTable reperni = Models.Izvestaj.ProdajaRobePoPoslednjimCenama(periodi, new List<int>() { 12, 13 }, listaRobe_cb.Checked ? _listaRobe : null);
                //foreach (DataRow row in reperni.Rows)
                    //row["MAGACINID"] = -8;

                //DataTable finalReperni = reperni.AsEnumerable().GroupBy(x => x.Field<int>("MagacinID")).Select(x => x.First()).CopyToDataTable();

                // -8       39      GIPS
                // -8       49      GIPS
                // -8       39      LEPAK
                // -8       49      LEPAK

                this.Invoke((MethodInvoker)delegate
                {
                    status_lbl.Text = $"";
                    splitContainer1.Enabled = true;
                    splitContainer2.Enabled = true;
                });
                this.Invoke((MethodInvoker)delegate
                {
                    using (fm_Izvestaj_Prodaja_Roba_Index ind = new fm_Izvestaj_Prodaja_Roba_Index(dt, periodi, "PPV",_listaRobe, magacini))
                    {
                        /// Checkpoint koristim da bih znao da je korisnik izvrsio zdatak
                        /// Ako se zadrzao duze od 15 sekundi onda prihvatam kao da je izvrsio zadatak
                        /// U suprotnom ne prihvatam zadatak
                        DateTime checkPoint = DateTime.Now;
                        ind.ShowDialog();
                        Task.Run(() =>
                        {
                            if ((DateTime.Now - checkPoint).TotalSeconds > 15)
                            {
                                List<TDOffice.CheckListItem> korisnikZadaci = TDOffice.CheckListItem.ListByKorisnikID(Program.TrenutniKorisnik.ID);
                                TDOffice.CheckListItem item = korisnikZadaci.FirstOrDefault(x => x.Job == TDOffice.CheckList.Jobs.IzvestajProdajeRobe);
                                if (item != null)
                                {
                                    item.DatumIzvrsenja = checkPoint;
                                    item.Update();
                                }
                            }
                        });
                    }
                    prikaziIzvestaj_btn.Enabled = true;
                });
            });
        }


        private void godine_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // determine if click was on our date column
            if (godine_dgv.Columns[e.ColumnIndex].DataPropertyName == "OdDatuma" || godine_dgv.Columns[e.ColumnIndex].DataPropertyName == "DoDatuma")
            {
                // initialize DateTimePicker
                dtp = new DateTimePicker();
                dtp.Format = DateTimePickerFormat.Short;
                dtp.Visible = true;
                dtp.Value = DateTime.Parse(godine_dgv.CurrentCell.Value.ToString());

                // set size and location
                var rect = godine_dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(rect.Width, rect.Height);
                dtp.Location = new Point(rect.X, rect.Y);

                // attach events
                dtp.CloseUp += new EventHandler(dtp_CloseUp);
                dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                godine_dgv.Controls.Add(dtp);
                _initializedtp = true;
            }
        }
        // on text change of dtp, assign back to cell
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            godine_dgv.CurrentCell.Value = dtp.Text.ToString();
        }
        // on close of cell, hide dtp
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        private void listaRobe_btn_Click(object sender, EventArgs e)
        {
            using (fm_ListaRobe lr = new fm_ListaRobe())
            {
                lr.robaUListi = _listaRobe;
                lr.ShowDialog();
                _listaRobe = new List<int>(lr.robaUListi);

                DataTable dt = new DataTable();
                dt.Columns.Add("Proizvod");
                
                foreach(int r in _listaRobe)
                {
                    DataRow dr = dt.NewRow();
                    dr["Proizvod"] = _komercijalnoRoba.Result.FirstOrDefault(x => x.ID == r).Naziv;
                    dt.Rows.Add(dr);
                }
                roba_dgv.DataSource = dt;

                if (dt.Rows.Count > 0)
                    listaRobe_cb.Checked = true;

                roba_dgv.Columns["Proizvod"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < magacini_clb.Items.Count; i++)
                magacini_clb.SetItemChecked(i, true);
        }

        private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < magacini_clb.Items.Count; i++)
                magacini_clb.SetItemChecked(i, false);
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _help.Result.ShowDialog();
        }

        private void godine_dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!_initializedtp)
                return;
            dtp.Visible = false;
        }
    }
}
