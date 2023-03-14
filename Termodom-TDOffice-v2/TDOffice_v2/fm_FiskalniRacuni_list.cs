using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_FiskalniRacuni_list : Form
    {
        private DataTable dt { get; set; } = new DataTable();
        private List<Komercijalno.PFRS> _pfrs = Komercijalno.PFRS.List(DateTime.Now.Year);
        //private Task<PFRDictionary> _pfr { get; set; } = Termodom.Data.Entities.Komercijalno.PFRDictionary();
        private Task<List<Komercijalno.Stavka>> _stavke = Komercijalno.Stavka.ListAsync(DateTime.Now.Year, "VRDOK = 15 OR VRDOK = 22");
        //private Task<StavkaDictionary> _stavke { get; set; } = Termodom.Data.Entities.Komercijalno.StavkaDictionary();
        private Task<List<Komercijalno.Dokument>> _dokumenti = Komercijalno.Dokument.ListAsync(null, new int[] { 15, 22 });
        //private Task<DokumentDictionary> _dokumenti = Komercijalno.DokumentManager.DictionaryAsync();
        private List<TDOffice.FiskalniRacun> _fiskalniRacuni { get; set; } = new List<TDOffice.FiskalniRacun>();

        private Dictionary<string, List<TDOffice.FiskalniRacun_TaxItem>> _fiskalniRacuniTaxItems { get; set; } = new Dictionary<string, List<TDOffice.FiskalniRacun_TaxItem>>();
        private List<Komercijalno.Magacin> _magacini = Komercijalno.Magacin.ListAsync().Result;
        private List<int> _stornirani { get; set; } = new List<int>();
        /// <summary>
        /// string = ReferentDocumentNumber
        /// </summary>
        private Task<Dictionary<string, Komercijalno.DokumentFisk>> _dokumentFisk = Task.Run(() =>
        {
            Dictionary<string, Komercijalno.DokumentFisk> dict = new Dictionary<string, Komercijalno.DokumentFisk>();
            var list = Komercijalno.DokumentFisk.List(DateTime.Now.Year);
            foreach(var dok in list)
            {
                if (dok.ReferentDocumentNumber == "greska")
                    continue;

                dict.Add(dok.ReferentDocumentNumber, dok);
            }
            return dict;
        });

        private int[] _magaciniZaIzvestaj = new int[]
        {
            12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28
        };
        private string st;

        private static List<Tuple<string, string>> _veza_pfr_firma = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("", "< bilo koja >"),
            new Tuple<string, string>("100005295", "Termodom doo"),
            new Tuple<string, string>("109321256", "Termoshop doo"),
            new Tuple<string, string>("103960626", "Magacin doo"),
            new Tuple<string, string>("0211966710005", "Sasa Ristic"),
        };
        public fm_FiskalniRacuni_list()
        {
            InitializeComponent();
            panel1.DesniKlik_DatumRange();

            for (int i = 0; i < tipTransakcije_clb.Items.Count; i++)
                tipTransakcije_clb.SetItemChecked(i, true);
        }

        private void fm_FiskalniRacuni_list_Load(object sender, EventArgs e)
        {
            List<Tuple<string, string>> fiskalizatori = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("< svi fiskalizatori >", "")
            };

            foreach (Komercijalno.Magacin m in Komercijalno.Magacin.ListAsync().Result)
                if (m.PFRID != null)
                {
                    Komercijalno.PFRS pfr = _pfrs.FirstOrDefault(x => x.PFRID == m.PFRID);
                    fiskalizatori.Add(new Tuple<string, string>($"{m.Naziv} - {pfr.JID}", pfr.JID));
                }

            magacin_cmb.DataSource = fiskalizatori;
            magacin_cmb.DisplayMember = "Item1";
            magacin_cmb.ValueMember = "Item2";

            senderFirma_cmb.DataSource = _veza_pfr_firma;
            senderFirma_cmb.DisplayMember = "Item2";
            senderFirma_cmb.ValueMember = "Item1";

            UcitajFiskalneRacune();
            PopuniDGV();
        }

        private void UcitajFiskalneRacune()
        {
            _fiskalniRacuni = TDOffice.FiskalniRacun.List();

            var taxItems = TDOffice.FiskalniRacun_TaxItem.List();
            foreach (var ti in taxItems)
            {
                if (!_fiskalniRacuniTaxItems.ContainsKey(ti.InvoiceNumber))
                    _fiskalniRacuniTaxItems.Add(ti.InvoiceNumber, new List<TDOffice.FiskalniRacun_TaxItem>());

                _fiskalniRacuniTaxItems[ti.InvoiceNumber].Add(ti);
            }
        }

        private void PopuniDGV()
        {
            dt = new DataTable();
            dt.Columns.Add("SignedBy", typeof(string));
            dt.Columns.Add("InvoiceCounter", typeof(string));
            dt.Columns.Add("VremeFiskalizacije", typeof(DateTime));
            dt.Columns.Add("Cashier", typeof(string));
            dt.Columns.Add("BuyerTin", typeof(string));
            dt.Columns.Add("TotalAmount", typeof(double));
            dt.Columns.Add("InvoiceType", typeof(string));
            dt.Columns.Add("TransactionType", typeof(string));
            dt.Columns.Add("InvoiceNumber", typeof(string));
            dt.Columns.Add("TIN", typeof(string));
            dt.Columns.Add("VrDok", typeof(int));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("PDV", typeof(double));
            dt.Columns.Add("TaxItems", typeof(string));

            double zbirPovratnica = 0;
            double zbirRacuna = 0;
            double zbirPDVPovratnica = 0;
            double zbirPDVRacuna = 0;
            Dictionary<double, double> pdvPoStopamaRacuni = new Dictionary<double, double>();
            Dictionary<double, double> pdvPoStopamaPovratnice = new Dictionary<double, double>();

            foreach (TDOffice.FiskalniRacun fr in _fiskalniRacuni)
            {
                List<TDOffice.FiskalniRacun_TaxItem> taxItems = _fiskalniRacuniTaxItems[fr.InvoiceNumber];
                Komercijalno.DokumentFisk df = _dokumentFisk.Result.ContainsKey(fr.InvoiceNumber) ? _dokumentFisk.Result[fr.InvoiceNumber] : null;

                if(df == null)
                {
                    Task.Run(() =>
                    {
                        MessageBox.Show($"Fiskali racun identifikacije {fr.InvoiceNumber} ne postoji u komercijalnom poslovanju!");
                    });
                }

                if (df != null && df.VrDok == 22)
                    _stornirani.Add((int)Komercijalno.Dokument.Get(DateTime.Now.Year, 22, df.BrDok).Popust1Dana);

                DataRow dr = dt.NewRow();
                dr["InvoiceNumber"] = fr.InvoiceNumber;
                dr["TIN"] = fr.TIN;
                dr["Cashier"] = fr.Cashier;
                dr["VremeFiskalizacije"] = fr.SDCTime_ServerTimeZone;
                dr["BuyerTin"] = fr.BuyerTin;
                dr["InvoiceCounter"] = fr.InvoiceCounter;
                dr["SignedBy"] = fr.SignedBy;
                dr["TotalAmount"] = fr.TotalAmount;
                dr["InvoiceType"] = fr.InvoiceType;
                dr["TransactionType"] = fr.TransactionType;
                dr["VrDok"] = df == null ? -1 : df.VrDok;
                dr["BrDok"] = df == null ? -1 : df.BrDok;
                dr["PDV"] = taxItems.Sum(x => x.Amount);
                dr["TaxItems"] = JsonConvert.SerializeObject(taxItems);
                dt.Rows.Add(dr);

                if (fr.InvoiceType == "Normal" || fr.InvoiceType == "Промет")
                {
                    if(fr.TransactionType == "Sale" || fr.TransactionType == "Продаја")
                    {
                        zbirRacuna += fr.TotalAmount;
                        zbirPDVRacuna += taxItems.Sum(x => x.Amount);

                        foreach (double stopa in taxItems.Select(x => x.Rate).Distinct())
                        {
                            if (!pdvPoStopamaRacuni.ContainsKey(stopa))
                                pdvPoStopamaRacuni.Add(stopa, 0);

                            pdvPoStopamaRacuni[stopa] += taxItems.Where(x => x.Rate == stopa).Sum(x => x.Amount);
                        }
                    }
                    else if(fr.TransactionType == "Refund" || fr.TransactionType == "Рефундација")
                    {
                        zbirPovratnica += fr.TotalAmount;
                        zbirPDVPovratnica += taxItems.Sum(x => x.Amount);

                        foreach (double stopa in taxItems.Select(x => x.Rate).Distinct())
                        {
                            if (!pdvPoStopamaPovratnice.ContainsKey(stopa))
                                pdvPoStopamaPovratnice.Add(stopa, 0);

                            pdvPoStopamaPovratnice[stopa] += taxItems.Where(x => x.Rate == stopa).Sum(x => x.Amount);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Nepoznat transaction type ({fr.TransactionType})");
                    }
                }
            }

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Opis", typeof(string));
            dt2.Columns.Add("Zbir", typeof(double));

            DataRow dr1 = dt2.NewRow();
            dr1["Opis"] = "Promet zbir sa PDV";
            dr1["Zbir"] = zbirRacuna;

            DataRow dr2 = dt2.NewRow();
            dr2["Opis"] = "Refund zbir sa PDV";
            dr2["Zbir"] = zbirPovratnica;

            DataRow dr3 = dt2.NewRow();
            dr3["Opis"] = "Promet PDV";
            dr3["Zbir"] = zbirPDVRacuna;

            DataRow dr4 = dt2.NewRow();
            dr4["Opis"] = "Refund PDV";
            dr4["Zbir"] = zbirPDVPovratnica;

            dt2.Rows.Add(dr1);
            dt2.Rows.Add(dr2);
            dt2.Rows.Add(dr3);
            dt2.Rows.Add(dr4);

            foreach(double key in pdvPoStopamaRacuni.Keys)
            {
                DataRow d = dt2.NewRow();
                d["Opis"] = $"Promet PDV ({key}%)";
                dt2.Rows.Add(d);
            }

            foreach (double key in pdvPoStopamaPovratnice.Keys)
            {
                DataRow d = dt2.NewRow();
                d["Opis"] = $"Refund PDV ({key}%)";
                dt2.Rows.Add(d);
            }

            dataGridView2.DataSource = dt2;
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["InvoiceNumber"].Width = 200;
            dataGridView1.Columns["Cashier"].Width = 100;

            dataGridView1.Columns["TotalAmount"].DefaultCellStyle.Format = "#,##0.00 RSD";

            dataGridView1.Columns["VremeFiskalizacije"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";

            dataGridView1.Columns["InvoiceNumber"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";

            dataGridView1.Columns["TaxItems"].Visible = false;

            dataGridView2.Columns["Zbir"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            ObojiDGV();
            DataRow[] rezultat = dt.Select(@"InvoiceType <> 'Normal' AND 
InvoiceType <> 'Промет' AND 
InvoiceType <> 'Refund' AND
InvoiceType <> 'Рефундација' AND 
InvoiceType <> 'Copy' AND 
InvoiceType <> 'Копија' AND 
InvoiceType <> 'Advance' AND 
InvoiceType <> 'Training' AND 
InvoiceType <> 'Обука'");
            if (rezultat.Length > 0)
            {
                st = "  Type\tInvoiceType\t\tSignedBy\n";
                st = st + "===========================================\n";
                foreach (DataRow dr in rezultat)
                {
                    st += $"{ dr["InvoiceType"]}   { dr["InvoiceNumber"]}   { dr["SignedBy"]},\n";
                }
                MessageBox.Show(st, "Fiskalni racuni nepoznatog tipa");
            }
        }
        private void ObojiDGV()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["VrDok"].Value) == 15 && _stornirani.Contains(Convert.ToInt32(dataGridView1.Rows[i].Cells["BrDok"].Value)))
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Coral;
        }
        private void uvuciFiskalne_btn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            UcitajFiskalneRacune();

            var format = "dd.MM.yyyy. H:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            //foreach (string f in openFileDialog1.FileNames)
            //{
            //    List<DTO.Fiskalizacija.FiskalniRacun> frs = JsonConvert.DeserializeObject<List<DTO.Fiskalizacija.FiskalniRacun>>(File.ReadAllText(f), dateTimeConverter);
            //    using(FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
            //    {
            //        con.Open();
            //        foreach(DTO.Fiskalizacija.FiskalniRacun fr in frs)
            //        {
            //            if (_fiskalniRacuni.Any(x => x.InvoiceNumber == fr.InvoiceNumber))
            //                continue;

            //            _fiskalniRacuni.Add(new TDOffice.FiskalniRacun()
            //            {
            //                InvoiceNumber = fr.InvoiceNumber,
            //                InvoiceCounter = fr.InvoiceCounter,
            //                BuyersCostCenter = fr.BuyersCostCenter,
            //                DateAndTimeOfPos = fr.DateAndTimeOfPos,
            //                TotalAmount = fr.TotalAmount,
            //                BuyerTin = fr.BuyerTin,
            //                Cashier = fr.Cashier,
            //                InvoiceType = fr.InvoiceType,
            //                PaymentMethod = fr.PaymentMethod,
            //                PosInvoiceNumber = fr.PosInvoiceNumber,
            //                RequestedBy = fr.RequestedBy,
            //                SDCTime_ServerTimeZone = fr.SDCTime_ServerTimeZone,
            //                SignedBy = fr.SignedBy,
            //                TIN = fr.TIN,
            //                TransactionType = fr.TransactionType
            //            });

            //            TDOffice.FiskalniRacun.Insert(con, fr.InvoiceNumber, fr.TIN, fr.RequestedBy, fr.DateAndTimeOfPos, fr.Cashier,
            //                fr.BuyerTin, fr.BuyersCostCenter, fr.PosInvoiceNumber, fr.PaymentMethod, fr.SDCTime_ServerTimeZone,
            //                fr.InvoiceCounter, fr.SignedBy, fr.TotalAmount, fr.TransactionType, fr.InvoiceType);

            //            foreach (var ti in fr.TaxItems)
            //            {
            //                if (!_fiskalniRacuniTaxItems.ContainsKey(fr.InvoiceNumber))
            //                    _fiskalniRacuniTaxItems.Add(fr.InvoiceNumber, new List<TDOffice.FiskalniRacun_TaxItem>());

            //                _fiskalniRacuniTaxItems[fr.InvoiceNumber].Add(new TDOffice.FiskalniRacun_TaxItem()
            //                {
            //                    Amount = ti.Amount,
            //                    CategoryName = ti.CategoryName,
            //                    InvoiceNumber = fr.InvoiceNumber,
            //                    Label = ti.Label,
            //                    Rate = ti.Rate
            //                });
            //                TDOffice.FiskalniRacun_TaxItem.Insert(con, fr.InvoiceNumber, ti.Label, ti.Amount, ti.Rate, ti.CategoryName);
            //            }
            //        }
            //    }
            //    PopuniDGV();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable cdt = dt.Clone();
            List<string> selectString = new List<string>();

            if (magacin_cmb.SelectedIndex > 0)
                selectString.Add($"SignedBy = '{magacin_cmb.SelectedValue.ToString()}'");

            DateTime odDatuma = new DateTime(odDatuma_dtp.Value.Year, odDatuma_dtp.Value.Month, odDatuma_dtp.Value.Day, 0, 0, 0);
            DateTime doDatuma = new DateTime(doDatuma_dtp.Value.Year, doDatuma_dtp.Value.Month, doDatuma_dtp.Value.Day, 23, 59, 59);
            selectString.Add($"VremeFiskalizacije >= '{odDatuma.ToString()}' AND VremeFiskalizacije <= '{doDatuma.ToString()}'");

            List<string> tipSelectitems = new List<string>();
            foreach (string s in tipTransakcije_clb.CheckedItems)
            {
                switch(s)
                {
                    case "Prodaja - Promet":
                        tipSelectitems.Add($"(InvoiceType = 'Normal' AND TransactionType = 'Sale') OR (InvoiceType = 'Промет' AND TransactionType = 'Продаја')");
                        break;
                    case "Prodaja - Kopija":
                        tipSelectitems.Add($"(InvoiceType = 'Copy' AND TransactionType = 'Sale') OR (InvoiceType = 'Копија' AND TransactionType = 'Продаја')");
                        break;
                    case "Prodaja - Obuka":
                        tipSelectitems.Add($"(InvoiceType = 'Training' AND TransactionType = 'Sale') OR (InvoiceType = 'Обука' AND TransactionType = 'Продаја')");
                        break;
                    case "Prodaja - Avans":
                        tipSelectitems.Add($"(InvoiceType = 'Advance' AND TransactionType = 'Sale')");
                        break;
                    case "Povracaj - Kopija":
                        tipSelectitems.Add($"(InvoiceType = 'Copy' AND TransactionType = 'Refund') OR (InvoiceType = 'Копија' AND TransactionType = 'Рефундација')");
                        break;
                    case "Povracaj - Promet":
                        tipSelectitems.Add($"(InvoiceType = 'Normal' AND TransactionType = 'Refund') OR (InvoiceType = 'Промет' AND TransactionType = 'Рефундација')");
                        break;
                    case "Povracaj - Obuka":
                        tipSelectitems.Add($"(InvoiceType = 'Training' AND TransactionType = 'Refund') OR (InvoiceType = 'Обука' AND TransactionType = 'Рефундација')");
                        break;
                    case "Povracaj - Avans":
                        tipSelectitems.Add($"(InvoiceType = 'Advance' AND TransactionType = 'Refund')");
                        break;
                }
            }

            if(tipSelectitems.Count > 0)
                selectString.Add("(" + String.Join(" OR ", tipSelectitems) + ")");

            if (!string.IsNullOrWhiteSpace(pib_txt.Text))
                selectString.Add($"(BuyerTin Like '%{pib_txt.Text.Trim()}')");

            string selektovanaSenderFirma = senderFirma_cmb.SelectedValue.ToStringOrDefault();
            if (!string.IsNullOrWhiteSpace(selektovanaSenderFirma))
                selectString.Add($"(TIN Like '%{selektovanaSenderFirma}')");

            DataRow[] rows = dt.Select(String.Join(" AND ", selectString));

            double zbirPovratnica = 0;
            double zbirRacuna = 0;
            double zbirPDVPovratnica = 0;
            double zbirPDVRacuna = 0;
            Dictionary<double, double> pdvPoStopamaRacuni = new Dictionary<double, double>();
            Dictionary<double, double> pdvPoStopamaPovratnice = new Dictionary<double, double>();

            foreach (DataRow r in rows)
            {
                DataRow ndr = cdt.NewRow();
                ndr.ItemArray = (object[])r.ItemArray.Clone();
                cdt.Rows.Add(ndr);
                string invoiceType = r["InvoiceType"].ToString();
                string transactionType = r["TransactionType"].ToString();
                double amount = Convert.ToDouble(r["TotalAmount"]);
                double pdv = Convert.ToDouble(r["PDV"]);

                if (invoiceType == "Normal" || invoiceType == "Промет")
                {
                    List<TDOffice.FiskalniRacun_TaxItem> taxItems = JsonConvert.DeserializeObject<List<TDOffice.FiskalniRacun_TaxItem>>(r["TaxItems"].ToString());
                    if (transactionType == "Sale" || transactionType == "Продаја")
                    {
                        zbirRacuna += amount;
                        zbirPDVRacuna += pdv;

                        foreach (double stopa in taxItems.Select(x => x.Rate).Distinct())
                        {
                            if (!pdvPoStopamaRacuni.ContainsKey(stopa))
                                pdvPoStopamaRacuni.Add(stopa, 0);

                            pdvPoStopamaRacuni[stopa] += taxItems.Where(x => x.Rate == stopa).Sum(x => x.Amount);
                        }
                    }
                    else if (transactionType == "Refund" || transactionType == "Рефундација")
                    {
                        zbirPovratnica += amount;
                        zbirPDVPovratnica += pdv;

                        foreach (double stopa in taxItems.Select(x => x.Rate).Distinct())
                        {
                            if (!pdvPoStopamaPovratnice.ContainsKey(stopa))
                                pdvPoStopamaPovratnice.Add(stopa, 0);

                            pdvPoStopamaPovratnice[stopa] += taxItems.Where(x => x.Rate == stopa).Sum(x => x.Amount);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Greska!");
                    }
                }
            }

            dataGridView1.DataSource = cdt;

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Opis", typeof(string));
            dt2.Columns.Add("Zbir", typeof(double));

            DataRow dr1 = dt2.NewRow();
            dr1["Opis"] = "Promet zbir sa PDV";
            dr1["Zbir"] = zbirRacuna;

            DataRow dr2 = dt2.NewRow();
            dr2["Opis"] = "Refund zbir sa PDV";
            dr2["Zbir"] = zbirPovratnica;

            DataRow dr3 = dt2.NewRow();
            dr3["Opis"] = "Promet PDV";
            dr3["Zbir"] = zbirPDVRacuna;

            DataRow dr4 = dt2.NewRow();
            dr4["Opis"] = "Refund PDV";
            dr4["Zbir"] = zbirPDVPovratnica;

            dt2.Rows.Add(dr1);
            dt2.Rows.Add(dr2);
            dt2.Rows.Add(dr3);
            dt2.Rows.Add(dr4);

            foreach (double key in pdvPoStopamaRacuni.Keys)
            {
                DataRow d = dt2.NewRow();
                d["Opis"] = $"Promet PDV ({key}%)";
                dt2.Rows.Add(d);
            }

            foreach (double key in pdvPoStopamaPovratnice.Keys)
            {
                DataRow d = dt2.NewRow();
                d["Opis"] = $"Refund PDV ({key}%)";
                dt2.Rows.Add(d);
            }

            dataGridView2.DataSource = dt2;

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            ObojiDGV();
        }

        private double GetVrednostPovratnicaNaDan(int magacinID, DateTime datum)
        {
            List<Komercijalno.Dokument> doks = Komercijalno.Dokument.List("VRDOK = 22 AND MAGACINID = " + magacinID + " AND DATUM = '" + datum.ToString("dd.MM.yyyy") + "'");
            if (doks.Count == 0)
                return 0;

            return doks.Sum(x => x.Potrazuje);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            UcitajFiskalneRacune();

            DataTable dt = new DataTable();
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("MagacinID", typeof(int));
            dt.Columns.Add("Magacin", typeof(string));
            dt.Columns.Add("NeslaganjeNaNivouDana", typeof(double));

            for (int i = 0; i <= (doDatuma_dtp.Value.Date - odDatuma_dtp.Value.Date).TotalDays; i++)
            {
                DateTime datum = doDatuma_dtp.Value.Date.AddDays(-i);
                List<Tuple<int, double>> prometiNaDan = Komercijalno.Magacin.PrometList(datum);

                List<int> mags = new List<int>();
                if (magacin_cmb.SelectedIndex == 0)
                    mags.AddRange(_magaciniZaIzvestaj);
                else
                    mags.Add(_magacini.FirstOrDefault(x => x.PFRID == _pfrs.FirstOrDefault(y => y.JID == magacin_cmb.SelectedValue.ToString()).PFRID).ID);

                foreach (int mag in mags)
                {
                    try
                    {
                        var pfr = _pfrs.FirstOrDefault(x => x.PFRID == _magacini.FirstOrDefault(y => y.ID == mag).PFRID);

                        if (pfr == null)
                            continue;

                        string pfrMagacina = pfr.JID;
                        double prometNaDan = prometiNaDan.FirstOrDefault(x => x.Item1 == mag).Item2 + GetVrednostPovratnicaNaDan(mag, datum);
                        double ukupanFiskalizovanPometProdaja = _fiskalniRacuni
                            .Where(x =>
                                x.SignedBy == pfrMagacina &&
                                x.SDCTime_ServerTimeZone.Date == datum.Date &&
                                (x.InvoiceType == "Normal" || x.InvoiceType == "Промет") &&
                                (x.TransactionType == "Sale" || x.TransactionType == "Продаја"))
                            .Sum(x => x.TotalAmount);
                        double ukupanFiskalizovanRefund = _fiskalniRacuni
                            .Where(x =>
                                x.SignedBy == pfrMagacina &&
                                x.SDCTime_ServerTimeZone.Date == datum.Date &&
                                (x.InvoiceType == "Normal" || x.InvoiceType == "Промет") &&
                                (x.TransactionType == "Refund" || x.TransactionType == "Рефундација"))
                            .Sum(x => x.TotalAmount);

                        if(Math.Abs((prometNaDan - (ukupanFiskalizovanPometProdaja + ukupanFiskalizovanRefund))) > 1)
                        {
                            List<Komercijalno.Dokument> dokumentiUDanu = _dokumenti.Result.Where(x => x.Datum.Date == datum.Date && x.MagacinID == mag).ToList();
                            List<Komercijalno.DokumentFisk> dfs = new List<Komercijalno.DokumentFisk>(_dokumentFisk.Result.Values).Where(x => x.InvoiceType == 0).ToList();
                            dokumentiUDanu.RemoveAll(x => dfs.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok));
                            List<Komercijalno.Stavka> stavkeUDanu = _stavke.Result.Where(x => dokumentiUDanu.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok)).ToList();
                            List<TDOffice.FiskalniRacun> ffs = new List<TDOffice.FiskalniRacun>(_fiskalniRacuni).Where(x =>
                                    x.SignedBy == pfrMagacina &&
                                    x.SDCTime_ServerTimeZone.Date == datum.Date &&
                                    x.InvoiceType == "Normal"
                                ).ToList();
                            ffs.RemoveAll(x => dfs.Any(y => y.ReferentDocumentNumber == x.InvoiceNumber));

                            List<TDOffice.FiskalniRacun_TaxItem> taxItems = new List<TDOffice.FiskalniRacun_TaxItem>();
                            foreach(TDOffice.FiskalniRacun f in ffs)
                                taxItems.AddRange(_fiskalniRacuniTaxItems[f.InvoiceNumber]);

                            List<double> porezi = stavkeUDanu.Select(x => x.Porez).Distinct().ToList();
                            porezi.AddRange(taxItems.Where(x => !porezi.Contains(x.Rate)).Select(x => x.Rate));

                            var a = (ukupanFiskalizovanPometProdaja + ukupanFiskalizovanRefund);

                            foreach(double p in porezi)
                                if (!dt.Columns.Contains($"PDV Neslaganje ({p}%)"))
                                    dt.Columns.Add($"PDV Neslaganje ({p}%)");


                            DataRow dr = dt.NewRow();
                            dr["Datum"] = datum;
                            dr["MagacinID"] = mag;
                            dr["Magacin"] = _magacini.FirstOrDefault(x => x.ID == mag).Naziv;
                            dr["NeslaganjeNaNivouDana"] = (prometNaDan - a);
                            foreach (double p in porezi)
                                dr[$"PDV Neslaganje ({p}%)"] =
                                    stavkeUDanu.Sum(x => x.Kolicina * (x.ProdajnaCena * x.Porez / 100)) + taxItems.Where(x => x.Rate == p).Sum(x => x.Amount);
                            dt.Rows.Add(dr);
                        }
                    }
                    catch
                    {

                    }
                }
            }

            Task.Run(() =>
            {
                DataGridViewSelectBox sb = new DataGridViewSelectBox(dt);
                sb.RowHeaderVisible = false;
                sb.Text = "Neslaganja poreske uprave i komercijalnog poslovanja zbirno po danu";
                sb.OnRowSelected += (args) =>
                {
                    using (_1344_fm_SpecifikacijaNovca_Index si = new _1344_fm_SpecifikacijaNovca_Index())
                    {
                        si.Shown += (a1, a2) =>
                        {
                            si.Datum = Convert.ToDateTime(args.SelectedRow["Datum"]);
                            si.MagacinID = Convert.ToInt32(args.SelectedRow["MagacinID"]);
                        };
                        si.ShowDialog();
                    }
                    DataRow dr = args.SelectedRow;
                };
                sb.ShowDialog();
            });
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiDGV();
        }
    }
}
