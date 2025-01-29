using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

namespace TD.Office.InterneOtpremnice.Domain.Managers;

public class InterneOtpremniceManager(
    IInternaOtpremnicaRepository internaOtpremnicaRepository,
    TDKomercijalnoClient komercijalnoClient
) : IInterneOtpremniceManager
{
    public InternaOtpremnicaDto Get(LSCoreIdRequest request) =>
        internaOtpremnicaRepository
            .Get(request.Id)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request) =>
        internaOtpremnicaRepository
            .Create(request.PolazniMagacinId, request.DestinacioniMagacinId)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

    public async Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
        GetMultipleRequest request
    )
    {
        request.Validate();
        List<long> magaciniId = [];

        if (request.MagacinId.HasValue)
        {
            magaciniId.Add(request.MagacinId.Value);
        }
        else
        {
            var magaciniIzabraneVrste = await komercijalnoClient.Magacini.GetMultipleAsync(
                new MagaciniGetMultipleRequest
                {
                    Vrsta =
                    [
                        request.Vrsta switch
                        {
                            InternaOtpremnicaVrsta.VP => MagacinVrsta.Veleprodajni,
                            InternaOtpremnicaVrsta.MP => MagacinVrsta.Maloprodajni,
                            _ => throw new NotImplementedException()
                        }
                    ]
                }
            );

            magaciniId = magaciniIzabraneVrste.Select(x => x.MagacinId).ToList();
        }
        return internaOtpremnicaRepository
            .GetMultiple()
            .Where(x => magaciniId.Contains(x.PolazniMagacinId))
            .ToSortedAndPagedResponse<
                InternaOtpremnicaEntity,
                InterneOtpremniceSortColumnCodes.InterneOtpremnice,
                InternaOtpremnicaDto
            >(request, InterneOtpremniceSortColumnCodes.Rules);
    }
}
