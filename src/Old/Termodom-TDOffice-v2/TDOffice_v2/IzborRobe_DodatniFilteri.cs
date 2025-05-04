using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class IzborRobe_DodatniFilteri : Form
	{
		#region Enums
		public enum DodatniFilterIzvorRobe
		{
			NULL = -1,
			NePopisanaRoba = 0,
			RobaSaPrometom = 1,
			RobaVanPopisa = 2
		}

		public enum DodatniFilterTrenutnoStanjeRobe
		{
			NULL = -1,
			ImaNaStanju = 0,
			NemaNaStanju = 1
		}

		public enum DodatniFilterKriticneOptimalneZalihe
		{
			NULL = -1,
			IspodKriticnihZaliha = 0,
			IspodOptimalnihZaliha = 1
		}
		#endregion

		public class OnFilterChangedArgs
		{
			public string GrupaID { get; set; } = null;
			public int? PodgrupaID { get; set; } = null;
			public int? DobavljacID { get; set; } = null;
			public string ProID { get; set; } = null; // ProizvodjacID
			public int? MagacinID { get; set; } = null;
			public DodatniFilterIzvorRobe IzvorRobe { get; set; } = DodatniFilterIzvorRobe.NULL;
			public DodatniFilterTrenutnoStanjeRobe StanjeRobe { get; set; } =
				DodatniFilterTrenutnoStanjeRobe.NULL;
			public DodatniFilterKriticneOptimalneZalihe OptimalneKriticneZalihe { get; set; } =
				DodatniFilterKriticneOptimalneZalihe.NULL;

			/// <summary>
			/// int / Tuple<DateTime, DateTime>
			/// </summary>
			public object IzvorRobeTag { get; set; }
		}

		public bool load { get; set; } = false;
		public bool loadPodgrupa { get; set; } = false;

		public delegate void FilterChanged(OnFilterChangedArgs args);
		public FilterChanged OnFilterChanged;

		private OnFilterChangedArgs _args = new OnFilterChangedArgs();

		private List<Komercijalno.Grupa> _sveGrupe = new List<Komercijalno.Grupa>();
		private List<Komercijalno.PodGrupa> _svePodgrupe = new List<Komercijalno.PodGrupa>();
		private List<Komercijalno.Proizvodjac> _sviProizvodjaci =
			new List<Komercijalno.Proizvodjac>();
		private List<Komercijalno.Dobavljac> _sviDobavljaci = new List<Komercijalno.Dobavljac>();
		private List<Komercijalno.Magacin> _sviMagacini = new List<Komercijalno.Magacin>();

		public IzborRobe_DodatniFilteri()
		{
			InitializeComponent();

			this.cb_NePopisanaRoba.Tag = DodatniFilterIzvorRobe.NePopisanaRoba;
			this.cb_RobaSaPrometom.Tag = DodatniFilterIzvorRobe.RobaSaPrometom;
			this.cb_RobaVanPopisaBr.Tag = DodatniFilterIzvorRobe.RobaVanPopisa;

			this.cb_BezRobeSaStanjemNula.Tag = DodatniFilterTrenutnoStanjeRobe.ImaNaStanju;
			this.cb_SamoRobaSaStanjemNula.Tag = DodatniFilterTrenutnoStanjeRobe.NemaNaStanju;

			this.cb_SveIspodKriticnihZaliha.Tag =
				DodatniFilterKriticneOptimalneZalihe.IspodKriticnihZaliha;
			this.cb_SveIspodOptimalnihZaliha.Tag =
				DodatniFilterKriticneOptimalneZalihe.IspodOptimalnihZaliha;

			SetUI();
		}

		private void SetUI()
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();

				_sveGrupe = Komercijalno.Grupa.List(con);
				_svePodgrupe = Komercijalno.PodGrupa.List(con);
				_sviProizvodjaci = Komercijalno.Proizvodjac.List(con);
				_sviDobavljaci = Komercijalno.Dobavljac.List(con);
				_sviMagacini = Komercijalno.Magacin.List(con);
			}

			List<Komercijalno.Grupa> gr = _sveGrupe.Where(x => x.Vrsta == 1).ToList();
			gr.Add(new Komercijalno.Grupa() { GrupaID = "-1", Naziv = "<izaberi grupu>" });
			gr.Sort((x, y) => x.GrupaID.CompareTo(y.GrupaID));
			cmb_Grupa.DataSource = gr;
			cmb_Grupa.DisplayMember = "Naziv";
			cmb_Grupa.ValueMember = "GrupaID";

			List<Komercijalno.Proizvodjac> pro = _sviProizvodjaci;
			pro.Add(
				new Komercijalno.Proizvodjac() { ProID = "-1", Naziv = "<izaberi proizvodjaca>" }
			);
			pro.Sort((x, y) => x.ProID.CompareTo(y.ProID));
			cmb_Proizvodjac.DataSource = pro;
			cmb_Proizvodjac.DisplayMember = "Naziv";
			cmb_Proizvodjac.ValueMember = "ProID";
			cmb_Proizvodjac.SelectedValue = "-1";

			List<Komercijalno.Dobavljac> dob = _sviDobavljaci;
			dob.Add(
				new Komercijalno.Dobavljac() { DobavljacID = -1, Naziv = "<izaberi dobavljaca>" }
			);
			dob.Sort((x, y) => x.DobavljacID.CompareTo(y.DobavljacID));
			cmb_Dobavljac.DataSource = dob;
			cmb_Dobavljac.DisplayMember = "Naziv";
			cmb_Dobavljac.ValueMember = "DobavljacID";

			List<Komercijalno.Magacin> magacini = new List<Komercijalno.Magacin>();
			magacini = _sviMagacini;
			magacini.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Svi magacini" });

			load = true;
		}

		#region Events
		private void cmb_Grupa_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (load == false || cmb_Grupa.SelectedIndex == 0)
			{
				if (cmb_PodGrupa.Items.Count > 0)
					cmb_PodGrupa.SelectedIndex = 0;
				cmb_PodGrupa.DataSource = null;
				_args.GrupaID = null;
				_args.PodgrupaID = null;

				return;
			}

			loadPodgrupa = true;

			List<Komercijalno.PodGrupa> pgr = _svePodgrupe
				.Where(x => x.GrupaID == Convert.ToString(cmb_Grupa.SelectedValue))
				.ToList();
			pgr.Add(new Komercijalno.PodGrupa() { PodGrupaID = -1, Naziv = "<izaberi podgrupu>" });
			pgr.Sort((x, y) => x.PodGrupaID.CompareTo(y.PodGrupaID));
			cmb_PodGrupa.DataSource = pgr;
			cmb_PodGrupa.DisplayMember = "Naziv";
			cmb_PodGrupa.ValueMember = "PodGrupaID";
			_args.GrupaID = Convert.ToString(cmb_Grupa.SelectedValue);
			_args.PodgrupaID = null;

			loadPodgrupa = false;
		}

		private void cmb_Proizvodjac_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (load == false || cmb_Proizvodjac.SelectedIndex == 0)
			{
				_args.ProID = null;
				return;
			}
			_args.ProID = Convert.ToString(cmb_Proizvodjac.SelectedValue);
		}

		private void cmb_Dobavljac_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (load == false || cmb_Dobavljac.SelectedIndex == 0)
			{
				_args.DobavljacID = null;
				return;
			}
			_args.DobavljacID = Convert.ToInt32(cmb_Dobavljac.SelectedValue);
		}

		private void cmb_PodGrupa_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (load == false || loadPodgrupa == true || cmb_PodGrupa.SelectedIndex == 0)
			{
				_args.PodgrupaID = null;
				return;
			}
			_args.PodgrupaID = Convert.ToInt32(cmb_PodGrupa.SelectedValue);
		}

		private void cb_Click(object sender, EventArgs e)
		{
			CheckBox rb = sender as CheckBox;
			foreach (CheckBox r in rb.Parent.Controls.OfType<CheckBox>())
			{
				if (r.Name == rb.Name)
					continue;

				r.Checked = false;
			}

			switch (rb.Parent.Tag)
			{
				case "IzvorRobe":
					_args.IzvorRobe = rb.Checked
						? (DodatniFilterIzvorRobe)rb.Tag
						: DodatniFilterIzvorRobe.NULL;

					if ((DodatniFilterIzvorRobe)rb.Tag == DodatniFilterIzvorRobe.RobaVanPopisa)
						try
						{
							_args.IzvorRobeTag = Convert.ToInt32(tb_RobaVanDok.Text);
						}
						catch (Exception)
						{
							_args.IzvorRobeTag = 0;
						}
					else if (
						(DodatniFilterIzvorRobe)rb.Tag == DodatniFilterIzvorRobe.RobaSaPrometom
					)
						_args.IzvorRobeTag = new Tuple<DateTime, DateTime>(
							dtp_Od.Value,
							dtp_Do.Value
						);

					break;

				case "StanjeRobe":
					_args.StanjeRobe = rb.Checked
						? (DodatniFilterTrenutnoStanjeRobe)rb.Tag
						: DodatniFilterTrenutnoStanjeRobe.NULL;
					break;
				case "OptKrit":
					_args.OptimalneKriticneZalihe = rb.Checked
						? (DodatniFilterKriticneOptimalneZalihe)rb.Tag
						: DodatniFilterKriticneOptimalneZalihe.NULL;
					break;
				default:
					throw new Exception("Greska");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			cmb_Grupa.SelectedIndex = 0;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (cmb_PodGrupa.Items.Count > 0)
				cmb_PodGrupa.SelectedIndex = 0;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			cmb_Proizvodjac.SelectedIndex = 0;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			cmb_Dobavljac.SelectedIndex = 0;
		}
		#endregion

		private void btn_Primeni_Click(object sender, EventArgs e)
		{
			if (_args.IzvorRobe == DodatniFilterIzvorRobe.RobaSaPrometom)
				_args.IzvorRobeTag = new Tuple<DateTime, DateTime>(dtp_Od.Value, dtp_Do.Value);
			else if (_args.IzvorRobe == DodatniFilterIzvorRobe.RobaVanPopisa)
				_args.IzvorRobeTag = Convert.ToInt32(tb_RobaVanDok.Text);
			else
				_args.IzvorRobeTag = null;

			OnFilterChanged(_args);
		}
	}
}
