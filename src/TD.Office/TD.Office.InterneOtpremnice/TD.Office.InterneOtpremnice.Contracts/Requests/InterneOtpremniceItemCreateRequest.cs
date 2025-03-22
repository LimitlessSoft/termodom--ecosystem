namespace TD.Office.InterneOtpremnice.Contracts.Requests;

public class InterneOtpremniceItemCreateRequest
{
	public long? Id { get; set; }
	public long InternaOtpremnicaId { get; set; }
	public int RobaId { get; set; }
	public decimal Kolicina { get; set; }
	public long CreatedBy { get; set; }
}
