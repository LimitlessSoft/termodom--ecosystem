using LSCore.Mapper.Domain;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ProfessionManager(IProfessionRepository repository) : IProfessionManager
{
	public List<ProfessionsGetMultipleDto> GetMultiple() =>
		repository.GetMultiple().ToMappedList<ProfessionEntity, ProfessionsGetMultipleDto>();

	public long Save(SaveProfessionRequest request)
	{
		var entity = request.Id == 0 ? new ProfessionEntity() : repository.Get(request.Id!.Value);
		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);
		return entity.Id;
	}
}
