namespace TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

public class GetNextSpecifikacijaNovcaRequest
{
	public long RelativeToId { get; set; }
	public bool FixMagacin { get; set; }
}
