using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.DtoMappings.Magacini;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class MagacinManager : BaseManager<MagacinManager, Magacin>, IMagacinManager
    {
        public MagacinManager(ILogger<MagacinManager> logger, KomercijalnoDbContext komercijalnoDbContext)
            : base(logger, komercijalnoDbContext)
        {

        }

        public ListResponse<MagacinDto> GetMultiple()
        {
            return new ListResponse<MagacinDto>(Queryable().ToList().ToMagacinDtoList());
        }
    }
}
