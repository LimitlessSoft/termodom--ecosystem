using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
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
                Type = request.Type
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
            item.Naziv =
                komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId)?.Naziv
                ?? "Roba u komercijalnom nije pronadjena";

        return resp;
    }
}
