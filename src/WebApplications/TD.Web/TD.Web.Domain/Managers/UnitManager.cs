using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.DtoMappings.Units;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Interfaces.Managers;

namespace TD.Web.Domain.Managers
{
    public class UnitManager : BaseManager<UnitManager, UnitsEntity>, IUnitManager
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
    }
}
