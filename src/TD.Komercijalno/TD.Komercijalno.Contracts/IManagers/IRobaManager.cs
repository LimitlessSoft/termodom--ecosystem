using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IRobaManager
    {
        LSCoreListResponse<RobaDto> GetMultiple(RobaGetMultipleRequest request);
        LSCoreResponse<Roba> Create(RobaCreateRequest request);
    }
}
