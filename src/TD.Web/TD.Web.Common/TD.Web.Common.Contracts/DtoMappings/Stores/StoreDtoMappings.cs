using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Stores;

public class StoreDtoMappings : ILSCoreMapper<StoreEntity, StoreDto>
{
	public StoreDto ToMapped(StoreEntity sender)
	{
		var dto = new StoreDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
