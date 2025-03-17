using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Units;

public class UnitsGetDtoMappings : ILSCoreMapper<UnitEntity, UnitsGetDto>
{
	public UnitsGetDto ToMapped(UnitEntity sender)
	{
		var dto = new UnitsGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
