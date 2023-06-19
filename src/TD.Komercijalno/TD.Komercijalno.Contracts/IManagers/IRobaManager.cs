using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IRobaManager
    {
        ListResponse<RobaDto> GetMultiple(RobaGetMultipleRequest request);
        Response<Roba> Create(RobaCreateRequest request);
    }
}
