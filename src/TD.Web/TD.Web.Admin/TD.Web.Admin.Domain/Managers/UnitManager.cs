using LSCore.Contracts;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Admin.Domain.Managers;

public class UnitManager (ILogger<UnitManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<UnitManager, UnitEntity>(logger, dbContext, contextUser), IUnitManager
{
    public UnitsGetDto Get(LSCoreIdRequest request) =>
        Queryable()
            .FirstOrDefault(x => x.Id == request.Id && x.IsActive)?
            .ToDto<UnitEntity, UnitsGetDto>()
        ?? throw new LSCoreNotFoundException();

    public List<UnitsGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<UnitEntity, UnitsGetDto>();

    public long Save(UnitSaveRequest request) =>
        Save(request, (entity) => entity.Id);

    public void Delete(LSCoreIdRequest request) =>
        HardDelete(request.Id);
}