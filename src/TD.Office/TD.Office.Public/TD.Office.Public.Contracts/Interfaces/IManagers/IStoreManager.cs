using LSCore.Contracts.Http;
using TD.Office.Public.Contracts.Dtos.Stores;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IStoreManager
    {
        Task<LSCoreListResponse<GetStoreDto>> GetMultiple();
    }
}