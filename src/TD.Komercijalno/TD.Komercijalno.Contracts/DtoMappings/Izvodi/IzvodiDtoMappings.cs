using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.Izvodi;

public static class IzvodiDtoMappings
{
	public static IzvodDto ToIzvodDto(this Izvod source)
	{
		var dto = new IzvodDto();
		dto.InjectFrom(source);
		return dto;
	}

	public static List<IzvodDto> ToIzvodiDtoList(this List<Izvod> source) =>
		source.Select(x => x.ToIzvodDto()).ToList();
}
