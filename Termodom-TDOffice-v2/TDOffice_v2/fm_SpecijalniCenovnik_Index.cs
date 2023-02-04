using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_SpecijalniCenovnik_Index : Form
    {
        private int _cenovnikID { get; }
        private TDOffice.SpecijalniCenovnik sc { get; set; }
        private Task<fm_Help> _helpForm { get; set; }
        private List<TDOffice.SpecijalniCenovnik.Item> _items { get; set; }
        private Task<List<Komercijalno.Dokument>> _dokumentiNabavke { get; set; } = Task.Run(() => { return Komercijalno.Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)"); });
        private Task<List<Komercijalno.Stavka>> _stavkeNabavke { get; set; } = Task.Run(() => { return Komercijalno.Stavka.List(DateTime.Now.Year, $"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)"); });
        private Task<List<TDOffice.PartnerKomercijalno>> _partnerKomercijalnoKojiSadrzeOvajCenovnik { get; set; }
        private Task<List<Komercijalno.Partner>> _partneriKomercijalno { get; set; } = Komercijalno.Partner.ListAsync();
        public static readonly List<Tuple<int, string>> _uslovTipovi = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(1, "Pocetna Cena + Marza % (modifikator)"),
            new Tuple<int, string>(2, "Prodajna Cena - Rabat % (modifikator)"),
            new Tuple<int, string>(3, "Prodajna Cena - % (modifikator) od razlike izmedju Pocetne Cene i Prodajne Cene")
        };
        private Task<List<Komercijalno.Roba>> _roba = Task.Run(() => { return Komercijalno.Roba.List(DateTime.Now.Year); });
        private Task<List<Komercijalno.Tarife>> _tarife = Task.Run(() => { return Komercijalno.Tarife.List(); });
        private Task<List<DTO.TDBrain_v3.NabavnaCenaDTO>> _realneNabavneCene = Task.Run(async () =>
        {
            List<DTO.TDBrain_v3.NabavnaCenaDTO> list = new List<DTO.TDBrain_v3.NabavnaCenaDTO>();
            HttpResponseMessage response = await TDBrain_v3.GetAsync($"/komercijalno/roba/getnabavnacena?datum={DateTime.Now.ToString("dd-MM-yyyy")}");
            if ((int)response.StatusCode == 200)
            {
                list = JsonConvert.DeserializeObject<List<DTO.TDBrain_v3.NabavnaCenaDTO>>(await response.Content.ReadAsStringAsync());
                return list;
            }
            else
            {
                MessageBox.Show("Doslo je do greske prilikom ucitavanja nabavnih cena!");
                return null;
            }    
        });

        public fm_SpecijalniCenovnik_Index(int cenovnikID)
        {
            InitializeComponent();
            if(!Program.TrenutniKorisnik.ImaPravo(133610))
            {
                TDOffice.Pravo.NematePravoObavestenje(133610);
                this.Close();
                return;
            }
            _cenovnikID = cenovnikID;
        }
        private void fm_SpecijalniCenovnik_Index_Load(object sender, EventArgs e)
        {
            UcitajSpecijalniCenovnik(_cenovnikID);
            _helpForm = this.InitializeHelpModulAsync(Modul._1325_fm_ZamenaRobe_Index);
        }
        private void UcitajSpecijalniCenovnik(int cenovnikID)
        {
            sc = TDOffice.SpecijalniCenovnik.Get(cenovnikID);
            button4.Enabled = false;
            _partnerKomercijalnoKojiSadrzeOvajCenovnik = Task.Run(() =>
            {
                var list = TDOffice.PartnerKomercijalno.List();
                var newList = new List<TDOffice.PartnerKomercijalno>();

                foreach (TDOffice.PartnerKomercijalno p in list)
                    if (p.SpecijalniCenovnikPars != null && p.SpecijalniCenovnikPars.SpecijalniCenovnikList != null && p.SpecijalniCenovnikPars.SpecijalniCenovnikList.Contains(sc.ID))
                        newList.Add(p);

                this.Invoke((MethodInvoker) delegate
                {
                    button4.Enabled = true;
                });

                return newList;
            });
            _items = TDOffice.SpecijalniCenovnik.Item.List("CENOVNIKID = " + cenovnikID);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("CenovnikID", typeof(int));
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("Proizvod", typeof(string));
            dt.Columns.Add("TrenutnaNabavnaCena", typeof(double));
            dt.Columns.Add("NabavnaCenaMargina", typeof(double));
            dt.Columns.Add("PocetnaCena", typeof(double));
            dt.Columns.Add("UslovTip", typeof(int));
            dt.Columns.Add("UslovModifikator", typeof(double));
            dt.Columns.Add("MaxRabat", typeof(double));
            dt.Columns.Add("TrenutnaProdajnaCena", typeof(double));
            dt.Columns.Add("ProdajnaCenaSaUslovima", typeof(double));

            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                foreach (TDOffice.SpecijalniCenovnik.Item item in _items)
                {
                    Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == item.RobaID);
                    var rnc = _realneNabavneCene.Result.FirstOrDefault(x => x.RobaID == item.RobaID);
                    if(rnc == null)
                    {
                        MessageBox.Show($"Roba [({r.ID}) {r.Naziv}] nema pronadjen ni jedan ulaz (nabavnu cenu)! Bice zobidjena i nece biti prikazana u cenovniku!");
                        continue;
                    }
                    Komercijalno.Tarife tarifa = _tarife.Result.FirstOrDefault(x => x.TarifaID == r.TarifaID);
                    Tuple<double, double> cenaNaDan = Komercijalno.Procedure.CenaNaDan(con, 12, item.RobaID, DateTime.Now);
                    double realnaNabavnaCena = rnc.NabavnaCenaBezPDV;
                    DataRow dr = dt.NewRow();
                    dr["ID"] = item.ID;
                    dr["CenovnikID"] = item.CenovnikID;
                    dr["RobaID"] = item.RobaID;
                    dr["KatBr"] = r == null ? "UNDEFINED" : r.KatBr;
                    dr["Proizvod"] = r == null ? "UNDEFINED" : r.Naziv;
                    dr["TrenutnaNabavnaCena"] = realnaNabavnaCena;
                    dr["NabavnaCenaMargina"] = item.NabavnaCenaMargina;
                    dr["PocetnaCena"] = realnaNabavnaCena * (1 + (item.NabavnaCenaMargina / 100));
                    dr["UslovTip"] = item.UslovTip;
                    dr["UslovModifikator"] = item.UslovModifikator;
                    dr["MaxRabat"] = item.MaxRabat;
                    dr["TrenutnaProdajnaCena"] = (1 - ((double)tarifa.Stopa / (100 + (double)tarifa.Stopa))) * cenaNaDan.Item2;
                    dr["ProdajnaCenaSaUslovima"] = IzracunajSpecijalnuCenu(realnaNabavnaCena, cenaNaDan.Item2, item.UslovTip, item.UslovModifikator, (double)tarifa.Stopa, item.NabavnaCenaMargina);
                    dt.Rows.Add(dr);
                }
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
                Width = 70,
                ReadOnly = true,
                Visible = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "ID"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CenovnikID",
                HeaderText = "CenovnikID",
                DataPropertyName = "CenovnikID",
                Width = 70,
                ReadOnly = true,
                Visible = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "CenovnikID"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "RobaID",
                HeaderText = "RobaID",
                DataPropertyName = "RobaID",
                Width = 70,
                ReadOnly = true,
                Visible = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "RobaID"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "KatBr",
                HeaderText = "KatBr",
                DataPropertyName = "KatBr",
                Width = 80,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "KatBr"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Proizvod",
                HeaderText = "Proizvod",
                DataPropertyName = "Proizvod",
                Width = 250,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Proizvod"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TrenutnaNabavnaCena",
                HeaderText = "Trenutna Nabavna Cena bez PDV",
                DataPropertyName = "TrenutnaNabavnaCena",
                Width = 70,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Trenutna Nabavna Cena bez PDV"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    Format = "#,##0.00"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "NabavnaCenaMargina",
                HeaderText = "Nabavna Cena Margina %",
                DataPropertyName = "NabavnaCenaMargina",
                Width = 70,
                ReadOnly = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Nabavna Cena Margina %"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    BackColor = Color.FromArgb(255, 255, 204)
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PocetnaCena",
                HeaderText = "Pocetna Cena",
                DataPropertyName = "PocetnaCena",
                Width = 70,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Pocetna Cena"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    Format = "#,##0.00"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = "UslovTip",
                HeaderText = "Uslov Tip",
                DataPropertyName = "UslovTip",
                Width = 400,
                ReadOnly = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Value = "Uslov Tip"
                },
                DataSource = _uslovTipovi,
                DisplayMember = "Item2",
                ValueMember = "Item1"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "UslovModifikator",
                HeaderText = "Uslov Modifikator",
                DataPropertyName = "UslovModifikator",
                Width = 70,
                ReadOnly = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Uslov Modifikator"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    BackColor = Color.FromArgb(255, 255, 204)
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MaxRabat",
                HeaderText = "Max Rabat",
                DataPropertyName = "MaxRabat",
                Width = 70,
                ReadOnly = false,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Max Rabat %"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    BackColor = Color.FromArgb(255, 255, 204)
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TrenutnaProdajnaCena",
                HeaderText = "Trenutna Prodajna Cena bez PDV",
                DataPropertyName = "TrenutnaProdajnaCena",
                Width = 70,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Trenutna Prodajna Cena bez PDV"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Black,
                    Format = "#,##0.00"
                }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ProdajnaCenaSaUslovima",
                HeaderText = "Prodajna Cena Sa Uslovima",
                DataPropertyName = "ProdajnaCenaSaUslovima",
                Width = 70,
                ReadOnly = true,
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    },
                    Value = "Prodajna Cena bez PDV Sa Uslovima"
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.White,
                    Format = "#,##0.00",
                    BackColor = Color.FromArgb(150, 50, 255)
                }
            });

            dataGridView1.DataSource = dt;

            id_txt.Text = sc.ID.ToString();
            naziv_txt.Text = sc.Naziv;
        }
        public static double IzracunajSpecijalnuCenu(FbConnection con, TDOffice.SpecijalniCenovnik.Item specijalniCenovnikItem, int magacinID,
            List<Komercijalno.Dokument> dokumentiNabavke = null, List<Komercijalno.Stavka> stavkeNabavke = null)
        {
            if(dokumentiNabavke == null)
                dokumentiNabavke = Komercijalno.Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

            if(stavkeNabavke == null)
                stavkeNabavke = Komercijalno.Stavka.List(DateTime.Now.Year, $"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

            Tuple<double, double> cenaNaDan = Komercijalno.Procedure.CenaNaDan(con, magacinID, specijalniCenovnikItem.RobaID, DateTime.Now);
            Komercijalno.Roba r = Komercijalno.Roba.Get(con, specijalniCenovnikItem.RobaID);
            Komercijalno.Tarife tarifa = Komercijalno.Tarife.Get(r.TarifaID);
            return IzracunajSpecijalnuCenu(Komercijalno.Komercijalno.GetRealnaNabavnaCena(r.ID, DateTime.Now, dokumentiNabavke, stavkeNabavke), cenaNaDan.Item2, specijalniCenovnikItem.UslovTip, specijalniCenovnikItem.UslovModifikator, (double)tarifa.Stopa, specijalniCenovnikItem.NabavnaCenaMargina);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nabavnaCena">Nabavna cena bez PDV</param>
        /// <param name="prodajnaCena">Prodajna cena bez PDV</param>
        /// <param name="uslovTip"></param>
        /// <param name="uslovModifikator"></param>
        /// <param name="stopa"></param>
        /// <param name="nabavnaCenaMargina"></param>
        /// <returns></returns>
        private static double IzracunajSpecijalnuCenu(double nabavnaCena, double prodajnaCena, int uslovTip, double uslovModifikator, double stopa, double nabavnaCenaMargina)
        {
            double prodajnaBezPDV, pocetnaCena, razlika;
            switch(uslovTip)
            {
                case 1:
                    return nabavnaCena * (1 + (nabavnaCenaMargina / 100)) * (1 + (uslovModifikator / 100));
                case 2:
                    prodajnaBezPDV = (1 - (stopa / (100 + stopa))) * prodajnaCena;
                    return prodajnaBezPDV * (1 - (uslovModifikator / 100));
                case 3:
                    pocetnaCena = nabavnaCena * (1 + (nabavnaCenaMargina / 100));
                    prodajnaBezPDV = (1 - (stopa / (100 + stopa))) * prodajnaCena;
                    razlika = prodajnaBezPDV - pocetnaCena;
                    return prodajnaBezPDV - (razlika * (uslovModifikator / 100));
                default:
                    return 0;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string noviNaziv = naziv_txt.Text;

            if (noviNaziv == sc.Naziv)
            {
                MessageBox.Show("Naziv je ostao nepromenjen!");
                return;
            }

            if(string.IsNullOrWhiteSpace(noviNaziv))
            {
                MessageBox.Show("Morate uneti neki naziv!");
                return;
            }

            sc.Naziv = noviNaziv;
            sc.Update();

            MessageBox.Show("Naziv uspesno sacuvan!");
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            using(IzborRobe ir = new IzborRobe(12))
            {
                ir.OnRobaClickHandler += (Komercijalno.RobaUMagacinu[] rums) =>
                {
                    DataTable dt = (dataGridView1.DataSource as DataTable).Copy();
                    using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
                    {
                        con.Open();
                        foreach(Komercijalno.RobaUMagacinu rum in rums)
                        {
                            Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == rum.RobaID);
                            if(_items.Any(x => x.RobaID == rum.RobaID))
                            {
                                MessageBox.Show($"Proizvod {r.Naziv} vec postoji u ovom cenovniku!");
                                continue;
                            }
                            int itemID = TDOffice.SpecijalniCenovnik.Item.Insert(sc.ID, rum.RobaID, 2, 0, 10, 0);
                            _items.Add(new TDOffice.SpecijalniCenovnik.Item()
                            {
                                ID = itemID,
                                CenovnikID = sc.ID,
                                MaxRabat = 10,
                                NabavnaCenaMargina = 0,
                                RobaID = rum.RobaID,
                                UslovModifikator = 0,
                                UslovTip = 2
                            });
                            Komercijalno.Tarife tarifa = _tarife.Result.FirstOrDefault(x => x.TarifaID == r.TarifaID);
                            Tuple<double, double> cenaNaDan = Komercijalno.Procedure.CenaNaDan(con, 12, rum.RobaID, DateTime.Now);
                            double realnaNabavnaCena = _realneNabavneCene.Result.First(x => x.RobaID == rum.RobaID).NabavnaCenaBezPDV;
                            DataRow dr = dt.NewRow();
                            dr["ID"] = itemID;
                            dr["CenovnikID"] = sc.ID;
                            dr["RobaID"] = rum.RobaID;
                            dr["KatBr"] = r == null ? "UNDEFINED" : r.KatBr;
                            dr["Proizvod"] = r == null ? "UNDEFINED" : r.Naziv;
                            dr["TrenutnaNabavnaCena"] = realnaNabavnaCena;
                            dr["NabavnaCenaMargina"] = 0;
                            dr["PocetnaCena"] = realnaNabavnaCena;
                            dr["UslovTip"] = 2;
                            dr["UslovModifikator"] = 0;
                            dr["MaxRabat"] = 10;
                            dr["TrenutnaProdajnaCena"] = (1 - ((double)tarifa.Stopa / (100 + (double)tarifa.Stopa))) * cenaNaDan.Item2;
                            dr["ProdajnaCenaSaUslovima"] = IzracunajSpecijalnuCenu(realnaNabavnaCena, cenaNaDan.Item2, 1, 0, (double)tarifa.Stopa, 0);
                            dt.Rows.Add(dr);
                        }
                    }
                    dataGridView1.DataSource = dt;
                };
                ir.ShowDialog();
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue.ToString() == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() ||
                dataGridView1.Columns[e.ColumnIndex].ReadOnly)
                return;

            int itemID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            TDOffice.SpecijalniCenovnik.Item item = _items.FirstOrDefault(x => x.ID == itemID);
            Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == item.RobaID);
            Komercijalno.Tarife tarifa = _tarife.Result.FirstOrDefault(x => x.TarifaID == r.TarifaID);
            double realnaNabavnaCena = _realneNabavneCene.Result.First(x => x.RobaID == item.RobaID).NabavnaCenaBezPDV;
            Tuple<double, double> cenaNaDan = null;
            using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                cenaNaDan = Komercijalno.Procedure.CenaNaDan(con, 12, item.RobaID, DateTime.Now);
            }

            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "UslovTip":
                    item.UslovTip = _uslovTipovi.FirstOrDefault(x => x.Item2 == e.FormattedValue.ToString()).Item1;
                    item.Update();
                    dataGridView1.Rows[e.RowIndex].Cells["ProdajnaCenaSaUslovima"].Value = IzracunajSpecijalnuCenu(realnaNabavnaCena, cenaNaDan.Item2, item.UslovTip, item.UslovModifikator, (double)tarifa.Stopa, item.NabavnaCenaMargina);
                    break;
                case "UslovModifikator":
                    item.UslovModifikator = Convert.ToDouble(e.FormattedValue);
                    item.Update();
                    dataGridView1.Rows[e.RowIndex].Cells["ProdajnaCenaSaUslovima"].Value = IzracunajSpecijalnuCenu(realnaNabavnaCena, cenaNaDan.Item2, item.UslovTip, item.UslovModifikator, (double)tarifa.Stopa, item.NabavnaCenaMargina);
                    break;
                case "MaxRabat":
                    item.MaxRabat = Convert.ToDouble(e.FormattedValue);
                    item.Update();
                    break;
                case "NabavnaCenaMargina":
                    item.NabavnaCenaMargina = Convert.ToDouble(e.FormattedValue);
                    item.Update();
                    dataGridView1.Rows[e.RowIndex].Cells["PocetnaCena"].Value = realnaNabavnaCena * (1 + (item.NabavnaCenaMargina / 100));
                    dataGridView1.Rows[e.RowIndex].Cells["ProdajnaCenaSaUslovima"].Value = IzracunajSpecijalnuCenu(realnaNabavnaCena, cenaNaDan.Item2, item.UslovTip, item.UslovModifikator, (double)tarifa.Stopa, item.NabavnaCenaMargina);
                    break;
                default:
                    break;
            }
        }

        private void ukloniStavkuIzCenovnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Morate selektovati neku stavku!");
                return;
            }
            int itemID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            TDOffice.SpecijalniCenovnik.Item.Delete(itemID);
            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _helpForm.Result.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using(InputBox ib = new InputBox("Kloniraj", "Kloniraj stavke u cenovnik ID (stavke koje ne postoje ce biti dodate)"))
            {
                ib.ShowDialog();
                if (ib.DialogResult != DialogResult.OK)
                    return;

                object returnData = ib.returnData;

                if(returnData == null)
                {
                    MessageBox.Show("Neispravan ID!");
                    return;
                }

                int id;
                try
                {
                    id = Convert.ToInt32(returnData);
                }
                catch
                {
                    MessageBox.Show("Neispravan ID!");
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PPID", typeof(int));
            dt.Columns.Add("Naziv", typeof(string));

            foreach(var p in _partnerKomercijalnoKojiSadrzeOvajCenovnik.Result)
            {
                Komercijalno.Partner kp = _partneriKomercijalno.Result.FirstOrDefault(x => x.PPID == p.PPID);
                DataRow dr = dt.NewRow();
                dr["PPID"] = p.PPID;
                dr["Naziv"] = kp == null ? "UNKNOWN" : kp.Naziv;
                dt.Rows.Add(dr);
            }

            using (DataGridViewSelectBox sb = new DataGridViewSelectBox(dt))
                sb.ShowDialog();
        }
    }
}
