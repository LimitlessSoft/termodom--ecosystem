using TD.Komercijalno.Contracts.DtoMappings.Namene;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Managers;

namespace TD.Komercijalno.Domain.Managers
{
    public class NamenaManager (ILogger<NamenaManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<NamenaManager, Namena>(logger, dbContext), INamenaManager
    {
        public List<NamenaDto> GetMultiple() =>
            Queryable().ToList().ToNamenaDtoList();
    }
}
