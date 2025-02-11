using LSCore.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Contracts.Requests;

public class InterneOtpremniceItemCreateRequest : LSCoreSaveRequest
{
    public long InternaOtpremnicaId { get; set; }
    public int RobaId { get; set; }
    public decimal Kolicina { get; set; }
    public long CreatedBy { get; set; }
}