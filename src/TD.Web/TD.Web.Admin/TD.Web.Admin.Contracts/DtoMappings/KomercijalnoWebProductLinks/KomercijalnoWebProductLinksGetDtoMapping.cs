using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.KomercijalnoWebProductLinks
{
    public class KomercijalnoWebProductLinksGetDtoMapping : ILSCoreDtoMapper<KomercijalnoWebProductLinksGetDto, KomercijalnoWebProductLinkEntity>
    {
        public KomercijalnoWebProductLinksGetDto ToDto(KomercijalnoWebProductLinkEntity sender) =>
            new KomercijalnoWebProductLinksGetDto()
            {
                Id = sender.Id,
                RobaId = sender.RobaId,
                WebId = sender.WebId,
            };
    }
}
