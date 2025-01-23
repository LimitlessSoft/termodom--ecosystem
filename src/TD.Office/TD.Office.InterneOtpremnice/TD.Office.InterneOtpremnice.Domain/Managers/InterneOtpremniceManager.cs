using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

namespace TD.Office.InterneOtpremnice.Domain.Managers;

public class InterneOtpremniceManager(IInternaOtpremnicaRepository internaOtpremnicaRepository)
    : IInterneOtpremniceManager
{
    public InternaOtpremnicaDto Get(LSCoreIdRequest request) =>
        internaOtpremnicaRepository
            .Get(request.Id)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request) =>
        internaOtpremnicaRepository
            .Create(request.PolazniMagacinId, request.DestinacioniMagacinId)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public LSCoreSortedAndPagedResponse<InternaOtpremnicaDto> GetMultiple(
        GetMultipleRequest request
    ) =>
        internaOtpremnicaRepository
            .GetMultiple()
            .ToSortedAndPagedResponse<
                InternaOtpremnicaEntity,
                InterneOtpremniceSortColumnCodes.InterneOtpremnice,
                InternaOtpremnicaDto
            >(request, InterneOtpremniceSortColumnCodes.Rules);
}
