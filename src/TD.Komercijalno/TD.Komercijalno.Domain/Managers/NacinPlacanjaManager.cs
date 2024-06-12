using TD.Komercijalno.Contracts.DtoMappings.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Managers;

namespace TD.Komercijalno.Domain.Managers
{
    public class NacinPlacanjaManager (ILogger<NacinPlacanjaManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<NacinPlacanjaManager, NacinPlacanja>(logger, dbContext), INacinPlacanjaManager
    {
        public List<NacinPlacanjaDto> GetMultiple() =>
            Queryable().ToList().ToNacinPlacanjaDtoList();
    }
}
