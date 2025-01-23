using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Domain.Managers;

public class InterneOtpremniceManager(IInternaOtpremnicaRepository internaOtpremnicaRepository) 
    : IInterneOtpremniceManager
{
    public InternaOtpremnicaDto Get(LSCoreIdRequest request) =>
        internaOtpremnicaRepository.Get(request.Id).ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request) =>
        internaOtpremnicaRepository.Create(request.PolazniMagacinId, request.DestinacioniMagacinId)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public List<InternaOtpremnicaDto> GetMultiple() =>
        internaOtpremnicaRepository
            .GetMultiple()
            .ToDtoList<InternaOtpremnicaEntity, InternaOtpremnicaDto>();
}
