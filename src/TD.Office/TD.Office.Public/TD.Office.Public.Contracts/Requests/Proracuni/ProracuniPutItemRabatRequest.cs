using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutItemRabatRequest : LSCoreIdRequest
{
    public long StavkaId { get; set; }
    public decimal Rabat { get; set; }
}
