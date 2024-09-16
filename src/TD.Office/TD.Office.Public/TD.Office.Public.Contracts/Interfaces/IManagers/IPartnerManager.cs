using LSCore.Contracts.Responses;
using TD.Office.Public.Contracts.Dtos.Partners;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IPartnerManager
{
    Task<LSCoreSortedAndPagedResponse<PartnerDto>> GetRecentlyCreatedPartnersAsync();
}
