using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.Stores;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Office.Public.Domain.Managers
{
    public class StoreManager (
        ILogger<StoreManager> logger,
        OfficeDbContext dbContext,
        ITDKomercijalnoApiManager tdKomercijalnoApiManager)
        : LSCoreManagerBase<StoreManager>(logger, dbContext), IStoreManager
    {
        public async Task<List<GetStoreDto>> GetMultiple() =>
            (await tdKomercijalnoApiManager.GetMagaciniAsync()).Select(x => new GetStoreDto()
                {
                    Id = x.MagacinId,
                    Name = x.Naziv
                }).ToList();
    }
}