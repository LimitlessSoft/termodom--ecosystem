using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;

namespace TD.TDOffice.Repository.Mappings
{
    public class SaveMCPartnerCenovnikItemRequestMapping : ILSCoreMap<MCPartnerCenovnikItemEntity, SaveMCPartnerCenovnikItemRequest>
    {
        public void Map(MCPartnerCenovnikItemEntity entity, SaveMCPartnerCenovnikItemRequest request)
        {
            entity.InjectFrom(request);
            if(request.Id.HasValue)
                entity.Id = request.Id.Value;
        }
    }
}
