using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.VrstaDoks
{
	public static class VrstaDokDtoMappings
	{
		public static List<VrstaDokDto> ToVrstaDokDtoList(this List<VrstaDok> vrstaDoks)
		{
			var list = new List<VrstaDokDto>();

			foreach (var vrDok in vrstaDoks)
			{
				var dto = new VrstaDokDto();
				dto.InjectFrom(vrDok);
				dto.VrDok = vrDok.Id;
				list.Add(dto);
			}
			return list;
		}
	}
}
