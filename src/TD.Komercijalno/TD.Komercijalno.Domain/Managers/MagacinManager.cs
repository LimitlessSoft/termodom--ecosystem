using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Magacini;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class MagacinManager(
        ILogger<MagacinManager> logger,
        KomercijalnoDbContext komercijalnoDbContext
    ) : LSCoreManagerBase<MagacinManager>(logger, komercijalnoDbContext), IMagacinManager
    {
        public List<MagacinDto> GetMultiple(MagaciniGetMultipleRequest request) =>
            komercijalnoDbContext
                .Magacini.Where(x =>
                    request.Vrsta == null
                    || request.Vrsta.Length == 0
                    || request.Vrsta.Contains(x.Vrsta)
                )
                .ToList()
                .ToMagacinDtoList();
    }
}
