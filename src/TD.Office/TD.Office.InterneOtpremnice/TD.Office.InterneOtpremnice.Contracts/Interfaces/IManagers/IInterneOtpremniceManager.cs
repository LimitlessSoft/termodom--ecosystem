using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;

public interface IInterneOtpremniceManager
{
    Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request);
    InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request);
    Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
        GetMultipleRequest request
    );

    InternaOtpremnicaItemDto SaveItem(InterneOtpremniceItemCreateRequest request);
    void DeleteItem(InterneOtpremniceItemDeleteRequest request);
    void ChangeState(LSCoreIdRequest request, InternaOtpremnicaStatus state);
    Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalnoAsync(LSCoreIdRequest request);
}
