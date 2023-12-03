using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Dtos.Cities;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface ICityManager : ILSCoreBaseManager
    {
        LSCoreListResponse<CityDto> GetMultiple();
    }
}
