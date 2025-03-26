using LSCore.Common.Contracts;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Requests.Units;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IUnitManager
{
	UnitsGetDto Get(LSCoreIdRequest request);
	List<UnitsGetDto> GetMultiple();
	long Save(UnitSaveRequest request);
	void Delete(UnitDeleteRequest request);
}
