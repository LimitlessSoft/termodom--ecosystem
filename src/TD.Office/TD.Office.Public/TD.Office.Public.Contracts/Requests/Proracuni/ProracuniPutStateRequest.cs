using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutStateRequest
{
	public long? Id { get; set; }
	public ProracunState State { get; set; }
}
