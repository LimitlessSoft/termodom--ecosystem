using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Requests.Units;

namespace TD.Web.Admin.Contracts.Interfaces.Managers
{
    public interface IUnitManager : IBaseManager
    {
        Response<UnitsGetDto> Get(IdRequest request);
        ListResponse<UnitsGetDto> GetMultiple();
        Response<long> Save(UnitSaveRequest request);
        Response Delete(IdRequest request);
    }
}
