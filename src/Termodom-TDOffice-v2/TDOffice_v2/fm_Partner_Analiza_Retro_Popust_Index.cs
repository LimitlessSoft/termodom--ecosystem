using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Partner_Analiza_Retro_Popust_Index : Form
    {
        private Task<List<Komercijalno.Partner>> _partneri { get; set; }
        private List<Tuple<int, string>> _meseci { get; set; } = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(0, "Januar"),
            new Tuple<int, string>(1, "Februar"),
            new Tuple<int, string>(2, "Mart"),
            new Tuple<int, string>(3, "April"),
            new Tuple<int, string>(4, "Maj"),
            new Tuple<int, string>(5, "Jun"),
            new Tuple<int, string>(6, "Jul"),
            new Tuple<int, string>(7, "Avgust"),
            new Tuple<int, string>(8, "Septembar"),
            new Tuple<int, string>(9, "Oktobar"),
            new Tuple<int, string>(10, "Novembar"),
            new Tuple<int, string>(11, "Decembar")
        };

        private List<Tuple<int, string>> _naciniPlacanja { get; set; } = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(1, "Virman"),
            new Tuple<int, string>(5, "Gotovina"),
            new Tuple<int, string>(11, "Kartica")
        };

        public fm_Partner_Analiza_Retro_Popust_Index()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(170110))
            {
                TDOffice.Pravo.NematePravoObavestenje(170110);
                this.Close();
                return;
            }
            
            InitializeComponent();
            _partneri = Task.Run(async () =>
            {
                List<Komercijalno.Partner> pl = await Komercijalno.Partner.ListAsync();
                this.Invoke((MethodInvoker)delegate
                {
                    pl.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                    partner_cmb.DisplayMember = "Naziv";
                    partner_cmb.ValueMember = "PPID";
                    partner_cmb.DataSource = pl;
                    partner_cmb.Enabled = true;
                    mesec_cmb.Enabled = true;
                    analiziraj_btn.Enabled = true;
                });
                return pl;
            });
            mesec_cmb.ValueMember = "Item1";
            mesec_cmb.DisplayMember = "Item2";
            mesec_cmb.DataSource = _meseci;

            nacinUplate_clb.DataSource = _naciniPlacanja;
            nacinUplate_clb.ValueMember = "Item1";
            nacinUplate_clb.DisplayMember = "Item2";

            for(int i = 0; i < nacinUplate_clb.Items.Count; i++)
                nacinUplate_clb.SetItemChecked(i, true);
        }

        private void fm_Partner_Analiza_Retro_Popust_Index_Load(object sender, EventArgs e)
        {

        }

        private void analiziraj_btn_Click(object sender, EventArgs e)
        {
            int ppid = Convert.ToInt32(partner_cmb.SelectedValue);
            int mesec = Convert.ToInt32(mesec_cmb.SelectedValue) + 1;

            if(nacinUplate_clb.CheckedItems.Count == 0)
            {
                MessageBox.Show("Morate izabrati barem jedan nacin uplate!");
                return;
            }

            List<int> nuids = new List<int>();
            foreach (var ci in nacinUplate_clb.CheckedItems)
                nuids.Add((ci as Tuple<int, string>).Item1);

            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                double vrednostPovratnica = 0;
                using(FbCommand cmd = new FbCommand($@"
SELECT COALESCE(SUM(POTRAZUJE - POREZ), 0) FROM DOKUMENT
WHERE
    DOKUMENT.VRDOK = 22 AND
    DOKUMENT.PPID = @PPID AND
    DOKUMENT.DATUM >= @OD AND
    DOKUMENT.DATUM <= @DO
", con))
                {
                    cmd.Parameters.AddWithValue("@PPID", ppid);
                    cmd.Parameters.AddWithValue("@OD", new DateTime(DateTime.Now.Year, mesec, 1, 0, 0, 1));
                    cmd.Parameters.AddWithValue("@DO", new DateTime(DateTime.Now.Year, mesec, DateTime.DaysInMonth(DateTime.Now.Year, mesec), 23, 59, 59));
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            vrednostPovratnica = Convert.ToDouble(dr[0]);
                }

                using(FbCommand cmd = new FbCommand($@"
SELECT DOKUMENT.VRDOK, DOKUMENT.BRDOK, DOKUMENT.DATUM,
  SUM(STAVKA.NABAVNACENA * STAVKA.KOLICINA) AS NABAVNA_VREDNOST,
  SUM((STAVKA.PRODCENABP - STAVKA.NABAVNACENA) * STAVKA.KOLICINA) AS RAZLIKA,
  SUM(STAVKA.PRODCENABP * STAVKA.KOLICINA) AS PRODAJNA_VREDNOST,
  SUM(STAVKA.KOLICINA * ((STAVKA.PRODCENABP * (STAVKA.RABAT / (100 - STAVKA.RABAT))))) AS RABAT_VREDNOST
FROM DOKUMENT
LEFT JOIN
    STAVKA ON DOKUMENT.VRDOK = STAVKA.VRDOK AND DOKUMENT.BRDOK = STAVKA.BRDOK
WHERE
    (DOKUMENT.VRDOK = 15 OR DOKUMENT.VRDOK = 22) AND
    DOKUMENT.PPID = @PPID AND
    DOKUMENT.DATUM >= @OD AND
    DOKUMENT.DATUM <= @DO AND
    DOKUMENT.POTRAZUJE >= 10000 AND
    DOKUMENT.NUID IN ({string.Join(", ", nuids)})
GROUP BY DOKUMENT.VRDOK, DOKUMENT.BRDOK, DOKUMENT.DATUM", con))
                {
                    cmd.Parameters.AddWithValue("@PPID", ppid);
                    cmd.Parameters.AddWithValue("@OD", new DateTime(DateTime.Now.Year, mesec, 1, 0, 0, 1));
                    cmd.Parameters.AddWithValue("@DO", new DateTime(DateTime.Now.Year, mesec, DateTime.DaysInMonth(DateTime.Now.Year, mesec), 23, 59, 59));

                    DataTable dt = new DataTable();
                    using(FbDataAdapter da = new FbDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("Opis", typeof(string));
                        dt2.Columns.Add("Vrednost", typeof(string));

                        DataRow dr5 = dt2.NewRow();
                        dr5["Opis"] = "Prodajna Vrednost MP Racuna";
                        dr5["Vrednost"] = dt.Rows.Count == 0 ? "0" : Convert.ToDouble(dt.Compute("SUM(PRODAJNA_VREDNOST)", "VRDOK = 15")).ToString("#,##0.00 RSD");
                        dt2.Rows.Add(dr5);

                        DataRow dr2 = dt2.NewRow();
                        dr2["Opis"] = "Nabavna Vrednost MP Racuna";
                        dr2["Vrednost"] = dt.Rows.Count == 0 ? "0" : Convert.ToDouble(dt.Compute("SUM(NABAVNA_VREDNOST)", String.Empty)).ToString("#,##0.00 RSD");
                        dt2.Rows.Add(dr2);

                        DataRow dr3 = dt2.NewRow();
                        dr3["Opis"] = "Ostvarena Razlika MP Racuna";
                        dr3["Vrednost"] = dt.Rows.Count == 0 ? "0" : Convert.ToDouble(dt.Compute("SUM(RAZLIKA)", String.Empty)).ToString("#,##0.00 RSD");
                        dt2.Rows.Add(dr3);

                        DataRow dr4 = dt2.NewRow();
                        dr4["Opis"] = "Dati Rabat MP Racuna";
                        dr4["Vrednost"] = dt.Rows.Count == 0 ? "0" : Convert.ToDouble(dt.Compute("SUM(RABAT_VREDNOST)", String.Empty)).ToString("#,##0.00 RSD");
                        dt2.Rows.Add(dr4);

                        DataRow dr6 = dt2.NewRow();
                        dr6["Opis"] = "Vrednost povratnica";
                        dr6["Vrednost"] = vrednostPovratnica.ToString("#,##0.00 RSD");
                        dt2.Rows.Add(dr6);

                        dataGridView2.DataSource = dt2;
                        dataGridView2.Columns["Opis"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView2.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        dataGridView1.Columns["NABAVNA_VREDNOST"].HeaderText = "Nabavna Vrednost";
                        dataGridView1.Columns["NABAVNA_VREDNOST"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["NABAVNA_VREDNOST"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns["PRODAJNA_VREDNOST"].HeaderText = "Prodajna Vrednost";
                        dataGridView1.Columns["PRODAJNA_VREDNOST"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["PRODAJNA_VREDNOST"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns["RAZLIKA"].HeaderText = "Razlika";
                        dataGridView1.Columns["RAZLIKA"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["RAZLIKA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridView1.Columns["RABAT_VREDNOST"].HeaderText = "Vrednost Rabata";
                        dataGridView1.Columns["RABAT_VREDNOST"].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["RABAT_VREDNOST"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                }
            }
        }
    }
}
