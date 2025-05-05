namespace TDBrain_v3.RequestBodies.Komercijalno
{
	public class NapraviUsluguRequestBody
	{
		public int BazaId { get; set; }
		public int? GodinaBaze { get; set; }
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public int RobaId { get; set; }
		public double CenaBezPdv { get; set; }
		public double Kolicina { get; set; }
		public double Rabat { get; set; } = 0;
	}
}
