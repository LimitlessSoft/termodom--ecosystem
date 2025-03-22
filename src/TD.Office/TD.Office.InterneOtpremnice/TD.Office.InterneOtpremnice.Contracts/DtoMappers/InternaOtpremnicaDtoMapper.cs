using LSCore.Mapper.Contracts;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.DtoMappers;

public class InternaOtpremnicaDtoMapper
	: ILSCoreMapper<InternaOtpremnicaEntity, InternaOtpremnicaDto>
{
	public InternaOtpremnicaDto ToMapped(InternaOtpremnicaEntity sender) =>
		new()
		{
			Id = sender.Id,
			MagacinId = sender.PolazniMagacinId,
			DestinacioniMagacinId = sender.DestinacioniMagacinId,
			State = (int)sender.Status,
			CreatedAt = sender.CreatedAt,
			Referent = sender.CreatedBy.ToString(),
			KomercijalnoDokument = sender.KomercijalnoVrDok.HasValue
				? $"{sender.KomercijalnoVrDok} - {sender.KomercijalnoBrDok}"
				: null
		};
}
