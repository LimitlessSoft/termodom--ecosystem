using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Admin.Contracts.Dtos.Professions;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProfessionManager
{
    List<ProfessionsGetMultipleDto> GetMultiple();
    long Save(SaveProfessionRequest request);
}