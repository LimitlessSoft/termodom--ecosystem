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

namespace TDOffice_v2.Forms.MC
{
    public partial class fm_mc_NabavkaRobe_UporediCenovnike_Index : Form
    {
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

                var ppids = new List<int>();
                uporediCenovnikeResponse.Payload.ForEach((item) =>
                {
                    ppids.AddRange(item.SubItems.Select(x => x.DobavljacPPID).Distinct());
                });

                ppids = ppids.Distinct().ToList();

                var dt = new DataTable();
                dt.Columns.Add("RobaId", typeof(int));
                dt.Columns.Add("KatBr", typeof(string));
                dt.Columns.Add("Naziv", typeof(string));

                foreach (var ppid in ppids)
                {
                    dt.Columns.Add($"Dobavljac ({ppid}) Kat Br", typeof(string));
                    dt.Columns.Add($"VP Cena sa popustom: {ppid}", typeof(double));
                }

                foreach (var item in uporediCenovnikeResponse.Payload)
                {
                    var dr = dt.NewRow();
                    dr["RobaId"] = item.RobaId;
                    dr["KatBr"] = string.IsNullOrWhiteSpace(item.KatBr) ? "Undefined" : item.KatBr;
                    dr["Naziv"] = string.IsNullOrWhiteSpace(item.Naziv) ? "Undefined" : item.Naziv;
                    foreach (var ppid in ppids)
                    {
                        var c = item.SubItems.FirstOrDefault(x => x.DobavljacPPID == ppid);
                        dr[$"Dobavljac ({ppid}) Kat Br"] = c == null ? "None" : c.DobavljacKatBr;
                        dr[$"VP Cena sa popustom: {ppid}"] = c == null ? 0 : c.VPCenaSaPopustom;
                    }
                    dt.Rows.Add(dr);
                }

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    foreach (var ppid in ppids)
                        dataGridView1.Columns[$"VP Cena sa popustom: {ppid}"].DefaultCellStyle.Format = "#,##0.00 RSD";
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
