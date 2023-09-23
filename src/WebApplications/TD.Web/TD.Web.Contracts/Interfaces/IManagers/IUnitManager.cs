using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Requests.Units;

namespace TD.Web.Contracts.Interfaces.Managers
{
    public interface IUnitManager
    {
        Response<UnitsGetDto> Get(IdRequest request);
        Response<long> Save(UnitSaveRequest request);
    }
}
