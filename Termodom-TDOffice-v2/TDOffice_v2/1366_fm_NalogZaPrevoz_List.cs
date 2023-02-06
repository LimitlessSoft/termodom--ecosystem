using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TDOffice_v2
{
    public partial class _1366_fm_NalogZaPrevoz_List : Form
    {
        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();
        private Task<fm_Help> _helpFrom { get; set; }
        private TDOffice.NalogZaPrevoz _nalog { get; set; }
        private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; }
        private PartialBuffer<TDOffice.NalogZaPrevoz> _partialBuffer { get; set; } = new PartialBuffer<TDOffice.NalogZaPrevoz>(TDOffice.NalogZaPrevoz.List, TDOffice.NalogZaPrevoz.MinDatum, 60);

        public _1366_fm_NalogZaPrevoz_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.NalogZaPrevoz_List);

            panel1.DesniKlik_DatumRange(odDatuma_dtp_CloseUp);
        }
        private void _1366_fm_NalogZaPrevoz_List_Load(object sender, EventArgs e)
        {
            _ = SetUIAsync().ContinueWith((prev) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    FillDGV();
                    odDatuma_dtp.Enabled = true;
                    doDatuma_dtp.Enabled = true;
                });
            });
        }

        private async Task SetUIAsync()
        {
            magacini_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136604);

            this.odDatuma_dtp.Value = DateTime.Now;
            this.doDatuma_dtp.Value = DateTime.Now;

            _komercijalnoMagacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);

            magacini_cmb.DataSource = await _komercijalnoMagacini;
            magacini_cmb.DisplayMember = "Naziv";
            magacini_cmb.ValueMember = "ID";

            magacini_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
        }
        private async void FillDGV()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MagacinID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Magacin", typeof(string));
            dt.Columns.Add("Referent", typeof(string));

            List<TDOffice.NalogZaPrevoz> nalozi = _partialBuffer.Data(odDatuma_dtp.Value.Date.AddDays(-1));

            foreach (TDOffice.NalogZaPrevoz np in nalozi.Where(x =>
                x.Datum.Date >= odDatuma_dtp.Value.Date &&
                x.Datum.Date <= doDatuma_dtp.Value.Date &&
                x.MagacinID == Convert.ToInt32(magacini_cmb.SelectedValue)))
            {
                DataRow dr = dt.NewRow();
                dr["MagacinID"] = np.MagacinID;
                dr["Magacin"] = (await _komercijalnoMagacini).FirstOrDefault(x => x.ID == np.MagacinID).Naziv;
                dr["Referent"] = _korisnici.Result.FirstOrDefault(x => x.ID == np.UserID).Username;
                dr["Status"] = np.Status;
                dr["UserID"] = np.UserID;
                dr["ID"] = np.ID;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            SetDGVColumns();
            slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;
        }

        private void SetDGVColumns()
        {
            dataGridView1.Columns["MagacinID"].Visible = false;
            dataGridView1.Columns["UserID"].Visible = false;
            dataGridView1.Columns["Status"].Visible = false;

            dataGridView1.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Magacin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Referent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["Status"].Value) == 0)
                    row.DefaultCellStyle.ForeColor = Color.Black;
                else
                    row.DefaultCellStyle.ForeColor = Color.Red;
            }
        }
        private void nova_btn_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136601))
            {
                TDOffice.Pravo.NematePravoObavestenje(136601);
                return;
            }

            int nalogID = TDOffice.NalogZaPrevoz.Insert(Program.TrenutniKorisnik.MagacinID, Program.TrenutniKorisnik.ID, new TDOffice.NalogZaPrevoz.Detalji());
            TDOffice.NalogZaPrevoz npp = TDOffice.NalogZaPrevoz.Get(nalogID);
            _partialBuffer.Append(npp);
            Task.Run(() =>
            {
                using (_1366_fm_NalogZaPrevoz_Index np = new _1366_fm_NalogZaPrevoz_Index(npp))
                    np.ShowDialog();
            });
            FillDGV();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int nalogID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            using (_1366_fm_NalogZaPrevoz_Index np = new _1366_fm_NalogZaPrevoz_Index(TDOffice.NalogZaPrevoz.Get(nalogID)))
            {
                np.Text = "Nalog za prevoz <" + nalogID.ToString() + ">";
                np.ShowDialog();
            }
        }
        private void odDatuma_dtp_CloseUp(object sender, EventArgs e)
        {
            FillDGV();
        }
        private void doDatuma_dtp_CloseUp(object sender, EventArgs e)
        {
            FillDGV();
        }
        private void btn_Stampaj_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Nalozi za prevoz";
            PdfPage page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Landscape;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont h1 = new XFont("Arial", 24, XFontStyle.Bold);
            XFont p = new XFont("Arial", 16, XFontStyle.Regular);
            XFont p1 = new XFont("Arial", 14, XFontStyle.Regular);
            XFont p2 = new XFont("Arial", 10, XFontStyle.Regular);

            gfx.DrawString("Nalozi za prevoz magacina " + magacini_cmb.Text + "   " + odDatuma_dtp.Value.ToString("dd.MM.yyyy") + " - " + doDatuma_dtp.Value.ToString("dd.MM.yyyy"), p1, XBrushes.Black, new Point(30, 30));

            int y = 70;

            gfx.DrawString("Br. naloga", p2, XBrushes.Black, new Point(20, y));
            gfx.DrawString("Datum", p2, XBrushes.Black, new Point(80, y));
            gfx.DrawString("Prevoznik nama naplatio ", p2, XBrushes.Black, new Point(150, y));
            gfx.DrawString("Kupcu naplatili virmanom ", p2, XBrushes.Black, new Point(280, y));
            gfx.DrawString("Kupcu naplatili kesom ", p2, XBrushes.Black, new Point(420, y));
            gfx.DrawString("Kupcu naplatili ", p2, XBrushes.Black, new Point(530, y));
            gfx.DrawString("Mi placamo", p2, XBrushes.Black, new Point(610, y));
            
            y += 20;
            List<TDOffice.NalogZaPrevoz> _nalozi = TDOffice.NalogZaPrevoz.List();
            double sumMiKupnaplatili = 0;
            double sumMiPlacamo = 0;
            double sumKupNapKesom = 0;
            double sumPrevoznikNamaNaplatio = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                double miKupNapVirmanom = 0;
                double miKupNapKesom = 0;
                double miKupNap = 0;
                double miPlacamo = 0;
                _nalog = TDOffice.NalogZaPrevoz.Get(Convert.ToInt32(r.Cells["ID"].Value));
                
                foreach (TDOffice.NalogZaPrevoz.Destinacija dest in _nalog.Tag.Destinacije)
                {
                    if (dest == null)
                        continue;
                    if ((int)dest.NacinPlacanja == 1)
                    {
                        List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 15, Convert.ToInt32(dest.MiKupcuNaplatili)).Where(x => x.MagacinID == _nalog.MagacinID).ToList();
                        if (stavke.Count(x => _1366_fm_NalogZaPrevoz_Odrediste.uslugaIDPrevoza.Contains(x.RobaID)) != 0)
                        {
                            miKupNapVirmanom += stavke.Where(x => _1366_fm_NalogZaPrevoz_Odrediste.uslugaIDPrevoza.Contains(x.RobaID)).Sum(x => x.ProdCenaBP * x.Kolicina);
                        }
                    }
                    if ((int)dest.NacinPlacanja == 0)
                    {
                        if (dest.MiKupcuNaplatili == null)
                            continue;
                        try
                        {
                            miKupNapKesom += Convert.ToDouble(dest.MiKupcuNaplatili.ToString());
                        }
                        
                        catch
                        {
                            continue;
                        }
                    }
                }
                miKupNap = miKupNapKesom + miKupNapVirmanom;
                miPlacamo = _nalog.NamaNaplacenPrevozBezPDV - miKupNap;
                double razlika = 0 - _nalog.NamaNaplacenPrevozBezPDV;
                sumMiKupnaplatili += miKupNap;
                sumMiPlacamo += miPlacamo;
                sumKupNapKesom += miKupNapKesom;
                sumPrevoznikNamaNaplatio += _nalog.NamaNaplacenPrevozBezPDV;
                gfx.DrawString(r.Cells["ID"].Value.ToString(), p2, XBrushes.Black, new Point(20, y));
                gfx.DrawString(_nalog.Datum.ToString("dd.MM.yyyy"), p2, XBrushes.Black, new Point(80, y));
                gfx.DrawString(_nalog.NamaNaplacenPrevozBezPDV.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(150, y));
                gfx.DrawString(miKupNapVirmanom.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(280, y));
                gfx.DrawString(miKupNapKesom.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(420, y));
                gfx.DrawString(miKupNap.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(530, y));
                gfx.DrawString(miPlacamo.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(610, y));

                y += 20;

                if(y + 30 > page.Height)
                {
                    y = 10;
                    page = document.AddPage();
                    page.Orientation = PdfSharp.PageOrientation.Landscape;

                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawString("Br. naloga", p2, XBrushes.Black, new Point(20, y));
                    gfx.DrawString("Datum", p2, XBrushes.Black, new Point(80, y));
                    gfx.DrawString("Prevoznik nama naplatio ", p2, XBrushes.Black, new Point(150, y));
                    gfx.DrawString("Kupcu naplatili virmanom ", p2, XBrushes.Black, new Point(280, y));
                    gfx.DrawString("Kupcu naplatili kesom ", p2, XBrushes.Black, new Point(420, y));
                    gfx.DrawString("Kupcu naplatili ", p2, XBrushes.Black, new Point(530, y));
                    gfx.DrawString("Mi placamo", p2, XBrushes.Black, new Point(610, y));
                    y = 30;
                }
            }
            gfx.DrawString(sumPrevoznikNamaNaplatio.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(150, y + 30));
            gfx.DrawString(sumKupNapKesom.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(420, y + 30));
            gfx.DrawString(sumMiKupnaplatili.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(530, y + 30));
            gfx.DrawString(sumMiPlacamo.ToString("#,##0.00 RSD"), p2, XBrushes.Black, new Point(610, y + 30));
            string fn = Path.Combine(Path.GetTempPath(), "NaloziZaPrevoz.pdf");
            document.Save(fn);
            Process.Start(fn);
        }
        private void magacini_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!magacini_cmb.Enabled)
                return;

            FillDGV();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SetDGVColumns();
        }
    }
}
