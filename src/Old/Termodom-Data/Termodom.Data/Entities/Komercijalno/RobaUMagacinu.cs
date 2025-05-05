namespace Termodom.Data.Entities.Komercijalno
{
	public class RobaUMagacinu
	{
		public int MagacinID { get; set; }
		public int RobaID { get; set; }
		public double ProdajnaCena { get; set; }
		public double Stanje { get; set; }
		public double OptimalneZalihe { get; set; }
		public double KriticneZalihe { get; set; }
		public double NabavnaCena { get; set; }
	}
}
