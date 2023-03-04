using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_KarticaPartnera : Form
    {
        List<Dokument> _dokumentiPartnera { get; set; }
        List<Dokument> _sviDokumenti { get; set; }
        List<Izvod> _izvodi { get; set; }
        List<IstUpl> _istorijeUplata { get; set; }
        Partner _partner { get; set; }

        DataTable dt = new DataTable();

        public fm_KarticaPartnera(List<Dokument> dokumenti, List<Izvod> izvodi, List<IstUpl> istorijeUplata, int PPID)
        {
            InitializeComponent();

            _sviDokumenti = dokumenti;
            _dokumentiPartnera = dokumenti.Where(x => x.PPID == PPID).ToList();
            _partner = Partner.GetAsync(PPID).Result;
            _izvodi = izvodi.Where(x => x.PPID == PPID && new int[] { 90, 95 }.Contains(x.VrDok)).ToList();
            _istorijeUplata = istorijeUplata.Where(x => x.PPID == PPID).ToList();

            miIsplatili_txt.Text = _izvodi.Sum(x => x.Duguje).ToString("#,##0.00 RSD");
            namaUplatio_txt.Text = _izvodi.Sum(x => x.Potrazuje).ToString("#,##0.00 RSD");

            NamestiDGV();
            UcitajPodatke();
            dataGridView1.DataSource = dt;
        }

        private void fm_KarticaPartnera_Load(object sender, EventArgs e)
        {
            ObojiDGV();
        }


        private void NamestiDGV()
        {
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("Dokument", typeof(string));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("Izlaz", typeof(double));
            dt.Columns.Add("Ulaz", typeof(double));
            dt.Columns.Add("Stanje", typeof(double));

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView1.Columns["Datum"].Width = 70;

            dataGridView1.Columns["Dokument"].Width = 180;

            dataGridView1.Columns["BrDok"].Width = 70;

            dataGridView1.Columns["Izlaz"].Width = 100;
            dataGridView1.Columns["Izlaz"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Izlaz"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["Ulaz"].Width = 100;
            dataGridView1.Columns["Ulaz"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Ulaz"].DefaultCellStyle.Format = "#,##0.00";

            dataGridView1.Columns["Stanje"].Width = 100;
            dataGridView1.Columns["Stanje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Stanje"].DefaultCellStyle.Format = "#,##0.00";
        }
        private void ObojiDGV()
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                double ulaz = Convert.ToDouble(row.Cells["Ulaz"].Value);
                double izlaz = Convert.ToDouble(row.Cells["Ulaz"].Value);

                if (ulaz - izlaz > 0)
                    row.DefaultCellStyle.ForeColor = Color.LightGreen;
                else if (ulaz - izlaz < 0)
                    row.DefaultCellStyle.ForeColor = Color.IndianRed;
                else
                    row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
        }
        private void UcitajPodatke()
        {
            DataRow dr = null;

            Termodom.Data.Entities.Komercijalno.VrstaDokDictionary vrsteDok = VrstaDokManager.DictionaryAsync().GetAwaiter().GetResult();

            foreach (IstUpl psKupac in _istorijeUplata.Where(x => x.VrDok == -61 && x.Datum.Day == 1 && x.Datum.Month == 1).ToList())
            {
                dr = dt.NewRow();
                dr["Datum"] = psKupac.Datum;
                dr["Dokument"] = "Pocetno stanje kupac ( -61 )";
                dr["BrDok"] = psKupac.BrDok;
                dr["Ulaz"] = psKupac.Iznos;
                dr["Izlaz"] = 0;
                dr["Stanje"] = -999999999999;
                dt.Rows.Add(dr);
            }

            foreach (IstUpl psDobavljac in _istorijeUplata.Where(x => x.VrDok == -59 && x.Datum.Day == 1 && x.Datum.Month == 1).ToList())
            {
                dr = dt.NewRow();
                dr["Datum"] = psDobavljac.Datum;
                dr["Dokument"] = "Pocetno stanje dobavljac ( -59 )";
                dr["BrDok"] = psDobavljac.BrDok;
                dr["Izlaz"] = psDobavljac.Iznos;
                dr["Ulaz"] = 0;
                dr["Stanje"] = -999999999999;
                dt.Rows.Add(dr);
            }

            foreach (Dokument dok in _dokumentiPartnera.Where(x => new int[] { 10, 13, 14, 15, 39, 40 }.Contains(x.VrDok)))
            {
                dr = dt.NewRow();

                dr["Datum"] = dok.Datum;
                Termodom.Data.Entities.Komercijalno.VrstaDok vd = vrsteDok.Values.Where(x => x.VrDok == dok.VrDok).FirstOrDefault();
                dr["Dokument"] = vd == null ? "unknown" : vd.NazivDok + string.Format(" [ {0} ] ", dok.NUID.ToString());
                dr["BrDok"] = dok.BrDok;
                if (new int[] { 13, 14, 15, 40 }.Contains(dok.VrDok))
                {
                    dr["Izlaz"] = new int[] { 13, 40 }.Contains(dok.VrDok) ? dok.Duguje : dok.Potrazuje;
                    dr["Ulaz"] = new NacinUplate[] { NacinUplate.Gotovina, NacinUplate.Kartica }.Contains(dok.NUID) ? dok.Potrazuje : 0;
                }
                else
                {
                    dr["Ulaz"] = new int[] { 10, 39 }.Contains(dok.VrDok) ? dok.Duguje : dok.Potrazuje;
                    dr["Izlaz"] = new int[] { 10 }.Contains(dok.VrDok) && dok.NUID == NacinUplate.Gotovina ? dok.Duguje : 0;
                }
                dr["Stanje"] = -999999999999;

                dt.Rows.Add(dr);
            }

            List<Dokument> dokumentiIzvoda = _sviDokumenti.Where(x => new int[] { 90, 95 }.Contains(x.VrDok)).ToList();

            foreach (Izvod izv in _izvodi)
            {
                Dokument dokIzvoda = dokumentiIzvoda.Where(x => x.VrDok == izv.VrDok && x.BrDok == izv.BrDok).FirstOrDefault();

                dr = dt.NewRow();

                dr["Datum"] = dokIzvoda.Datum;
                dr["Dokument"] = izv.VrDok == 90 ? "Uplata po izvodu" : "Kompenzacija ( " + (izv.Duguje + izv.Potrazuje) + " )";
                dr["BrDok"] = dokIzvoda.BrDok;
                dr["Izlaz"] = izv.VrDok == 90 ? izv.Duguje : 0;
                dr["Ulaz"] = izv.VrDok == 90 ? izv.Potrazuje : 0;

                dr["Stanje"] = -999999999999;

                dt.Rows.Add(dr);
            }

            PreracunajStanje();
        }
        private void PreracunajStanje()
        {
            DataView dv = dt.DefaultView;
            dv.Sort = "Datum asc";
            dt = dv.ToTable();

            double trenutnoStanjePartnera = 0;
            double ukupanUlaz = 0;
            double ukupanIzlaz = 0;

            foreach(DataRow dr in dt.Rows)
            {
                ukupanUlaz += (double)dr["Ulaz"];
                ukupanIzlaz += (double)dr["Izlaz"];
                double novoStanjePartnera = trenutnoStanjePartnera + (double)dr["Ulaz"] - (double)dr["Izlaz"];
                dr["Stanje"] = novoStanjePartnera;
                trenutnoStanjePartnera = novoStanjePartnera;
            }

            ulaz_txt.Text = ukupanUlaz.ToString("#,##0.00 RSD");
            izlaz_txt.Text = ukupanIzlaz.ToString("#,##0.00 RSD");
        }
    }
}
