using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace TDOffice_v2.PDFSharpARExtensions
{
	public static class PDFSharpARExtensions
	{
		public static Color TableBorder { get; private set; } = Color.FromRgb(0, 0, 0);
		public static Color TableHeaderBackground { get; private set; } =
			Color.FromRgb(245, 245, 245);

		/// <summary>
		/// Returns Pdf Document with data from given table
		/// </summary>
		/// <param name="columnSettings">Tuple which will set column settings as defined for Item1 column name from datatable.
		/// If column is not inside columnSettings, it will be skipped and not added to the document</param>
		/// <returns></returns>
		public static Document ToPdf(
			this DataTable dataTable,
			string title,
			string subTitle,
			List<Tuple<string, Unit, ParagraphAlignment>> columnSettings,
			double tableRowHeight = 10
		)
		{
			Unit rowHeight = tableRowHeight;
			Document document = new Document();
			document.Info.Title = title;
			document.Info.Author = "TDOffice_v2";

			Section section = document.AddSection();
			section.PageSetup.TopMargin = 15;
			section.PageSetup.LeftMargin = 15;

			Paragraph p = section.AddParagraph(title);
			p.Format.Font.Size = 18;

			p = section.AddParagraph(subTitle);
			p.Format.Font.Size = 12;

			section.AddParagraph();

			Table table = section.AddTable();

			table.Style = "Table";
			table.Borders.Color = TableBorder;
			table.Borders.Width = 0.25;
			table.Borders.Left.Width = 0.5;
			table.Borders.Right.Width = 0.5;
			table.Rows.LeftIndent = 0;

			// Inicijalizacija kolona
			foreach (Tuple<string, Unit, ParagraphAlignment> t in columnSettings)
			{
				Column column = table.AddColumn(t.Item2);
				column.Format.Alignment = t.Item3;
			}

			// Inicijalizacija zaglavlja
			Row row = table.AddRow();
			row.HeadingFormat = true;
			row.Format.Alignment = ParagraphAlignment.Center;
			row.Format.Font.Bold = true;
			row.Height = rowHeight;
			row.Shading.Color = TableHeaderBackground;

			for (int i = 0; i < columnSettings.Count; i++)
			{
				row.Cells[i].AddParagraph(columnSettings[i].Item1);
				table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
			}

			foreach (DataRow dr in dataTable.Rows)
			{
				Row r = table.AddRow();
				r.Height = rowHeight;
				int currCol = 0;
				foreach (var t in columnSettings)
				{
					r.Cells[currCol].AddParagraph(dr[t.Item1].ToString());
					currCol++;
				}
				table.SetEdge(0, table.Rows.Count - 2, 1, 2, Edge.Box, BorderStyle.Single, 0.75);
			}

			return document;
		}

		/// <summary>
		/// Renders current document into PDF file and openes it
		/// </summary>
		/// <param name="document"></param>
		public static void Render(this Document document)
		{
			PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);

			// Associate the MigraDoc document with a renderer
			pdfRenderer.Document = document;

			// Layout and render document to PDF
			pdfRenderer.RenderDocument();

			// Save the document...
			string filename = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fffff") + ".pdf";
			pdfRenderer.PdfDocument.Save(filename);
			// ...and start a viewer.
			Process.Start(filename);
		}
	}
}
