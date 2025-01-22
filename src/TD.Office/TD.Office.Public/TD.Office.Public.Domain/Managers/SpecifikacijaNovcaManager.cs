using System.ComponentModel;
using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Enums;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Domain.Managers;

public class SpecifikacijaNovcaManager(
    ILogger<SpecifikacijaNovcaManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ISpecifikacijaNovcaRepository specifikacijaNovcaRepository,
    IUserRepository userRepository,
    ITDKomercijalnoApiManager komercijalnoApiManager
)
    : LSCoreManagerBase<SpecifikacijaNovcaManager, SpecifikacijaNovcaEntity>(
        logger,
        dbContext,
        currentUser
    ),
        ISpecifikacijaNovcaManager
{
    /// <summary>
    /// Returns current specifikacija novca for current user.
    /// If it doesn't exist, it will create new one.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LSCoreBadRequestException"></exception>
    public async Task<GetSpecifikacijaNovcaDto> GetCurrentAsync()
    {
        var user = userRepository.GetCurrentUser();

        if (user.StoreId == null)
            throw new LSCoreBadRequestException(
                SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!
            );

        var entity =
            specifikacijaNovcaRepository.GetCurrentOrDefault((int)user.StoreId)
            ?? Insert(
                new SpecifikacijaNovcaEntity { MagacinId = (int)user.StoreId, Datum = DateTime.Now } // Check other big comment and think about this DateTime
            );

        var response = entity.ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();
        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);
        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetNextAsync(
        GetNextSpecifikacijaNovcaRequest request
    )
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null) // If uer doesn't have storeId, but has permission to work with all stores, this should be allowed
            throw new LSCoreBadRequestException(
                SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!
            );

        var response = specifikacijaNovcaRepository
            .GetNext(request.RelativeToId, request.FixMagacin, ListSortDirection.Ascending)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId); // What if user has permission to work with all stores and he is looking at specifikacija novca from another store?

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetPrevAsync(
        GetPrevSpecifikacijaNovcaRequest request
    )
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null) // If uer doesn't have storeId, but has permission to work with all stores, this should be allowed
            throw new LSCoreBadRequestException(
                SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!
            );

        // What if user can work only with his magacin (so no permission to work with all stores) and he sends FixMagacin = false?
        var response = specifikacijaNovcaRepository
            .GetNext(request.RelativeToId, request.FixMagacin, ListSortDirection.Descending)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId); // What if user has permission to work with all stores and he is looking at specifikacija novca from another store?

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetSingleAsync(
        GetSingleSpecifikacijaNovcaRequest request
    )
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null) // What if user has permission to work with all stores and doesn't have default Magacin? Why would we block him?
            throw new LSCoreBadRequestException(
                SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!
            );

        var response = specifikacijaNovcaRepository
            .Get(request.Id)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId); // What if specifikacija novca is from another store?

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetSpecifikacijaByDate(
        GetSpecifikacijaByDateRequest request
    )
    {
        var user = userRepository.GetCurrentUser();

        // If I am not mistaken, this will throw forbidden if user doesn't have permission to work with all stores
        // What if he tries seeing specifikacija novca from his own magacin? I think this will fail him, but it should not
        if (
            !user.Permissions.Any(x =>
                x.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSviMagacini
                && x.IsActive
            )
            || (
                request.Date.AddDays(7) > DateTime.Now // Check big comment and think about this DateTime
                && !user.Permissions.Any(x =>
                    (
                        x.Permission
                            == Common.Contracts.Enums.Permission.SpecifikacijaNovcaPrethodnih7Dana
                        || x.Permission
                            == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSviDatumi
                    ) && x.IsActive
                )
            )
        )
            throw new LSCoreForbiddenException();

        var response = specifikacijaNovcaRepository
            .GetByDate(request.Date, request.MagacinId)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)response.MagacinId);

        return response;
    }

    public void Save(SaveSpecifikacijaNovcaRequest request)
    {
        request.Validate();
        var entity = specifikacijaNovcaRepository.Get((long)request.Id!);
        var user = userRepository.GetCurrentUser();
        if (
            entity.MagacinId != user.StoreId
            && !user.Permissions.Any(permission => // Check this EP, I think it may break
                permission.IsActive
                && permission.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSave
            )
        )
            throw new LSCoreForbiddenException();
        base.Save(request);
    }

    private async Task<SpecifikacijaNovcaRacunarDto> CalculateRacunarDataAsync(int storeId)
    {
        var racuniIPovratnice = await komercijalnoApiManager.GetMultipleDokumentAsync(
            new Komercijalno.Contracts.Requests.Dokument.DokumentGetMultipleRequest()
            {
                VrDok = [15, 22],
                DatumOd = DateTime.Today, // See big comment and think about this DateTime
                DatumDo = DateTime.Today.AddDays(1), // See big comment and think about this DateTime
                MagacinId = storeId,
                Flag = 1
            }
        );

        return new SpecifikacijaNovcaRacunarDto
        {
            GotovinskiRacuni = racuniIPovratnice
                .Where(x => x is { NuId: (short)NacinUplate.Gotovina, VrDok: 15 })
                .Sum(x => x.Potrazuje),
            Kartice = racuniIPovratnice
                .Where(x => x is { NuId: (short)NacinUplate.Kartica, VrDok: 15 })
                .Sum(x => x.Potrazuje),
            VirmanskiRacuni = racuniIPovratnice
                .Where(x => x is { NuId: (short)NacinUplate.Virman, VrDok: 15 })
                .Sum(x => x.Potrazuje),
            GotovinskePovratnice = racuniIPovratnice
                .Where(x => x is { NuId: (short)NacinUplate.Gotovina, VrDok: 22 })
                .Sum(x => x.Potrazuje),
            VirmanskePovratnice = racuniIPovratnice
                .Where(x => x is { NuId: (short)NacinUplate.Virman, VrDok: 22 })
                .Sum(x => x.Potrazuje),
            OstalePovratnice = racuniIPovratnice
                .Where(x =>
                    x.NuId != (short)NacinUplate.Gotovina
                    && x.NuId != (short)NacinUplate.Virman
                    && x.VrDok == 22
                )
                .Sum(x => x.Potrazuje)
        };
    }
}
