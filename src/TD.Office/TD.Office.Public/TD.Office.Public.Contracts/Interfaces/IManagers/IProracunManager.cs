using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
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
    Task AddItem(ProracuniAddItemRequest request);
}
