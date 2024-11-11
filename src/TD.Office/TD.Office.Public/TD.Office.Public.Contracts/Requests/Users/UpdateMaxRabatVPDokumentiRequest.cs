using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users;
public class UpdateMaxRabatVPDokumentiRequest : LSCoreSaveRequest
{
    public decimal MaxRabatVPDokumenti { get; set; }
}
