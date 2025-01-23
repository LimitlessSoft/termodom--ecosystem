using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;

public interface IInterneOtpremniceManager
{
    InternaOtpremnicaDto Get(LSCoreIdRequest request);
    InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request);
    Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
        GetMultipleRequest request
    );
}
