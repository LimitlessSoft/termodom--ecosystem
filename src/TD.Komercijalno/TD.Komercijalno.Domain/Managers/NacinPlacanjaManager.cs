using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class NacinPlacanjaManager : LSCoreBaseManager<NacinPlacanjaManager, NacinPlacanja>, INacinPlacanjaManager
    {
        public NacinPlacanjaManager(ILogger<NacinPlacanjaManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<NacinPlacanjaDto> GetMultiple()
        {
            var response = new LSCoreListResponse<NacinPlacanjaDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList().ToNacinPlacanjaDtoList();
            return response;
        }
    }
}
