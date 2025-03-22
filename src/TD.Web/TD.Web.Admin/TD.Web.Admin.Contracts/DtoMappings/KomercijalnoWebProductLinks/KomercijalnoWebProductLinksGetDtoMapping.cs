using LSCore.Mapper.Contracts;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.KomercijalnoWebProductLinks;

public class KomercijalnoWebProductLinksGetDtoMapping
	: ILSCoreMapper<KomercijalnoWebProductLinkEntity, KomercijalnoWebProductLinksGetDto>
{
	public KomercijalnoWebProductLinksGetDto ToMapped(KomercijalnoWebProductLinkEntity sender) =>
		new()
		{
			Id = sender.Id,
			RobaId = sender.RobaId,
			WebId = sender.WebId,
		};
}
