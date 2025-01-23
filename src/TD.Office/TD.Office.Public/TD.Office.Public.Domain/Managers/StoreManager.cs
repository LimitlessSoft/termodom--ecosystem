using TD.Office.Public.Contracts.Dtos.Stores;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Domain.Managers;

public class StoreManager(ITDKomercijalnoApiManager tdKomercijalnoApiManager) : IStoreManager
{
    public async Task<List<GetStoreDto>> GetMultiple() =>
        (await tdKomercijalnoApiManager.GetMagaciniAsync())
            .Select(x => new GetStoreDto() { Id = x.MagacinId, Name = x.Naziv })
            .ToList();
}
