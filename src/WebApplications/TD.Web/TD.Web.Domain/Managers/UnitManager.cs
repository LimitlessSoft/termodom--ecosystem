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
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class UnitManager : BaseManager<UnitManager, UnitEntity>, IUnitManager
    {
        public UnitManager(ILogger<UnitManager> logger, WebDbContext dbContext) 
            
            : base(logger, dbContext)
        {
        }

        public Response<UnitsGetDto> Get(IdRequest request)
        {
            var response = new Response<UnitsGetDto>();
            var unitResponse = First(x => x.Id == request.Id && x.IsActive);

            response.Merge(unitResponse);
            if (response.NotOk)
                return response;

            response.Payload = unitResponse.Payload.ToDto();
            return response;
        }

        public ListResponse<UnitsGetDto> GetMultiple() => new ListResponse<UnitsGetDto>(
            Queryable()
            .Where(x => x.IsActive)
            .ToList()
            .ToListDto());

        public Response<long> Save(UnitSaveRequest request)
        {
            var response = new Response<long>();

            if (request.IsRequestInvalid(response))
                return response;

            var unitEntityResponse = base.Save(request);
            response.Merge(unitEntityResponse);
            if(response.NotOk || unitEntityResponse.Payload == null)
                return response;

            response.Payload = unitEntityResponse.Payload.Id;

            return response;
        }

        public Response<bool> Delete(IdRequest request)
        {
            var response = new Response<bool>();
            var entityResponse = First(x => x.Id == request.Id);

            response.Merge(entityResponse);
            if(response.NotOk)
                return response;

            HardDelete(entityResponse.Payload);
            return response;
        }
    }
}
