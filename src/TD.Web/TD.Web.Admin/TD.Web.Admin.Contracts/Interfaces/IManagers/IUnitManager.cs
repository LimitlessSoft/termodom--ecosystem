using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Admin.Contracts.Dtos.Units;
using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IUnitManager
{
    UnitsGetDto Get(LSCoreIdRequest request);
    List<UnitsGetDto> GetMultiple();
    long Save(UnitSaveRequest request);
    void Delete(UnitDeleteRequest request);
}