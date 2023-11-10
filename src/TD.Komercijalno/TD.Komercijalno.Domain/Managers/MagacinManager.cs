using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Magacini;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class MagacinManager : LSCoreBaseManager<MagacinManager, Magacin>, IMagacinManager
    {
        public MagacinManager(ILogger<MagacinManager> logger, KomercijalnoDbContext komercijalnoDbContext)
            : base(logger, komercijalnoDbContext)
        {

        }

        public LSCoreListResponse<MagacinDto> GetMultiple()
        {
            return new LSCoreListResponse<MagacinDto>(Queryable().ToList().ToMagacinDtoList());
        }
    }
}
