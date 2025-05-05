using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TDOffice_v2
{
	public partial class fm_Partner_Index : Form
	{
		public delegate void PartnerIzmenjenHandle();
		public delegate void NoviPartnerKreiranHandle();
		public NoviPartnerKreiranHandle NoviPartnerKreiran;
		public PartnerIzmenjenHandle PartnerIzmenjen;

		private Task<List<Komercijalno.Magacin>> _magaciniKomercijalno = Task.Run(() =>
		{
			List<Komercijalno.Magacin> list = new List<Komercijalno.Magacin>();
			list.AddRange(Komercijalno.Magacin.ListAsync().Result);
			list.Add(new Komercijalno.Magacin() { ID = -2, Naziv = "Greska" });
			list.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Nedefinisano" });
			list.Add(new Komercijalno.Magacin() { ID = -5, Naziv = "Dostava" });
			list.Sort((x, y) => x.ID.CompareTo(y.ID));
			return list;
		});
		private TDOffice.Partner _partner { get; set; }

		public fm_Partner_Index()
		{
			InitializeComponent();
			PrivateInitialize();
			sacuvaj_btn.Text = "Kreiraj";
		}

		public fm_Partner_Index(int partnerID)
		{
			InitializeComponent();
			PrivateInitialize();
			_partner = TDOffice.Partner.Get(partnerID);
			naziv_txt.Text = _partner.Naziv;
			mobilni_txt.Text = _partner.Mobilni;
			mejl_txt.Text = _partner.Mejl;
			grad_cmb.SelectedValue = _partner.Grad;
			komentar_rtx.Text = _partner.Komentar;
			webID_txt.Text = _partner.WebID.ToStringOrDefault();
			ppidKomercijalno_txt.Text = _partner.KomercijalnoPPID.ToStringOrDefault();
			magacin_cmb.SelectedValue =
				_partner.MagacinID == null
					? -1
					: _magaciniKomercijalno.Result.Any(x => x.ID == _partner.MagacinID)
						? (int)_partner.MagacinID
						: -2;
			for (int i = 0; i < grupe_clb.Items.Count; i++)
				if (_partner.Grupe.Any(x => x == (grupe_clb.Items[i] as TDOffice.PartnerGrupa).ID))
					grupe_clb.SetItemChecked(i, true);
				else
					grupe_clb.SetItemChecked(i, false);
		}

		private void PrivateInitialize()
		{
			grupe_clb.DataSource = TDOffice.PartnerGrupa.List();
			grupe_clb.DisplayMember = "Naziv";
			grupe_clb.ValueMember = "ID";

			grad_cmb.DataSource = TDOffice.Grad.List();
			grad_cmb.DisplayMember = "Naziv";
			grad_cmb.ValueMember = "ID";

			magacin_cmb.DataSource = _magaciniKomercijalno.Result;
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";
		}

		private void fm_Partner_Index_Load(object sender, EventArgs e) { }

		private void sacuvaj_btn_Click(object sender, EventArgs e)
		{
			if (_partner == null)
			{
				/// Proveravam da li u ovoj tabeli postoji partner sa ovim mobilnim
				/// Ukoliko postoji kao ***undefined***, onda cu samo da ga prepravim i ne obavestavam korisnika
				/// Ukoliko postoji kao neki drugi partner, onda cu da obavestim korisnika i necu ga kreirati
				TDOffice.Partner p = TDOffice
					.Partner.List($"MOBILNI = '{MobileNumber.Collate(mobilni_txt.Text)}'")
					.FirstOrDefault();

				if (p != null)
				{
					if (p.Naziv.ToLower().Contains("undefined"))
					{
						p.WebID = string.IsNullOrWhiteSpace(webID_txt.Text)
							? null
							: (int?)Convert.ToInt32(webID_txt.Text);
						p.KomercijalnoPPID = string.IsNullOrWhiteSpace(ppidKomercijalno_txt.Text)
							? null
							: (int?)Convert.ToInt32(ppidKomercijalno_txt.Text);
						p.MagacinID =
							Convert.ToInt32(magacin_cmb.SelectedValue) > 0
								? (int?)Convert.ToInt32(magacin_cmb.SelectedValue)
								: null;
						p.Naziv = naziv_txt.Text;
						p.Mobilni = MobileNumber.Collate(mobilni_txt.Text);
						p.Mejl = mejl_txt.Text;
						p.Komentar = komentar_rtx.Text;
						p.Grad = Convert.ToInt32(grad_cmb.SelectedValue);
						p.Grupe = new List<int>();
						foreach (TDOffice.PartnerGrupa pg in grupe_clb.CheckedItems)
							p.Grupe.Add(pg.ID);

						p.Update();
						MessageBox.Show("Partner uspesno kreiran!");
						return;
					}
					else
					{
						MessageBox.Show(
							$"Partner sa ovim mobilnim telefonom vec postoji!\nNjegov ID je {p.ID}, a naziv {p.Naziv}!"
						);
						return;
					}
				}

				List<int> grupe = new List<int>();
				foreach (TDOffice.PartnerGrupa pg in grupe_clb.CheckedItems)
					grupe.Add(pg.ID);

				TDOffice.Partner.Insert(
					naziv_txt.Text,
					MobileNumber.Collate(mobilni_txt.Text),
					mejl_txt.Text,
					Convert.ToInt32(grad_cmb.SelectedValue),
					komentar_rtx.Text,
					grupe,
					string.IsNullOrWhiteSpace(webID_txt.Text)
						? null
						: (int?)Convert.ToInt32(webID_txt.Text),
					string.IsNullOrWhiteSpace(ppidKomercijalno_txt.Text)
						? null
						: (int?)Convert.ToInt32(ppidKomercijalno_txt.Text),
					Convert.ToInt32(magacin_cmb.SelectedValue) > 0
						? (int?)Convert.ToInt32(magacin_cmb.SelectedValue)
						: null
				);

				MessageBox.Show("Partner uspesno kreiran!");
				if (NoviPartnerKreiran != null)
					NoviPartnerKreiran();
			}
			else
			{
				_partner.WebID = string.IsNullOrWhiteSpace(webID_txt.Text)
					? null
					: (int?)Convert.ToInt32(webID_txt.Text);
				_partner.KomercijalnoPPID = string.IsNullOrWhiteSpace(ppidKomercijalno_txt.Text)
					? null
					: (int?)Convert.ToInt32(ppidKomercijalno_txt.Text);
				_partner.Naziv = naziv_txt.Text;
				_partner.Mobilni = MobileNumber.Collate(mobilni_txt.Text);
				_partner.MagacinID =
					Convert.ToInt32(magacin_cmb.SelectedValue) > 0
						? (int?)Convert.ToInt32(magacin_cmb.SelectedValue)
						: null;
				_partner.Mejl = mejl_txt.Text;
				_partner.Komentar = komentar_rtx.Text;
				_partner.Grad = Convert.ToInt32(grad_cmb.SelectedValue);
				_partner.Grupe = new List<int>();
				foreach (TDOffice.PartnerGrupa pg in grupe_clb.CheckedItems)
					_partner.Grupe.Add(pg.ID);

				_partner.Update();

				if (PartnerIzmenjen != null)
					PartnerIzmenjen();
				MessageBox.Show("Izmene uspesno sacuvane!");
			}
		}

		private void uporediInformacijeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			API.Korisnik kor = API.Korisnik.Get((int)_partner.WebID);
			if (kor.MagacinID != _partner.MagacinID)
			{
				Komercijalno.Magacin webMag = _magaciniKomercijalno.Result.FirstOrDefault(x =>
					x.ID == kor.MagacinID
				);
				Komercijalno.Magacin tdMag =
					_partner.MagacinID == null
						? _magaciniKomercijalno.Result.FirstOrDefault(x => x.ID == -1)
						: _magaciniKomercijalno.Result.FirstOrDefault(x => x.ID == kor.MagacinID);
				Task.Run(() =>
				{
					MessageBox.Show(
						$"Korisnik ima neslaganje u parametru MagacinID na webu i tdofficeu.\nTDOffice_v2: {tdMag.Naziv}\nWEB: {webMag.Naziv}"
					);
				});
			}
			MessageBox.Show("Finish!");
		}
	}
}
