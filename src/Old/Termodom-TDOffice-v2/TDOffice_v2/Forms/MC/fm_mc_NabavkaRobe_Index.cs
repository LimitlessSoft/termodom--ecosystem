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
using System.Drawing;

namespace TDOffice_v2.Forms.MC
{
    public partial class fm_mc_NabavkaRobe_Index : Form
    {
        private DataTable baseDataTable = null;
        private DataTable dataGridViewDataTable = null;
        private bool _prikaziKoloneZaStelovanjeJM = false;

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
                public List<JMLink> JMs { get; set; } = new List<JMLink>();
            }

            public class JMLink
            {
                public int RobaId { get; set; }
                public double LocalKolicina { get; set; }
                public double DobavljacKolicina { get; set; }
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
            public int? KomercijalnoRobaId { get; set; }
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
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            doDatuma_dtp.Enabled = false;
            uvuciCenovnik_btn.Enabled = false;

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
                uvuciCenovnik_btn.Enabled = true;
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

            ReloadAsync();
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

        private async void ReloadAsync()
        {
            dataGridView1.DataSource = null;
            comboBox1.Enabled = false;
            dataGridView1.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            doDatuma_dtp.Enabled = false;

            if (fileBuffer == null)
                return;

            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;

            var dt = new DataTable();
            dt.Columns.Add("KatBrLocal", typeof(string));
            dt.Columns.Add("KatBrDobavljac", typeof(string));
            dt.Columns.Add("NazivLocal", typeof(string));
            dt.Columns.Add("NazivDobavljac", typeof(string));
            dt.Columns.Add("JMLocal", typeof(string));
            dt.Columns.Add("JMDobavljac", typeof(string));
            dt.Columns.Add("FoundInRoba", typeof(bool));
            dt.Columns.Add("VezaId", typeof(int));
            dt.Columns.Add("LocalKolicina", typeof(double));
            dt.Columns.Add("DobavljacKolicina", typeof(double));
            dt.Columns.Add("RobaIdKomercijalno", typeof(int));

            foreach (var d in comboBox1.Items)
                dt.Columns.Add("Dobavljac: " + (d as Tuple<int, string>).Item2, typeof(decimal));

            var resp = UvuciFajl();
            if (resp == null || resp.NotOk)
            {
                MessageBox.Show("Doslo je do greske!");
                return;
            }

            var uporediCenovnikeResponse = await TDAPI.GetAsync<List<MCNabavkaRobeUporediCenovnikeItemDto>>("/mc-nabavka-robe-uporedi-cenovnike");
            if (uporediCenovnikeResponse.NotOk)
            {
                MessageBox.Show("Doslo je do greske!");
                return;
            }

            foreach (var item in resp.Payload)
            {
                var dob = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);
                var jmLink = dob.JMs.FirstOrDefault(x => x.RobaId == item.KomercijalnoRobaId);

                var dr = dt.NewRow();
                dr["KatBrLocal"] = item.KatBr;
                dr["KatBrDobavljac"] = item.KatBrPro;
                dr["NazivLocal"] = item.Naziv;
                dr["NazivDobavljac"] = item.NazivPro;
                dr["JMLocal"] = item.JM;
                dr["JMDobavljac"] = item.JMPro;
                dr["FoundInRoba"] = item.FoundInRoba;
                dr["VezaId"] = item.VezaId ?? -1;
                dr["LocalKolicina"] = jmLink?.LocalKolicina ?? 1;
                dr["DobavljacKolicina"] = jmLink?.DobavljacKolicina ?? 1;
                dr["RobaIdKomercijalno"] = item.KomercijalnoRobaId ?? -1;

                foreach (var d in comboBox1.Items)
                {
                    if (item.KomercijalnoRobaId == null)
                    {
                        dr["Dobavljac: " + (d as Tuple<int, string>).Item2] = -1;
                        continue;
                    }

                    var dCena = uporediCenovnikeResponse.Payload
                        .FirstOrDefault(x => x.RobaId == item.KomercijalnoRobaId.Value)?
                        .SubItems
                        .FirstOrDefault(x => x.DobavljacPPID == (d as Tuple<int, string>).Item1);

                    dr["Dobavljac: " + (d as Tuple<int, string>).Item2] = dCena?.VPCenaSaPopustom ?? 0;
                }

                dt.Rows.Add(dr);
            }

            baseDataTable = dt;
            dataGridViewDataTable = dt;

            UpdateDGV();
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
                        ReloadAsync();
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                FilterEnter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FilterCtrlA();
        }

        private void FilterCtrlA()
        {
            string selectString = "";
            string input = textBox1.Text;
            string[] inputElemets = input.Split('+');

            foreach (object o in comboBox2.Items)
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

        private void UpdateDGV()
        {
            dataGridView1.DataSource = dataGridViewDataTable;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns["KatBrLocal"].Width = 80;
            dataGridView1.Columns["KatBrLocal"].HeaderText = "KatBr local";
            dataGridView1.Columns["KatBrLocal"].ReadOnly = true;

            dataGridView1.Columns["KatBrDobavljac"].Width = 80;
            dataGridView1.Columns["KatBrDobavljac"].HeaderText = "KatBr dobavljac";
            dataGridView1.Columns["KatBrDobavljac"].ReadOnly = true;

            dataGridView1.Columns["NazivLocal"].Width = 250;
            dataGridView1.Columns["NazivLocal"].HeaderText = "Naziv local";
            dataGridView1.Columns["NazivLocal"].ReadOnly = true;

            dataGridView1.Columns["NazivDobavljac"].Width = 250;
            dataGridView1.Columns["NazivDobavljac"].HeaderText = "Naziv dobavljac";
            dataGridView1.Columns["NazivDobavljac"].ReadOnly = true;

            dataGridView1.Columns["JMLocal"].Width = 50;
            dataGridView1.Columns["JMLocal"].HeaderText = "JM local";
            dataGridView1.Columns["JMLocal"].ReadOnly = true;

            dataGridView1.Columns["JMDobavljac"].Width = 50;
            dataGridView1.Columns["JMDobavljac"].HeaderText = "JM dobavljac";
            dataGridView1.Columns["JMDobavljac"].Visible = _prikaziKoloneZaStelovanjeJM;
            dataGridView1.Columns["JMDobavljac"].ReadOnly = true;

            dataGridView1.Columns["FoundInRoba"].Visible = false;
            dataGridView1.Columns["FoundInRoba"].ReadOnly = true;

            dataGridView1.Columns["VezaId"].Visible = false;
            dataGridView1.Columns["VezaId"].ReadOnly = true;

            dataGridView1.Columns["RobaIdKomercijalno"].Visible = false;
            dataGridView1.Columns["RobaIdKomercijalno"].ReadOnly = true;

            dataGridView1.Columns["LocalKolicina"].Width = 50;
            dataGridView1.Columns["LocalKolicina"].HeaderText = "Local kolicina";
            dataGridView1.Columns["LocalKolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["LocalKolicina"].ReadOnly = false;
            dataGridView1.Columns["LocalKolicina"].Visible = _prikaziKoloneZaStelovanjeJM;
            dataGridView1.Columns["LocalKolicina"].DefaultCellStyle.BackColor = Color.LightYellow;

            dataGridView1.Columns["DobavljacKolicina"].Width = 80;
            dataGridView1.Columns["DobavljacKolicina"].HeaderText = "Dobavljac kolicina";
            dataGridView1.Columns["DobavljacKolicina"].ReadOnly = false;
            dataGridView1.Columns["DobavljacKolicina"].Visible = _prikaziKoloneZaStelovanjeJM;
            dataGridView1.Columns["DobavljacKolicina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["DobavljacKolicina"].DefaultCellStyle.BackColor = Color.LightYellow;

            foreach (var d in comboBox1.Items)
            {
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].Width = 80;
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].ReadOnly = true;
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].DefaultCellStyle.Format = "#,##0.00";
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].Visible = !_prikaziKoloneZaStelovanjeJM;
            }

            dataGridView1.Enabled = true;
            button1.Enabled = true;
            doDatuma_dtp.Enabled = true;
            comboBox2.Enabled = true;
            textBox1.Enabled = true;
            button2.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void FilterEnter()
        {
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Morate izabrati kolonu!");
                return;
            }
            dataGridView1.ClearSelection();
            string kolona = comboBox2.SelectedItem.ToString();
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var column = dataGridView1.Columns[e.ColumnIndex];

            if (column.Name != "LocalKolicina" && column.Name != "DobavljacKolicina")
                return;

            double kolicina;

            if (!Double.TryParse(e.FormattedValue.ToString(), out kolicina))
            {
                MessageBox.Show("Neispravna vrednost!");
                e.Cancel = true;
                return;
            }

            var currValue = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            if (currValue == kolicina)
                return;

            var dobavljacSelect = comboBox1.SelectedItem as Tuple<int, string>;
            var dobavljac = _dobavljaciSettings.Tag.Dobavljaci.First(x => x.PPID == dobavljacSelect.Item1);
            var robaIdKomercijalno = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaIdKomercijalno"].Value);

            if (robaIdKomercijalno <= 0)
            {
                MessageBox.Show("Stavka mora biti povezana sa robom iz komercijalnog poslovanja!");
                e.Cancel = true;
                return;
            }

            var jmVeza = dobavljac.JMs.FirstOrDefault(x => x.RobaId == robaIdKomercijalno);
            if (jmVeza == null)
                jmVeza = new NabavkaRobeDobavljaciSettings.JMLink()
                {
                    RobaId = robaIdKomercijalno,
                    DobavljacKolicina = 1,
                    LocalKolicina = 1
                };

            if (column.Name == "LocalKolicina")
            {
                jmVeza.LocalKolicina = kolicina;
            }
            else if (column.Name == "DobavljacKolicina")
            {
                jmVeza.DobavljacKolicina = kolicina;
            }
            else
            {
                MessageBox.Show("Unknown error!");
                e.Cancel = true;
                return;
            }

            dobavljac.JMs.RemoveAll(x => x.RobaId == robaIdKomercijalno);
            dobavljac.JMs.Add(jmVeza);
            _dobavljaciSettings.UpdateOrInsert();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            _prikaziKoloneZaStelovanjeJM = !_prikaziKoloneZaStelovanjeJM;

            button3.Text = _prikaziKoloneZaStelovanjeJM ?
                "Sakrij kolone za podesavanje jedinica mere za ovog dobavljaca" :
                "Prikazi kolone za podesavanje jedinica mere za ovog dobavljaca";

            if (dataGridView1.DataSource == null)
                return;

            dataGridView1.Columns["DobavljacKolicina"].Visible = _prikaziKoloneZaStelovanjeJM;
            dataGridView1.Columns["LocalKolicina"].Visible = _prikaziKoloneZaStelovanjeJM;
            dataGridView1.Columns["JMDobavljac"].Visible = _prikaziKoloneZaStelovanjeJM;

            foreach (var d in comboBox1.Items)
                dataGridView1.Columns["Dobavljac: " + (d as Tuple<int, string>).Item2].Visible = !_prikaziKoloneZaStelovanjeJM;
        }
    }
}
