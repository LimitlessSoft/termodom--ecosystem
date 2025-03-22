using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;

namespace TD.Komercijalno.Contracts.DtoMappings.RobaUMagacinu;

public class RobaUMagacinuGetDtoMapping : ILSCoreMapper<Entities.RobaUMagacinu, RobaUMagacinuGetDto>
{
	public RobaUMagacinuGetDto ToMapped(Entities.RobaUMagacinu sender)
	{
		var dto = new RobaUMagacinuGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
