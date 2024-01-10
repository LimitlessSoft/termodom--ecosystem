using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class KomercijalnoPriceManager : LSCoreBaseManager<KomercijalnoPriceManager, KomercijalnoPriceEntity>, IKomercijalnoPriceManager
    {
        public KomercijalnoPriceManager(ILogger<KomercijalnoPriceManager> logger, WebDbContext dbContext)
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
