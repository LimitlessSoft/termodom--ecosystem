using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaUMagacinuManager (ILogger<RobaUMagacinuManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<RobaUMagacinuManager>(logger, dbContext), IRobaUMagacinuManager
    {
        public List<RobaUMagacinuGetDto> GetMultiple(RobaUMagacinuGetMultipleRequest request) =>
            Queryable<RobaUMagacinu>()
                .Where(x =>
                    (request.MagacinId == null || x.MagacinId == request.MagacinId))
                .ToDtoList<RobaUMagacinu, RobaUMagacinuGetDto>();
    }
}
