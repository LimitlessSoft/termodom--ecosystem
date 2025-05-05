using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.IstorijaUplata;

namespace TD.Komercijalno.Contracts.DtoMappings.IstorijaUplata;

public static class IstorijaUplataDtoMappings
{
	public static IstorijaUplataDto ToIstorijaUplataDto(this Entities.IstorijaUplata source)
	{
		var dto = new IstorijaUplataDto();
		dto.InjectFrom(source);
		return dto;
	}

	public static List<IstorijaUplataDto> ToIstorijaUpaltaDtoList(
		this List<Entities.IstorijaUplata> source
	) => source.Select(x => x.ToIstorijaUplataDto()).ToList();
}
