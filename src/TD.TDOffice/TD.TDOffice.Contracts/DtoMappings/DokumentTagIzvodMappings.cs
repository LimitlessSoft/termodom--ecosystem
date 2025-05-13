using System.ComponentModel;
using Omu.ValueInjecter;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Contracts.DtoMappings
{
	public static class DokumentTagIzvodMappings
	{
		public static List<DokumentTagIzvodGetDto> ToListDto(this List<DokumentTagIzvod> list)
		{
			var dtoList = new List<DokumentTagIzvodGetDto>();

			foreach (var item in list)
			{
				var dto = new DokumentTagIzvodGetDto();
				dto.InjectFrom(item);
				dtoList.Add(dto);
			}

			return dtoList;
		}

		public static DokumentTagIzvod ToDokumentTagIzvod(this DokumentTagizvodPutRequest request)
		{
			var entity = new DokumentTagIzvod();

			entity.InjectFrom(request);

			if (request.Id.HasValue)
				entity.Id = (int)request.Id;

			if (request.BrojDokumentaIzvoda.HasValue)
				entity.BrojDokumentaIzvoda = request.BrojDokumentaIzvoda.Value;

			return entity;
		}
	}
}
