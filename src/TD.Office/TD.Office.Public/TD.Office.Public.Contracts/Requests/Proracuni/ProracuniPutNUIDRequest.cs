using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutNUIDRequest : LSCoreSaveRequest
{
    public int NUID { get; set; }
}
