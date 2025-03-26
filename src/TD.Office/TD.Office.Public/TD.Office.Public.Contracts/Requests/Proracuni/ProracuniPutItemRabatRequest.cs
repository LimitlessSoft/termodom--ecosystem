using LSCore.Common.Contracts;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutItemRabatRequest : LSCoreIdRequest
{
	public long StavkaId { get; set; }
	public decimal Rabat { get; set; }
}
