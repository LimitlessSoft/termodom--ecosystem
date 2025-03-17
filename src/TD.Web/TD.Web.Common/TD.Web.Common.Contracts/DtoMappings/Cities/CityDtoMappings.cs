using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Cities;

public class CityDtoMappings : ILSCoreMapper<CityEntity, CityDto>
{
	public CityDto ToMapped(CityEntity sender)
	{
		var dto = new CityDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
