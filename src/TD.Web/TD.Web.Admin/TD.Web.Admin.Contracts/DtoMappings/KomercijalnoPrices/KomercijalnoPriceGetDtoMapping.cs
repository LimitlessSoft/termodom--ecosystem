
using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.KomercijalnoPrices
{
    public class KomercijalnoPriceGetDtoMapping : ILSCoreDtoMapper<KomercijalnoPriceGetDto, KomercijalnoPriceEntity>
    {
        public KomercijalnoPriceGetDto ToDto(KomercijalnoPriceEntity sender) =>
            new KomercijalnoPriceGetDto
            {
                RobaId = sender.RobaId,
                NabavnaCenaBezPDV = sender.NabavnaCenaBezPDV,
                ProdajnaCenaBezPDV = sender.ProdajnaCenaBezPDV
            };
    }
}
