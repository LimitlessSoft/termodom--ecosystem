using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Dtos.TDApi;

namespace TDOffice_v2.Forms.Menadzment
{
    public partial class fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama : Form
    {
        public fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama()
        {
            InitializeComponent();
        }

        private void fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            UcitajUIAsync();
        }

        private async Task UcitajUIAsync()
        {
            try
            {
                var magaciniTask = TDAPI.GetAsync<List<MagacinDto>>("/magacini");
                var vrsteDokumenataTask = TDAPI.GetAsync<List<VrstaDokumentaDto>>("/vrste-dokumenata");
                var naciniPlacanjaTask = TDAPI.GetAsync<List<NacinPlacanjaDto>>("/nacini-placanja");
                var nameneTask = TDAPI.GetAsync<List<NamenaDto>>("/namene-dokumenta");

                izvorMagacin_cmb.DataSource = new List<MagacinDto>((await magaciniTask).Payload);
                izvorMagacin_cmb.DisplayMember = "Naziv";
                izvorMagacin_cmb.ValueMember = "MagacinId";

                destinacijaNoviDokumentMagacin_cmb.DataSource = new List<MagacinDto>((await magaciniTask).Payload);
                destinacijaNoviDokumentMagacin_cmb.DisplayMember = "Naziv";
                destinacijaNoviDokumentMagacin_cmb.ValueMember = "MagacinId";

                izvorVrDok_cmb.DataSource = new List<VrstaDokumentaDto>((await vrsteDokumenataTask).Payload);
                izvorVrDok_cmb.DisplayMember = "Naziv";
                izvorVrDok_cmb.ValueMember = "Id";

                destinacijaVrDok_cmb.DataSource = new List<VrstaDokumentaDto>((await vrsteDokumenataTask).Payload);
                destinacijaVrDok_cmb.DisplayMember = "Naziv";
                destinacijaVrDok_cmb.ValueMember = "Id";

                izvorNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>((await naciniPlacanjaTask).Payload);
                izvorNacinPlacanja_cmb.DisplayMember = "Naziv";
                izvorNacinPlacanja_cmb.ValueMember = "Id";

                destinacijaNoviDokumentNacinPlacanja_cmb.DataSource = new List<NacinPlacanjaDto>((await naciniPlacanjaTask).Payload);
                destinacijaNoviDokumentNacinPlacanja_cmb.DisplayMember = "Naziv";
                destinacijaNoviDokumentNacinPlacanja_cmb.ValueMember = "Id";

                izvorNamena_cmb.DataSource = new List<NamenaDto>((await nameneTask).Payload);
                izvorNamena_cmb.DisplayMember = "Naziv";
                izvorNamena_cmb.ValueMember = "Id";

                destinacijaNoviDokumentNamena_cmb.DataSource = new List<NamenaDto>((await nameneTask).Payload);
                destinacijaNoviDokumentNamena_cmb.DisplayMember = "Naziv";
                destinacijaNoviDokumentNamena_cmb.ValueMember = "Id";


                this.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Enabled = true;
            }
        }

        private void pripremiDokumente_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
