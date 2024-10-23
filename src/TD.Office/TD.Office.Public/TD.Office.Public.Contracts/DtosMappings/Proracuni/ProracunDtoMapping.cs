using LSCore.Contracts.Extensions;
using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Proracuni;

namespace TD.Office.Public.Contracts.DtosMappings.Proracuni;

public class ProracunDtoMapping : ILSCoreDtoMapper<ProracunEntity, ProracunDto>
{
    public ProracunDto ToDto(ProracunEntity sender) =>
        new()
        {
            Id = sender.Id,
            CreatedAt = sender.CreatedAt,
            MagacinId = sender.MagacinId,
            State = sender.State,
            Referent = sender.User.Nickname,
            KomercijalnoDokument =
                sender.KomercijalnoVrDok == null
                    ? ""
                    : $"{sender.KomercijalnoVrDok} - {sender.KomercijalnoBrDok}",
            Type = sender.Type.GetDescription()!,
            Items = sender
                .Items.Select(x => new ProracunItemDto
                {
                    Id = x.Id,
                    RobaId = x.RobaId,
                    Kolicina = x.Kolicina,
                    CenaBezPdv = x.CenaBezPdv,
                    Pdv = x.Pdv,
                    Rabat = x.Rabat,
                })
                .ToList()
        };
}
