namespace TD.Komercijalno.Contracts.Requests.Stavke;

public class StavkeGetMultipleByRobaId
{
	public int RobaId { get; set; }
	public DateTime? From { get; set; }
	public DateTime? To { get; set; }
	public int? MagacinId { get; set; }
}
