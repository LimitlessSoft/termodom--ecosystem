using TD.Komercijalno.Contracts.Requests.Partneri;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IPartnerManager
{
    int Create(PartneriCreateRequest request);
}
