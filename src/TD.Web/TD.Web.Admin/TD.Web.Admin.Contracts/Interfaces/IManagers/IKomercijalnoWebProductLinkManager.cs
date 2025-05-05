using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
	public interface IKomercijalnoWebProductLinkManager
	{
		List<KomercijalnoWebProductLinksGetDto> GetMultiple();
		KomercijalnoWebProductLinksGetDto Put(KomercijalnoWebProductLinksSaveRequest request);
	}
}
