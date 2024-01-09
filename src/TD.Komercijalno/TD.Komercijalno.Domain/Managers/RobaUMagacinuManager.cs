using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Extensions;
using TD.Komercijalno.Repository;
using LSCore.Contracts.Extensions;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaUMagacinuManager : LSCoreBaseManager<RobaUMagacinuManager>, IRobaUMagacinuManager
    {
        public RobaUMagacinuManager(ILogger<RobaUMagacinuManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public LSCoreListResponse<RobaUMagacinuGetDto> GetMultiple(RobaUMagacinuGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<RobaUMagacinuGetDto>();

            var qResponse = Queryable<RobaUMagacinu>();
            response.Merge(qResponse);
            if(response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .Where(x =>
                    (request.MagacinId == null || x.MagacinId == request.MagacinId))
                .ToDtoList<RobaUMagacinuGetDto, RobaUMagacinu>();

            return response;
        }
    }
}
