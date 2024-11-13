using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users;
public class UpdateMaxRabatMPDokumentiRequest : LSCoreSaveRequest
{
    public decimal MaxRabatMPDokumenti { get; set; }
}
