using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoWebProductLinkManager
    {
        List<KomercijalnoWebProductLinksGetDto> GetMultiple();
        KomercijalnoWebProductLinksGetDto Put(KomercijalnoWebProductLinksSaveRequest request);
    }
}
