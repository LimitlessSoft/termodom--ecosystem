using JasperFx.CodeGeneration.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
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
