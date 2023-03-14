using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TDOffice_v2
{
    public partial class fm_LagerListaStavki_Index : Form
    {
        private int __magacinID { get; set; }
        private int[] __robaIDs { get; set; }
        private int _magacinID
        {
            get { return __magacinID; }
            set
            {
                UpdateUI();
                __magacinID = value;
            }
        }
        private int[] _robaIDs { get; set; }
        private static Task<List<Komercijalno.Roba>> _roba { get; set; }

        public fm_LagerListaStavki_Index(int[] robaIDs, int magacinID)
        {
            InitializeComponent();
            _robaIDs = robaIDs;
            _magacinID = magacinID;
        }

        private void fm_LagerListaStavki_Index_Load(object sender, EventArgs e)
        {
            UcitajRobu_Begin();
            UcitajStavkeAsync();
        }

        public void PozicionirajNaRobu(int robaID)
        {
            dataGridView1.ClearSelection();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["RobaID"].Value) == robaID)
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                        row.Cells["RobaID"].Selected = true;
                        dataGridView1.CurrentCell = row.Cells["RobaID"];
                    });
                    return;
                }
            }
        }
        private void UpdateUI()
        {
            if (!this.Visible)
                return;

            this.Invoke((MethodInvoker) delegate
            {
                this.Text = $"Stanje robe po godinama za magacin {_magacinID}";
            });
        }
        private void UcitajRobu_Begin()
        {
            _roba = Task.Run(() =>
            {
                return Komercijalno.Roba.List(DateTime.Now.Year).Where(x => _robaIDs.Contains(x.ID)).ToList();
            });
        }
        private Task UcitajStavkeAsync()
        {
            return Task.Run(() =>
            {
                List<int> godine = Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderByDescending(x => x).Take(5).ToList();
                var doks = Komercijalno.Dokument.List($"VRDOK = 0 AND MAGACINID = {_magacinID}");
                if(doks == null || doks.Count == 0)
                {
                    MessageBox.Show($"Greska prilikom ucitavanja pocetnog stanja za dati magacin {_magacinID}");
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("RobaID", typeof(int));
                dt.Columns.Add("Proizvod", typeof(string));

                foreach(int god in godine.OrderBy(x => x))
                {
                    dt.Columns.Add($"Pocetak {god} (kol)", typeof(double));
                    dt.Columns.Add($"Kraj {god} (kol)", typeof(double));
                }

                foreach (int god in godine.OrderBy(x => x))
                {
                    dt.Columns.Add($"Pocetak {god} (vr)", typeof(double));
                    dt.Columns.Add($"Kraj {god} (vr)", typeof(double));
                }

                Dictionary<int, List<Komercijalno.Stavka>> stavkePocetnihStanja = new Dictionary<int, List<Komercijalno.Stavka>>();
                Dictionary<int, List<Komercijalno.RobaUMagacinu>> krajnjeStanjeStavki = new Dictionary<int, List<Komercijalno.RobaUMagacinu>>();

                Parallel.ForEach(godine, god =>
                {
                    using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[god]))
                    {
                        con.Open();
                        krajnjeStanjeStavki.Add(god, Komercijalno.RobaUMagacinu.ListByMagacinID(con, _magacinID));
                        stavkePocetnihStanja.Add(god, Komercijalno.Stavka.List(con,
                            $"VRDOK = 0 AND BRDOK = {doks[0].BrDok} AND ROBAID IN ({string.Join(", ", _robaIDs)})"));
                    }
                });

                foreach (int robaid in _robaIDs)
                {
                    Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == robaid);
                    DataRow dr = dt.NewRow();
                    dr["RobaID"] = robaid;
                    dr["Proizvod"] = r == null ? "Unknown" : r.Naziv;
                    foreach (int god in godine)
                    {
                        Komercijalno.Stavka stavka = stavkePocetnihStanja[god].FirstOrDefault(x => x.RobaID == robaid);
                        Komercijalno.RobaUMagacinu rum = krajnjeStanjeStavki[god].FirstOrDefault(x => x.RobaID == robaid);
                        dr[$"Pocetak {god} (kol)"] = stavka == null ? 0 : stavka.Kolicina;
                        dr[$"Kraj {god} (kol)"] = rum == null ? 0 : rum.Stanje;
                        dr[$"Pocetak {god} (vr)"] = stavka == null ? 0 : stavka.Kolicina * stavka.ProdajnaCena;
                        dr[$"Kraj {god} (vr)"] = rum == null ? 0 : rum.Stanje * rum.ProdajnaCena;
                    }
                    dt.Rows.Add(dr);
                }


                this.Invoke((MethodInvoker) delegate
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                    foreach (int god in godine)
                    {
                        dataGridView1.Columns[$"Pocetak {god} (kol)"].DefaultCellStyle.Format = "#,##0";
                        dataGridView1.Columns[$"Pocetak {god} (kol)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns[$"Kraj {god} (kol)"].DefaultCellStyle.Format = "#,##0";
                        dataGridView1.Columns[$"Kraj {god} (kol)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns[$"Pocetak {god} (vr)"].DefaultCellStyle.Format = "#,##0";
                        dataGridView1.Columns[$"Pocetak {god} (vr)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns[$"Kraj {god} (vr)"].DefaultCellStyle.Format = "#,##0";
                        dataGridView1.Columns[$"Kraj {god} (vr)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    groupBox1.Enabled = true;
                    label1.Text = "";
                });

                return;
            });
        }

        private void ObojiRazlike()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int midIndex = row.Cells.Count - ((row.Cells.Count - 2) / 2);
                for (int i = 2; i < row.Cells.Count; i++)
                {
                    if (!dataGridView1.Columns[i].Visible)
                        continue;

                    if (i % 2 == 0 || i + 1 == midIndex || i + 1 >= row.Cells.Count)
                        continue;

                    if (Convert.ToDouble(row.Cells[i].Value) != Convert.ToDouble(row.Cells[i + 1].Value))
                        row.Cells[i].Style.ForeColor = Color.Red;
                    else
                        row.Cells[i].Style.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                }
            }
        }

        private void fm_LagerListaStavki_Index_Shown(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void kolicinski_cb_CheckedChanged(object sender, EventArgs e)
        {
            PrimeniFilter();
        }

        private void vrednosno_cb_CheckedChanged(object sender, EventArgs e)
        {
            PrimeniFilter();
        }
        
        private void PrimeniFilter()
        {
            foreach(DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name.ToLower().Contains("kol"))
                    col.Visible = kolicinski_cb.Checked;
                else if (col.Name.ToLower().Contains("vr"))
                    col.Visible = vrednosno_cb.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int midIndex = row.Cells.Count - ((row.Cells.Count - 2) / 2);
                bool sakrij = true;
                for (int i = 2; i < row.Cells.Count; i++)
                {
                    if (!dataGridView1.Columns[i].Visible)
                        continue;

                    if (i % 2 == 0 || i + 1 == midIndex || i + 1 >= row.Cells.Count)
                        continue;

                    if (Convert.ToDouble(row.Cells[i].Value) != Convert.ToDouble(row.Cells[i + 1].Value))
                    {
                        sakrij = false;
                        break;
                    }
                }
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                currencyManager1.SuspendBinding();
                row.Visible = !sakrij;
                currencyManager1.ResumeBinding();
            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiRazlike();
        }
    }
}
