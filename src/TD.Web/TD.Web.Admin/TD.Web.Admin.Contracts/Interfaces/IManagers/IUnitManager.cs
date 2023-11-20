using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Requests.Units;

namespace TD.Web.Admin.Contracts.Interfaces.Managers
{
    public interface IUnitManager : ILSCoreBaseManager
    {
        LSCoreResponse<UnitsGetDto> Get(LSCoreIdRequest request);
        LSCoreListResponse<UnitsGetDto> GetMultiple();
        LSCoreResponse<long> Save(UnitSaveRequest request);
        LSCoreResponse Delete(LSCoreIdRequest request);
    }
}
