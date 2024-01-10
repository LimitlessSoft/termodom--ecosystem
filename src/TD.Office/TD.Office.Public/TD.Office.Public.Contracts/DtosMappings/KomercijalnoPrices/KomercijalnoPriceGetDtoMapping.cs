using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoPrices
{
    public class KomercijalnoPriceGetDtoMapping : ILSCoreDtoMapper<KomercijalnoPriceGetDto, KomercijalnoPriceEntity>
    {
        public KomercijalnoPriceGetDto ToDto(KomercijalnoPriceEntity sender) =>
            new KomercijalnoPriceGetDto()
            {
                RobaId = sender.RobaId,
                ProdajnaCenaBezPDV = sender.ProdajnaCenaBezPDV,
                NabavnaCenaBezPDV = sender.NabavnaCenaBezPDV
            };
    }
}
