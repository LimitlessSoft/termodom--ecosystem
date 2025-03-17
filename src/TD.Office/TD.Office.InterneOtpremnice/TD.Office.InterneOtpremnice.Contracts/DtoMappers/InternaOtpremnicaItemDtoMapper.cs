using LSCore.Mapper.Contracts;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.DtoMappers;

public class InternaOtpremnicaItemDtoMapper
	: ILSCoreMapper<InternaOtpremnicaItemEntity, InternaOtpremnicaItemDto>
{
	public static Func<InternaOtpremnicaItemEntity, InternaOtpremnicaItemDto> ToDtoFunc = (
		InternaOtpremnicaItemEntity sender
	) =>
		new InternaOtpremnicaItemDto
		{
			Id = sender.Id,
			InternaOtpremnicaId = sender.InternaOtpremnica.Id,
			Kolicina = sender.Kolicina,
			RobaId = sender.RobaId
		};

	public InternaOtpremnicaItemDto ToMapped(InternaOtpremnicaItemEntity sender) =>
		ToDtoFunc(sender);
}
