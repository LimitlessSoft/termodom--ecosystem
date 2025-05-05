using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
	public static class RobaHelpers
	{
		public static RobaDto ToRobaDto(this Roba source)
		{
			var robaDto = new RobaDto();
			robaDto.RobaId = source.Id;
			robaDto.InjectFrom(source);
			robaDto.Tarifa.InjectFrom(source.Tarifa);

			return robaDto;
		}

		public static List<RobaDto> ToRobaDtoList(this List<Roba> source) =>
			source.Select(roba => roba.ToRobaDto()).ToList();
	}
}
