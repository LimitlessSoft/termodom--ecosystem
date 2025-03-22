using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsPrices;

public class ProductsPricesGetDtoMapping : ILSCoreMapper<ProductPriceEntity, ProductsPricesGetDto>
{
	public ProductsPricesGetDto ToMapped(ProductPriceEntity sender)
	{
		var dto = new ProductsPricesGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
