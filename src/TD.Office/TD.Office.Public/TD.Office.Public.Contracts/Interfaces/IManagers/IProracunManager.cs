using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IProracunManager
{
    ProracunDto Create(ProracuniCreateRequest request);
}
