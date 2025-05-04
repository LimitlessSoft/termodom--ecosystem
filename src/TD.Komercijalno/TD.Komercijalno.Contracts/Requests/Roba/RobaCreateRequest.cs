namespace TD.Komercijalno.Contracts.Requests.Roba
{
	public class RobaCreateRequest
	{
		public string KatBr { get; set; }
		public string KatBrPro { get; set; }
		public string Naziv { get; set; }
		public short? Vrsta { get; set; } = 1;
		public short? Aktivna { get; set; } = 1;
		public string GrupaId { get; set; }
		public short? Podgrupa { get; set; }
		public string? ProId { get; set; }
		public string JM { get; set; }
		public string TarifaId { get; set; }
	}
}
