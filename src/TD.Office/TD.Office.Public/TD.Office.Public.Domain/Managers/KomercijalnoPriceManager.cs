using LSCore.Domain.Managers;
using TD.Office.Common.Repository;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using LSCore.Contracts.Http;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Extensions;

namespace TD.Office.Public.Domain.Managers
{
    public class KomercijalnoPriceManager : LSCoreBaseManager<KomercijalnoPriceManager, KomercijalnoPriceEntity>, IKomercijalnoPriceManager
    {
        public KomercijalnoPriceManager(ILogger<KomercijalnoPriceManager> logger, OfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<KomercijalnoPriceGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<KomercijalnoPriceGetDto>();

            var rQuery = Queryable();
            response.Merge(rQuery);
            if(response.NotOk)
                return response;

            response.Payload = rQuery.Payload!.ToDtoList<KomercijalnoPriceGetDto, KomercijalnoPriceEntity>();
            return response;
        }
    }
}
