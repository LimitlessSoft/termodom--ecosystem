namespace TD.Komercijalno.Contracts.Requests.Stavke;

public class StavkeGetMultipleByRobaId
{
	public int RobaId { get; set; }
	public DateTime? FromUtc { get; set; }
	public DateTime? ToUtc { get; set; }
}
