using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Managers;

public class ProracunManager(
    ILogger<ProracunManager> logger,
    IUserRepository userRepository,
    OfficeDbContext dbContext,
    IProracunRepository proracunRepository,
    ITDKomercijalnoApiManager tdKomercijalnoApiManager,
    LSCoreContextUser currentUser
)
    : LSCoreManagerBase<ProracunManager, ProracunEntity>(logger, dbContext, currentUser),
        IProracunManager
{
    public void Create(ProracuniCreateRequest request)
    {
        var userEntity = userRepository.Get(new LSCoreIdRequest() { Id = currentUser.Id!.Value });

        if (
            request.Type is ProracunType.Maloprodajni or ProracunType.NalogZaUtovar
            && userEntity.StoreId == null
        )
            throw new LSCoreBadRequestException("Korisnik nema dodeljen MP magacin"); // TODO: Move message to validation codes & move whole validation to validator

        if (request.Type == ProracunType.Veleprodajni && userEntity.VPMagacinId == null)
            throw new LSCoreBadRequestException("Korisnik nema dodeljen VP magacin"); // TODO: Move message to validation codes & move whole validation to validator

        Insert(
            new ProracunEntity
            {
                MagacinId = request.Type switch
                {
                    ProracunType.Maloprodajni => userEntity.StoreId!.Value,
                    ProracunType.Veleprodajni => userEntity.VPMagacinId!.Value,
                    ProracunType.NalogZaUtovar => userEntity.StoreId!.Value,
                    _ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
                },
                State = ProracunState.Open,
                Type = request.Type,
                NUID = request.Type switch
                {
                    ProracunType.Maloprodajni => Constants.ProracunDefaultNUID,
                    ProracunType.Veleprodajni => Constants.ProfakturaDefaultNUID,
                    ProracunType.NalogZaUtovar => Constants.NalogZaUtovarDefaultNUID,
                    _ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
                }
            }
        );
    }

    public LSCoreSortedAndPagedResponse<ProracunDto> GetMultiple(
        ProracuniGetMultipleRequest request
    )
    {
        var resp = Queryable<ProracunEntity>()
            .Include(x => x.User)
            .Include(x => x.Items)
            .Where(x =>
                x.IsActive
                && x.MagacinId == request.MagacinId
                && x.CreatedAt >= request.FromUtc
                && x.CreatedAt <= request.ToUtc
            )
            .ToSortedAndPagedResponse<
                ProracunEntity,
                ProracuniSortColumnCodes.Proracuni,
                ProracunDto
            >(request, ProracuniSortColumnCodes.ProracuniSortRules);

        var komercijalnoRoba = tdKomercijalnoApiManager
            .GetMultipleRobaAsync(new RobaGetMultipleRequest())
            .GetAwaiter()
            .GetResult();

        foreach (var item in resp.Payload!.SelectMany(proracun => proracun.Items))
        {
            var kRoba = komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId);
            item.Naziv = kRoba?.Naziv ?? Constants.ProracunRobaNotFoundText;
            item.JM = kRoba?.JM ?? Constants.ProracunRobaNotFoundText;
        }

        return resp;
    }

    public ProracunDto GetSingle(LSCoreIdRequest request)
    {
        var proracun = Queryable<ProracunEntity>()
            .Include(x => x.User)
            .Include(x => x.Items)
            .FirstOrDefault(x => x.IsActive && x.Id == request.Id);

        if (proracun == null)
            throw new LSCoreNotFoundException();

        var dto = proracun.ToDto<ProracunEntity, ProracunDto>();

        var komercijalnoRoba = tdKomercijalnoApiManager
            .GetMultipleRobaAsync(new RobaGetMultipleRequest())
            .GetAwaiter()
            .GetResult();

        foreach (var item in dto.Items)
        {
            var kRoba = komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId);
            item.Naziv = kRoba?.Naziv ?? Constants.ProracunRobaNotFoundText;
            item.JM = kRoba?.JM ?? Constants.ProracunRobaNotFoundText;
        }

        return dto;
    }

    public void PutState(ProracuniPutStateRequest request) => Save(request);

    public void PutPPID(ProracuniPutPPIDRequest request) => Save(request);

    public void PutNUID(ProracuniPutNUIDRequest request) => Save(request);

    public async Task<ProracunItemDto> AddItemAsync(ProracuniAddItemRequest request)
    {
        var proracun = proracunRepository.Get(request.Id);

        var roba = await tdKomercijalnoApiManager.GetRobaAsync(
            new LSCoreIdRequest() { Id = request.RobaId }
        );

        var prodajnaCenaNaDan = await tdKomercijalnoApiManager.GetProdajnaCenaNaDanAsync(
            new ProceduraGetProdajnaCenaNaDanRequest()
            {
                Datum = DateTime.Now,
                MagacinId = proracun.Type switch
                {
                    ProracunType.Maloprodajni => proracun.MagacinId,
                    ProracunType.NalogZaUtovar => proracun.MagacinId,
                    ProracunType.Veleprodajni => 150,
                    _ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
                },
                RobaId = request.RobaId
            }
        );

        var item = new ProracunItemEntity
        {
            RobaId = request.RobaId,
            Kolicina = request.Kolicina,
            CenaBezPdv = (decimal)prodajnaCenaNaDan * (100 / (100 + (decimal)roba.Tarifa.Stopa)),
            Pdv = (decimal)roba.Tarifa.Stopa,
            Rabat = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUser.Id!.Value
        };
        proracun.Items.Add(item);
        Update(proracun);

        var dto = item.ToDto<ProracunItemEntity, ProracunItemDto>();
        dto.Naziv = roba.Naziv;
        dto.JM = roba.JM;
        return dto;
    }

    public void DeleteItem(LSCoreIdRequest request) => HardDelete<ProracunItemEntity>(request.Id);

    public void PutItemKolicina(ProracuniPutItemKolicinaRequest request)
    {
        var item = Queryable<ProracunItemEntity>()
            .FirstOrDefault(x => x.Id == request.StavkaId && x.IsActive);
        if (item == null)
            throw new LSCoreNotFoundException();

        item.Kolicina = request.Kolicina;
        Update(item);
    }

    public async Task<ProracunDto> ForwardToKomercijalnoAsync(LSCoreIdRequest request)
    {
        var proracun = proracunRepository.Get(request.Id);

        if (proracun.State != ProracunState.Closed)
            throw new LSCoreBadRequestException("Proračun nije zaključan!");

        if (proracun.KomercijalnoVrDok != null)
            throw new LSCoreBadRequestException("Proračun je već prosleđen u komercijalno!");

        var vrDok = proracun.Type switch
        {
            ProracunType.Maloprodajni => 32,
            ProracunType.Veleprodajni => 4,
            ProracunType.NalogZaUtovar => 34,
            _ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
        };

        if (proracun is { NUID: 1, PPID: null })
            throw new LSCoreBadRequestException("Za ovaj nacin uplate obavezan je partner!");

        #region Create document in Komercijalno
        var komercijalnoDokument = await tdKomercijalnoApiManager.DokumentiPostAsync(
            new DokumentCreateRequest
            {
                VrDok = vrDok,
                MagacinId = (short)proracun.MagacinId,
                ZapId = 107,
                RefId = 107,
                // IntBroj = "Web: " + request.OneTimeHash[..8],
                Flag = 0,
                KodDok = 0,
                Linked = "0000000000",
                PPID = proracun.PPID,
                Placen = 0,
                NuId = (short)proracun.NUID,
                NrId = 1,
            }
        );
        #endregion

        proracun.KomercijalnoVrDok = komercijalnoDokument.VrDok;
        proracun.KomercijalnoBrDok = komercijalnoDokument.BrDok;

        Update(proracun);

        #region Insert items into komercijalno dokument
        foreach (var item in proracun.Items)
        {
            await tdKomercijalnoApiManager.StavkePostAsync(
                new StavkaCreateRequest
                {
                    VrDok = komercijalnoDokument.VrDok,
                    BrDok = komercijalnoDokument.BrDok,
                    RobaId = item.RobaId,
                    Kolicina = Convert.ToDouble(item.Kolicina),
                    ProdajnaCenaBezPdv = Convert.ToDouble(item.CenaBezPdv),
                    Rabat = (double)item.Rabat,
                    CeneVuciIzOvogMagacina = proracun.Type switch
                    {
                        ProracunType.Maloprodajni => null,
                        ProracunType.Veleprodajni => 150,
                        ProracunType.NalogZaUtovar => null,
                        _ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
                    }
                }
            );
        }
        #endregion

        return GetSingle(request);
    }

    public void PutItemRabat(ProracuniPutItemRabatRequest request)
    {
        var currentUserEntity = userRepository.Get(
            new LSCoreIdRequest() { Id = currentUser.Id!.Value }
        );

        var item = Queryable<ProracunItemEntity>()
            .FirstOrDefault(x => x.Id == request.StavkaId && x.IsActive);
        if (item == null)
            throw new LSCoreNotFoundException();

        var proracun = proracunRepository.Get(request.Id);

        if (
            proracun.Type == ProracunType.Maloprodajni
            || proracun.Type == ProracunType.NalogZaUtovar
                && request.Rabat > currentUserEntity.MaxRabatMPDokumenti
        )
            throw new LSCoreBadRequestException("Nemate pravo da date ovako visok rabat!");

        if (
            proracun.Type == ProracunType.Veleprodajni
            && request.Rabat > currentUserEntity.MaxRabatVPDokumenti
        )
            throw new LSCoreBadRequestException("Nemate pravo da date ovako visok rabat!");

        item.Rabat = request.Rabat;
        Update(item);
    }
}
