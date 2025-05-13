using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TDOffice_v2.PDFSharpARExtensions;

namespace TDOffice_v2
{
	public partial class fm_Cekovi_Stampa_Setup : Form
	{
		private List<TDOffice.Cek> _cekoviZaStampu;
		private TDOffice.Config<Dictionary<int, string>> _tekuciRacuniZaCekove = TDOffice.Config<
			Dictionary<int, string>
		>.Get(TDOffice.ConfigParameter.TekuciRacunZaCekove);

		public fm_Cekovi_Stampa_Setup(List<TDOffice.Cek> cekoviZaStampu)
		{
			MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktirajte administratora!");
			return;

			InitializeComponent();

			//if (_tekuciRacuniZaCekove.Tag == null)
			//{
			//    _tekuciRacuniZaCekove.Tag = new Dictionary<int, string>();
			//    _tekuciRacuniZaCekove.UpdateOrInsert();
			//}

			//_cekoviZaStampu = cekoviZaStampu;

			//firma_cmb.Enabled = false;
			//mesto_cmb.Enabled = false;
			//prijemnoMesto_cmb.Enabled = false;
			//odDatuma_dtp.Enabled = false;

			//prijemnoMesto_cmb.DisplayMember = "Naziv";
			//prijemnoMesto_cmb.ValueMember = "BankaID";
			//prijemnoMesto_cmb.DataSource = Komercijalno.Banka.List(DateTime.Now.Year);

			//mesto_cmb.DisplayMember = "Naziv";
			//mesto_cmb.ValueMember = "ID";
			//mesto_cmb.DataSource = new List<TDOffice.Grad>(TDOffice.Grad.List());

			//firma_cmb.DisplayMember = "Naziv";
			//firma_cmb.ValueMember = "ID";
			//firma_cmb.DataSource = new List<TDOffice.Firma>(TDOffice.Firma.List());

			//odDatuma_dtp.Value = DateTime.Now;

			//firma_cmb.Enabled = true;
			//mesto_cmb.Enabled = true;
			//prijemnoMesto_cmb.Enabled = true;
			//odDatuma_dtp.Enabled = true;
		}

		private void fm_Cekovi_Stampa_Setup_Load(object sender, EventArgs e) { }

		private void button1_Click(object sender, EventArgs e)
		{
			//List<int> bankeCekova = _cekoviZaStampu.Select(x => x.PodnosilacBanka).Distinct().ToList();

			//// Proveravam da li su sve banke iz cekova postojece u bazi

			//List<Komercijalno.Banka> banke = Komercijalno.Banka.List(DateTime.Now.Year);
			//foreach(var c in _cekoviZaStampu)
			//    if(!banke.Any(x => x.BankaID == c.PodnosilacBanka))
			//    {
			//        MessageBox.Show($"Cek sa IDem {c.ID} ima banku koje nema u bazi!");
			//        return;
			//    }

			//foreach (int podnosilacBanka in bankeCekova)
			//{
			//    var trasat = Komercijalno.Banka.Get(DateTime.Now.Year, podnosilacBanka);
			//    if(!_tekuciRacuniZaCekove.Tag.ContainsKey(podnosilacBanka))
			//    {
			//        MessageBox.Show($"Banka {trasat.Naziv} nema upisan racun za cekove!");
			//        continue;
			//    }
			//    int tableFontSize = 14;
			//    Unit rowHeight = Unit.FromCentimeter(1);

			//    Document document = new Document();
			//    document.Info.Title = "Specifikacija Cekova";
			//    document.Info.Author = "TDOffice_v2";

			//    Section section = document.AddSection();
			//    section.PageSetup.TopMargin = 50;
			//    section.PageSetup.LeftMargin = 25;
			//    section.PageSetup.BottomMargin = 25;
			//    section.PageSetup.RightMargin = 25;

			//    Table table = section.AddTable();

			//    table.Style = "Table";
			//    table.Borders.Color = Colors.Black;
			//    table.Borders.Width = 0.25;
			//    table.Borders.Left.Width = 0.5;
			//    table.Borders.Right.Width = 0.5;
			//    table.Rows.LeftIndent = 0;

			//    Column column1 = table.AddColumn(Unit.FromCentimeter(3));
			//    column1.Format.Alignment = ParagraphAlignment.Left;
			//    Column column2 = table.AddColumn(Unit.FromCentimeter(6));
			//    column2.Format.Alignment = ParagraphAlignment.Left;
			//    Column columnM = table.AddColumn(Unit.FromCentimeter(1));
			//    columnM.Format.Alignment = ParagraphAlignment.Left;
			//    Column column3 = table.AddColumn(Unit.FromCentimeter(3));
			//    column3.Format.Alignment = ParagraphAlignment.Left;
			//    Column column4 = table.AddColumn(Unit.FromCentimeter(6));
			//    column4.Format.Alignment = ParagraphAlignment.Left;

			//    TDOffice.Firma podnosilac = TDOffice.Firma.Get(Convert.ToInt32(firma_cmb.SelectedValue));
			//    Komercijalno.Banka banka = Komercijalno.Banka.Get(DateTime.Now.Year, Convert.ToInt32(prijemnoMesto_cmb.SelectedValue));

			//    Row row = table.AddRow();
			//    row.Height = rowHeight;
			//    row.Cells[0].AddParagraph("Podnosilac cekova");
			//    row.Cells[1].AddParagraph(podnosilac.Naziv).Format.Font.Size = 16;
			//    row.Cells[3].AddParagraph("Banka - posta (prijemno mesto)");
			//    row.Cells[4].AddParagraph(banka.Naziv).Format.Font.Size = 16;

			//    Row row1 = table.AddRow();
			//    row1.Height = rowHeight;
			//    row1.Cells[0].AddParagraph("Broj racuna podnosioca");
			//    row1.Cells[1].AddParagraph(podnosilac.TekuciRacun).Format.Font.Size = 16;
			//    row1.Cells[3].AddParagraph("Broj racuna prijemnog mesta");
			//    row1.Cells[4].AddParagraph(banka.ZiroRacun).Format.Font.Size = 16;

			//    Row row2 = table.AddRow();
			//    row2.Height = rowHeight;
			//    row2.Cells[0].AddParagraph("Mesto");
			//    row2.Cells[1].AddParagraph(podnosilac.Grad).Format.Font.Size = 16;
			//    row2.Cells[3].AddParagraph("Mesto");
			//    row2.Cells[4].AddParagraph(podnosilac.Grad).Format.Font.Size = 16;

			//    section.AddParagraph();

			//    Paragraph p = section.AddParagraph("S P E C I F I K A C I J A");
			//    p.Format.Font.Bold = true;
			//    p.Format.Font.Size = 18;
			//    p.Format.Alignment = ParagraphAlignment.Center;

			//    Paragraph p1 = section.AddParagraph("cekova po tekucim racunima gradjana br. __________");
			//    p1.Format.Font.Bold = false;
			//    p1.Format.Font.Size = 14;
			//    p1.Format.Alignment = ParagraphAlignment.Center;

			//    section.AddParagraph();

			//    table = section.AddTable();

			//    table.Style = "Table";
			//    table.Borders.Color = Colors.Black;
			//    table.Borders.Width = 0.25;
			//    table.Borders.Left.Width = 0.5;
			//    table.Borders.Right.Width = 0.5;
			//    table.Rows.LeftIndent = 0;

			//    Column column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(19);
			//    column.Format.Alignment = ParagraphAlignment.Left;

			//    rowHeight = tableFontSize;

			//    row = table.AddRow();
			//    row.Height = rowHeight;
			//    p = row.Cells[0].AddParagraph("Naziv trasata: " + trasat.Naziv);
			//    p.Format.Font.Size = tableFontSize;

			//    row = table.AddRow();
			//    row.Height = rowHeight;
			//    p = row.Cells[0].AddParagraph("Broj racuna trasata: " + _tekuciRacuniZaCekove.Tag[podnosilacBanka]);
			//    p.Format.Font.Size = tableFontSize;

			//    table = section.AddTable();

			//    table.Style = "Table";
			//    table.Format.Font.Size = tableFontSize;
			//    table.Borders.Color = Colors.Black;
			//    table.Borders.Width = 0.25;
			//    table.Borders.Left.Width = 0.5;
			//    table.Borders.Right.Width = 0.5;
			//    table.Rows.LeftIndent = 0;

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(1);
			//    column.Format.Alignment = ParagraphAlignment.Left;

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(4);
			//    column.Format.Alignment = ParagraphAlignment.Center;

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(4);
			//    column.Format.Alignment = ParagraphAlignment.Right;

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(6);
			//    column.Format.Alignment = ParagraphAlignment.Right;

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(4);
			//    column.Format.Alignment = ParagraphAlignment.Right;

			//    row = table.AddRow();
			//    row.Height = rowHeight;
			//    row.Cells[0].AddParagraph("Rb");
			//    row.Cells[1].AddParagraph("Serijski broj ceka");
			//    row.Cells[2].AddParagraph("Datum izdavanja ceka");
			//    row.Cells[3].AddParagraph("Broj tekuceg racuna gradjana");
			//    row.Cells[4].AddParagraph("Iznos ceka");

			//    int i = 0;
			//    foreach (TDOffice.Cek cek in _cekoviZaStampu.Where(x => x.PodnosilacBanka == podnosilacBanka))
			//    {
			//        i++;
			//        row = table.AddRow();
			//        row.Height = rowHeight;
			//        row.Cells[0].AddParagraph(i.ToString());
			//        row.Cells[1].AddParagraph(cek.BrojCeka);
			//        row.Cells[2].AddParagraph(odDatuma_dtp.Value.ToString("dd.MM.yyyy"));
			//        row.Cells[3].AddParagraph(cek.TRGradjana);
			//        row.Cells[4].AddParagraph(cek.Vrednost.ToString("#,##0.00"));
			//    }

			//    section.AddParagraph();
			//    section.AddParagraph();

			//    table = section.AddTable();

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(8);

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(2);

			//    column = table.AddColumn();
			//    column.Width = Unit.FromCentimeter(8);

			//    row = table.AddRow();
			//    p = row.Cells[2].AddParagraph(_cekoviZaStampu.Where(x => x.PodnosilacBanka == podnosilacBanka).Sum(x => x.Vrednost).ToString("#,##0.00"));
			//    p.Format.Alignment = ParagraphAlignment.Right;
			//    p.Format.Borders.Style = MigraDoc.DocumentObjectModel.BorderStyle.Single;
			//    p.Format.Font.Size = 16;
			//    p.Format.Borders.Distance = Unit.FromMillimeter(1);

			//    section.AddParagraph();
			//    section.AddParagraph();
			//    MigraDoc.DocumentObjectModel.Shapes.Image img = section.AddImage(@"C:\Program Files\LimitlessSoft\TDOffice_v2\Assets\SpecifikacijaCekovaFooter.jpg");

			//    document.Render();
			//}
		}

		public void SetPrijemnoMesto(int prijemnoMesto)
		{
			prijemnoMesto_cmb.SelectedValue = prijemnoMesto;
		}

		public void SetMesto(int mesto)
		{
			mesto_cmb.SelectedValue = mesto;
		}

		public void SetFirma(int firma)
		{
			firma_cmb.SelectedValue = firma;
		}

		public void SetDatumCeka(DateTime datumCeka)
		{
			odDatuma_dtp.Value = datumCeka;
		}

		private void prijemnoMesto_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender as Control).Enabled)
				return;
			fm_Cekovi_List._stampaPrijemnoMestoBuffer = Convert.ToInt32(
				prijemnoMesto_cmb.SelectedValue
			);
		}

		private void mesto_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender as Control).Enabled)
				return;
			fm_Cekovi_List._stampaMestoBuffer = Convert.ToInt32(mesto_cmb.SelectedValue);
		}

		private void firma_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender as Control).Enabled)
				return;
			fm_Cekovi_List._stampaFirmaBuffer = Convert.ToInt32(firma_cmb.SelectedValue);
		}

		private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
		{
			if (!(sender as Control).Enabled)
				return;
			fm_Cekovi_List._stampaDatumBuffer = odDatuma_dtp.Value;
		}
	}
}
