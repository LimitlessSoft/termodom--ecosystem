namespace TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

public class SpecifikacijaNovcaRacunarDto
{
	public decimal GotovinskiRacuniValue { get; set; }
	public string GotovinskiRacuni => GotovinskiRacuniValue.ToString("#,##0.00 RSD");
	public decimal VirmanskiRacuniValue { get; set; }
	public string VirmanskiRacuni => VirmanskiRacuniValue.ToString("#,##0.00 RSD");
	public decimal KarticeValue { get; set; }
	public string Kartice => KarticeValue.ToString("#,##0.00 RSD");
	public decimal GotovinskePovratniceValue { get; set; }
	public string GotovinskePovratnice => GotovinskePovratniceValue.ToString("#,##0.00 RSD");
	public decimal VirmanskePovratniceValue { get; set; }
	public string VirmanskePovratnice => VirmanskePovratniceValue.ToString("#,##0.00 RSD");
	public decimal OstalePovratniceValue { get; set; }
	public string OstalePovratnice => OstalePovratniceValue.ToString("#,##0.00 RSD");
	public decimal RacunarTraziValue =>
		GotovinskiRacuniValue + KarticeValue + GotovinskePovratniceValue - OstalePovratniceValue;
	public string RacunarTrazi => RacunarTraziValue.ToString("#,##0.00 RSD");
	public bool ImaNefiskalizovanih { get; set; }
}
