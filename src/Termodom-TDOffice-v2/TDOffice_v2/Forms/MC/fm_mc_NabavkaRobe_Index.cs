using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Core.Http;

namespace TDOffice_v2.Forms.MC
{
    public partial class fm_mc_NabavkaRobe_Index : Form
    {
        public class MCPartnerCenovnikKatBrRobaIdSaveRequest
        {
            public int? Id { get; set; }
            public string KatBrProizvodjaca { get; set; }
            public int RobaId { get; set; }
            public string Proizvodjac { get; set; }
        }

        public class MCPartnerCenovnikKatBrRobaIdEntity
        {
            public int Id { get; set; }
            public string KatBrProizvodjaca { get; set; }
            public int RobaId { get; set; }
            public string Proizvodjac { get; set; }
        }

        public class CenovnikItem
        {
            public string? KatBr { get; set; }
            public string KatBrPro { get; set; }
            public string? Naziv { get; set; }
            public string NazivPro { get; set; }
            public string? JM { get; set; }
            public string JMPro { get; set; }
            public bool FoundInRoba { get; set; }
            public int? VezaId { get; set; }
        }

        private byte[] fileBuffer = null;
        private readonly IzborRobe _izborRobe = new IzborRobe(50);
        public fm_mc_NabavkaRobe_Index()
        {
            InitializeComponent();
            dataGridView1.Enabled = false;
        }

        private void fm_mc_NabavkaRobe_Index_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
        }

        private void uvuciCenovnik_btn_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Morate izabrati dobavljaca!");
                return;
            }
            dataGridView1.Enabled = false;
            var thread = new Thread(() =>
            {
                using (var fd = new OpenFileDialog())
                {
                    fd.ShowDialog();
                    if (string.IsNullOrWhiteSpace(fd.FileName))
                    {
                        fileBuffer = null;
                        return;
                    }

                    if (!Path.GetExtension(fd.FileName).ToLower().Contains("xls"))
                    {
                        MessageBox.Show("Neispravan format fajla!");
                        fileBuffer = null;
                        return;
                    }
                    using var stream = fd.OpenFile();

                    fileBuffer = new byte[stream.Length];
                    stream.Read(fileBuffer, 0, fileBuffer.Length);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Reload();
        }

        private void Reload()
        {
            if (fileBuffer == null)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Enabled = false;
                return;
            }
            string proizvodjac = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            dataGridView1.Enabled = false;

            var dt = new DataTable();
            dt.Columns.Add("KatBrLocal", typeof(string));
            dt.Columns.Add("KatBrDobavljac", typeof(string));
            dt.Columns.Add("NazivLocal", typeof(string));
            dt.Columns.Add("NazivDobavljac", typeof(string));
            dt.Columns.Add("JMLocal", typeof(string));
            dt.Columns.Add("JMDobavljac", typeof(string));
            dt.Columns.Add("FoundInRoba", typeof(bool));
            dt.Columns.Add("VezaId", typeof(int));

            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent((numericUpDown1.Value - 1).ToString()), "KolonaKataloskiBroj");
            formData.Add(new StringContent((numericUpDown2.Value - 1).ToString()), "KolonaNaziv");
            formData.Add(new StringContent((numericUpDown3.Value - 1).ToString()), "KolonaJediniceMere");
            formData.Add(new StringContent(proizvodjac), "Proizvodjac");
            formData.Add(new ByteArrayContent(fileBuffer), "File", "File");

            var response = httpClient.PostAsync(Path.Join(TDAPI.HttpClient.BaseAddress.ToString(), "mc-nabavka-robe-uvuci-fajl"), formData).Result;
            var respText = response.Content.ReadAsStringAsync().Result;
            var resp = JsonConvert.DeserializeObject<ListResponse<CenovnikItem>>(respText);

            foreach (var item in resp.Payload)
            {
                var dr = dt.NewRow();
                dr["KatBrLocal"] = item.KatBr;
                dr["KatBrDobavljac"] = item.KatBrPro;
                dr["NazivLocal"] = item.Naziv;
                dr["NazivDobavljac"] = item.NazivPro;
                dr["JMLocal"] = item.JM;
                dr["JMDobavljac"] = item.JMPro;
                dr["FoundInRoba"] = item.FoundInRoba;
                dr["VezaId"] = item.VezaId ?? -1;

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns["KatBrLocal"].Width = 150;

            dataGridView1.Columns["KatBrDobavljac"].Width = 150;

            dataGridView1.Columns["NazivLocal"].Width = 250;

            dataGridView1.Columns["NazivDobavljac"].Width = 250;

            dataGridView1.Columns["JMLocal"].Width = 50;

            dataGridView1.Columns["JMDobavljac"].Width = 50;

            dataGridView1.Columns["FoundInRoba"].Visible = false;

            dataGridView1.Columns["VezaId"].Visible = false;
            dataGridView1.Enabled = true;
        }

        private void poveziSaRobomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            var currItem = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["VezaId"].Value);
            var currKatBr = dataGridView1.Rows[rowIndex].Cells["KatBrDobavljac"].Value.ToString();
            _izborRobe.PrikaziKolonuCena = false;
            _izborRobe.DisposeOnClose = false;
            _izborRobe.DozvoliMenjanjeMagacina = false;
            _izborRobe.DozvoliMultiSelect = false;
            var proizvodjac = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            _izborRobe.OnRobaClickHandler = (Komercijalno.RobaUMagacinu[] robaUMagacinu) =>
            {
                var r = robaUMagacinu[0];

                Task.Run(() =>
                {
                    var response = TDAPI.PutAsync<MCPartnerCenovnikKatBrRobaIdSaveRequest, MCPartnerCenovnikKatBrRobaIdEntity>("mc-nabavka-robe-sacuvaj-partner-cenovnik-item", new MCPartnerCenovnikKatBrRobaIdSaveRequest()
                    {
                        Id = currItem > 0 ? currItem : null,
                        KatBrProizvodjaca = currKatBr,
                        Proizvodjac = proizvodjac,
                        RobaId = r.RobaID
                    }).Result;

                    if (response.NotOk)
                    {
                        MessageBox.Show("Doslo je do greske!");
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        Reload();
                        _izborRobe.Hide();
                    });
                });
            };
            _izborRobe.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
