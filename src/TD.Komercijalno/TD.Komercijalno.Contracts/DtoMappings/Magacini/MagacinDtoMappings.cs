using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.Magacini;

public static class MagacinDtoMappings
{
	public static List<MagacinDto> ToMagacinDtoList(this List<Magacin> magacini)
	{
		var list = new List<MagacinDto>();

		foreach (var magacin in magacini)
		{
			var dto = new MagacinDto();
			dto.InjectFrom(magacin);
			dto.MagacinId = magacin.Id;
			list.Add(dto);
		}

		return list;
	}
}
