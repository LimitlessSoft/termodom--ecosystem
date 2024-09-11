using TD.Komercijalno.Contracts.Dtos.Partneri;
using TD.Komercijalno.Contracts.Requests.Partneri;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IPartnerManager
{
    int Create(PartneriCreateRequest request);
    List<PartnerDto> GetMultiple(PartneriGetMultipleRequest request);
}
