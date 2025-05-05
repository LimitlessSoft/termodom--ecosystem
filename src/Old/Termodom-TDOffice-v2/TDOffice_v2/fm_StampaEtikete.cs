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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharpCompress.Common;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
	public partial class fm_StampaEtikete : Form
	{
		/// <summary>
		/// Koristimo da skladistimo izabranu robu za stampu
		/// </summary>
		private DataTable _dt { get; set; } = null;
		private Task<fm_Help> _helpFrom { get; set; }
		private Task<List<Roba>> _roba { get; set; } = Roba.ListAsync(DateTime.Now.Year);

		public fm_StampaEtikete()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.StampaEtikete);
		}

		private void fm_StampaEtikete_Load(object sender, EventArgs e)
		{
			PopulateVrstaDokCmbAsync();
			SetDGV();
		}

		private Task PopulateVrstaDokCmbAsync()
		{
			return Task.Run(() =>
			{
				List<Termodom.Data.Entities.Komercijalno.VrstaDok> vrdokList = VrstaDokManager
					.DictionaryAsync()
					.GetAwaiter()
					.GetResult()
					.Values.Where(x => new int[] { 15, 21, 32 }.Contains(x.VrDok))
					.ToList();
				vrdokList.Add(
					new Termodom.Data.Entities.Komercijalno.VrstaDok()
					{
						VrDok = -1,
						NazivDok = " < vrsta dokumenta > "
					}
				);
				vrdokList.Sort((x, y) => x.VrDok.CompareTo(y.VrDok));
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							cmb_VrstaDokumenta.DataSource = vrdokList;
							cmb_VrstaDokumenta.DisplayMember = "NazivDok";
							cmb_VrstaDokumenta.ValueMember = "VrDok";
						}
				);
			});
		}

		private void SetDGV()
		{
			_dt = new DataTable();
			_dt.Columns.Add("RobaID", typeof(int));
			_dt.Columns.Add("KatBr", typeof(string));
			_dt.Columns.Add("Naziv", typeof(string));
			_dt.Columns.Add("JM", typeof(string));
			_dt.Columns.Add("ProdajnaCena", typeof(double));
			_dt.Columns.Add("ProdajnaCenaDokumenta", typeof(double));
			_dt.Columns.Add("NabavnaCenaSaPorezom", typeof(double));
			_dt.Columns.Add("FakturnaCena", typeof(double));
			_dt.Columns.Add("NabavnaCena", typeof(double));
			_dt.Columns.Add("ProdajnaCenaBezPoreza", typeof(double));

			_dt.TableNewRow += _dt_TableNewRow;
			_dt.RowDeleted += _dt_RowDeleted;

			dataGridView1.DataSource = _dt;

			dataGridView1.Columns["RobaID"].Visible = false;

			dataGridView1.Columns["KatBr"].Width = 100;

			dataGridView1.Columns["Naziv"].Width = 300;

			dataGridView1.Columns["JM"].Width = 35;

			dataGridView1.Columns["ProdajnaCena"].Width = 90;
			dataGridView1.Columns["ProdajnaCena"].HeaderText = "Prodajna Cena";
			dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

			dataGridView1.Columns["ProdajnaCenaDokumenta"].Width = 90;
			dataGridView1.Columns["ProdajnaCenaDokumenta"].HeaderText = "Prodajna Cena Dokumenta";
			dataGridView1.Columns["ProdajnaCenaDokumenta"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["ProdajnaCenaDokumenta"].DefaultCellStyle.Format = "#,##0.00 RSD";

			dataGridView1.Columns["NabavnaCenaSaPorezom"].Width = 90;
			dataGridView1.Columns["NabavnaCenaSaPorezom"].HeaderText = "Nabavna cena sa porezom";
			dataGridView1.Columns["NabavnaCenaSaPorezom"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["NabavnaCenaSaPorezom"].DefaultCellStyle.Format = "#,##0.00 RSD";

			dataGridView1.Columns["FakturnaCena"].Width = 90;
			dataGridView1.Columns["FakturnaCena"].HeaderText = "Fakturna cena";
			dataGridView1.Columns["FakturnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["FakturnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

			dataGridView1.Columns["NabavnaCena"].Width = 90;
			dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna cena";
			dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";

			dataGridView1.Columns["ProdajnaCenaBezPoreza"].Width = 90;
			dataGridView1.Columns["ProdajnaCenaBezPoreza"].HeaderText = "Prodajna cena bez poreza";
			dataGridView1.Columns["ProdajnaCenaBezPoreza"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["ProdajnaCenaBezPoreza"].DefaultCellStyle.Format = "#,##0.00 RSD";
		}

		private void _dt_RowDeleted(object sender, DataRowChangeEventArgs e)
		{
			slogova_lbl.Text = "Slogova: " + _dt.Rows.Count.ToString();
		}

		private void _dt_TableNewRow(object sender, DataTableNewRowEventArgs e)
		{
			slogova_lbl.Text = "Slogova: " + _dt.Rows.Count.ToString();
		}

		private void btn_Prikazi_Click(object sender, EventArgs e)
		{
			if (this.cmb_VrstaDokumenta.SelectedIndex == 0)
			{
				MessageBox.Show("Niste izabrali vrstu dokumenta");
				return;
			}

			int vrDok = Convert.ToInt32(cmb_VrstaDokumenta.SelectedValue);
			int brDok;

			_dt.Clear();

			if (!int.TryParse(tb_BrojDokumenta.Text, out brDok))
			{
				MessageBox.Show("Neispravan broj dokumenta!");
				return;
			}

			Dokument _dokument = Dokument.Get(DateTime.Now.Year, vrDok, brDok);

			if (_dokument == null)
			{
				MessageBox.Show("Dokument " + brDok + " nije pronadjen");
				return;
			}

			List<Stavka> stavke = Stavka.ListByDokument(DateTime.Now.Year, vrDok, brDok);

			foreach (Stavka s in stavke)
			{
				Roba r = _roba.Result.Where(x => x.ID == s.RobaID).FirstOrDefault();
				if (r == null)
					r = new Roba() { Naziv = "undefined", KatBr = "undefined" };

				DataRow dr = _dt.NewRow();
				dr["RobaID"] = s.RobaID;
				dr["KatBr"] = r.KatBr;
				dr["Naziv"] = r.Naziv;
				dr["JM"] = r.JM;
				dr["ProdajnaCena"] = s.ProdajnaCena;

				_dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = _dt;
		}

		private void tb_BrojDokumenta_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (Char)Keys.Enter)
			{
				btn_Prikazi.PerformClick();
			}
		}

		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			using (IzborRobe ir = new IzborRobe(50))
			{
				ir.OnRobaClickHandler += OnIzborRobeTrigger;
				ir.ShowDialog();
			}
		}

		private void OnIzborRobeTrigger(Komercijalno.RobaUMagacinu[] args)
		{
			if (args == null || args.Length == 0)
				return;

			foreach (RobaUMagacinu rum in args)
			{
				if (_dt.Select("RobaID = " + rum.RobaID).Count() != 0)
					continue;
				Roba rr = _roba.Result.Where(x => x.ID == rum.RobaID).FirstOrDefault();

				if (rr == null)
					rr = new Roba() { Naziv = "undefined", KatBr = "undefined" };

				DataRow dr = _dt.NewRow();
				dr["RobaID"] = rum.RobaID;
				dr["KatBr"] = rr.KatBr;
				dr["Naziv"] = rr.Naziv;
				dr["JM"] = rr.JM;
				dr["ProdajnaCena"] = rum.ProdajnaCena;
				_dt.Rows.Add(dr);
			}
		}

		private void brisiSelektovanuStavkuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridView1.SelectedRows)
			{
				int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);

				_dt.Rows.Remove(_dt.Select("Robaid = " + robaID).First());
			}
		}

		private void brisiSveStavkeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Da li sigurno zelite obrisati sve stavke iz liste?",
					"Potvrdi",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			_dt.Clear();
		}

		private void btn_Stampaj_Click(object sender, EventArgs e)
		{
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

			PdfDocument document = new PdfDocument();
			document.Info.Title = "Stampa etikete";

			PdfPage page = document.AddPage();

			XGraphics gfx = XGraphics.FromPdfPage(page);

			XFont h1 = new XFont("Arial", 24, XFontStyle.Bold);
			XFont p = new XFont("Arial", 8, XFontStyle.Regular);
			XFont p1 = new XFont("Arial", 14, XFontStyle.Regular);
			XFont p2 = new XFont("Arial", 10, XFontStyle.Regular);
			XFont p3 = new XFont("Arial", 16, XFontStyle.Bold);
			XFont p4 = new XFont("Arial", 4, XFontStyle.Regular);

			//int x = 10;
			//int y = 33;
			//int sirinaKolone = 196;
			//int visinaReda = 124;
			//int kolonaIndex = 0;
			//int dxCapKatBr = 5;
			//int dyCapKatbr = 10;
			//int dxKatBr = 5;
			//int dyKatbr = 25;
			//int dxNaziv = 5;
			//int dyNaziv = 40;
			//int dxSaPorezom = 50;
			//int dySaPorezom = 80;
			//int dxJM = 5;
			//int dyJM = 110;
			//int dxCena = 65;
			//int dyCena = 108;
			//int dxDatum = 170;
			//int dyDatum = 118;
			//int dxURafu = 170;
			//int dyURafu = 25;
			//int brVrsta = 0;
			//int brCelija = 0;
			//double br1 = dataGridView1.Rows.Count / 3;
			//int brVrstaZaUporedjivanje = (int)Math.Truncate(br1);
			//string strKatBr;
			//string strKatBr1;
			//string strEkstraData = "";
			//int zasdfas = 0;
			//foreach (DataRow r in _dt.Rows)
			//{
			//    gfx.DrawLine(XPens.Black, new Point(x + kolonaIndex * sirinaKolone, y), new Point(x + (kolonaIndex + 1) * sirinaKolone, y));
			//    gfx.DrawLine(XPens.Black, new Point(x + kolonaIndex * sirinaKolone, y), new Point(x + kolonaIndex * sirinaKolone, visinaReda));

			//    zasdfas++;
			//    if (zasdfas == 3)
			//    {
			//        y += visinaReda;
			//        x += 5;
			//    }
			//    if (zasdfas >= 6)
			//        continue;

			//    strKatBr1 = r["KatBr"].ToString();
			//    if (strKatBr1.Length > 12)
			//    {
			//        strKatBr = r["KatBr"].ToString().Substring(0, 11);
			//    }
			//    else
			//        strKatBr = strKatBr1;

			//    gfx.DrawString("Kataloski broj", p, XBrushes.Black, new Point(x + dxCapKatBr + kolonaIndex * sirinaKolone, y + dyCapKatbr));
			//    gfx.DrawString(strKatBr, p1, XBrushes.Black, new Point(x + dxKatBr + kolonaIndex * sirinaKolone, y + dyKatbr));
			//    gfx.DrawString(strEkstraData, p1, XBrushes.Black, new Point(x + dxURafu + kolonaIndex * sirinaKolone, y + dyURafu));
			//    gfx.DrawString(r["Naziv"].ToString(), p2, XBrushes.Black, new Point(x + dxNaziv + kolonaIndex * sirinaKolone, y + dyNaziv));
			//    gfx.DrawString("SA POREZOM", p2, XBrushes.Black, new Point(x + dxSaPorezom + kolonaIndex * sirinaKolone, y + dySaPorezom));
			//    gfx.DrawString(r["JM"].ToString(), p2, XBrushes.Black, new Point(x + dxJM + kolonaIndex * sirinaKolone, y + dyJM));
			//    gfx.DrawString(r["ProdajnaCena"].ToString(), p3, XBrushes.Black, new Point(x + dxCena + kolonaIndex * sirinaKolone, y + dyCena));
			//    gfx.DrawString(DateTime.Now.ToString("dd MM yyyy"), p4, XBrushes.Black, new Point(x + dxDatum + kolonaIndex * sirinaKolone, y + dyDatum));

			//    kolonaIndex++;

			//    if (kolonaIndex == 3)
			//    {
			//        gfx.DrawLine(XPens.Black, new Point(x + kolonaIndex * sirinaKolone, y), new Point(x + kolonaIndex * sirinaKolone, visinaReda));
			//        gfx.DrawLine(XPens.Black, new Point(x + kolonaIndex * sirinaKolone, y), new Point(x + kolonaIndex * sirinaKolone, visinaReda));

			//        y = y + visinaReda;
			//        kolonaIndex = 0;
			//        brVrsta = brVrsta + 1;
			//    }
			//    brCelija = brCelija + 1;
			//    if (brCelija == 18)
			//    {
			//        for (int i = 0; i <= 2; i++)
			//        {
			//            gfx.DrawLine(XPens.Black, new Point(x + i * sirinaKolone, y), new Point(x + (i + 1) * sirinaKolone, y));
			//            gfx.DrawLine(XPens.Black, new Point(x + i * sirinaKolone, y), new Point(x + i * sirinaKolone, visinaReda));
			//        }

			//        gfx.DrawLine(XPens.Black, new Point(x + 3 * sirinaKolone, y), new Point(x + 3 * sirinaKolone, visinaReda));
			//        gfx.DrawLine(XPens.Black, new Point(x + 3 * sirinaKolone, y), new Point(x + 3 * sirinaKolone, visinaReda));
			//        page = document.AddPage();
			//        gfx = XGraphics.FromPdfPage(page);
			//        kolonaIndex = 0;
			//        brCelija = 0;
			//        y = 33;

			//    }
			//    if (brVrsta == brVrstaZaUporedjivanje)
			//    {
			//        for (int i = 0; i <= 2; i++)
			//        {
			//            gfx.DrawLine(XPens.Black, new Point(x + i * sirinaKolone, y), new Point(x + (i + 1) * sirinaKolone, y));
			//            gfx.DrawLine(XPens.Black, new Point(x + i * sirinaKolone, y), new Point(x + i * sirinaKolone, 150));
			//        }
			//        gfx.DrawLine(XPens.Black, new Point(x + 3 * sirinaKolone, y), new Point(x + 3 * sirinaKolone, 150));
			//        gfx.DrawLine(XPens.Black, new Point(x + 3 * sirinaKolone, y), new Point(x + 3 * sirinaKolone, 150));
			//    }
			//}



			int marginLeft = 10;
			int sirinaEtikete = 196;
			int visinaEtikete = 124;
			int currentY = 33;
			int currentX;
			int padding = 5;

			for (int i = 0; i < _dt.Rows.Count; i++)
			{
				currentX = marginLeft + (i % 3) * sirinaEtikete;
				int tempY = currentY + 0;
				gfx.DrawLine(
					XPens.Black,
					new Point(currentX, currentY),
					new Point(marginLeft + ((i % 3) + 1) * sirinaEtikete, currentY)
				);
				gfx.DrawLine(
					XPens.Black,
					new Point(currentX, currentY + visinaEtikete),
					new Point(marginLeft + ((i % 3) + 1) * sirinaEtikete, currentY + visinaEtikete)
				);
				gfx.DrawLine(
					XPens.Black,
					new Point(currentX, currentY),
					new Point(marginLeft + (i % 3) * sirinaEtikete, currentY + visinaEtikete)
				);
				gfx.DrawLine(
					XPens.Black,
					new Point(marginLeft + ((i % 3) + 1) * sirinaEtikete, currentY),
					new Point(marginLeft + ((i % 3) + 1) * sirinaEtikete, currentY + visinaEtikete)
				);

				tempY += padding + (int)p.Size + 1;
				gfx.DrawString(
					"Kataloski broj",
					p,
					XBrushes.Black,
					new Point(currentX + padding, tempY)
				);

				tempY += (int)p1.Size + 1;
				gfx.DrawString(
					_dt.Rows[i]["KatBr"]
						.ToString()
						.Substring(0, Math.Min(_dt.Rows[i]["KatBr"].ToString().Length, 11)),
					p1,
					XBrushes.Black,
					new Point(currentX + padding, tempY)
				);

				tempY += +5;
				string[] nazivRedovi = _dt.Rows[i]["Naziv"].ToString().DivideInPieces(30);
				foreach (string naziv in nazivRedovi)
				{
					tempY += (int)p2.Size + 3;
					gfx.DrawString(naziv, p2, XBrushes.Black, new Point(currentX + padding, tempY));
				}
				gfx.DrawString(
					_dt.Rows[i]["JM"].ToString(),
					p1,
					XBrushes.Black,
					new Point(currentX + padding, currentY + visinaEtikete - 2 * padding)
				);

				tempY = currentY + visinaEtikete - padding - 30;
				gfx.DrawString(
					"SA POREZOM",
					p2,
					XBrushes.Black,
					new Point(currentX + sirinaEtikete / 2 + padding, tempY)
				);

				tempY += (int)p2.Size + 10;
				string strProdajnaCena = string.Format(
					"{0:#,##0.00 DIN}",
					Convert.ToDouble(_dt.Rows[i]["ProdajnaCena"])
				);
				Size s = TextRenderer.MeasureText(
					strProdajnaCena,
					new Font(
						new FontFamily(p3.FontFamily.Name.ToString()),
						1.0f,
						p3.Bold ? FontStyle.Bold : FontStyle.Regular
					)
				);

				gfx.DrawString(
					strProdajnaCena,
					p3,
					XBrushes.Black,
					new Point(currentX + sirinaEtikete - padding - s.Width * 7, tempY)
				);

				gfx.DrawString(
					DateTime.Now.ToString("dd MM yyyy"),
					p4,
					XBrushes.Black,
					new Point(currentX + sirinaEtikete - 6 * padding, currentY + visinaEtikete - 5)
				);

				if (i != 0 && i % 3 == 2)
					currentY += visinaEtikete;

				if (currentY + visinaEtikete + 5 > page.Height)
				{
					page = document.AddPage();
					gfx = XGraphics.FromPdfPage(page);
					currentY = 33;
				}
			}

			string fn = Path.Combine(Path.GetTempPath(), "StampaEtikete.pdf");
			document.Save(fn);
			var pr = new Process();
			pr.StartInfo = new ProcessStartInfo(fn) { UseShellExecute = true };
			pr.Start();
		}
	}
}
