using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2.Forms
{
    public partial class fm_WebUredjivanjeProizvoda : Form
    {
        private static System.Drawing.Color WebInfoBoja = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
        private static System.Drawing.Color KomercijalnoInfoBoja = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private class ProizvodGetDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ThumbnailImagePath { get; set; }
            public string FullSizedImagePath { get; set; }
            /// <summary>
            /// Kataloski broj
            /// </summary>
            public string Sku { get; set; }
            /// <summary>
            /// Jedinica mere
            /// </summary>
            public string Unit { get; set; }
            public bool IsActive { get; set; }
        }

        public fm_WebUredjivanjeProizvoda()
        {
            InitializeComponent();
        }

        private void fm_WebUredjivanjeProizvoda_Load(object sender, EventArgs e)
        {
            LoadDataAsync();
        }

        private Task LoadDataAsync()
        {
            this.Enabled = false;
            return Task.Run(async () =>
            {
                var webProizvodiTask = TDWebApi.GetAsync<List<ProizvodGetDto>>("/products");

                var webProizvodi = await webProizvodiTask;
                if (webProizvodi.Status != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show($"Greska u komunikaciji sa API serverom! [{webProizvodi.Status.ToString()}]");
                    return;
                }

                var dt = new DataTable();
                dt.Columns.Add("RobaId", typeof(int));

                dt.Columns.Add("KatBr Web", typeof(string));
                dt.Columns.Add("Naziv Web", typeof(string));
                dt.Columns.Add("JM Web", typeof(string));

                dt.Columns.Add("KatBr Komercijalno", typeof(string));
                dt.Columns.Add("Naziv Komercijalno", typeof(string));
                dt.Columns.Add("JM Komercijalno", typeof(string));

                foreach (var proizvod in webProizvodi.Payload)
                {
                    var dr = dt.NewRow();
                    dr["RobaId"] = proizvod.Id;
                    dr["Naziv Web"] = proizvod.Name;
                    dr["KatBr Web"] = proizvod.Sku;
                    dr["Jm Web"] = proizvod.Unit;
                    dt.Rows.Add(dr);
                }

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    dataGridView1.Columns["RobaId"].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;

                    dataGridView1.Columns["KatBr Web"].DefaultCellStyle.BackColor = WebInfoBoja;
                    dataGridView1.Columns["Naziv Web"].DefaultCellStyle.BackColor = WebInfoBoja;
                    dataGridView1.Columns["JM Web"].DefaultCellStyle.BackColor = WebInfoBoja;

                    this.Enabled = true;
                });
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataAsync();
        }
    }
}
