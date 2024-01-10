using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoWebProductLinkManager
    {
        LSCoreListResponse<KomercijalnoWebProductLinksGetDto> GetMultiple();
    }
}
