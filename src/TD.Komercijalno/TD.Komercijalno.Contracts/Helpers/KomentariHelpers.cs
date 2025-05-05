using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
	public static class KomentariHelpers
	{
		public static KomentarDto ToKomentarDto(this Komentar entity)
		{
			var dto = new KomentarDto();
			dto.InjectFrom(entity);
			return dto;
		}
	}
}
