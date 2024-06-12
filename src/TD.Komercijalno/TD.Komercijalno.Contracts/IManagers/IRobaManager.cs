using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IRobaManager
    {
        List<RobaDto> GetMultiple(RobaGetMultipleRequest request);
        Roba Create(RobaCreateRequest request);
    }
}
