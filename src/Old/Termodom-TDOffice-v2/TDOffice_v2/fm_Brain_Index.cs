using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Brain_Index : Form
    {
        public class PutanjeDoBazaDTO
        {
            public class Item
            {
                public int MagacinID { get; set; }
                public int Godina { get; set; }
                public string PutanjaDoBaze { get; set; } = "";
            }

            public List<Item> Items { get; set; } = new List<Item>();

            public PutanjeDoBazaDTO()
            {

            }
        }
        private class FTPDTO
        {
            public string Url { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public fm_Brain_Index()
        {
            InitializeComponent();
            button2.Visible = true;
        }

        private async void fm_Brain_Index_LoadAsync(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Task t1 = UcitajDBServerNameAsync();
            Task t2 = UcitajOdredistaBazaKomercijalnogPoslovanja();
            Task t3 = UcitajVudimConfigBazu();
            Task t4 = UcitajFtpPodatke();

            await t1;
            await t2;
            await t3;
            await t4;
        }

        private async Task UcitajFtpPodatke()
        {
            var response = await TDBrain_v3.GetAsync("/ftpsettings/info");
            if((int)response.StatusCode == 200)
            {
                FTPDTO dto = JsonConvert.DeserializeObject<FTPDTO>(await response.Content.ReadAsStringAsync());
                ftpUrl_txt.Text = dto.Url;
                ftpUsername_txt.Text = dto.Username;

                ftpUrl_txt.Enabled = true;
                ftpUsername_txt.Enabled = true;
                ftpNovaSifra_txt.Enabled = true;
            }
        }
        private async Task UcitajVudimConfigBazu()
        {
            var response = await TDBrain_v3.GetAsync("/dbsettings/komercijalno/config/path/get");
            vudimConfigBaza_txt.Text = (int)response.StatusCode == 200 ? await response.Content.ReadAsStringAsync() :
                (int)response.StatusCode == 204 ? "" : "Greska";
            vudimConfigBaza_txt.Enabled = true;
        }
        private async Task UcitajDBServerNameAsync()
        {
            var response = await TDBrain_v3.GetAsync("/dbsettings/firebird/servername/get");
            txt_fbdbServerName.Text = await response.Content.ReadAsStringAsync();
            txt_fbdbServerName.Enabled = true;
        }
        private async Task UcitajOdredistaBazaKomercijalnogPoslovanja()
        {
            var response = await TDBrain_v3.GetAsync("/dbsettings/baza/komercijalno/list");
            string respString = await response.Content.ReadAsStringAsync();
            List<Termodom.Data.Entities.DBSettings.ConnectionInfo> putanjeDoBaza = JsonConvert.DeserializeObject<List<Termodom.Data.Entities.DBSettings.ConnectionInfo>>(respString);

            DataTable dt = new DataTable();
            dt.Columns.Add("Godina", typeof(int));
            dt.Columns.Add("MagacinID", typeof(int));
            dt.Columns.Add("Putanja", typeof(string));

            foreach(Termodom.Data.Entities.DBSettings.ConnectionInfo it in putanjeDoBaza)
            {
                DataRow dr = dt.NewRow();
                dr["Godina"] = it.Godina;
                dr["MagacinID"] = it.MagacinID;
                dr["Putanja"] = it.PutanjaDoBaze;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Enabled = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            this.Enabled = false;
            await LoadDataAsync();
            this.Enabled = true;
        }

        private void txt_dbServerName_TextChanged(object sender, EventArgs e)
        {
            if (!txt_fbdbServerName.Enabled)
                return;

            button1.Visible = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txt_fbdbServerName.Text))
            {
                MessageBox.Show("Neispravna vrednost!");
                return;
            }

            txt_fbdbServerName.Enabled = false;
            button1.Enabled = false;
            var response = await TDBrain_v3.PostAsync("/dbsettings/firebird/servername/set", new Dictionary<string, string>() { { "serverName", txt_fbdbServerName.Text } });

            if((int)response.StatusCode == 200)
            {
                MessageBox.Show("Uspesno azuriran FB DB Server Name!");
            }
            else
            {
                MessageBox.Show("Doslo je do greske prilikom azuriranja FB DB Server Name-a!");
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }

            txt_fbdbServerName.Enabled = true;
            button1.Enabled = true;
            button1.Visible = false;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_fbdbPassword.Text))
            {
                MessageBox.Show("Neispravna vrednost!");
                return;
            }

            txt_fbdbPassword.Enabled = false;
            button3.Enabled = false;
            var response = await TDBrain_v3.PostAsync("/dbsettings/firebird/password/set", new Dictionary<string, string>() { { "password", txt_fbdbPassword.Text } });

            if ((int)response.StatusCode == 200)
            {
                MessageBox.Show("Uspesno azuriran FB DB Sifra!");
            }
            else
            {
                MessageBox.Show("Doslo je do greske prilikom azuriranja FB DB Sifre!");
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }

            txt_fbdbPassword.Enabled = true;
            button3.Enabled = true;
            button3.Visible = false;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!dataGridView1.Enabled)
                return;

            if (e.FormattedValue != dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
            {
                button4.Visible = true;
                button4.Enabled = true;
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            dataGridView1.Enabled = false;

            var response = await TDBrain_v3.PostAsync("/dbsettings/baza/komercijalno/deleteall");

            if((int)response.StatusCode != 200)
            {
                MessageBox.Show("Greska prilikom azuriranja putanja do baza part 1!");
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int magacinID = Convert.ToInt32(row.Cells["MagacinID"].Value);
                int godina = Convert.ToInt32(row.Cells["Godina"].Value);
                string putanja = row.Cells["Putanja"].Value.ToString();

                var response1 = await TDBrain_v3.PostAsync("/dbsettings/baza/komercijalno/addorupdate", new Dictionary<string, string>() {
                    { "godina", godina.ToString() },
                    { "magacinid", magacinID.ToString() },
                    { "putanja", putanja }
                });

                if((int)response1.StatusCode != 200)
                {
                    MessageBox.Show("Fatalna greska. Obustavite sve i zovite administratora!");
                    return;
                }
            }

            MessageBox.Show("Uspesno azurirane putanje do baza!");
            dataGridView1.Enabled = true;
            button4.Enabled = true;
        }

        private void txt_fbdbPassword_TextChanged(object sender, EventArgs e)
        {
            button3.Show();
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;

            var response = await TDBrain_v3.PostAsync($"/dbsettings/komercijalno/config/path/set", new Dictionary<string, string>()
            {
                { "connString", vudimConfigBaza_txt.Text }
            });
            if((int)response.StatusCode != 200)
            {
                MessageBox.Show("Greska prilikom azuriranja connection string komercijalno config!");
                return;
            }

            MessageBox.Show("Uspesno azuriran connection string komercijalno config!");

            button6.Enabled = true;
            button6.Visible = false;
        }

        private void vudimConfigBaza_txt_TextChanged(object sender, EventArgs e)
        {
            if (!vudimConfigBaza_txt.Enabled)
                return;

            button6.Visible = true;
        }

        private void novaSifra_ftp_TextChanged(object sender, EventArgs e)
        {
            if (!ftpNovaSifra_txt.Enabled)
                return;

            button5.Visible = true;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            string vrednost = ftpNovaSifra_txt.Text;

            if (string.IsNullOrWhiteSpace(vrednost))
            {
                MessageBox.Show("Morate uneti vrednost nove sifre!");
                return;
            }

            button5.Enabled = false;
            ftpNovaSifra_txt.Enabled = false;
            var response = await TDBrain_v3.PostAsync($"/ftpsettings/password/set?password={vrednost}");
            if ((int)response.StatusCode == 200)
            {
                MessageBox.Show("Uspesno azuriran sifra ftp-a!");
            }
            else
            {
                MessageBox.Show("API je vratio gresku: " + (response.StatusCode));
            }
            button5.Enabled = true;
            ftpNovaSifra_txt.Enabled = true;
        }

        private void ftpUrl_txt_TextChanged(object sender, EventArgs e)
        {
            if (!ftpUrl_txt.Enabled)
                return;

            button7.Visible = true;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            string vrednost = ftpUrl_txt.Text;

            if(string.IsNullOrWhiteSpace(vrednost))
            {
                MessageBox.Show("Morate uneti vrednost URL-a!");
                return;
            }

            button7.Enabled = false;
            ftpUrl_txt.Enabled = false;
            var response = await TDBrain_v3.PostAsync($"/ftpsettings/url/set?url={vrednost}");
            if((int)response.StatusCode == 200)
            {
                MessageBox.Show("Uspesno azuriran URL ftp-a!");
            }
            else
            {
                MessageBox.Show("API je vratio gresku: " + (response.StatusCode));
            }
            button7.Enabled = true;
            ftpUrl_txt.Enabled = true;
        }

        private void ftpUsername_txt_TextChanged(object sender, EventArgs e)
        {
            if (!ftpUsername_txt.Enabled)
                return;

            button8.Visible = true;
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            string vrednost = ftpUsername_txt.Text;

            if (string.IsNullOrWhiteSpace(vrednost))
            {
                MessageBox.Show("Morate uneti vrednost za username!");
                return;
            }

            button8.Enabled = false;
            ftpUsername_txt.Enabled = false;
            var response = await TDBrain_v3.PostAsync($"/ftpsettings/username/set?username={vrednost}");
            if ((int)response.StatusCode == 200)
            {
                MessageBox.Show("Uspesno azuriran username ftp-a!");
            }
            else
            {
                MessageBox.Show("API je vratio gresku: " + (response.StatusCode));
            }
            button8.Enabled = true;
            ftpUsername_txt.Enabled = true;
        }
    }
}
