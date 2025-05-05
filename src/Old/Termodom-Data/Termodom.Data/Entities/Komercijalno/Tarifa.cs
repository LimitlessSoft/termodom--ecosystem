namespace Termodom.Data.Entities.Komercijalno
{
	public class Tarifa
	{
		public string TarifaID { get; set; }
		public string Naziv { get; set; }
		public double? Stopa { get; set; }
		public int? FKod { get; set; }
		public int Vrsta { get; set; }
	}
}
