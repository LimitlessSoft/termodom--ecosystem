using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
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

                foreach(Termodom.Data.Entities.Komercijalno.Magacin m in (await _komercijalnoMagacini).Values)
                    list.Add(new Tuple<int, string>(m.ID, m.Naziv));

                this.Invoke((MethodInvoker) delegate
                {
                    clb_Magacini.DataSource = list;
                    clb_Magacini.DisplayMember = "Item2";
                    clb_Magacini.ValueMember = "Item1";
                });
            });
            _loaded= true;
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
                    this.Invoke((MethodInvoker) delegate
                    {
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
                    dt.Columns.Add("Potrazuje", typeof(double));
                    dt.Columns.Add("MagacinId", typeof(int));
                    dt.Columns.Add("Datum", typeof(DateTime));
                    dt.Columns.Add("Mp Racuni", typeof(double));
                    dt.Columns.Add("Povratnice", typeof(double));
                    dt.Columns.Add("Za Uplatu (Mp. Racuni - Povratnice)", typeof(double));
                    dt.Columns.Add("Razlika", typeof(double));

                    foreach (Tuple<int, string> tup in clb_Magacini.CheckedItems)
                    {
                        Termodom.Data.Entities.Komercijalno.Magacin magacin = magacini[tup.Item1];

                        DateTime odDatuma = odDatuma_dtp.Value;
                        DateTime datumObrade = odDatuma;
                        DateTime doDatuma = doDatuma_dtp.Value;

                        List<int> godine = new List<int>();
                        for(int i = odDatuma.Year; i <= doDatuma.Year; i++)
                            godine.Add(i);

                        foreach(int godina in godine)
                        {
                            IzvodDictionary izvodi = await IzvodManager.DictionaryAsync(magacin.ID, godina);
                            DokumentDictionary dokumenti = await DokumentManager.DictionaryAsync(magacin.ID, godina, new int[] { 15, 22 },new int[] { magacin.ID }, odDatuma, doDatuma);

                            while (datumObrade <= doDatuma)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    toolStripStatusLabel1.Text = $"Ucitavanje {datumObrade.ToString("dd.MM.yyyy")}";
                                });
                                string konto = "Bez Uplata";
                                string pozNaBroj = "";
                                double potrazuje = 0;
                                double mpRacuni = 0;
                                double povratnice = 0;
                                var izvodiNaDan = izvodi.Values.Where(x =>
                                    !string.IsNullOrEmpty(x.Konto) &&
                                    (x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240") &&
                                    !string.IsNullOrWhiteSpace(x.PozNaBroj) &&
                                    x.PozNaBroj.Length == 6 &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(0, 2)) == datumObrade.Month &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(2, 2)) == datumObrade.Day &&
                                    Convert.ToInt32(x.PozNaBroj.Substring(4, 2)) == magacin.ID);
                                foreach (Izvod i in izvodiNaDan)
                                {
                                    konto = i.Konto;
                                    pozNaBroj = i.PozNaBroj;
                                    potrazuje += i.Potrazuje;
                                }

                                mpRacuni = !dokumenti.ContainsKey(15) ? 0 : dokumenti[15].Values.Where(x =>
                                    x.Datum.Date == datumObrade.Date &&
                                    x.MagacinID == magacin.ID &&
                                    x.NUID != 1).Sum(x => x.Potrazuje);

                                povratnice = !dokumenti.ContainsKey(22) ? 0 : dokumenti[22].Values.Where(x =>
                                    x.Datum.Date == datumObrade.Date &&
                                    x.MagacinID == magacin.ID).Sum(x => x.Potrazuje);


                                DataRow dr = dt.NewRow();
                                dr["Konto"] = konto;
                                dr["PozNaBroj"] = pozNaBroj;
                                dr["Potrazuje"] = potrazuje;
                                dr["MagacinId"] = magacin.ID;
                                dr["Datum"] = datumObrade;
                                dr["Mp Racuni"] = mpRacuni;
                                dr["Povratnice"] = povratnice;
                                dr["Za Uplatu (Mp. Racuni - Povratnice)"] = mpRacuni - povratnice;
                                dr["Razlika"] = (potrazuje - mpRacuni - povratnice);
                                dt.Rows.Add(dr);

                                datumObrade = datumObrade.AddDays(1);
                            }
                        }
                    }

                    this.Invoke((MethodInvoker) delegate
                    {
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["Konto"].Value.ToString() == "Bez Uplata" &&
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["Mp Racuni"].Value) == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                }
                else if (Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells["Razlika"].Value)) > tolerancija)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                }
            }
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
    }
}
