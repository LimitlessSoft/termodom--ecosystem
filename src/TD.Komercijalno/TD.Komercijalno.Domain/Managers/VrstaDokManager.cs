using TD.Komercijalno.Contracts.DtoMappings.VrstaDoks;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Managers;

namespace TD.Komercijalno.Domain.Managers
{
    public class VrstaDokManager (ILogger<VrstaDokManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<VrstaDokManager, VrstaDok>(logger, dbContext), IVrstaDokManager
    {
        public List<VrstaDokDto> GetMultiple() =>
            Queryable().ToList().ToVrstaDokDtoList();
    }
}
