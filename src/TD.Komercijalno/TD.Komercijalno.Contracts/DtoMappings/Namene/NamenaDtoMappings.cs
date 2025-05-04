using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.Namene
{
	public static class NamenaDtoMappings
	{
		public static List<NamenaDto> ToNamenaDtoList(this List<Namena> namene)
		{
			var list = new List<NamenaDto>();

			foreach (var namena in namene)
			{
				var dto = new NamenaDto();
				dto.InjectFrom(namena);
				list.Add(dto);
			}
			return list;
		}
	}
}
