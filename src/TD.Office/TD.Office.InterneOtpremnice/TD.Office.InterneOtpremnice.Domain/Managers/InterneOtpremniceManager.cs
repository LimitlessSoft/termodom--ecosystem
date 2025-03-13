using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
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
    public async Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request)
    {
        var robaTask = komercijalnoClient.Roba.GetMultipleAsync(new RobaGetMultipleRequest { Vrsta = 1 });
        
        var dto = internaOtpremnicaRepository
            .GetDetailed(request.Id)
            .ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDetailsDto>();
        
        var robaDict = (await robaTask).ToDictionary(x => x.RobaId, x => x);
        foreach(var item in dto.Items)
        {
            item.Proizvod = robaDict[item.RobaId].Naziv;
            item.JM = robaDict[item.RobaId].JM;
        }

        return dto;
    }

    public InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request) =>
        internaOtpremnicaRepository
            .Create(request.PolazniMagacinId, request.DestinacioniMagacinId, request.CreatedBy)
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

    public InternaOtpremnicaItemDto SaveItem(InterneOtpremniceItemCreateRequest request) =>
        internaOtpremnicaRepository
            .SaveItem(request.Id, request.InternaOtpremnicaId, request.RobaId, request.Kolicina)
            .ToDto<InternaOtpremnicaItemEntity, InternaOtpremnicaItemDto>();

    public void DeleteItem(InterneOtpremniceItemDeleteRequest request) =>
        internaOtpremnicaRepository.HardDeleteItem(request.Id);

    public void ChangeState(LSCoreIdRequest request, InternaOtpremnicaStatus state) =>
        internaOtpremnicaRepository.SetStatus(request.Id, state);

    public async Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalnoAsync(LSCoreIdRequest request)
    {
        var magaciniTask = komercijalnoClient.Magacini.GetMultipleAsync(new MagaciniGetMultipleRequest());
        var internaOtpremnica = internaOtpremnicaRepository.GetDetailed(request.Id);

        var magacini = await magaciniTask;
        var magacin = magacini.First(x => x.MagacinId == internaOtpremnica.PolazniMagacinId);
        
        #region Otpremnica
        var dokumentOtpremniceKomercijalno = await komercijalnoClient.Dokumenti.CreateAsync(new DokumentCreateRequest
        {
            VrDok = magacin.Vrsta switch
            {
                MagacinVrsta.Maloprodajni => 19,
                MagacinVrsta.Veleprodajni => 25,
                _ => throw new LSCoreBadRequestException("Nepoznata vrsta magacina")
            },
            MagacinId = (short)internaOtpremnica.PolazniMagacinId,
            ZapId = 107,
            RefId = 107,
            // IntBroj = "Web: " + request.OneTimeHash[..8],
            Flag = 0,
            KodDok = 0,
            Linked = "0000000000",
            Placen = 0,
            NrId = 1,
        });
        
        foreach(var item in internaOtpremnica.Items)
        {
            await komercijalnoClient.Stavke.CreateAsync(new StavkaCreateRequest
            {
                VrDok = dokumentOtpremniceKomercijalno.VrDok,
                BrDok = dokumentOtpremniceKomercijalno.BrDok,
                RobaId = item.RobaId,
                Kolicina = (double)item.Kolicina
            });
        }
        #endregion
        
        #region Kalkulacija
        var dokumentKalkulacijeKomercijalno = await komercijalnoClient.Dokumenti.CreateAsync(new DokumentCreateRequest
        {
            VrDok = magacin.Vrsta switch
            {
                MagacinVrsta.Maloprodajni => 18,
                MagacinVrsta.Veleprodajni => 26,
                _ => throw new LSCoreBadRequestException("Nepoznata vrsta magacina")
            },
            MagacinId = (short)internaOtpremnica.DestinacioniMagacinId,
            ZapId = 107,
            RefId = 107,
            // IntBroj = "Web: " + request.OneTimeHash[..8],
            Flag = 0,
            KodDok = 0,
            Linked = "0000000000",
            Placen = 0,
            NrId = 1,
            VrdokIn = (short)dokumentOtpremniceKomercijalno.VrDok,
            BrDokIn = dokumentOtpremniceKomercijalno.BrDok
        });
        
        foreach(var item in internaOtpremnica.Items)
        {
            await komercijalnoClient.Stavke.CreateAsync(new StavkaCreateRequest
            {
                VrDok = dokumentKalkulacijeKomercijalno.VrDok,
                BrDok = dokumentKalkulacijeKomercijalno.BrDok,
                RobaId = item.RobaId,
                Kolicina = (double)item.Kolicina
            });
        }
        #endregion
        
        // Update otpremnica out with kalkulacija values
        await komercijalnoClient.Dokumenti.UpdateDokOut(new DokumentSetDokOutRequest()
        {
            VrDok = dokumentOtpremniceKomercijalno.VrDok,
            BrDok = dokumentOtpremniceKomercijalno.BrDok,
            VrDokOut = dokumentKalkulacijeKomercijalno.VrdokOut,
            BrDokOut = dokumentKalkulacijeKomercijalno.BrdokOut
        });
        
        internaOtpremnica.KomercijalnoVrDok = dokumentOtpremniceKomercijalno.VrDok;
        internaOtpremnica.KomercijalnoBrDok = dokumentOtpremniceKomercijalno.BrDok;
        internaOtpremnicaRepository.Update(internaOtpremnica);
        
        return internaOtpremnica.ToDto<InternaOtpremnicaEntity, InternaOtpremnicaDetailsDto>();
    }
}
