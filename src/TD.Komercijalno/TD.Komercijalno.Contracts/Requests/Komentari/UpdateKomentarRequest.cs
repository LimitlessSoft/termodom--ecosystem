namespace TD.Komercijalno.Contracts.Requests.Komentari;

public class UpdateKomentarRequest
{
	public int VrDok { get; set; }
	public int BrDok { get; set; }
	public string? Komentar { get; set; }
	public string? InterniKomentar { get; set; }
	public string? PrivatniKomentar { get; set; }
}
