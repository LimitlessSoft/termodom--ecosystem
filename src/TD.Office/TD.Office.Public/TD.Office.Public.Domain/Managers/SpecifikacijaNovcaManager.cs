using LSCore.Contracts;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts.Exceptions;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using TD.Komercijalno.Contracts.Enums;

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
    public async Task<GetSpecifikacijaNovcaDto> GetCurrentAsync()
    {
        var user = userRepository.GetCurrentUser();

        if(user.StoreId == null)
            throw new LSCoreBadRequestException(SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!);
        
        var entity = specifikacijaNovcaRepository.GetCurrent((int)user.StoreId);
        if(entity == null)
            entity = Insert(
                new SpecifikacijaNovcaEntity()
                {
                    MagacinId = (int)user.StoreId,
                    Datum = DateTime.Now
                });

        var response = entity.ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();
        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetNextAsync(GetNextSpecifikacijaNovcaRequest request)
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null)
            throw new LSCoreBadRequestException(SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!);

        var response = specifikacijaNovcaRepository
            .GetNextOrPrevious((int)request.RelativeToId, request.FixMagacin, true)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetPrevAsync(GetPrevSpecifikacijaNovcaRequest request)
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null)
            throw new LSCoreBadRequestException(SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!);

        var response = specifikacijaNovcaRepository
            .GetNextOrPrevious((int)request.RelativeToId, request.FixMagacin, false)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetSingleAsync(GetSingleSpecifikacijaNovcaRequest request)
    {
        var user = userRepository.GetCurrentUser();
        if (user.StoreId == null)
            throw new LSCoreBadRequestException(SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!);

        var response = specifikacijaNovcaRepository
            .GetById(request.Id)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);

        return response;
    }

    public async Task<GetSpecifikacijaNovcaDto> GetSpecifikacijaByDate(GetSpecifikacijaByDateRequest request)
    {
        var user = userRepository.GetCurrentUser();

        if (!user.Permissions
                .Any(x => x.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSviMagacini && x.IsActive) ||
                (request.Date.AddDays(7) > DateTime.Now && user.Permissions
                    .Any(x => (x.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaPrethodnih7Dana ||
                                x.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSviDatumi
                              ) &&
                            x.IsActive
                        )
                )
            )
            throw new LSCoreForbiddenException();

        var response = specifikacijaNovcaRepository
            .GetByDate(request.Date, request.MagacinId)
            .ToDto<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

        response.Racunar = await CalculateRacunarDataAsync((int) response.MagacinId);

        return response;
    }

    public void Save(SaveSpecifikacijaNovcaRequest request)
    {
        request.Validate();
        base.Save(request);
    }

    private async Task<SpecifikacijaNovcaRacunarDto> CalculateRacunarDataAsync(int storeId)
    {
        var racuniIPovratnice = await komercijalnoApiManager
            .GetMultipleDokumentAsync(
                new Komercijalno.Contracts.Requests.Dokument.DokumentGetMultipleRequest()
                {
                    VrDok = [15, 22],
                    DatumOd = DateTime.Today,
                    DatumDo = DateTime.Today.AddDays(1),
                    MagacinId = storeId,
                    Flag = 1
                }
            );

        return new SpecifikacijaNovcaRacunarDto()
        {
            GotovinskiRacuni = racuniIPovratnice
                .Where(x => x.NuId == (short)NacinUplate.Gotovina
                && x.VrDok == 15)
                .Sum(x => x.Potrazuje),
            Kartice = racuniIPovratnice
                .Where(x => x.NuId == (short)NacinUplate.Kartica
                && x.VrDok == 15)
                .Sum(x => x.Potrazuje),
            VirmanskiRacuni = racuniIPovratnice
                .Where(x => x.NuId == (short)NacinUplate.Virman
                && x.VrDok == 15)
                .Sum(x => x.Potrazuje),
            GotovinskePovratnice = racuniIPovratnice
                .Where(x => x.NuId == (short)NacinUplate.Gotovina
                && x.VrDok == 22)
                .Sum(x => x.Potrazuje),
            VirmanskePovratnice = racuniIPovratnice
                .Where(x => x.NuId == (short)NacinUplate.Virman
                && x.VrDok == 22)
                .Sum(x => x.Potrazuje),
            OstalePovratnice = racuniIPovratnice
                .Where(x => x.NuId != (short)NacinUplate.Gotovina &&
                x.NuId != (short)NacinUplate.Virman
                && x.VrDok == 22)
                .Sum(x => x.Potrazuje)
        };
    }
}
