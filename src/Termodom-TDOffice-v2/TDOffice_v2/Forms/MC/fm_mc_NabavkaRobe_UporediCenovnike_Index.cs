using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.TDOffice_v2;
using static TDOffice_v2.Forms.MC.fm_mc_NabavkaRobe_Index;

namespace TDOffice_v2.Forms.MC
{
    public partial class fm_mc_NabavkaRobe_UporediCenovnike_Index : Form
    {
        private DataTable baseDataTable = null;
        private DataTable dataGridViewDataTable = null;
        private Task<List<Komercijalno.Partner>> partneriTask = Komercijalno.Partner.ListAsync(DateTime.UtcNow.Year);
        private List<int> ppidsInCenovnici = new List<int>();
        private readonly TDOffice.Config<NabavkaRobeDobavljaciSettings> _dobavljaciSettings = TDOffice.Config<NabavkaRobeDobavljaciSettings>.Get(TDOffice.ConfigParameter.NabavkaRobeDobavljacCenovnikSettings);

        public fm_mc_NabavkaRobe_UporediCenovnike_Index()
        {
            InitializeComponent();
        }

        private void fm_mc_NabavkaRobe_UporediCenovnike_Index_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            _ = LoadDataAsync().ContinueWith((prev) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Enabled = true;
                });
            });
        }

        private class MCNabavkaRobeUporediCenovnikeItemDto
        {
            public int RobaId { get; set; }
            public string KatBr { get; set; }
            public string Naziv { get; set; }
            public List<MCNabavkaRobeUporediCenovnikeSubItemDto> SubItems { get; set; } = new List<MCNabavkaRobeUporediCenovnikeSubItemDto>();
        }

        private class MCNabavkaRobeUporediCenovnikeSubItemDto
        {
            public int DobavljacPPID { get; set; }
            public double VPCenaSaPopustom { get; set; }
            public string DobavljacKatBr { get; set; }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var uporediCenovnikeResponse = await TDAPI.GetAsync<List<MCNabavkaRobeUporediCenovnikeItemDto>>("/mc-nabavka-robe-uporedi-cenovnike");
                if (uporediCenovnikeResponse.NotOk)
                {
                    MessageBox.Show("Doslo je do greske!");
                    return;
                }

                ppidsInCenovnici.Clear();
                uporediCenovnikeResponse.Payload.ForEach((item) =>
                {
                    ppidsInCenovnici.AddRange(item.SubItems.Select(x => x.DobavljacPPID).Distinct());
                });

                ppidsInCenovnici = ppidsInCenovnici.Distinct().ToList();

                var dt = new DataTable();
                dt.Columns.Add("RobaId", typeof(int));
                dt.Columns.Add("KatBr", typeof(string));
                dt.Columns.Add("Naziv", typeof(string));
                dt.Columns.Add("JM", typeof(string));

                foreach (var ppid in ppidsInCenovnici)
                {
                    var partner = (await partneriTask).FirstOrDefault(x => x.PPID == ppid);
                    dt.Columns.Add($"Dobavljac ({partner?.Naziv ?? ppid.ToString()}) Kat Br", typeof(string));
                    dt.Columns.Add($"VP Cena sa popustom: {partner?.Naziv ?? ppid.ToString()}", typeof(double));
                }

                foreach (var item in uporediCenovnikeResponse.Payload)
                {
                    var dr = dt.NewRow();
                    dr["RobaId"] = item.RobaId;
                    dr["KatBr"] = string.IsNullOrWhiteSpace(item.KatBr) ? "Undefined" : item.KatBr;
                    dr["Naziv"] = string.IsNullOrWhiteSpace(item.Naziv) ? "Undefined" : item.Naziv;
                    dr["JM"] = "JM";
                    foreach (var ppid in ppidsInCenovnici)
                    {
                        var partner = (await partneriTask).FirstOrDefault(x => x.PPID == ppid);
                        var c = item.SubItems.FirstOrDefault(x => x.DobavljacPPID == ppid);
                        dr[$"Dobavljac ({partner?.Naziv ?? ppid.ToString()}) Kat Br"] = c == null ? "None" : c.DobavljacKatBr;

                        var vpCena = c.VPCenaSaPopustom;
                        var razmeraItem = _dobavljaciSettings.Tag.Dobavljaci
                            .FirstOrDefault(x => x.PPID == ppid)
                            .JMs
                            .FirstOrDefault(x => x.RobaId == item.RobaId);

                        if(razmeraItem != null)
                        {
                            var razmera = razmeraItem.LocalKolicina / razmeraItem.DobavljacKolicina;
                            vpCena *= razmera;
                        }

                        dr[$"VP Cena sa popustom: {partner?.Naziv ?? ppid.ToString()}"] = c == null ? 0 : vpCena;
                    }
                    dt.Rows.Add(dr);
                }
                baseDataTable = dt;
                dataGridViewDataTable = dt;

                this.Invoke((MethodInvoker)delegate
                {
                    UpdateDGV();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ObojiJeftinije()
        {
            var cols = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (!col.Name.Contains("VP Cena sa popustom"))
                    continue;

                cols.Add(col);
            }

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                var min = Double.MaxValue;
                DataGridViewColumn minCol = cols[0];
                foreach (var col in cols)
                {
                    var curr = Convert.ToDouble(dgvr.Cells[col.Index].Value);
                    if (curr != 0 && curr < min)
                    {
                        min = curr;
                        minCol = col;
                    }
                }

                dgvr.Cells[minCol.Index - 1].Style.BackColor = Color.Green;
                dgvr.Cells[minCol.Index].Style.BackColor = Color.Green;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                FilterEnter();
                dataGridView1.Focus();
            }
        }
        private void FilterCtrlA()
        {
            string selectString = "";
            string input = textBox1.Text;
            string[] inputElemets = input.Split('+');

            foreach (object o in comboBox1.Items)
            {
                for (int i = 0; i < inputElemets.Length; i++)
                    selectString += "CONVERT(" + o.ToString() + ", System.String) LIKE '%" + inputElemets[i] + "%' AND ";

                selectString = selectString.Remove(selectString.Length - 4);
                selectString += " OR ";
            }

            selectString = selectString.Remove(selectString.Length - 4);

            DataRow[] rows = baseDataTable.Copy().Select(selectString);
            dataGridViewDataTable = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

            UpdateDGV();
        }
        private void FilterEnter()
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Morate izabrati kolonu!");
                return;
            }
            dataGridView1.ClearSelection();
            string kolona = comboBox1.SelectedItem.ToString();
            string input = textBox1.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Naziv"];
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string vrednostCelije = row.Cells[kolona].Value.ToString();
                if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index > 0 ? row.Index - 1 : 0;
                    dataGridView1.Rows[row.Index].Selected = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells["Naziv"];
                    return;
                }
            }
        }

        private void UpdateDGV()
        {
            dataGridView1.DataSource = dataGridViewDataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            foreach (var ppid in ppidsInCenovnici)
            {
                var partner = partneriTask.GetAwaiter().GetResult().FirstOrDefault(x => x.PPID == ppid);
                dataGridView1.Columns[$"VP Cena sa popustom: {partner?.Naziv ?? ppid.ToString()}"].DefaultCellStyle.Format = "#,##0.00 RSD";
            }

            ObojiJeftinije();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilterCtrlA();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiJeftinije();
        }
    }
}
