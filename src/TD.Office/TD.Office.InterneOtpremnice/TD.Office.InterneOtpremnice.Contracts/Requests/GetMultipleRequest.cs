using LSCore.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

namespace TD.Office.InterneOtpremnice.Contracts.Requests;

public class GetMultipleRequest
    : LSCoreSortableAndPageableRequest<InterneOtpremniceSortColumnCodes.InterneOtpremnice>
{
    public InternaOtpremnicaVrsta Vrsta { get; set; }
    public long? MagacinId { get; set; }
}
