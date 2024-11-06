using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Proracuni;

namespace TD.Office.Public.Contracts.DtosMappings.Proracuni;

public class ProracunItemDtoMapping : ILSCoreDtoMapper<ProracunItemEntity, ProracunItemDto>
{
    public ProracunItemDto ToDto(ProracunItemEntity sender) =>
        new()
        {
            Id = sender.Id,
            RobaId = sender.RobaId,
            Kolicina = sender.Kolicina,
            CenaBezPdv = sender.CenaBezPdv,
            Pdv = sender.Pdv,
            Rabat = sender.Rabat
        };
}
