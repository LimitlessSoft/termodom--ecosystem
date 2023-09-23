using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Requests.Units;

namespace TD.Web.Contracts.Interfaces.Managers
{
    public interface IUnitManager
    {
        Response<UnitsGetDto> Get(IdRequest request);
        ListResponse<UnitsGetDto> GetMultiple();
        Response<long> Save(UnitSaveRequest request);
        Response<bool> Delete(IdRequest request);
    }
}
