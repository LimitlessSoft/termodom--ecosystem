using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IProracunManager
{
	void Create(ProracuniCreateRequest request);
	LSCoreSortedAndPagedResponse<ProracunDto> GetMultiple(ProracuniGetMultipleRequest request);
	ProracunDto GetSingle(LSCoreIdRequest request);
	void PutState(ProracuniPutStateRequest request);
	void PutPPID(ProracuniPutPPIDRequest request);
	void PutNUID(ProracuniPutNUIDRequest request);
	Task<ProracunItemDto> AddItemAsync(ProracuniAddItemRequest request);
	void DeleteItem(LSCoreIdRequest request);
	void PutItemKolicina(ProracuniPutItemKolicinaRequest request);
	Task<ProracunDto> ForwardToKomercijalnoAsync(LSCoreIdRequest request);
	void PutItemRabat(ProracuniPutItemRabatRequest request);
	void PutEmail(ProracuniPutEmailRequest request);
	void PutRecommendedValue(ProracuniPutRecommendedValueRequest request);
}
