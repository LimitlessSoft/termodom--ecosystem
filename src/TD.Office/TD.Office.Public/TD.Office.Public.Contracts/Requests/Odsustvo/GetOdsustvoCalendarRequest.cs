namespace TD.Office.Public.Contracts.Requests.Odsustvo;

public class GetOdsustvoCalendarRequest
{
	public int Month { get; set; }
	public int Year { get; set; }
	public long? UserId { get; set; }
}
