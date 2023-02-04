using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using TDOffice_v2.Komercijalno;
using System.Data;
using TDOffice_v2.EventHandlers;
using System.Threading.Tasks;

namespace TDOffice_v2
{
    public partial class _1360_fm_OcekivaneUplate_Index : Form
    {
        public int PPID { get; set; }
        private Task<fm_Help> _helpFrom { get; set; }

        private Partner _partner;

        private const string KONTO_KUPAC = "20400";
        private const string KONTO_DOBAVLJAC = "43500";
        private DataTable dt { get; set; } = null;

        //private double PSKF =0;//pocetno stanje kupca finansijski
        //private double KSKF = 0;//Krajnje stanje kupca finansijski
        //private double PSDF = 0;//Pocetno stanje dobavljaca finansijski
        //private double KSDF = 0;//Krajnje stanje dobavljaca finansijski

        public _1360_fm_OcekivaneUplate_Index()
        {
            InitializeComponent();
            throw new Exception("Ne radi trenutno!");
            _helpFrom = this.InitializeHelpModulAsync(Modul.OcekivaneUplate_Index);
            SetUI();
        }

        private void SetUI()
        {
            dt = new DataTable();
            dt.Columns.Add("PPID", typeof(int));
            dt.Columns.Add("DATUM", typeof(DateTime));
            dt.Columns.Add("Potrazuje", typeof(double));
            dt.Columns.Add("Konto", typeof(string));
            dt.Columns.Add("IzvodID", typeof(int));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("VrDok", typeof(int));
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["PPID"].Width = 90;
            dataGridView1.Columns["PPID"].HeaderText = "PPID";
            dataGridView1.Columns["PPID"].Visible = false;

            dataGridView1.Columns["Konto"].Width = 90;
            dataGridView1.Columns["Konto"].HeaderText = "Konto";
            dataGridView1.Columns["Konto"].Visible = false;

            dataGridView1.Columns["Potrazuje"].Width = 90;
            dataGridView1.Columns["Potrazuje"].HeaderText = "Potrazuje";
            dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Format = "#,##0.00 RSD";

            tb_FinansijskoDobavljacKrajnje.Text = "0.00 RSD";
            tb_FinansijskoDobavljacPocetno.Text = "0.00 RSD";
            tb_FinansijskoKupacKrajnje.Text = "0.00 RSD";
            tb_FinansijskoKupacPocetno.Text = "0.00 RSD";
            tb_KomercijalnoPocetno.Text = "0.00 RSD";
            tb_Komercijalno_Krajnje.Text = "0.00 RSD";
            tb_Partner.Text = "";
        }

        private void _1360_fm_OcekivaneUplate_Index_Load(object sender, EventArgs e)
        {
            tb_KontoKupac.Text = KONTO_KUPAC;
            tb_KontoDobavljac.Text = KONTO_DOBAVLJAC;
        }

        private void btn_Prikazi_Click(object sender, EventArgs e)
        {
        //    if (_partner == null)
        //        return;

        //    int ppid = _partner.PPID;
        //    DateTime datum = DateTime.Now.AddDays(-7);

        //    List<Izvod> ds1 = Izvod.List().Where(x => x.BrDok != 0 && x.PPID == ppid && x.Potrazuje > 0 && x.Konto is null).ToList();
        //    List<Dokument> ds2 = Dokument.List().Where(x => x.VrDok == 90 && x.Datum >= datum).ToList();

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PPID", typeof(int));
        //    //dt.Columns.Add("Naziv", typeof(string));
        //    dt.Columns.Add("DATUM", typeof(DateTime));
        //    dt.Columns.Add("Potrazuje", typeof(double));
        //    dt.Columns.Add("Konto", typeof(string));
        //    dt.Columns.Add("IzvodID", typeof(int));
        //    dt.Columns.Add("BrDok", typeof(int));
        //    dt.Columns.Add("VrDok", typeof(int));

        //    foreach (Izvod izvod in ds1)
        //    {
        //        Dokument d = ds2.Where(x => x.BrDok == izvod.BrDok).FirstOrDefault();
        //        if (d == null)
        //            continue;
        //        if (d.Datum == null)
        //            continue;
        //        DataRow dr = dt.NewRow();
        //        dr["PPID"] = izvod.PPID;
        //        dr["DATUM"] = d.Datum ;
        //        dr["Potrazuje"] = izvod.Potrazuje;
        //        dr["Konto"] = izvod.Konto; 
        //        dr["IzvodID"] = izvod.IzvodID;
        //        dr["BrDok"] = izvod.BrDok;
        //        dr["VrDok"] = izvod.VrDok;

        //        dt.Rows.Add(dr);
        //    }
            
        //    dataGridView1.DataSource = dt;

        //    List<Promene> svePromene = Promene.List();

        //    double PSKF = 0;
        //    foreach (Promene prom in svePromene.Where(x => x.PPID == _partner.PPID && x.Konto == KONTO_KUPAC && x.VrstaNal == 0))
        //    {
        //        PSKF = PSKF + prom.Duguje - prom.Potrazuje;
        //    }
        //    tb_FinansijskoKupacPocetno.Text = PSKF.ToString("#,##0.00 RSD");
        //    double KSKF = 0;
        //    foreach (Promene prom in svePromene.Where(x => x.PPID == _partner.PPID && x.Konto == KONTO_KUPAC))
        //    {
        //        KSKF = KSKF + prom.Duguje - prom.Potrazuje;
        //    }
        //    tb_FinansijskoKupacKrajnje.Text = KSKF.ToString("#,##0.00 RSD");
        //    double PSDF = 0;
        //    foreach (Promene prom in svePromene.Where(x => x.PPID == _partner.PPID && x.Konto == KONTO_DOBAVLJAC && x.Vrstanal == 0))
        //    {
        //        PSDF = PSDF + prom.Potrazuje - prom.Duguje;
        //    }
        //    tb_FinansijskoDobavljacPocetno.Text = PSDF.ToString("#,##0.00 RSD"); 
        //    double KSDF = 0;
        //    foreach (Promene prom in svePromene.Where(x => x.PPID == _partner.PPID && x.Konto == KONTO_DOBAVLJAC))
        //    {
        //        KSDF = KSDF + prom.Potrazuje - prom.Duguje;
        //    }
        //    tb_FinansijskoDobavljacKrajnje.Text = KSDF.ToString("#,##0.00 RSD");
        //    //Komercijalno
        //    DateTime d1 = Convert.ToDateTime("12/31/"+Convert.ToString(DateTime.Now.Year));
        //    Dictionary<string, double> kom = Komercijalno.Procedure.SrediStanjePP(_partner.PPID, null, d1, "din", 0, 1);

        //    double PocSt = kom["POC_STA"];
        //    double krajnje = kom["ULAZ"] - kom["IZLAZ"];
        //    this.tb_KomercijalnoPocetno.Text = PocSt.ToString("#,##0.00 RSD");
        //    this.tb_Komercijalno_Krajnje.Text = krajnje.ToString("#,##0.00 RSD");
        }

        private void btn_IzborPartnera_Click(object sender, EventArgs e)
        {
        //    SetUI();
        //    using (fm_IzborPartnera ip = new fm_IzborPartnera())
        //    {
        //        ip.IzborPartneraSelect += async (IzborPartneraSelectArgs args) =>
        //        {
        //            int selektovaniID = args.PPID;
        //            _partner = await Komercijalno.Partner.GetAsync(selektovaniID);
        //            if (_partner == null)
        //            return;
        //            this.tb_Partner.Text = Convert.ToString(_partner.Naziv);
        //            ip.Close();
        //        };
        //        ip.DozvoliMultiSelect = false;
        //        ip.ShowDialog();
        //    }
        }

    }
}
