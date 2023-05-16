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
    public partial class _1301_fm_Poruka_Nova : Form
    {
        private TDOffice.Poruka _poruka { get; set; }
        public bool IsNaslovReadOnly { get; set; } = false;

        private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();

        public TDOffice.PorukaAction Akcija { get; set; } = TDOffice.PorukaAction.NULL;
        public object AdditionalInfo { get; set; }
        public bool VidljivostPorukeIzbor
        {
            get
            {
                return _vidljivostPorukeIzbor;
            }
            set
            {
                if (!Program.TrenutniKorisnik.ImaPravo(130108))
                    return;

                vidljivostPoruke_cmb.Visible = value;
                _vidljivostPorukeIzbor = value;
            }
        }
        private bool _vidljivostPorukeIzbor = true;

        public _1301_fm_Poruka_Nova()
        {
            InitializeComponent();
            _poruka = new TDOffice.Poruka() { Tag = new TDOffice.PorukaAdditionalInfo() { Action = TDOffice.PorukaAction.NULL } };
            SetUI();
        }
        public _1301_fm_Poruka_Nova(TDOffice.Poruka poruka)
        {
            InitializeComponent();
            this._poruka = poruka;

            if (_poruka.Status == TDOffice.PorukaTip.Sticky)
                _poruka.Tag.Action = TDOffice.PorukaAction.OdgovorNaStickyPoruku;

            SetUI();
            tekst_rtb.Text = poruka.Tekst;
            naslov_txt.Text = poruka.Naslov;
        }

        private void _1301_fm_Poruka_Nova_Load(object sender, EventArgs e)
        {
            vidljivostPoruke_cmb.SelectedIndex = 0;
        }

        private void SetUI()
        {
            this.Location = new System.Drawing.Point(10 + (System.DateTime.Now.Millisecond / 5), 10 + (System.DateTime.Now.Millisecond / 5));

            if (IsNaslovReadOnly)
                naslov_txt.Enabled = false;

            primalac_clb.DisplayMember = "Username";
            bccPrimalac_clb.DisplayMember = "Username";

            primalac_clb.Sorted = true;
            bccPrimalac_clb.Sorted = true;
            foreach (TDOffice.User u in _korisnici.Result)
            {
                primalac_clb.Items.Add(u);
                bccPrimalac_clb.Items.Add(u);
            }

            vidljivostPoruke_cmb.Items.Add("Standard");
            vidljivostPoruke_cmb.Items.Add("Sticky");
            vidljivostPoruke_cmb.Items.Add("Expanding");

            vidljivostPoruke_cmb.Visible = VidljivostPorukeIzbor;

            vidljivostPoruke_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(130108);

            if (_poruka != null && _poruka.Primalac != 0)
            {
                splitContainer1.SplitterDistance = 0;
                splitContainer1.Panel1.Enabled = false;
                splitContainer1.Panel1.Hide();

                for (int i = 0; i < primalac_clb.Items.Count; i++)
                {
                    if((primalac_clb.Items[i] as TDOffice.User).ID == _poruka.Primalac)
                    {
                        primalac_clb.SetItemChecked(i, true);
                        break;
                    }
                }
            }
            
        }
        
        private void posalji_btn_Click(object sender, EventArgs e)
        {
            string naslov = naslov_txt.Text;
            string tekst = tekst_rtb.Text;

            if(naslov.Length > 64)
            {
                MessageBox.Show("Naslov ne sme prelaziti 64 karaktera!");
                return;
            }
            if (string.IsNullOrWhiteSpace(tekst))
            {
                MessageBox.Show("Morate uneti tekst poruke!");
                return;
            }
            if (tekst.Length > 4096)
            {
                MessageBox.Show("Poruka ne sme prelaziti 4096 karaktera!");
                return;
            }

            if(primalac_clb.CheckedItems.Count == 0)
            {
                MessageBox.Show("Morate selektovati barem jednog primaoca!");
                return;
            }
            int maxPaketID = TDOffice.Poruka.MaxPaketID();
            foreach (TDOffice.User korisnik in primalac_clb.CheckedItems)
            {
                switch (_poruka.Tag.Action)
                {
                    case TDOffice.PorukaAction.NULL:
                        {
                            TDOffice.Poruka.Insert(new TDOffice.Poruka()
                            {
                                Datum = DateTime.Now,
                                Naslov = string.IsNullOrWhiteSpace(naslov) ? "Bez naslova" : naslov,
                                Posiljalac = Program.TrenutniKorisnik.ID,
                                Primalac = korisnik.ID,
                                Status = vidljivostPoruke_cmb.SelectedIndex == 0 ? TDOffice.PorukaTip.Standard : vidljivostPoruke_cmb.SelectedIndex == 1 ? TDOffice.PorukaTip.Sticky : TDOffice.PorukaTip.Expanding,
                                Tag = new TDOffice.PorukaAdditionalInfo() { Action = Akcija, AdditionalInfo = AdditionalInfo },
                                Tekst = tekst,
                                Paket = maxPaketID,
                                TipPrimaoca = TDOffice.Enums.PorukaTipPrimaoca.CC
                            }) ;
                            break;
                        }
                    case TDOffice.PorukaAction.NoviPartner:
                        {
                            TDOffice.Poruka.Insert(new TDOffice.Poruka()
                            {
                                Datum = DateTime.Now,
                                Naslov = naslov,
                                Posiljalac = Program.TrenutniKorisnik.ID,
                                Primalac = korisnik.ID,
                                Status = TDOffice.PorukaTip.Standard,
                                Tag = new TDOffice.PorukaAdditionalInfo()
                                {
                                    Action = TDOffice.PorukaAction.NoviPartner,
                                    AdditionalInfo = Convert.ToInt32(_poruka.Tag.AdditionalInfo)
                                },
                                Tekst = tekst
                            });
                            break;
                        }
                    case TDOffice.PorukaAction.OdgovorNaStickyPoruku:
                        {
                            TDOffice.Poruka.Insert(new TDOffice.Poruka()
                            {
                                Datum = DateTime.Now,
                                Naslov = naslov,
                                Posiljalac = Program.TrenutniKorisnik.ID,
                                Primalac = korisnik.ID,
                                Status = TDOffice.PorukaTip.Standard,
                                Tag = new TDOffice.PorukaAdditionalInfo()
                                {
                                    Action = TDOffice.PorukaAction.OdgovorNaStickyPoruku,
                                    AdditionalInfo = Convert.ToInt32(_poruka.ID)
                                },
                                Tekst = tekst
                            });
                            break;
                        }
                }
            }
            foreach (TDOffice.User korisnik in bccPrimalac_clb.CheckedItems)
            {
                switch (_poruka.Tag.Action)
                {
                    case TDOffice.PorukaAction.NULL:
                        {
                            TDOffice.Poruka.Insert(new TDOffice.Poruka()
                            {
                                Datum = DateTime.Now,
                                Naslov = string.IsNullOrWhiteSpace(naslov) ? "Bez naslova" : naslov,
                                Posiljalac = Program.TrenutniKorisnik.ID,
                                Primalac = korisnik.ID,
                                Status = vidljivostPoruke_cmb.SelectedIndex == 0 ? TDOffice.PorukaTip.Standard : vidljivostPoruke_cmb.SelectedIndex == 1 ? TDOffice.PorukaTip.Sticky : TDOffice.PorukaTip.Expanding,
                                Tag = new TDOffice.PorukaAdditionalInfo() { Action = Akcija, AdditionalInfo = AdditionalInfo },
                                Tekst = tekst,
                                Paket = maxPaketID,
                                TipPrimaoca = TDOffice.Enums.PorukaTipPrimaoca.BCC
                            });
                            break;
                        }
                    
                }
            }
                MessageBox.Show("Poruka uspesno poslata!");

            this.Close();
        }

       
        private void SelektujDeselektujSvePrimaoce_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;

                    if(sourceControl.Name != "bccPrimalac_clb")
                    {
                        Boolean checkirano = false;
                        if (primalac_clb.CheckedItems.Count != 0)
                            checkirano = false;
                        else
                            checkirano = true;


                        for (int i = 0; i < primalac_clb.Items.Count; i++)
                        {
                            if ((primalac_clb.Items[i] as TDOffice.User).ID != Program.TrenutniKorisnik.ID)
                            {
                                primalac_clb.SetItemChecked(i, checkirano);
                            }
                        }
                    }
                    else
                    {
                        Boolean checkirano = false;
                        if (bccPrimalac_clb.CheckedItems.Count != 0)
                            checkirano = false;
                        else
                            checkirano = true;


                        for (int i = 0; i < bccPrimalac_clb.Items.Count; i++)
                        {
                            if ((bccPrimalac_clb.Items[i] as TDOffice.User).ID != Program.TrenutniKorisnik.ID)
                            {
                                bccPrimalac_clb.SetItemChecked(i, checkirano);
                            }
                        }
                    }
                }
            }
        }

        private void primalac_clb_SelectedIndexChanged(object sender, EventArgs e)
        {
             
        }


        private void bccPrimalac_clb_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void primalac_clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            TDOffice.User current = primalac_clb.Items[e.Index] as TDOffice.User;

            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < bccPrimalac_clb.Items.Count; i++)
                {
                    if ((bccPrimalac_clb.Items[i] as TDOffice.User).ID == current.ID)
                    {
                        bccPrimalac_clb.Items.RemoveAt(i);
                        break;
                    }
                }
            }
            else
                if (!bccPrimalac_clb.Items.OfType<TDOffice.User>().Select(x => x.ID).Contains(current.ID))
                bccPrimalac_clb.Items.Add(current);
        }

        private void bccPrimalac_clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            TDOffice.User current = bccPrimalac_clb.Items[e.Index] as TDOffice.User;

            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < primalac_clb.Items.Count; i++)
                {
                    if ((primalac_clb.Items[i] as TDOffice.User).ID == current.ID)
                    {
                        primalac_clb.Items.RemoveAt(i);
                        break;
                    }
                }
            }
            else
                if (!primalac_clb.Items.OfType<TDOffice.User>().Select(x => x.ID).Contains(current.ID))
                primalac_clb.Items.Add(current);
        }
    }

    public class NovaPorukaPoslataEventArgs
    {
        public TDOffice.Poruka poruka { get; set; }
    }
}

