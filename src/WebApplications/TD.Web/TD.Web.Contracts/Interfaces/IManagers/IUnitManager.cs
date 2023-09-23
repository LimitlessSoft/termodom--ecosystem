using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.Units;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IUnitManager
    {
        Response<UnitsGetDto> Get(IdRequest request);
    }
}
