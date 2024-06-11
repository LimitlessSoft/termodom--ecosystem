using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsPrices;

public class ProductsPricesGetDtoMapping : ILSCoreDtoMapper<ProductPriceEntity, ProductsPricesGetDto>
{
    public ProductsPricesGetDto ToDto(ProductPriceEntity sender)
    {
        var dto = new ProductsPricesGetDto();
        dto.InjectFrom(sender);
        return dto;
    }
}