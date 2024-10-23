using LSCore.Contracts.Responses;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IPartnerManager
{
    Task<List<PartnerDto>> GetRecentlyCreatedPartnersAsync();
    PartnerYearsDto GetPartnersReportByYearsKomercijalnoFinansijsko();
    Task<List<GetPartnersReportByYearsKomercijalnoFinansijskoDto>> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(GetPartnersReportByYearsKomercijalnoFinansijskoRequest request);
}
