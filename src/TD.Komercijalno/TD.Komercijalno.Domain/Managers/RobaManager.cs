using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaManager : BaseManager<RobaManager, Roba>, IRobaManager
    {
        public RobaManager(ILogger<RobaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<Roba> Create(RobaCreateRequest request)
        {
            var response = new Response<Roba>();

            if (request.IsRequestInvalid(response))
                return response;

            var roba = new Roba();
            roba.InjectFrom(request);
            Save(roba);

            response.Payload = roba;
            return response;
        }

        public ListResponse<RobaDto> GetMultiple(RobaGetMultipleRequest request)
        {
            return Queryable()
                .Include(x => x.Tarifa)
                .Where(x =>
                    (!request.Vrsta.HasValue || x.Vrsta == request.Vrsta))
                .ToList()
                .ToRobaDtoListResponse();
        }
    }
}
