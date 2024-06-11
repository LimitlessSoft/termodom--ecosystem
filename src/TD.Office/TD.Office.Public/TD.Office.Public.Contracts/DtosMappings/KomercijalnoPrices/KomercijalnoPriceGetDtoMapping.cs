using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;
using TD.Office.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoPrices
{
    public class KomercijalnoPriceGetDtoMapping : ILSCoreDtoMapper<KomercijalnoPriceEntity, KomercijalnoPriceGetDto>
    {
        public KomercijalnoPriceGetDto ToDto(KomercijalnoPriceEntity sender) =>
            new ()
            {
                RobaId = sender.RobaId,
                ProdajnaCenaBezPDV = sender.ProdajnaCenaBezPDV,
                NabavnaCenaBezPDV = sender.NabavnaCenaBezPDV
            };
    }
}
