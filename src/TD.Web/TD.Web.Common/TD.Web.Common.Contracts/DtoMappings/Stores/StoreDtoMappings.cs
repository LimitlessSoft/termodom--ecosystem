using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Stores
{
    public class StoreDtoMappings : ILSCoreDtoMapper<StoreDto, StoreEntity>
    {
        public StoreDto ToDto(StoreEntity sender)
        {
            var dto = new StoreDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
