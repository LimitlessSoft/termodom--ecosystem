using LSCore.Contracts.Responses;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IProracunManager
{
    void Create(ProracuniCreateRequest request);
    LSCoreSortedAndPagedResponse<ProracunDto> GetMultiple(ProracuniGetMultipleRequest request);
}
