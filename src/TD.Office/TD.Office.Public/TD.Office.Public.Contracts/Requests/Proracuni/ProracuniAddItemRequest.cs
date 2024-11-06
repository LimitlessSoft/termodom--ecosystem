using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniAddItemRequest : LSCoreIdRequest
{
    public int RobaId { get; set; }
    public decimal Kolicina { get; set; }
}
