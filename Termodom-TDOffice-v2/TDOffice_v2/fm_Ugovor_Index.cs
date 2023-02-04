using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.TDOffice;

namespace TDOffice_v2
{
    public partial class fm_Ugovor_Index : Form
    {
        private DokumentUgovor _ugovor { get; set; }
        private Task<List<Komercijalno.Magacin>> _magaciniKomercijalno = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<List<Komercijalno.Partner>> _partnerniKomercijalno = Komercijalno.Partner.ListAsync(DateTime.Now.Year);

        public fm_Ugovor_Index(int ugovorID)
        {
            InitializeComponent();
            ResetUI();
            UcitajUgovor(ugovorID);

        }

        private void fm_Ugovor_Index_Load(object sender, EventArgs e)
        {

        }

        private void ResetUI()
        {
            List<Tuple<int, string>> tipoviUgovora = new List<Tuple<int, string>>();
            foreach (TDOffice.Enums.DokumentUgovorTip tip in Enum.GetValues(typeof(TDOffice.Enums.DokumentUgovorTip)))
                tipoviUgovora.Add(new Tuple<int, string>((int)tip, tip.ToString()));

            tipUgovora_cmb.DataSource = tipoviUgovora;
            tipUgovora_cmb.ValueMember = "Item1";
            tipUgovora_cmb.DisplayMember = "Item2";

            prodavac_cmb.DataSource = _partnerniKomercijalno.Result;
            prodavac_cmb.ValueMember = "PPID";
            prodavac_cmb.DisplayMember = "Naziv";

            kupac_cmb.DataSource = _partnerniKomercijalno.Result;
            kupac_cmb.ValueMember = "PPID";
            kupac_cmb.DisplayMember = "Naziv";

            magacin_cmb.DataSource = _magaciniKomercijalno.Result;
            magacin_cmb.ValueMember = "ID";
            magacin_cmb.DisplayMember = "Naziv";

            datumUgovora_dtp.Value = DateTime.Now;
        }

        private void UcitajUgovor(int id)
        {
            tipUgovora_cmb.Enabled = false;
            datumUgovora_dtp.Enabled = false;
            prodavac_cmb.Enabled = false;
            kupac_cmb.Enabled = false;
            magacin_cmb.Enabled = false;
            novaRata_btn.Enabled = false;

            _ugovor = DokumentUgovor.Get(id);

            tipUgovora_cmb.SelectedValue = _ugovor.Tip;
            datumUgovora_dtp.Value = _ugovor.Datum;
            prodavac_cmb.SelectedValue = _ugovor.Prodavac;
            kupac_cmb.SelectedValue = _ugovor.Kupac;
            magacin_cmb.SelectedValue = _ugovor.MagacinID;

            tipUgovora_cmb.Enabled = true;
            datumUgovora_dtp.Enabled = true;
            prodavac_cmb.Enabled = true;
            kupac_cmb.Enabled = true;
            magacin_cmb.Enabled = true;
            novaRata_btn.Enabled = true;
        }
    }
}
