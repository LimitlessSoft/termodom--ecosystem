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
    public partial class fm_PregledStorniranihDuploPovratnica_Index : Form
    {
        public fm_PregledStorniranihDuploPovratnica_Index()
        {
            InitializeComponent();
        }

        private void fm_PregledStorniranihDuploPovratnica_Index_Load(object sender, EventArgs e)
        {

        }

        private void UcitajPodatke()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("Magacin", typeof(string));
            dt.Columns.Add("BrDok", typeof(int));
            dt.Columns.Add("Opis", typeof(string));
            dt.Columns.Add("Vrednost", typeof(double));
            dt.Columns.Add("StigaoFizicki", typeof(int));
            dt.Columns.Add("SpecifikacijaID", typeof(int));

            using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                foreach (Komercijalno.Magacin m in Komercijalno.Magacin.List(con))
                {
                    DateTime date = odDatuma_dtp.Value;
                    while (true)
                    {
                        date = date.AddDays(1);

                        if (date.Date > doDatuma_dtp.Value.Date)
                            break;

                        TDOffice.SpecifikacijaNovca spec = TDOffice.SpecifikacijaNovca.Get(m.ID, date);

                        if (spec == null)
                            continue;

                        foreach (int storniraniMPRacun in spec.Tag.StorniraniMPRacuni)
                        {
                            Komercijalno.Dokument mpRacun = Komercijalno.Dokument.Get(con, 15, storniraniMPRacun);

                            DataRow dr = dt.NewRow();

                            dr["Datum"] = date;
                            dr["Magacin"] = m.Naziv;
                            dr["BrDok"] = storniraniMPRacun;
                            dr["Opis"] = "Storno Racun - " + (mpRacun.NUID == Komercijalno.NacinUplate.Gotovina ? "Gotovinski" : "Virmanski");
                            dr["Vrednost"] = mpRacun.Potrazuje;
                            dr["StigaoFizicki"] = 0;
                            dr["SpecifikacijaID"] = spec.ID;

                            dt.Rows.Add(dr);
                        }

                        foreach (int storniraniMPRacun in spec.Tag.StornoKasaDuplo)
                        {
                            Komercijalno.Dokument mpRacun = Komercijalno.Dokument.Get(con, 15, storniraniMPRacun);

                            DataRow dr = dt.NewRow();

                            dr["Datum"] = date;
                            dr["Magacin"] = m.Naziv;
                            dr["BrDok"] = storniraniMPRacun;
                            dr["Opis"] = "Duplo fiskalizovani racun - " + (mpRacun.NUID == Komercijalno.NacinUplate.Gotovina ? "Gotovinski" : "Virmanski");
                            dr["Vrednost"] = mpRacun.Potrazuje;
                            dr["StigaoFizicki"] = 0;
                            dr["SpecifikacijaID"] = spec.ID;

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }


            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dt;

            DataGridViewComboBoxColumn stigaoFizickiCmbCol = new DataGridViewComboBoxColumn()
            {
                Name = "StigaoFizicki",
                HeaderText = "Stigao Fizicki",
                DataPropertyName = "StigaoFizicki",
                Width = 100,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Stigao Fizicki"
                },
                DataSource = new List<Tuple<int, string>>()
                {
                    new Tuple<int, string>(0, "Nije Stigao"),
                    new Tuple<int, string>(1, "Stigao je")
                },
                DisplayMember = "Item2",
                ValueMember = "Item1"
            };

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Datum",
                HeaderText = "Datum",
                DataPropertyName = "Datum",
                Width = 100,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Datum"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Magacin",
                HeaderText = "Magacin",
                DataPropertyName = "Magacin",
                Width = 250,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Magacin"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BrDok",
                HeaderText = "BrDok",
                DataPropertyName = "BrDok",
                Width = 80,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "BrDok"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Opis",
                HeaderText = "Opis",
                DataPropertyName = "Opis",
                Width = 150,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Opis"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Vrednost",
                HeaderText = "Vrednost",
                DataPropertyName = "Vrednost",
                Width = 100,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Vrednost"
                }
            });
            dataGridView1.Columns.Add(stigaoFizickiCmbCol);


            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00 RSD";
            dataGridView1.Columns["Vrednost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UcitajPodatke();
        }
    }
}
