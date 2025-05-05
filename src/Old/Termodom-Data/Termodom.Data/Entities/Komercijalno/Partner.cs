using System;

namespace Termodom.Data.Entities.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public class Partner
	{
		public int PPID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string Posta { get; set; }
		public string Mesto { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string Kontakt { get; set; }
		public string MBroj { get; set; }
		public string MestoID { get; set; }
		public int OpstinaID { get; set; }
		public int DrzavaID { get; set; }
		public int ZapID { get; set; }
		public string Valuta { get; set; }
		public string Mobilni { get; set; }
		public Int64? Kategorija { get; set; }
		public float DozvoljeniMinus { get; set; }
		public int? NPPID { get; set; }
		public string PIB { get; set; }
		public int? ImaUgovor { get; set; }
		public int VrstaCenovnika { get; set; }
		public int RefID { get; set; }
		public int DrzavljanstvoID { get; set; }
		public int ZanimanjeID { get; set; }
		public int WEB_Status { get; set; }
		public int GPPID { get; set; }
		public int Cene_od_grupe { get; set; }
		public int? VPCID { get; set; }
		public double? PROCPC { get; set; }
		public int PDVO { get; set; }
		public string NazivZaStampu { get; set; }
		public string KatNaziv { get; set; }
		public int KatID { get; set; }
		public int Aktivan { get; set; }
		public double Duguje { get; set; }
		public double Potrazuje { get; set; }
	}
}
