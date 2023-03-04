using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDOffice_v2.PDFSharpARExtensions;

namespace TDOffice_v2
{
    public partial class fm_SpecifikacijaNovca_Storno : Form
    {
        public enum StornoTip
        {
            Standard = 0,
            KasaDuplo = 1,
        }
        private TDOffice.Config<Dictionary<int, Dictionary<int, int>>> _brojeviStornoNaloga = TDOffice.Config<Dictionary<int, Dictionary<int, int>>>.Get(TDOffice.ConfigParameter.NalogZaIspravkuPoslednjiBroj); // magacin brMpRacuna dodeljeniBroj
        private TDOffice.SpecifikacijaNovca _specifikacijaNovca { get; set; }
        private StornoTip _tip { get; set; }
        private DataTable _mpRacuniDT = new DataTable();
        private List<Komercijalno.Dokument> _danasnjiMPRacuni { get; set; }

        public fm_SpecifikacijaNovca_Storno(TDOffice.SpecifikacijaNovca specifikacijaNovca, StornoTip stornoTip)
        {
            InitializeComponent();
            #region Tag NULL check
            if (specifikacijaNovca.Tag == null)
            {
                specifikacijaNovca.Tag = new TDOffice.SpecifikacijaNovca.Detalji();
                specifikacijaNovca.Update();
            }

            if(specifikacijaNovca.Tag.StornoKasaDuplo == null)
            {
                specifikacijaNovca.Tag.StornoKasaDuplo = new List<int>();
                specifikacijaNovca.Update();
            }

            if (specifikacijaNovca.Tag.StorniraniMPRacuni == null)
            {
                specifikacijaNovca.Tag.StorniraniMPRacuni = new List<int>();
                specifikacijaNovca.Update();
            }
            #endregion
            _specifikacijaNovca = specifikacijaNovca;
            _tip = stornoTip;
            UcitajMPRacune();
            PrikaziOsnovneMPRacune();
            PrikaziStorniraneMPRacune();
        }
        private void UcitajMPRacune()
        {
            _danasnjiMPRacuni = Komercijalno.Dokument.ListByMagacinID(DateTime.Now.Year, _specifikacijaNovca.MagacinID).Where(x => x.VrDok == 15 && x.Datum == _specifikacijaNovca.Datum).ToList();
            _mpRacuniDT = new DataTable();
            _mpRacuniDT.Columns.Add("BrDok", typeof(int));
            _mpRacuniDT.Columns.Add("Vrednost", typeof(double));
            _mpRacuniDT.Columns.Add("NacinPlacanja", typeof(string));

            foreach (Komercijalno.Dokument dok in _danasnjiMPRacuni)
            { 
                DataRow dr = _mpRacuniDT.NewRow();
                dr["BrDok"] = dok.BrDok;
                dr["Vrednost"] = dok.Potrazuje;
                dr["NacinPlacanja"] = dok.NUID == Komercijalno.NacinUplate.Gotovina ? "Gotovina" : dok.NUID == Komercijalno.NacinUplate.Kartica ? "Kartica" : dok.NUID == Komercijalno.NacinUplate.Odlozeo ?  "Odlozeno" :  "Virman"; 
                _mpRacuniDT.Rows.Add(dr);
            }
        }
        private void PrikaziOsnovneMPRacune()
        {
            List<int> stornoMPRacuni = _tip == StornoTip.KasaDuplo ? _specifikacijaNovca.Tag.StornoKasaDuplo : _specifikacijaNovca.Tag.StorniraniMPRacuni;

            DataTable dt = _mpRacuniDT.Clone();

            foreach (Komercijalno.Dokument dok in _danasnjiMPRacuni.Where(x => !stornoMPRacuni.Contains(x.BrDok)))
            {
                DataRow dr = dt.NewRow();
                dr["BrDok"] = dok.BrDok;
                dr["Vrednost"] = dok.Potrazuje;
                dr["NacinPlacanja"] = dok.NUID == Komercijalno.NacinUplate.Gotovina ? "Gotovina" : dok.NUID == Komercijalno.NacinUplate.Kartica ? "Kartica" : dok.NUID == Komercijalno.NacinUplate.Odlozeo ? "Odlozeno" : "Virman";
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void PrikaziStorniraneMPRacune()
        {
            List<int> stornoMPRacuni = _tip == StornoTip.KasaDuplo ? _specifikacijaNovca.Tag.StornoKasaDuplo : _specifikacijaNovca.Tag.StorniraniMPRacuni;

            DataTable dt = _mpRacuniDT.Clone();

            foreach(int i in stornoMPRacuni)
            {
                DataRow dr = dt.NewRow();
                var dok = _danasnjiMPRacuni.Where(x => x.BrDok == i).FirstOrDefault();
                dr["BrDok"] = dok.BrDok;
                dr["Vrednost"] = dok.Potrazuje;
                dr["NacinPlacanja"] = dok.NUID == Komercijalno.NacinUplate.Gotovina ? "Gotovina" : dok.NUID == Komercijalno.NacinUplate.Kartica ? "Kartica" : dok.NUID == Komercijalno.NacinUplate.Odlozeo ? "Odlozeno" : "Virman";
                dt.Rows.Add(dr);
            }

            dataGridView2.DataSource = dt;

            dataGridView2.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView2.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int brDok = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["BrDok"].Value);

            //List<int> stornoMPRacuni = _tip == StornoTip.KasaDuplo ? _specifikacijaNovca.Tag.StornoKasaDuplo : _tip == StornoTip.KasaStandard ? _specifikacijaNovca.Tag.StornoKasaStandard : _specifikacijaNovca.Tag.StorniraniMPRacuni; ;
            List<int> stornoMPRacuni = _tip == StornoTip.KasaDuplo ? _specifikacijaNovca.Tag.StornoKasaDuplo : _specifikacijaNovca.Tag.StorniraniMPRacuni;

            if (!stornoMPRacuni.Contains(brDok))
            {
                stornoMPRacuni.Add(brDok);

                if (Komercijalno.Dokument.Get(DateTime.Now.Year, 15, brDok).Placen == 1)
                {
                    if (_brojeviStornoNaloga.Tag == null)
                        _brojeviStornoNaloga.Tag = new Dictionary<int, Dictionary<int, int>>();

                    if (!_brojeviStornoNaloga.Tag.ContainsKey(_specifikacijaNovca.MagacinID))
                        _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID] = new Dictionary<int, int>();

                    int poslednji = _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID].Values.Count == 0 ? 0 : _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID].Values.Max();
                    _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID][brDok] = poslednji + 1;

                    _brojeviStornoNaloga.UpdateOrInsert();
                }
            }

            if (_tip == StornoTip.KasaDuplo)
                _specifikacijaNovca.Tag.StornoKasaDuplo = stornoMPRacuni;
            else
                _specifikacijaNovca.Tag.StorniraniMPRacuni = stornoMPRacuni;

            _specifikacijaNovca.Update();

            PrikaziOsnovneMPRacune();
            PrikaziStorniraneMPRacune();
        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int brDok = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["BrDok"].Value);

            List<int> stornoMPRacuni = _tip == StornoTip.KasaDuplo ? _specifikacijaNovca.Tag.StornoKasaDuplo : _specifikacijaNovca.Tag.StorniraniMPRacuni;

            stornoMPRacuni.RemoveAll(x => x == brDok);

            if (Komercijalno.Dokument.Get(DateTime.Now.Year, 15, brDok).Placen == 1)
            {
                if (_brojeviStornoNaloga.Tag == null)
                    _brojeviStornoNaloga.Tag = new Dictionary<int, Dictionary<int, int>>();

                if (!_brojeviStornoNaloga.Tag.ContainsKey(_specifikacijaNovca.MagacinID))
                    _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID] = new Dictionary<int, int>();

                _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID].Remove(brDok);

                _brojeviStornoNaloga.UpdateOrInsert();
            }

            if (_tip == StornoTip.KasaDuplo)
                _specifikacijaNovca.Tag.StornoKasaDuplo = stornoMPRacuni;
            else
                _specifikacijaNovca.Tag.StorniraniMPRacuni = stornoMPRacuni;

            _specifikacijaNovca.Update();

            PrikaziOsnovneMPRacune();
            PrikaziStorniraneMPRacune();
        }

        private void stampajNIObrazacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktirajte administratora!");
            return;

            //int brDok = Convert.ToInt32(dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells["BrDok"].Value);

            //Komercijalno.Dokument dok = Komercijalno.Dokument.Get(DateTime.Now.Year, 15, brDok);
            //if (dok.Placen == 0)
            //{
            //    MessageBox.Show("Racun nije fiskalizovan!");
            //    return;
            //}
            //TDOffice.Magacin magacinTDOffice = TDOffice.Magacin.Get(dok.MagacinID);
            //TDOffice.Firma firmaTDOffice = TDOffice.Firma.Get(magacinTDOffice.FirmaID);
            //List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 15, brDok);
            //List<Komercijalno.Roba> roba = Komercijalno.Roba.List(DateTime.Now.Year);
            //List<Komercijalno.Tarife> tarife = Komercijalno.Tarife.List();

            //Document document = new Document();

            //document.Info.Title = "NI Obrazac";
            //document.Info.Author = "TDOffice_v2";

            //Section section = document.AddSection();
            //section.PageSetup.TopMargin = 15;
            //section.PageSetup.LeftMargin = 15;
            //section.PageSetup.RightMargin = 15;

            //Table table;
            //Paragraph p;
            //Row row;
            //Column column;

            //table = section.AddTable();
            //table.Style = "Table";
            //table.Format.Font.Size = 12;

            //column = table.AddColumn("10cm");
            //column.Format.Alignment = ParagraphAlignment.Center;

            //column = table.AddColumn("10cm");
            //column.Format.Alignment = ParagraphAlignment.Center;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("Obaveznik: " + firmaTDOffice.Naziv);
            //p.Format.Alignment = ParagraphAlignment.Left;
            //p = row.Cells[1].AddParagraph("OBRAZAC NI");
            //p.Format.Alignment = ParagraphAlignment.Right;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("Adresa: " + magacinTDOffice.Adresa);
            //p.Format.Alignment = ParagraphAlignment.Left;
            //p = row.Cells[1].AddParagraph("Racun veze: " + brDok);
            //p.Format.Alignment = ParagraphAlignment.Left;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("PIB: " + firmaTDOffice.PIB);
            //p.Format.Alignment = ParagraphAlignment.Left;
            //p = row.Cells[1].AddParagraph("Napomena: ");
            //p.Format.Alignment = ParagraphAlignment.Left;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("Opstina: " + magacinTDOffice.Opstina);
            //p.Format.Alignment = ParagraphAlignment.Left;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("Datum: " + DateTime.Now.ToString("dd.MM.yyyy"));
            //p.Format.Alignment = ParagraphAlignment.Left;

            //row = table.AddRow();
            //p = row.Cells[0].AddParagraph("Poslovna jedinica: M" + magacinTDOffice.ID);
            //p.Format.Alignment = ParagraphAlignment.Left;

            //section.AddParagraph();

            //if (_brojeviStornoNaloga.Tag == null)
            //    _brojeviStornoNaloga.Tag = new Dictionary<int, Dictionary<int, int>>();

            //if (!_brojeviStornoNaloga.Tag.ContainsKey(_specifikacijaNovca.MagacinID))
            //    _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID] = new Dictionary<int, int>();

            //int brojNaloga = _brojeviStornoNaloga.Tag[_specifikacijaNovca.MagacinID][brDok];
            //p = section.AddParagraph($"NALOG ZA ISPRAVKU Broj: {brojNaloga} po racunu broj " + brDok + " od " + dok.Datum.ToString("dd.MM.yyyy"));
            //p.Format.Alignment = ParagraphAlignment.Center;
            //p.Format.Font.Size = 16;
            //p.Format.Font.Bold = true;

            //int i = 0;
            //double ukupno = 0;
            //double osnovica = 0;
            //double pdv = 0;
            //foreach(Komercijalno.Stavka s in stavke)
            //{
            //    i++;

            //    Komercijalno.Roba r = roba.FirstOrDefault(x => x.ID == s.RobaID);
            //    Komercijalno.Tarife t = tarife.FirstOrDefault(x => x.TarifaID == r.TarifaID);

            //    double cenaBezPDV = (s.ProdCenaBP + (double)s.Korekcija);
            //    double vrednostBezPDV = cenaBezPDV * s.Kolicina;
            //    double cenaSaPDV = cenaBezPDV * ((100 + (double)t.Stopa) / 100);
            //    double vrednostSaPDV = cenaSaPDV * s.Kolicina;

            //    ukupno += vrednostSaPDV;
            //    osnovica += vrednostBezPDV;
            //    pdv += (vrednostSaPDV - vrednostBezPDV);

            //    section.AddParagraph();
            //    p = section.AddParagraph($"{i}) Umanjuje se promet evidentiran u fiskalnom isecku/fiskalnom racunu broj 123 od {dok.Datum.ToString("dd.MM.yyyy")}" +
            //        $" izdatom na fiskalnoj kasi identifikacioni broj AG-NESTO za iznos {vrednostSaPDV.ToString("#,##0.00")}" +
            //        $" na ime vracenog dobra '{s.Naziv}' jedinice mere {r.JM}, kolicine {s.Kolicina.ToString("#,##0.###")}, cene:" +
            //        $" {cenaSaPDV.ToString("#,##0.00")}, vrednosti: {vrednostSaPDV.ToString("#,##0.00")}," +
            //        $" iznosa poreza: {(vrednostSaPDV - vrednostBezPDV).ToString("#,##0.00")}");
            //    p.Format.Font.Size = 12;
            //    p.Format.Alignment = ParagraphAlignment.Justify;

            //    p.Format.Borders.Bottom = new Border() { Color = Colors.Black, Style = MigraDoc.DocumentObjectModel.BorderStyle.Single };
            //}
            //section.AddParagraph();
            //section.AddParagraph();

            //p = section.AddParagraph($"Ukupno: {ukupno.ToString("#,##0.00")}");
            //p.Format.Font.Size = 16;
            //p.Format.Alignment = ParagraphAlignment.Right;

            //p = section.AddParagraph($"Osnovica: {osnovica.ToString("#,##0.00")}");
            //p.Format.Font.Size = 16;
            //p.Format.Alignment = ParagraphAlignment.Right;

            //p = section.AddParagraph($"PDV: {pdv.ToString("#,##0.00")}");
            //p.Format.Font.Size = 16;
            //p.Format.Alignment = ParagraphAlignment.Right;

            //section.AddParagraph();
            //section.AddParagraph();
            //section.AddParagraph();

            //MigraDoc.DocumentObjectModel.Shapes.Image img = section.AddImage(@"C:\Program Files\LimitlessSoft\TDOffice_v2\Assets\StornoNiFooter.jpg");
            //img.ScaleWidth = 1.2;
            //document.Render();
        }
    }
}
