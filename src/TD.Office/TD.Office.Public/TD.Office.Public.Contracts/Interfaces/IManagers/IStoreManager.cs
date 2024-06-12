using TD.Office.Public.Contracts.Dtos.Stores;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IStoreManager
    {
        Task<List<GetStoreDto>> GetMultiple();
    }
}