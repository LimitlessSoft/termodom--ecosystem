using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Entities;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Admin.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class UnitManager : BaseManager<UnitManager, UnitEntity>, IUnitManager
    {
        public UnitManager(ILogger<UnitManager> logger, WebDbContext dbContext) 
            
            : base(logger, dbContext)
        {
        }

        public Response<UnitsGetDto> Get(IdRequest request) =>
            First<UnitEntity, UnitsGetDto>(x => x.Id == request.Id && x.IsActive);

        public ListResponse<UnitsGetDto> GetMultiple() =>
            new ListResponse<UnitsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<UnitsGetDto, UnitEntity>());

        public Response<long> Save(UnitSaveRequest request) =>
            Save(request, (entity) => new Response<long>(entity.Id));

        public Response Delete(IdRequest request) =>
            HardDelete(request.Id);
    }
}
