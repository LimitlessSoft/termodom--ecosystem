using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Komercijalno.Contracts.Dtos.Partneri;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Partneri;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IPartnerManager
{
	int Create(PartneriCreateRequest request);
	LSCoreSortedAndPagedResponse<PartnerDto> GetMultiple(PartneriGetMultipleRequest request);
	List<PPKategorija> GetKategorije();
	int GetPoslednjiId();
	bool GetDuplikat(PartneriGetDuplikatRequest request);
	PartnerDto GetSingle(LSCoreIdRequest request);
	int GetCount();
}
