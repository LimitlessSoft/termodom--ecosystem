using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Requests.Professions;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProfessionManager : ILSCoreBaseManager
    {
        LSCoreListResponse<ProfessionsGetMultipleDto> GetMultiple();
        LSCoreResponse<long> Save(SaveProfessionRequest request);
    }
}
