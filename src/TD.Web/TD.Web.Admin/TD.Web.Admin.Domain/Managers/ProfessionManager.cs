using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProfessionManager : LSCoreBaseManager<ProfessionManager, ProfessionEntity>, IProfessionManager
    {
        public ProfessionManager(ILogger<ProfessionManager> logger, WebDbContext dbContext) 
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<ProfessionsGetMultipleDto> GetMultiple() =>
            Queryable()
                .LSCoreFilters(x => x.IsActive)
                .ToLSCoreListResponse<ProfessionsGetMultipleDto, ProfessionEntity>();

        public LSCoreResponse<long> Save(SaveProfessionRequest request) =>
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));
    }
}
