using TD.Komercijalno.Contracts.DtoMappings.Magacini;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Managers;

namespace TD.Komercijalno.Domain.Managers
{
    public class MagacinManager (ILogger<MagacinManager> logger, KomercijalnoDbContext komercijalnoDbContext)
        : LSCoreManagerBase<MagacinManager>(logger, komercijalnoDbContext), IMagacinManager
    {
        public List<MagacinDto> GetMultiple() =>
            komercijalnoDbContext.Magacini.ToList().ToMagacinDtoList();
    }
}
