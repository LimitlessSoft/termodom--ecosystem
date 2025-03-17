using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductPricesGroup;

public class ProductsPricesGroupGetDtoMappings
	: ILSCoreMapper<ProductPriceGroupEntity, ProductPriceGroupGetDto>
{
	public ProductPriceGroupGetDto ToMapped(ProductPriceGroupEntity sender)
	{
		var dto = new ProductPriceGroupGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
