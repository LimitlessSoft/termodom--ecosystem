using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaManager : LSCoreBaseManager<RobaManager, Roba>, IRobaManager
    {
        public RobaManager(ILogger<RobaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<Roba> Create(RobaCreateRequest request)
        {
            var response = new LSCoreResponse<Roba>();

            if (request.IsRequestInvalid(response))
                return response;

            var roba = new Roba();
            roba.InjectFrom(request);
            InsertNonLSCoreEntity<Roba>(roba);

            response.Payload = roba;
            return response;
        }

        public LSCoreListResponse<RobaDto> GetMultiple(RobaGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<RobaDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            return qResponse.Payload!
                .Include(x => x.Tarifa)
                .Where(x =>
                    (!request.Vrsta.HasValue || x.Vrsta == request.Vrsta))
                .ToList().ToRobaDtoLSCoreListResponse();
        }
    }
}
