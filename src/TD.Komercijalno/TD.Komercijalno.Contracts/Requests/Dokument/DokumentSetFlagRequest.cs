namespace TD.Komercijalno.Contracts.Requests.Dokument;

public class DokumentSetFlagRequest
{
	public int VrDok { get; set; }
	public int BrDok { get; set; }
	public short Flag { get; set; }
}
