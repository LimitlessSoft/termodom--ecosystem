using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Extensions;
using TD.Komercijalno.Repository;
using LSCore.Contracts.Extensions;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaUMagacinuManager : LSCoreBaseManager<RobaUMagacinuManager>, IRobaUMagacinuManager
    {
        public RobaUMagacinuManager(ILogger<RobaUMagacinuManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public LSCoreListResponse<RobaUMagacinuGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<RobaUMagacinuGetDto>();

            var qResponse = Queryable<RobaUMagacinu>();
            response.Merge(qResponse);
            if(response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .ToDtoList<RobaUMagacinuGetDto, RobaUMagacinu>();

            return response;
        }
    }
}
