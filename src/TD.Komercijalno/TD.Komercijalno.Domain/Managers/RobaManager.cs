using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaManager (ILogger<RobaManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<RobaManager, Roba>(logger, dbContext), IRobaManager
    {
        public Roba Create(RobaCreateRequest request)
        {
            request.Validate();

            var roba = new Roba();
            roba.InjectFrom(request);
            InsertNonLSCoreEntity(roba);

            return roba;
        }

        public List<RobaDto> GetMultiple(RobaGetMultipleRequest request) =>
            Queryable()
                .Include(x => x.Tarifa)
                .Where(x =>
                    (!request.Vrsta.HasValue || x.Vrsta == request.Vrsta))
                .ToList().ToRobaDtoList();
    }
}
