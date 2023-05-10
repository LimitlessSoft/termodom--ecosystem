using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    // Help
    // Desni klik na CheckListBox selektuj/deselektuj sve
    public partial class fm_ObracunIUplataPazara : Form
    {
        private Task<MagacinDictionary> _komercijalnoMagacini { get; set; } = Komercijalno.Magacin.DictionaryAsync();

        private Task<fm_Help> _helpFrom { get; set; }

        private bool _loaded = false;
        public fm_ObracunIUplataPazara()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.fm_ObracunIUplataPazara);
            panel1.DesniKlik_DatumRange();

        }

        private void fm_ObracunIUplataPazara_Load(object sender, EventArgs e)
        {
            _komercijalnoMagacini.ContinueWith(async (prev) =>
            {
                List<Tuple<int, string>> list = new List<Tuple<int, string>>();

                foreach (Termodom.Data.Entities.Komercijalno.Magacin m in (await _komercijalnoMagacini).Values)
                    list.Add(new Tuple<int, string>(m.ID, m.Naziv));

                this.Invoke((MethodInvoker)delegate
                {
                    clb_Magacini.DataSource = list;
                    clb_Magacini.DisplayMember = "Item2";
                    clb_Magacini.ValueMember = "Item1";
                });
            });
            _loaded = true;
        }

        private void btn_Prikazi_Click(object sender, EventArgs e)
        {
            if (clb_Magacini.CheckedItems.Count == 0)
            {
                MessageBox.Show("Niste selektovali MAGACIN!!!");
                return;
            }
            Task.Run(async () =>
            {
                try
                {
                    var tolerancija = 0;
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (!int.TryParse(tb_tolerancija.Text, out tolerancija))
                        {
                            MessageBox.Show("Neispravna tolerancija!");
                            return;
                        }
                        doDatuma_dtp.Enabled = false;
                        odDatuma_dtp.Enabled = false;
                        btn_Prikazi.Enabled = false;
                        clb_Magacini.Enabled = false;
                        dataGridView1.Enabled = false;
                        toolStripStatusLabel1.Text = "Ucitavanje...";
                    });
                    MagacinDictionary magacini = await _komercijalnoMagacini;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Konto", typeof(string));
                    dt.Columns.Add("PozNaBroj", typeof(string));
                    dt.Columns.Add("MagacinId", typeof(int));
                    dt.Columns.Add("Datum", typeof(DateTime));
                    dt.Columns.Add("Mp Racuni", typeof(double));
                    dt.Columns.Add("Povratnice", typeof(double));
                    dt.Columns.Add("Za Uplatu (Mp. Racuni - Povratnice)", typeof(double));
                    dt.Columns.Add("Potrazuje", typeof(double));
                    dt.Columns.Add("Razlika", typeof(double));

                    var ukupnaRazlika = 0d;
                    foreach (Tuple<int, string> tup in clb_Magacini.CheckedItems)
                    {
                        Termodom.Data.Entities.Komercijalno.Magacin magacin = magacini[tup.Item1];

                        DateTime odDatuma = odDatuma_dtp.Value;
                        DateTime datumObrade = odDatuma;
                        DateTime doDatuma = doDatuma_dtp.Value;

                        List<int> godine = new List<int>();
                        for (int i = odDatuma.Year; i <= doDatuma.Year; i++)
                            godine.Add(i);

                        foreach (int godina in godine)
                        {
                            IzvodDictionary izvodi = await IzvodManager.DictionaryAsync(magacin.ID, godina);
                            DokumentDictionary dokumenti = await DokumentManager.DictionaryAsync(magacin.ID, godina, new int[] { 15, 22 }, new int[] { magacin.ID }, odDatuma, doDatuma);

                            while (datumObrade <= doDatuma)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    toolStripStatusLabel1.Text = $"Ucitavanje {datumObrade.ToString("dd.MM.yyyy")}";
                                });
                                //string konto = "Bez Uplata";
                                //string pozNaBroj = "";
                                //double potrazuje = 0;
                                //double mpRacuni = 0;
                                //double povratnice = 0;
                                //var izvodiNaDan = izvodi.Values.Where(x =>
                                //    !string.IsNullOrEmpty(x.Konto) &&
                                //    (x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240") &&
                                //    !string.IsNullOrWhiteSpace(x.PozNaBroj) &&
                                //    x.PozNaBroj.Length == 7 &&
                                //    Convert.ToInt32(x.PozNaBroj.Substring(0, 2)) == datumObrade.Month &&
                                //    Convert.ToInt32(x.PozNaBroj.Substring(2, 2)) == datumObrade.Day &&
                                //    Convert.ToInt32(x.PozNaBroj.Substring(4, 2)) == magacin.ID);
                                //foreach (Izvod i in izvodiNaDan)
                                //{
                                //    konto = i.Konto;
                                //    pozNaBroj = i.PozNaBroj;
                                //    potrazuje += i.Potrazuje;
                                //}

                                //mpRacuni = !dokumenti.ContainsKey(15) ? 0 : dokumenti[15].Values.Where(x =>
                                //    x.Datum.Date == datumObrade.Date &&
                                //    x.MagacinID == magacin.ID &&
                                //    x.NUID != 1).Sum(x => x.Potrazuje);

                                //povratnice = !dokumenti.ContainsKey(22) ? 0 : dokumenti[22].Values.Where(x =>
                                //    x.Datum.Date == datumObrade.Date &&
                                //    x.MagacinID == magacin.ID).Sum(x => x.Potrazuje);

                                //var razlika = (potrazuje - mpRacuni - povratnice);
                                //ukupnaRazlika += razlika;
                                //if(Math.Abs(razlika) < tolerancija)
                                //{
                                //    datumObrade = datumObrade.AddDays(1);
                                //    continue;
                                //}

                                DataRow dr = dt.NewRow();
                                //dr["Konto"] = konto;
                                //dr["PozNaBroj"] = pozNaBroj;
                                //dr["Potrazuje"] = potrazuje;
                                //dr["MagacinId"] = magacin.ID;
                                //dr["Datum"] = datumObrade;
                                //dr["Mp Racuni"] = mpRacuni;
                                //dr["Povratnice"] = povratnice;
                                //dr["Za Uplatu (Mp. Racuni - Povratnice)"] = mpRacuni - povratnice;
                                //dr["Razlika"] = razlika;
                                //dt.Rows.Add(dr);

                                //Za magacine 112, 113, ...

                                string konto_N = "Bez Uplata";
                                string pozNaBroj_N = "";
                                double potrazuje_N = 0;
                                double mpRacuni_N = 0;
                                double povratnice_N = 0;
                                var izvodiNaDan_N = izvodi.Values.Where(x =>
                                    !string.IsNullOrEmpty(x.Konto) &&
                                    (x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240") &&
                                    !string.IsNullOrWhiteSpace(x.PozNaBroj) &&
                                    x.PozNaBroj.Length == 7 &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(0, 2)) == datumObrade.Month &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(2, 2)) == datumObrade.Day &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(4, 3)) == magacin.ID);
                                foreach (Izvod i in izvodiNaDan_N)
                                {
                                    konto_N = i.Konto;
                                    pozNaBroj_N = i.PozNaBroj;
                                    potrazuje_N += i.Potrazuje;
                                }

                                mpRacuni_N = !dokumenti.ContainsKey(15) ? 0 : dokumenti[15].Values.Where(x =>
                                    x.Datum.Date == datumObrade.Date &&
                                    x.MagacinID == magacin.ID &&
                                    x.NUID != 1).Sum(x => x.Potrazuje);

                                povratnice_N = !dokumenti.ContainsKey(22) ? 0 : dokumenti[22].Values.Where(x =>
                                    x.Datum.Date == datumObrade.Date &&
                                    x.MagacinID == magacin.ID &&
                                    x.NUID != 1).Sum(x => x.Potrazuje);

                                if (izvodiNaDan_N.Count() > 0)
                                {
                                    DataRow drn = dt.NewRow();
                                    drn["Konto"] = konto_N;
                                    drn["PozNaBroj"] = pozNaBroj_N;
                                    drn["Potrazuje"] = potrazuje_N;
                                    drn["MagacinId"] = magacin.ID;
                                    drn["Datum"] = datumObrade;
                                    drn["Mp Racuni"] = mpRacuni_N;
                                    drn["Povratnice"] = povratnice_N;
                                    drn["Za Uplatu (Mp. Racuni - Povratnice)"] = mpRacuni_N - povratnice_N;
                                    drn["Razlika"] = (potrazuje_N - mpRacuni_N - povratnice_N);
                                    dt.Rows.Add(drn);

                                    ukupnaRazlika += (potrazuje_N - mpRacuni_N - povratnice_N);
                                }

                                datumObrade = datumObrade.AddDays(1);
                            }
                        }
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        ukupnaRazlika_txt.Text = ukupnaRazlika.ToString("#,##0.00 RSD");
                        dataGridView1.DataSource = dt;
                        dataGridView1.Visible = false;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns["Mp Racuni"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["Mp Racuni"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns["Povratnice"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["Povratnice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns["Za Uplatu (Mp. Racuni - Povratnice)"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["Za Uplatu (Mp. Racuni - Povratnice)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns["Razlika"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["Razlika"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Visible = true;
                        dataGridView1.Sort(dataGridView1.Columns["Datum"], ListSortDirection.Descending);
                        dataGridView1.Enabled = true;
                        doDatuma_dtp.Enabled = true;
                        odDatuma_dtp.Enabled = true;
                        btn_Prikazi.Enabled = true;
                        clb_Magacini.Enabled = true;
                        toolStripStatusLabel1.Text = $"Gotovo ucitavanje!";

                        ObojiDGV();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void ObojiDGV()
        {
            double tolerancija;
            tolerancija = Convert.ToDouble(tb_tolerancija.Text);
            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager.SuspendBinding();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["Konto"].Value.ToString() == "Bez Uplata" &&
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["Mp Racuni"].Value) == 0)
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                else
                    if (Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells["Razlika"].Value)) < tolerancija)
                        dataGridView1.Rows[i].Visible = false;
            }
            currencyManager.ResumeBinding();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiDGV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _helpFrom.Result.ShowDialog();
        }

        private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Magacini.Items.Count; i++)
                clb_Magacini.SetItemChecked(i, true);
        }

        private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Magacini.Items.Count; i++)
                clb_Magacini.SetItemChecked(i, false);
        }

        private void tb_tolerancija_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!_loaded)
                return;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private async void analitikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Morate selektovati red da bi pokrenuli analitiku nad tim redom!");
                return;
            }

            string pozNaBroj = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["PozNaBroj"].Value.ToString();
            int magacinId = Convert.ToInt32(pozNaBroj.Substring(4));

            IzvodDictionary izvodi = await IzvodManager.DictionaryAsync(magacinId, DateTime.Now.Year);


            var izvodiNaDan = izvodi.Values.Where(x =>
                !string.IsNullOrEmpty(x.Konto) &&
                (x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240") &&
                !string.IsNullOrWhiteSpace(x.PozNaBroj) &&
                x.PozNaBroj == pozNaBroj);

            DataTable dt = new DataTable();
            dt.Columns.Add("VrDok", typeof(int));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("ZiroRacun", typeof(string));
            dt.Columns.Add("Konto", typeof(string));
            dt.Columns.Add("PozNaBroj", typeof(string));
            dt.Columns.Add("MagacinId", typeof(int));
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("Potrazuje", typeof(double));
            dt.Columns.Add("Duguje", typeof(double));

            foreach (Izvod izvod in izvodiNaDan)
            {
                DataRow dr = dt.NewRow();
                dr["VrDok"] = izvod.VrDok;
                dr["BrDok"] = izvod.BrDok;
                dr["ZiroRacun"] = izvod.ZiroRacun;
                dr["Konto"] = izvod.Konto;
                dr["PoznaBroj"] = izvod.PozNaBroj;
                dr["MagacinId"] = magacinId;
                dr["Datum"] = new DateTime(
                    DateTime.Now.Year,
                    Convert.ToInt32(izvod.PozNaBroj.Substring(0, 2)),
                    Convert.ToInt32(izvod.PozNaBroj.Substring(2, 2)));
                dr["Potrazuje"] = izvod.Potrazuje;
                dr["Duguje"] = izvod.Duguje;
                dt.Rows.Add(dr);
            }

            await Task.Run(() =>
            {
                using(DataGridViewSelectBox sb = new DataGridViewSelectBox(dt))
                    sb.ShowDialog();
            });
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                dataGridView1.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ = Tasks.IspravnostIzvodStavkeTask.Run();
            MessageBox.Show("Akcija je pokrenuta, izvstaj ce Vam stici u vidu poruke!");
        }
    }
}
