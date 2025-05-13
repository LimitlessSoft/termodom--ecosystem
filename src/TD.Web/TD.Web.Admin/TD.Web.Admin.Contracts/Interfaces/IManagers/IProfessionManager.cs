using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Requests.Professions;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProfessionManager
{
	List<ProfessionsGetMultipleDto> GetMultiple();
	long Save(SaveProfessionRequest request);
}
