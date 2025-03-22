using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IPartnerManager
{
	Task<List<PartnerDto>> GetRecentlyCreatedPartnersAsync();
	PartnerYearsDto GetPartnersReportByYearsKomercijalnoFinansijsko();
	Task<
		LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>
	> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(
		GetPartnersReportByYearsKomercijalnoFinansijskoRequest request
	);
	bool SaveKomercijalnoFinansijskoStatus(SaveKomercijalnoFinansijskoStatusRequest request);
	bool SaveKomercijalnoFinansijskoKomentar(SaveKomercijalnoFinansijskoCommentRequest request);
	Task<Dictionary<string, Dictionary<int, object>>> GetPartnerAnalysisAsync(
		LSCoreIdRequest request
	);
	Dictionary<string, string> GetCompanyTypes();
}
