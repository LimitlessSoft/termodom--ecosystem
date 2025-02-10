using LSCore.Contracts.Interfaces;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.DtoMappers;

public class InternaOtpremnicaDetailsDtoMapper() : ILSCoreDtoMapper<InternaOtpremnicaEntity, InternaOtpremnicaDetailsDto>
{
    public InternaOtpremnicaDetailsDto ToDto(InternaOtpremnicaEntity sender) => 
        new ()
        {
            Id = sender.Id,
            MagacinId = sender.PolazniMagacinId,
            DestinacioniMagacinId = sender.DestinacioniMagacinId,
            State = (int)sender.Status,
            CreatedAt = sender.CreatedAt,
            Referent = sender.CreatedBy.ToString(),
            Items = sender.Items!.Select(InternaOtpremnicaItemDtoMapper.ToDtoFunc).ToList(),
            KomercijalnoDokument = sender.KomercijalnoVrDok.HasValue ? $"{sender.KomercijalnoVrDok} - {sender.KomercijalnoBrDok}" : null
        };
}