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
    public partial class fm_BiznisPlan_Index : Form
    {
        private Task<List<Komercijalno.Magacin>> _magacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private TDOffice.BiznisPlan biznisPlan = null;
        public fm_BiznisPlan_Index()
        {
            InitializeComponent();
        }

        private void fm_BiznisPlan_Index_Load(object sender, EventArgs e)
        {
            magacin_cmb.DataSource = _magacini.Result;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";
        }

        private void UcitajBiznisPlan()
        {
            int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
            double trenutniPrometMagacina = 0;
            double ukupniTroskovi = 0;
            double troskoviRacuna = 0;

            biznisPlan = TDOffice.BiznisPlan.Get(magacinID);
            Task<List<Komercijalno.Stavka>> troskoviMagacina = Komercijalno.Stavka.ListAsync(DateTime.Now.Year, $"MAGACINID = {magacinID} AND VRDOK = 6");

            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT COALESCE(SUM(POTRAZUJE), 0) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MAGID", con))
                {
                    cmd.Parameters.AddWithValue("@MAGID", magacinID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            trenutniPrometMagacina = Convert.ToDouble(dr[0]);
                }
            }

            #region Grafikon
            List<Tuple<int, double>> list1 = new List<Tuple<int, double>>();
            foreach(int key in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
            {
                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[key]))
                {
                    con.Open();
                    double prom = 0;
                    using (FbCommand cmd = new FbCommand("SELECT COALESCE(SUM(POTRAZUJE), 0) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MAGID", con))
                    {
                        cmd.Parameters.AddWithValue("@MAGID", magacinID);

                        using (FbDataReader dr = cmd.ExecuteReader())
                            if (dr.Read())
                                prom = Convert.ToDouble(dr[0]);
                            else
                                prom = 0;
                    }

                    list1.Add(new Tuple<int, double>(key, prom));
                }
            }

            list1.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            prometPoGodinama_chart.DataSource = list1;
            prometPoGodinama_chart.Series.First().XValueMember = "Item1";
            prometPoGodinama_chart.Series.First().YValueMembers = "Item2";
            
            prometPoGodinama_chart.Series[0].IsVisibleInLegend = false;
            prometPoGodinama_chart.Series[0].IsValueShownAsLabel = true;
            prometPoGodinama_chart.Series[0].LabelFormat = "#,##0.00";

            prometPoGodinama_chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            prometPoGodinama_chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            
            prometPoGodinama_chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            #endregion

            #region Troskovi
            TDOffice.Config<Dictionary<string, List<int>>> _grupeTroskova = TDOffice.Config<Dictionary<string, List<int>>>.Get(TDOffice.ConfigParameter.GrupeTroskova);
            if(_grupeTroskova == null || _grupeTroskova.Tag == null)
            {
                if (_grupeTroskova == null)
                    _grupeTroskova = new TDOffice.Config<Dictionary<string, List<int>>>();

                if (_grupeTroskova.Tag == null)
                    _grupeTroskova.Tag = new Dictionary<string, List<int>>();

                _grupeTroskova.UpdateOrInsert();
            }

            DataTable troskoviDT = new DataTable();
            troskoviDT.Columns.Add("Trosak", typeof(string));
            troskoviDT.Columns.Add("Vrednost", typeof(double));
            troskoviDT.Columns.Add("Planirano", typeof(double));

            foreach (string grupa in _grupeTroskova.Tag.Keys)
            {
                double planiraniTrosak = 0;

                if (biznisPlan.PlaniraniTroskovi != null &&
                    biznisPlan.PlaniraniTroskovi.ContainsKey(grupa))
                    planiraniTrosak = biznisPlan.PlaniraniTroskovi[grupa];

                DataRow dr = troskoviDT.NewRow();
                dr["Trosak"] = grupa;
                dr["Vrednost"] = troskoviMagacina.Result.Where(x => _grupeTroskova.Tag[grupa].Contains(x.RobaID)).Sum(x => x.NabCenSaPor * x.Kolicina);
                dr["Planirano"] = planiraniTrosak;
                troskoviDT.Rows.Add(dr);
                ukupniTroskovi += Convert.ToDouble(dr["Vrednost"]);
                troskoviRacuna += Convert.ToDouble(dr["Vrednost"]);
            }
            troskovi_dgv.DataSource = troskoviDT;
            troskovi_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            troskovi_dgv.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            troskovi_dgv.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            troskovi_dgv.Columns["Planirano"].DefaultCellStyle.Format = "#,##0.00 RSD";
            troskovi_dgv.Columns["Planirano"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            #region bojenjeTroskiDGV-a
            foreach(DataGridViewRow row in troskovi_dgv.Rows)
            {
                row.DefaultCellStyle.BackColor = Convert.ToDouble(row.Cells["Vrednost"].Value) < Convert.ToDouble(row.Cells["Planirano"].Value) ? Color.Green : Color.LightCoral;
            }
            #endregion
            #endregion

            double ukupnoPlaniranoTroskova = biznisPlan.PlaniraniTroskovi.Sum(x => x.Value);
            trenutniPrometSum_txt.Text = trenutniPrometMagacina.ToString("#,##0.00 RSD");
            ukupanPromet_txt.Text = trenutniPrometMagacina.ToString("#,##0.00 RSD");
            planiraniPrometSum_txt.Text = biznisPlan.PlaniraniPromet.ToString("#,##0.00 RSD");
            prometCilj_txt.Text = biznisPlan.PlaniraniPromet.ToString("#,##0.00 RSD");
            troskoviSumVrednost_txt.Text = ukupniTroskovi.ToString("#,##0.00 RSD") + " / " + ukupnoPlaniranoTroskova.ToString("#,##0.00 RSD");
            troskoviSumVrednost_txt.BackColor = ukupniTroskovi < ukupnoPlaniranoTroskova ? Color.Green : Color.LightCoral;
            ostvareniTroskovi_txt.Text = ukupniTroskovi.ToString("#,##0.00 RSD");
            troskoviRacuni_txt.Text = troskoviRacuna.ToString("#,##0.00 RSD");
            planiraniTroskovi_txt.Text = ukupnoPlaniranoTroskova.ToString("#,##0.00 RSD");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UcitajBiznisPlan();
        }

        private void postaviNovuVrednostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (InputBox ib = new InputBox("Nova Vrednost", "Nova Vrednost"))
            {
                ib.ShowDialog();

                if (ib.DialogResult != DialogResult.OK)
                {
                    MessageBox.Show("Obustavljeno!");
                    return;
                }

                double novaVrednost;

                if(!double.TryParse(ib.returnData, out novaVrednost))
                {
                    MessageBox.Show("Neispravna vrednost!");
                    return;
                }

                string Tag = ((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl.Tag.ToString();

                switch (Tag)
                {
                    case "planiraniPromet":
                        biznisPlan.PlaniraniPromet = novaVrednost;
                        break;
                    default:
                        throw new Exception("Greska Tag");
                }

                biznisPlan.UpdateOrInsert();

                UcitajBiznisPlan();
                MessageBox.Show("Vrednost uspesno izmenjena!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using(fm_GrupaTroskova_List gp = new fm_GrupaTroskova_List())
                gp.ShowDialog();
        }

        private void postaviNovuPlaniranuVrednostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string grupaTroska = troskovi_dgv.Rows[troskovi_dgv.SelectedCells[0].RowIndex].Cells["Trosak"].Value.ToString();

            using (InputBox ib = new InputBox("Nova Vrednost", "Nova Vrednost"))
            {
                ib.ShowDialog();

                if (ib.DialogResult != DialogResult.OK)
                {
                    MessageBox.Show("Obustavljeno!");
                    return;
                }

                double novaVrednost;

                if (!double.TryParse(ib.returnData, out novaVrednost))
                {
                    MessageBox.Show("Neispravna vrednost!");
                    return;
                }

                biznisPlan.PlaniraniTroskovi[grupaTroska] = novaVrednost;
                biznisPlan.UpdateOrInsert();

                UcitajBiznisPlan();
                MessageBox.Show("Vrednost uspesno izmenjena!");
            }
        }
    }
}
