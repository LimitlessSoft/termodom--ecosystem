using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IEnumManager : ILSCoreBaseManager
    {
        LSCoreListResponse<LSCoreIdNamePairDto> GetOrderStatuses();
    }
}
