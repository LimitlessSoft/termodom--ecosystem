using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Common.Contracts.DtoMappings.Stores
{
    public class StoreDtoMappings : ILSCoreDtoMapper<StoreEntity, StoreDto>
    {
        public StoreDto ToDto(StoreEntity sender)
        {
            var dto = new StoreDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
