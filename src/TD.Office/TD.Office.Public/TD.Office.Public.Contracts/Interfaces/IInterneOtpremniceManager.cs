using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.Public.Contracts.Interfaces;

public interface IInterneOtpremniceManager
{
    Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(GetMultipleRequest request);
    Task<InternaOtpremnicaDto> CreateAsync(InterneOtpremniceCreateRequest request);
    Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request);
    Task<InternaOtpremnicaItemDto> SaveItemAsync(InterneOtpremniceItemCreateRequest request);
    Task DeleteItemAsync(InterneOtpremniceItemDeleteRequest request);
    Task ChangeStateAsync(LSCoreIdRequest request, InternaOtpremnicaStatus state);
    Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalno(LSCoreIdRequest request);
}