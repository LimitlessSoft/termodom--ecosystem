using LSCore.Contracts.Requests;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IRobaManager
{
    List<RobaDto> GetMultiple(RobaGetMultipleRequest request);
    Roba Create(RobaCreateRequest request);
    RobaDto Get(LSCoreIdRequest request);
}
