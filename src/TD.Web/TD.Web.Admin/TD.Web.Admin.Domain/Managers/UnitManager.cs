using LSCore.Common.Contracts;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class UnitManager(IUnitRepository repository) : IUnitManager
{
	public UnitsGetDto Get(LSCoreIdRequest request) =>
		repository.Get(request.Id).ToMapped<UnitEntity, UnitsGetDto>();

	public List<UnitsGetDto> GetMultiple() =>
		repository.GetMultiple().ToMappedList<UnitEntity, UnitsGetDto>();

	public long Save(UnitSaveRequest request)
	{
		var entity = request.Id == 0 ? new UnitEntity() : repository.Get(request.Id!.Value);
		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);
		return entity.Id;
	}

	public void Delete(UnitDeleteRequest request)
	{
		request.Validate();
		repository.HardDelete(request.Id);
	}
}
