using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INamenaManager
    {
        LSCoreListResponse<NamenaDto> GetMultiple();
    }
}
