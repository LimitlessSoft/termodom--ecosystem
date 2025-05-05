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
	public partial class fm_PlanerPoruka_Nova : Form
	{
		public fm_PlanerPoruka_Nova()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1865000))
			{
				TDOffice.Pravo.NematePravoObavestenje(1865000);
				return;
			}

			InitializeComponent();

			korisnik_cmb.DataSource = TDOffice.User.List();
			korisnik_cmb.DisplayMember = "Username";
			korisnik_cmb.ValueMember = "ID";
		}

		private void fm_PlanerPoruka_Nova_Load(object sender, EventArgs e) { }

		private void button1_Click(object sender, EventArgs e)
		{
			if ((int)korisnik_cmb.SelectedIndex < 0)
			{
				MessageBox.Show("Morate izabrati korisnika!");
				return;
			}

			if (string.IsNullOrWhiteSpace(textBox1.Text))
			{
				MessageBox.Show("Morate uneti neki tekst stavke planera!");
				return;
			}

			TDOffice.Planer.Stavka s = TDOffice
				.Planer.Stavka.ListByUserID((int)korisnik_cmb.SelectedValue)
				.Where(x => x.Datum.Date == dateTimePicker1.Value.Date)
				.FirstOrDefault();

			if (s == null)
			{
				TDOffice.Planer.Stavka.Insert(
					(int)korisnik_cmb.SelectedValue,
					dateTimePicker1.Value,
					"***" + textBox1.Text + "***"
				);
			}
			else
			{
				s.Body += "***" + textBox1.Text + "***";
				s.Update();
			}

			TDOffice.Poruka.Insert(
				new TDOffice.Poruka()
				{
					Datum = DateTime.Now,
					Naslov = "Nova Planer Stavka",
					Posiljalac = Program.TrenutniKorisnik.ID,
					Primalac = (int)korisnik_cmb.SelectedValue,
					Status = TDOffice.PorukaTip.Standard,
					Tag = new TDOffice.PorukaAdditionalInfo(),
					Tekst =
						"Nova planer stavka na datum "
						+ dateTimePicker1.Value.ToString("dd.MM.yyyy")
						+ ": ***"
						+ textBox1.Text
				}
			);

			MessageBox.Show("Uspesno dodata stavka planera!");

			this.Close();
		}
	}
}
