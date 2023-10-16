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
using System.Collections.Generic;
using System.Linq;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2.Forms.MC
{
    public partial class fm_mc_NabavkaRobe_Index : Form
    {
        public class NabavkaRobeDobavljaciSettings
        {
            public class Item
            {
                public int PPID { get; set; }
                public int KolonaKatBr { get; set; }
                public int KolonaNaziv { get; set; }
                public int KolonaJm { get; set; }
                public int KolonaCena { get; set; }
                public int KolonaRabat { get; set; }
            }
            public List<Item> Dobavljaci { get; set; } = new List<Item>();
        }

        public class MCPartnerCenovnikKatBrRobaIdSaveRequest
        {
            public int? Id { get; set; }
            public string KatBrProizvodjaca { get; set; }
            public int RobaId { get; set; }
            public int DobavljacPPID { get; set; }
        }

        public class MCPartnerCenovnikKatBrRobaIdEntity
        {
            public int Id { get; set; }
            public string KatBrProizvodjaca { get; set; }
            public int RobaId { get; set; }
            public int DobavljacPPID { get; set; }
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
        private readonly IzborRobe _izborRobe = new IzborRobe(150);
        private readonly TDOffice.Config<NabavkaRobeDobavljaciSettings> _dobavljaciSettings;
        public fm_mc_NabavkaRobe_Index()
        {
            InitializeComponent();
            dataGridView1.Enabled = false;
            _dobavljaciSettings = TDOffice.Config<NabavkaRobeDobavljaciSettings>.Get(TDOffice.ConfigParameter.NabavkaRobeDobavljacCenovnikSettings);
            if (_dobavljaciSettings.Tag == null)
            {
                _dobavljaciSettings.Tag = new NabavkaRobeDobavljaciSettings()
                {
                    Dobavljaci = new List<NabavkaRobeDobavljaciSettings.Item>()
                    {
                        new NabavkaRobeDobavljaciSettings.Item()
                    }
                };
                _dobavljaciSettings.UpdateOrInsert();
            }
        }

        private void fm_mc_NabavkaRobe_Index_Load(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            dataGridView1.Enabled = false;

            _ = LoadUiAsync();
        }

        private async Task LoadUiAsync()
        {
            try
            {
                var partneri = await Komercijalno.Partner.ListAsync(DateTime.UtcNow.Year);
                var dobavljaciDto = new List<Tuple<int, string>>();
                foreach (var dobavljac in _dobavljaciSettings.Tag.Dobavljaci)
                {
                    var partner = partneri.FirstOrDefault(x => x.PPID == dobavljac.PPID);
                    dobavljaciDto.Add(new Tuple<int, string>(dobavljac.PPID, $"{partner?.Naziv} ({dobavljac.PPID})"));
                }

                comboBox1.ValueMember = "Item1";
                comboBox1.DisplayMember = "Item2";
                comboBox1.DataSource = dobavljaciDto;

                comboBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void uvuciCenovnik_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
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

            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;
            var dobavljac = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);
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

            var resp = UvuciFajl();
            if (resp == null || resp.NotOk)
            {
                MessageBox.Show("Doslo je do greske!");
                return;
            }

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
            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;
            var dobavljac = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);
            _izborRobe.OnRobaClickHandler = (Komercijalno.RobaUMagacinu[] robaUMagacinu) =>
            {
                var r = robaUMagacinu[0];

                Task.Run(() =>
                {
                    var response = TDAPI.PutAsync<MCPartnerCenovnikKatBrRobaIdSaveRequest, MCPartnerCenovnikKatBrRobaIdEntity>("mc-nabavka-robe-sacuvaj-partner-cenovnik-item", new MCPartnerCenovnikKatBrRobaIdSaveRequest()
                    {
                        Id = currItem > 0 ? currItem : null,
                        KatBrProizvodjaca = currKatBr,
                        DobavljacPPID = dobavljac.PPID,
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

        private class MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest
        {
            public DateTime Datum { get; set; }
            public int PPID { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;
            Task.Run(async () =>
            {
                try
                {
                    var dobavljac = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);

                    var postojeCeneNaDanResponse = await TDAPI.GetAsync<MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest, bool>("/mc-nabavka-robe-proveri-postojanje-cenovnika-na-dan", new MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest()
                    {
                        PPID = dobavljac.PPID,
                        Datum = DateTime.SpecifyKind(doDatuma_dtp.Value.Date, DateTimeKind.Utc)
                    });
                    if (postojeCeneNaDanResponse.NotOk)
                    {
                        MessageBox.Show("Doslo je do greske!");
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Enabled = true;
                        });
                        return;
                    }

                    if (postojeCeneNaDanResponse.Payload &&
                        MessageBox.Show("Za ovog dobavljaca vec postoji cenovnik na ovaj dan. Nastavljanjem akcije ce sve cene na ovaj dan biti uklonjene. Da li zelite nastaviti?", "Nastaviti?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Enabled = true;
                        });
                        return;
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        var resp = UvuciFajl(true);
                        if (resp.NotOk)
                            MessageBox.Show("Doslo je do greske!");
                        else
                            MessageBox.Show("Cenovnik uspesno sacuvan!");

                        this.Enabled = true;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }

        private ListResponse<CenovnikItem> UvuciFajl(bool sacuvajUBazu = false)
        {
            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;
            var dobavljac = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);

            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(dobavljac.KolonaKatBr.ToString()), "KolonaKataloskiBroj");
            formData.Add(new StringContent(dobavljac.KolonaNaziv.ToString()), "KolonaNaziv");
            formData.Add(new StringContent(dobavljac.KolonaJm.ToString()), "KolonaJediniceMere");
            formData.Add(new StringContent(dobavljac.KolonaCena.ToString()), "KolonaVPCenaBezRabata");
            if (dobavljac.KolonaRabat >= 0)
                formData.Add(new StringContent(dobavljac.KolonaRabat.ToString()), "KolonaRabat");
            formData.Add(new StringContent(dobavljac.PPID.ToString()), "DobavljacPPID");
            formData.Add(new ByteArrayContent(fileBuffer), "File", "File");
            formData.Add(new StringContent(sacuvajUBazu ? "true" : "false"), "SacuvajUBazu");
            if (sacuvajUBazu)
                formData.Add(new StringContent(DateTime.SpecifyKind(doDatuma_dtp.Value, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ")), "VaziOdDana");

            var response = httpClient.PostAsync(Path.Join(TDAPI.HttpClient.BaseAddress.ToString(), "mc-nabavka-robe-uvuci-fajl"), formData).Result;
            var respText = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ListResponse<CenovnikItem>>(respText);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
        }
    }
}
