namespace TD.Komercijalno.Contracts.Dtos.Komentari
{
	public class KomentarDto
	{
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public string? JavniKomentar { get; set; }
		public string? InterniKomentar { get; set; }
		public string? PrivatniKomentar { get; set; }
	}
}
