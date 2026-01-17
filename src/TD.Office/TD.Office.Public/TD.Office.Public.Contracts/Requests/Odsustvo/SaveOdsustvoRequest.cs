namespace TD.Office.Public.Contracts.Requests.Odsustvo;

public class SaveOdsustvoRequest
{
	public long? Id { get; set; }
	public long? UserId { get; set; }
	public long TipOdsustvaId { get; set; }
	public DateTime DatumOd { get; set; }
	public DateTime DatumDo { get; set; }
	public string? Komentar { get; set; }
}
