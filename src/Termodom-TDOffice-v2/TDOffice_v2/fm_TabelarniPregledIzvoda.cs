using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
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

        private void btn_UvuciIzvode_Click(object sender, EventArgs e)
        {
            //string constring = $"data source=4monitor; initial catalog = {baza_cmb.SelectedValue.ToString()}; user=SYSDBA; password=m";
        }

        private void baza_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!baza_cmb.Enabled)
                return;

            Task.Run(() =>
            {
                var response = TDAPI.GetAsync<List<TabelarniPregledIzvodaGetDto>>("/tabelarni-pregled-izvoda")
                    .GetAwaiter()
                    .GetResult();

                if (response.NotOk)
                {
                    MessageBox.Show("Doslo je do greske prilikom getovanja '/tabelarni-pregled-izvoda'");
                    return;
                }

                dataGridView1.DataSource = response.Payload;
            });
        }
    }
}
