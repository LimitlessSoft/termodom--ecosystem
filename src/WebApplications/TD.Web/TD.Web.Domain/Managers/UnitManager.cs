using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.DtoMappings.Units;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Contracts.Requests.Units;

namespace TD.Web.Domain.Managers
{
    public class UnitManager : BaseManager<UnitManager, UnitEntity>, IUnitManager
    {
        public UnitManager(ILogger<UnitManager> logger, DbContext dbContext) 
            
            : base(logger, dbContext)
        {
        }

        public Response<UnitsGetDto> Get(IdRequest request)
        {
            var unit = Queryable()
                .Where(x => x.Id == request.Id)
                .FirstOrDefault();

            if (unit == null)
                return Response<UnitsGetDto>.NotFound();

            return new Response<UnitsGetDto>(unit.ToDto());
        }

        public Response<long> Save(UnitSaveRequest request)
        {
            var response = new Response<long>();

            if (request.IsRequestInvalid(response))
                return response;

            var unitEntity = base.Save(request);
            response.Payload = unitEntity.Id;

            return response;
        }
    }
}
