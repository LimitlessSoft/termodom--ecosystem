using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Namene;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class NamenaManager : LSCoreBaseManager<NamenaManager, Namena>, INamenaManager
    {
        public NamenaManager(ILogger<NamenaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<NamenaDto> GetMultiple()
        {
            var response = new LSCoreListResponse<NamenaDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList().ToNamenaDtoList();
            return response;
        }
    }
}
