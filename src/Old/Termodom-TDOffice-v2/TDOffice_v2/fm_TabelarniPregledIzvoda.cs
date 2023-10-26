using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using Termodom.Data.Entities.DBSettings;

namespace TDOffice_v2
{
    public partial class fm_TabelarniPregledIzvoda : Form
    {
        private class TabelarniPregledIzvodaGetDto
        {
            public string FirmaPib { get; set; }
            public int? TagId { get; set; }
            public int BrDok { get; set; }
            public int VrDok { get; set; }
            public string? IntBroj { get; set; }
            public DateTime DatumIzvoda { get; set; }
            public decimal UnosPocetnoStanje { get; set; }
            public decimal UnosPotrazuje { get; set; }
            public decimal UnosDuguje { get; set; }
            public decimal NovoStanje { get => UnosPotrazuje - UnosDuguje; }
            public int Korisnik { get; set; }
            public bool FinansijskaIspravnost { get; set; }
            public bool Zakljucano { get; set; }
            public bool LogickaIspravnost { get; set; }
        }
        private class DokumentTagizvodPutRequest
        {
            public int? Id { get; set; }
            public int? BrojDokumentaIzvoda { get; set; }
            public decimal UnosPocetnoStanje { get; set; }
            public decimal UnosPotrazuje { get; set; }
            public decimal UnosDuguje { get; set; }
            public int Korisnik { get; set; }
        }
        public class TabelarniPregledIzvodaGetRequest
        {
            public DateTime? OdDatuma { get; set; }
            public DateTime? DoDatuma { get; set; }
        }
        public class DokumentTagIzvodGetDto
        {
            public int Id { get; set; }
            public int BrojDokumentaIzvoda { get; set; }
            public decimal UnosPocetnoStanje { get; set; }
            public decimal UnosPotrazuje { get; set; }
            public decimal UnosDuguje { get; set; }
            public int Korisnik { get; set; }
        }

        private Task<List<DistinctConnectionInfo>> _distinctPutanjeDoBaza { get; set; }

        public fm_TabelarniPregledIzvoda()
        {
            InitializeComponent();
        }

        private void fm_TabelarniPregledIzvoda_Load(object sender, EventArgs e)
        {
            _distinctPutanjeDoBaza = BazaManager.DistinctConnectionInfoListAsync();
            
            _distinctPutanjeDoBaza.ContinueWith(async (prev) =>
            {
                List<Tuple<string, string>> list = new List<Tuple<string, string>>();
                list.Add(new Tuple<string, string>("-1", "Izaberi bazu"));

                string fullPutanja2023tcmd = "";
                foreach (DistinctConnectionInfo csi in await _distinctPutanjeDoBaza)
                {
                    
                    string[] putanjaParts = csi.PutanjaDoBaze.Split("/");

                    if (csi.PutanjaDoBaze.ToLower().Contains("2023tcmd"))
                        fullPutanja2023tcmd = csi.PutanjaDoBaze;

                    list.Add(new Tuple<string, string>(csi.PutanjaDoBaze, $"{csi.Godina} - {putanjaParts[putanjaParts.Length - 1]}"));
                }

                this.Invoke((MethodInvoker)delegate
                {
                    baza_cmb.Enabled = false;
                    baza_cmb.DataSource = new List<Tuple<string, string>>(list);
                    baza_cmb.DisplayMember = "Item2";
                    baza_cmb.ValueMember = "Item1";
                    baza_cmb.Enabled = true;
                    baza_cmb.SelectedValue = fullPutanja2023tcmd;
                    baza_cmb.Enabled = false;
                });
            });
        }

        private void baza_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!baza_cmb.Enabled)
                return;

            UcitajPodatke();
        }

        private void UcitajPodatke()
        {

            this.Enabled = false;
            Task.Run(() =>
            {
                var response = TDAPI.GetAsync<List<TabelarniPregledIzvodaGetDto>>("/tabelarni-pregled-izvoda")
                    .GetAwaiter()
                    .GetResult();

                if (response.NotOk)
                {
                    MessageBox.Show("Doslo je do greske prilikom getovanja '/tabelarni-pregled-izvoda'");
                    this.Enabled = true;
                    return;
                }

                var dt = new DataTable();
                dt.Columns.Add("TagId", typeof(int));
                dt.Columns.Add("BrojDokumentaIzvoda", typeof(int));
                dt.Columns.Add("UnosDuguje", typeof(int));
                dt.Columns.Add("UnosPotrazuje", typeof(int));
                dt.Columns.Add("UnosPocetnoStanje", typeof(int));
                dt.Columns.Add("Korisnik", typeof(int));

                foreach (var item in response.Payload)
                {
                    var dataRow = dt.NewRow();
                    dataRow["TagId"] = item.TagId == null ? 0 : (int)item.TagId;
                    dataRow["BrojDokumentaIzvoda"] = item.BrDok;
                    dataRow["UnosPocetnoStanje"] = item.UnosPocetnoStanje;
                    dataRow["UnosPotrazuje"] = item.UnosPotrazuje;
                    dataRow["UnosDuguje"] = item.UnosDuguje;
                    dataRow["Korisnik"] = item.TagId == null ? Program.TrenutniKorisnik.ID : item.Korisnik;
                    dt.Rows.Add(dataRow);
                }

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.Enabled = false;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Enabled = true;
                    this.Enabled = true;
                });
            });
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!dataGridView1.Enabled)
                return;

            if (e.FormattedValue.ToString() == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                return;

            var row = dataGridView1.Rows[e.RowIndex];

            var brojDokumentaIzvoda = Convert.ToInt32(row.Cells["BrojDokumentaIzvoda"].Value);

            var unosDuguje =
                row.Cells["UnosDuguje"].ColumnIndex == e.ColumnIndex ?
                    Convert.ToDecimal(e.FormattedValue) :
                    Convert.ToDecimal(row.Cells["UnosDuguje"].Value);

            var unosPotrazuje =
                row.Cells["UnosPotrazuje"].ColumnIndex == e.ColumnIndex ?
                    Convert.ToDecimal(e.FormattedValue) :
                    Convert.ToDecimal(row.Cells["UnosPotrazuje"].Value);

            var unosPocetnoStanje =
                row.Cells["UnosPocetnoStanje"].ColumnIndex == e.ColumnIndex ?
                    Convert.ToDecimal(e.FormattedValue) :
                    Convert.ToDecimal(row.Cells["UnosPocetnoStanje"].Value);

            var korisnik = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Korisnik"].Value);

            var tagId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["TagId"].Value);

            Task.Run(() =>
            {
                var response = TDAPI.PutAsync<DokumentTagizvodPutRequest, DokumentTagIzvodGetDto>("/tabelarni-pregled-izvoda",
                    new DokumentTagizvodPutRequest()
                    {
                        Id = tagId == 0 ? null : tagId,
                        BrojDokumentaIzvoda = brojDokumentaIzvoda,
                        UnosPocetnoStanje = unosPocetnoStanje,
                        UnosPotrazuje = unosPotrazuje,
                        UnosDuguje = unosDuguje,
                        Korisnik = korisnik
                    })
                    .GetAwaiter()
                    .GetResult();

                if (response.NotOk)
                    MessageBox.Show("Greska prilikom azuriranja podataka u bazi! PUT '/tabelarni-pregled-izvoda'");

                dataGridView1.Rows[e.RowIndex].Cells["TagId"].Value = response.Payload.Id;
            });
        }

        private void od_dtp_ValueChanged(object sender, EventArgs e)
        {
            UcitajPodatke();
        }
    }
}
