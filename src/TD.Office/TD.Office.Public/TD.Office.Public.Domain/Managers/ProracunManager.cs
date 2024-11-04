using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Roba;
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

        if (request.Type == ProracunType.Maloprodajni && userEntity.StoreId == null)
            throw new LSCoreBadRequestException("Korisnik nema dodeljen MP magacin"); // TODO: Move message to validation codes & move whole validation to validator

        if (request.Type == ProracunType.Veleprodajni && userEntity.VPMagacinId == null)
            throw new LSCoreBadRequestException("Korisnik nema dodeljen VP magacin"); // TODO: Move message to validation codes & move whole validation to validator

        Insert(
            new ProracunEntity
            {
                MagacinId =
                    request.Type == ProracunType.Maloprodajni
                        ? userEntity.StoreId!.Value
                        : userEntity.VPMagacinId!.Value,
                State = ProracunState.Open,
                Type = request.Type,
                NUID = Constants.ProracunDefaultNUID
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

    public async Task AddItem(ProracuniAddItemRequest request)
    {
        var proracun = proracunRepository.Get(request.Id);

        var roba = await tdKomercijalnoApiManager.GetRobaAsync(
            new LSCoreIdRequest() { Id = request.RobaId }
        );

        var prodajnaCenaNaDan = await tdKomercijalnoApiManager.GetProdajnaCenaNaDanAsync(
            new ProceduraGetProdajnaCenaNaDanRequest()
            {
                Datum = DateTime.Now,
                MagacinId = proracun.MagacinId,
                RobaId = request.RobaId
            }
        );

        proracun.Items.Add(
            new ProracunItemEntity
            {
                RobaId = request.RobaId,
                Kolicina = request.Kolicina,
                CenaBezPdv = (decimal)prodajnaCenaNaDan,
                Pdv = (decimal)roba.Tarifa.Stopa,
                Rabat = 0
            }
        );
        Update(proracun);
    }
}
