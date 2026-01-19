namespace TD.Office.Public.Contracts.Requests.TipKorisnika;

public class SaveTipKorisnikaRequest
{
	public long? Id { get; set; }
	public string Naziv { get; set; } = string.Empty;
	public string Boja { get; set; } = string.Empty;
}
