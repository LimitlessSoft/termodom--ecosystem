using LSCore.Mapper.Domain;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class KomercijalnoWebProductLinkManager(IKomercijalnoWebProductLinkRepository repository)
	: IKomercijalnoWebProductLinkManager
{
	public List<KomercijalnoWebProductLinksGetDto> GetMultiple() =>
		repository
			.GetMultiple()
			.ToMappedList<KomercijalnoWebProductLinkEntity, KomercijalnoWebProductLinksGetDto>();

	public KomercijalnoWebProductLinksGetDto Put(KomercijalnoWebProductLinksSaveRequest request)
	{
		var entity = request.Id.HasValue
			? repository.Get(request.Id.Value)
			: new KomercijalnoWebProductLinkEntity();

		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);
		var dto = new KomercijalnoWebProductLinksGetDto();
		dto.InjectFrom(entity);
		return dto;
	}
}
