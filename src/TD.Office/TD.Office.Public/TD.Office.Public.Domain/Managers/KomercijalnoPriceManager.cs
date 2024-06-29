using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;
using TD.Office.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Office.Public.Domain.Managers
{
    public class KomercijalnoPriceManager (ILogger<KomercijalnoPriceManager> logger, OfficeDbContext dbContext)
        : LSCoreManagerBase<KomercijalnoPriceManager, KomercijalnoPriceEntity>(logger, dbContext),
            IKomercijalnoPriceManager
    {
        public List<KomercijalnoPriceGetDto> GetMultiple() =>
            Queryable()
                .Where(x => x.IsActive)
                .ToDtoList<KomercijalnoPriceEntity, KomercijalnoPriceGetDto>();
    }
}
