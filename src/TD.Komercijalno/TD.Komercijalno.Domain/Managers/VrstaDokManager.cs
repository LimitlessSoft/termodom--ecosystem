using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.VrstaDoks;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class VrstaDokManager : LSCoreBaseManager<VrstaDokManager, VrstaDok>, IVrstaDokManager
    {
        public VrstaDokManager(ILogger<VrstaDokManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<VrstaDokDto> GetMultiple()
        {
            var response = new LSCoreListResponse<VrstaDokDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList().ToVrstaDokDtoList();
            return response;
        }
    }
}
