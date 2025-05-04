namespace TD.Komercijalno.Contracts.Dtos.Mesto
{
	public class MestoDto
	{
		public string MestoId { get; set; }
		public string? Naziv { get; set; }
		public short? OkrugId { get; set; }
		public string? UKorist { get; set; }
		public string? NaTeretZR { get; set; }
		public short? Hitno { get; set; }
		public short? SifraPlac { get; set; }
		public string? UplRac { get; set; }
		public string? UplModul { get; set; }
		public string? UplPozNaBroj { get; set; }
		public short? EkspId { get; set; }
	}
}
