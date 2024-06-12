using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Admin.Contracts.DtoMappings.KomercijalnoWebProductLinks
{
    public class KomercijalnoWebProductLinksGetDtoMapping : ILSCoreDtoMapper<KomercijalnoWebProductLinkEntity, KomercijalnoWebProductLinksGetDto>
    {
        public KomercijalnoWebProductLinksGetDto ToDto(KomercijalnoWebProductLinkEntity sender) =>
            new ()
            {
                Id = sender.Id,
                RobaId = sender.RobaId,
                WebId = sender.WebId,
            };
    }
}
