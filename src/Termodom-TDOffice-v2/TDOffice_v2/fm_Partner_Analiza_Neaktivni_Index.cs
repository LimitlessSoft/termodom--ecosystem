using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Partner_Analiza_Neaktivni_Index : Form
    {
        private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<DataTable> _data { get; set; } = Task.Run(() =>
        {
            DataTable dt = new DataTable();

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"
SELECT
p.PPID, p.NAZIV, datediff (day from MAX(d.DATUM) to current_date) as DIFF FROM PARTNER p
LEFT OUTER JOIN DOKUMENT d ON d.PPID = p.PPID
WHERE d.VRDOK = 15 OR d.BRDOK = 13
GROUP BY p.PPID, p.NAZIV
ORDER BY DIFF DESC", con))
                {
                    using (FbDataAdapter da = new FbDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        });

        public fm_Partner_Analiza_Neaktivni_Index()
        {
            if(!Program.TrenutniKorisnik.ImaPravo(170100))
            {
                TDOffice.Pravo.NematePravoObavestenje(170100);
                this.Close();
                return;
            }
            InitializeComponent();

            magacin_cmb.Enabled = false;
        }
        private void fm_Partner_Analiza_Neaktivni_Index_Load(object sender, EventArgs e)
        {
            _komercijalnoMagacini.Result.Add(new Komercijalno.Magacin() { ID = -1, Naziv = " <<< Svi Magacini >>> " });
            _komercijalnoMagacini.Result.Sort((x, y) => x.ID.CompareTo(y.ID));

            magacin_cmb.ValueMember = "ID";
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.DataSource = _komercijalnoMagacini.Result;

            magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;

            magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(170101);
        }
        private void UcitajPodatke()
        {
            int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
            DataTable dt = new DataTable();

            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using(FbCommand cmd= new FbCommand(@"
SELECT TAB1.PPID, TAB1.NAZIV, datediff (day from TAB1.LAST to current_date) AS NEAKTIVNOST FROM
(SELECT d.PPID, p.NAZIV, MAX(d.DATUM) AS LAST, COUNT(d.PPID) AS CMAG
FROM DOKUMENT d
LEFT OUTER JOIN PARTNER p ON d.PPID = p.PPID
WHERE (d.VRDOK = 15 OR d.VRDOK = 13)
AND d.MAGACINID = @MAGACINID
AND d.PPID > 0
GROUP BY d.PPID, p.NAZIV
ORDER BY LAST DESC) AS TAB1,
(SELECT d.PPID, COUNT(d.PPID) AS CTOTAL
FROM DOKUMENT d
WHERE (d.VRDOK = 15 OR d.VRDOK = 13)
AND d.PPID > 0
GROUP BY d.PPID) AS TAB2
WHERE TAB1.PPID = TAB2.PPID AND (TAB1.CMAG > (TAB2.CTOTAL * 0.6))
ORDER BY NEAKTIVNOST DESC", con))
                {
                    cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

                    using (FbDataAdapter da = new FbDataAdapter(cmd))
                    {
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns["PPID"].Visible = false;
                        dataGridView1.Columns["NAZIV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dataGridView1.Columns["NEAKTIVNOST"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }

                Task.Run(() =>
                {
                    /// Ovo koristim da bih azuzirao da je korisnik izvrsio ovaj zadatak
                    /// Potrebno je da barem 10 sekundi gleda u izvestaj da bi se zadatak smatrao izvrsenim
                    DateTime checkPoint = DateTime.Now;
                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    if (!this.IsDisposed)
                    {
                        List<TDOffice.CheckListItem> korisnikZadaci = TDOffice.CheckListItem.ListByKorisnikID(Program.TrenutniKorisnik.ID);
                        TDOffice.CheckListItem item = korisnikZadaci.FirstOrDefault(x => x.Job == TDOffice.CheckList.Jobs.NeaktivniPartneri);
                        if (item != null)
                        {
                            item.DatumIzvrsenja = checkPoint;
                            item.Update();
                        }
                    }
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UcitajPodatke();
        }
    }
}
