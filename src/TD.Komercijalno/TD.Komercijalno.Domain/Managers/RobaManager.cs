using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
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
    public class RobaManager(ILogger<RobaManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<RobaManager>(logger, dbContext),
            IRobaManager
    {
        public Roba Create(RobaCreateRequest request)
        {
            request.Validate();

            var roba = new Roba();
            roba.InjectFrom(request);
            InsertNonLSCoreEntity(roba);

            return roba;
        }

        public RobaDto Get(LSCoreIdRequest request)
        {
            var roba = dbContext
                .Roba.Include(x => x.Tarifa)
                .FirstOrDefault(x => x.Id == request.Id);

            if (roba == null)
                throw new LSCoreNotFoundException();

            return roba.ToRobaDto();
        }

        public List<RobaDto> GetMultiple(RobaGetMultipleRequest request) =>
            dbContext
                .Roba.Include(x => x.Tarifa)
                .Where(x => (!request.Vrsta.HasValue || x.Vrsta == request.Vrsta))
                .ToList()
                .ToRobaDtoList();
    }
}
