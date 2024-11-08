using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutItemKolicinaRequest : LSCoreIdRequest
{
    public long StavkaId { get; set; }
    public decimal Kolicina { get; set; }
}
