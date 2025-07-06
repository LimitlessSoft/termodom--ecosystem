namespace TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

public class GetSpecifikacijaNovcaDto
{
	public long Id { get; set; }
	public long MagacinId { get; set; }
	public DateTime DatumUTC { get; set; }
	public SpecifikacijaNovcaDetailsDto SpecifikacijaNovca { get; set; } = new ();
	public string? Komentar { get; set; }
	public SpecifikacijaNovcaRacunarDto Racunar { get; set; } = new ();
	public SpecifikacijaNovcaPoreskaDto Poreska { get; set; } = new ();
}
