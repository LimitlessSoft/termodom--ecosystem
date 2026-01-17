namespace TD.Office.Public.Contracts.Requests.TipOdsustva;

public class SaveTipOdsustvaRequest
{
	public long? Id { get; set; }
	public string Naziv { get; set; } = string.Empty;
	public int Redosled { get; set; }
}
