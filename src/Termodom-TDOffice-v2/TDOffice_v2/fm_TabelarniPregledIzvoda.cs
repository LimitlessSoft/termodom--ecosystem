using FirebirdSql.Data.FirebirdClient;
using PdfSharp.Pdf.AcroForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2
{
    public partial class fm_TabelarniPregledIzvoda : Form
    {
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

                foreach (DistinctConnectionInfo csi in await _distinctPutanjeDoBaza)
                {
                    
                    string[] putanjaParts = csi.PutanjaDoBaze.Split("/");
                    list.Add(new Tuple<string, string>(csi.PutanjaDoBaze, $"{csi.Godina} - {putanjaParts[putanjaParts.Length - 1]}"));
                }

                this.Invoke((MethodInvoker)delegate
                {
                    baza_cmb.DataSource = new List<Tuple<string, string>>(list);
                    baza_cmb.DisplayMember = "Item2";
                    baza_cmb.ValueMember = "Item1";
                });
            });
        }

        private void btn_UvuciIzvode_Click(object sender, EventArgs e)
        {
            //string constring = $"data source=4monitor; initial catalog = {baza_cmb.SelectedValue.ToString()}; user=SYSDBA; password=m";
            
            
            string constring = $"data source=localhost; initial catalog = {baza_cmb.SelectedValue.ToString()}; user=SYSDBA; password=m";
            using (FbConnection con = new FbConnection(constring))
            {
               con.Open();
                
            }
        }
    }
}
