using LSCore.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

namespace TD.Office.InterneOtpremnice.Contracts.Requests;

public class GetMultipleRequest
    : LSCoreSortableAndPageableRequest<InterneOtpremniceSortColumnCodes.InterneOtpremnice> { }
