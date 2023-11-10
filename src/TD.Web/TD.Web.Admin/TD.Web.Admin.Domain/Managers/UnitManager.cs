using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class UnitManager : LSCoreBaseManager<UnitManager, UnitEntity>, IUnitManager
    {
        public UnitManager(ILogger<UnitManager> logger, WebDbContext dbContext) 
            
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<UnitsGetDto> Get(LSCoreIdRequest request) =>
            First<UnitEntity, UnitsGetDto>(x => x.Id == request.Id && x.IsActive);

        public LSCoreListResponse<UnitsGetDto> GetMultiple() =>
            new LSCoreListResponse<UnitsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<UnitsGetDto, UnitEntity>());

        public LSCoreResponse<long> Save(UnitSaveRequest request) =>
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));

        public LSCoreResponse Delete(LSCoreIdRequest request) =>
            HardDelete(request.Id);
    }
}
