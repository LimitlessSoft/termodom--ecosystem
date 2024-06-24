using LSCore.Contracts;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Admin.Domain.Managers;

public class ProfessionManager (ILogger<ProfessionManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<ProfessionManager, ProfessionEntity>(logger, dbContext, contextUser), IProfessionManager
{
    public List<ProfessionsGetMultipleDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<ProfessionEntity, ProfessionsGetMultipleDto>();

    public long Save(SaveProfessionRequest request) =>
        Save(request, (entity) => entity.Id);
}