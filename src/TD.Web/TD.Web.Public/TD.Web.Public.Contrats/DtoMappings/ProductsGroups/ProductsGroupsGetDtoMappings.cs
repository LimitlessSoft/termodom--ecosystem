using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;

namespace TD.Web.Public.Contracts.DtoMappings.ProductsGroups;

public class ProductsGroupsGetDtoMappings : ILSCoreMapper<ProductGroupEntity, ProductsGroupsGetDto>
{
	public ProductsGroupsGetDto ToMapped(ProductGroupEntity sender)
	{
		var dto = new ProductsGroupsGetDto();
		dto.InjectFrom(sender);
		dto.ParentName = sender.ParentGroup?.Name;
		dto.ParentSrc = sender.ParentGroup?.Src ?? string.Empty;
		dto.WelcomeMessage = sender.WelcomeMessage;
		dto.SalesMobile = sender.SalesMobile;
		dto.Type = sender.Type;
		dto.Src = sender.Src;
		return dto;
	}
}
