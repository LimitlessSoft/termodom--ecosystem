using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.DtoMappings.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class NacinPlacanjaManager : BaseManager<NacinPlacanjaManager, NacinPlacanja>, INacinPlacanjaManager
    {
        public NacinPlacanjaManager(ILogger<NacinPlacanjaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<NacinPlacanjaDto> GetMultiple()
        {
            return new ListResponse<NacinPlacanjaDto>(Queryable().ToList().ToNacinPlacanjaDtoList());
        }
    }
}
