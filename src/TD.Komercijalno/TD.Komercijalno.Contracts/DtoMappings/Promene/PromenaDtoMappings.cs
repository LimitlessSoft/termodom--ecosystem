using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Promene;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.Promene;

public static class PromenaDtoMappings
{
	public static PromenaDto ToPromenaDto(this Promena source)
	{
		var dto = new PromenaDto();
		dto.InjectFrom(source);
		return dto;
	}

	public static List<PromenaDto> ToPromenaDtoList(this List<Promena> source) =>
		source.Select(x => x.ToPromenaDto()).ToList();
}
