using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutPPIDRequest : LSCoreSaveRequest
{
    public int? PPID { get; set; }
}
