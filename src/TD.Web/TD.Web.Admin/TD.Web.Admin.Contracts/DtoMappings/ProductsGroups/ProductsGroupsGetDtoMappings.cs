using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsGroups;

public class ProductsGroupsGetDtoMappings : ILSCoreMapper<ProductGroupEntity, ProductsGroupsGetDto>
{
	public ProductsGroupsGetDto ToMapped(ProductGroupEntity sender)
	{
		var dto = new ProductsGroupsGetDto();
		dto.InjectFrom(sender);
		dto.TypeId = (int)sender.Type;
		return dto;
	}
}
