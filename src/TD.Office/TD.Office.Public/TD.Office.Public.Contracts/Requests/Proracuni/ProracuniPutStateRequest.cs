using LSCore.Contracts.Requests;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Proracuni;

public class ProracuniPutStateRequest : LSCoreSaveRequest
{
    public ProracunState State { get; set; }
}
