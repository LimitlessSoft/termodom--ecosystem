using LSCore.Common.Contracts;

namespace TD.Office.Public.Contracts.Requests.Web;

public class CreateOrUpdateKomercijalnoPriceKoeficijentRequest
{
	public long? Id { get; set; }
	public string Naziv { get; set; } = null!;
	public decimal Vrednost { get; set; }
}
