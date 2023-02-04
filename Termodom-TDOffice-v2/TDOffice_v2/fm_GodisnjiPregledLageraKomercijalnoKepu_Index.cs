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
    public partial class fm_GodisnjiPregledLageraKomercijalnoKepu_Index : Form
    {
        private Task<List<Komercijalno.Magacin>> _magacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<List<Komercijalno.Tarife>> _tarife = Task.Run(() => {
            List<Komercijalno.Tarife> list = Komercijalno.Tarife.List();

            list = list.DistinctBy(x => x.Stopa).ToList();
            return list;
        });
        public fm_GodisnjiPregledLageraKomercijalnoKepu_Index()
        {
            InitializeComponent();
        }

        private void fm_GodisnjiPregledLageraKomercijalnoKepu_Index_Load(object sender, EventArgs e)
        {
            UcitajPodatke();
        }

        private void UcitajPodatke()
        {
            dgv.Visible = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("Magacin", typeof(string));

            foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
            {
                dt.Columns.Add(godina.ToString() + "_Pocetno_Stanje", typeof(double));
                dt.Columns.Add(godina.ToString() + "_Krajnje_Stanje", typeof(double));
            }

            foreach (Komercijalno.Magacin mag in _magacini.Result.OrderBy(x => x.ID))
            {
                DataRow dataRowKomercijalno = dt.NewRow();
                Dictionary<double, DataRow> dataRowStopa = new Dictionary<double, DataRow>();

                string magString = "M" + mag.ID.ToString();
                dataRowKomercijalno["Magacin"] = magString + " Komercijalno";

                foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
                {
                    using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godina]))
                    {
                        con.Open();

                        int brDokPocetnogStanja = 0;

                        using (FbCommand cmd = new FbCommand("SELECT BRDOK FROM DOKUMENT WHERE VRDOK = 0 AND MAGACINID = @MID ORDER BY BRDOK ASC", con))
                        {
                            cmd.Parameters.AddWithValue("@MID", mag.ID);
                            using (FbDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    brDokPocetnogStanja = Convert.ToInt32(dr[0]);
                        }

                        if (brDokPocetnogStanja == 0)
                        {
                            dataRowKomercijalno[godina.ToString() + "_Pocetno_Stanje"] = 0;
                        }
                        else
                        {
                            using (FbCommand cmd = new FbCommand("SELECT COALESCE(SUM(KOLICINA * PRODAJNACENA), 0) FROM STAVKA WHERE VRDOK = 0 AND BRDOK = @BRDOK", con))
                            {
                                cmd.Parameters.AddWithValue("@BRDOK", brDokPocetnogStanja);

                                using (FbDataReader dr = cmd.ExecuteReader())
                                    if (dr.Read())
                                        dataRowKomercijalno[godina.ToString() + "_Pocetno_Stanje"] = Convert.ToDouble(dr[0]);
                            }
                        }
                        using (FbCommand cmd = new FbCommand("SELECT COALESCE(SUM(STANJE * PRODAJNACENA), 0) FROM ROBAUMAGACINU WHERE MAGACINID = @MID", con))
                        {
                            cmd.Parameters.AddWithValue("@MID", mag.ID);

                            using (FbDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    dataRowKomercijalno[godina.ToString() + "_Krajnje_Stanje"] = Convert.ToDouble(dr[0]);
                        }
                    }
                }
                dt.Rows.Add(dataRowKomercijalno);
            }

            dgv.DataSource = dt;

            foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
            {
                dgv.Columns[godina.ToString() + "_Pocetno_Stanje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns[godina.ToString() + "_Pocetno_Stanje"].DefaultCellStyle.Format = "#,##0.00 RSD";

                dgv.Columns[godina.ToString() + "_Krajnje_Stanje"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns[godina.ToString() + "_Krajnje_Stanje"].DefaultCellStyle.Format = "#,##0.00 RSD";
            }

            dgv.Columns["Magacin"].Frozen = true;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            ObojiPodatke();

            dgv.Visible = true;
        }

        private void ObojiPodatke()
        {
            System.Drawing.Color color1 = Color.FromArgb(255, 255, 255);
            System.Drawing.Color color2 = Color.FromArgb(215, 215, 215);

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.DefaultCellStyle.BackColor = row.Index % 2 == 0 ? color1 : color2;
                foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
                {
                    if (godina == DateTime.Now.Year)
                        break;

                    double krajnjeStanje = Convert.ToDouble(row.Cells[godina + "_Krajnje_Stanje"].Value);
                    double pocetnoStanje = Convert.ToDouble(row.Cells[(godina + 1) + "_Pocetno_Stanje"].Value);

                    if (Math.Abs(krajnjeStanje - pocetnoStanje) > 1000)
                    {
                        //row.Cells[godina + "_Krajnje_Stanje"].Style.Font = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);
                        //row.Cells[godina + "_Krajnje_Stanje"].Style.ForeColor = Color.Red;
                        row.Cells[(godina + 1) + "_Pocetno_Stanje"].Style.Font = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);
                        row.Cells[(godina + 1) + "_Pocetno_Stanje"].Style.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void dgv_Sorted(object sender, EventArgs e)
        {
            ObojiPodatke();
        }
    }
}
