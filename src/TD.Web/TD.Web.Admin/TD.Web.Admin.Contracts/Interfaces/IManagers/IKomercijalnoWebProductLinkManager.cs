using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoWebProductLinkManager
    {
        LSCoreListResponse<KomercijalnoWebProductLinksGetDto> GetMultiple();
        LSCoreResponse<KomercijalnoWebProductLinksGetDto> Put(KomercijalnoWebProductLinksSaveRequest request);
    }
}
