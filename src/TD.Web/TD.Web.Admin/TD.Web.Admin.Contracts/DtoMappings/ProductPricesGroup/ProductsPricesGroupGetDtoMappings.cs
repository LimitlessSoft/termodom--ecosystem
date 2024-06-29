using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductPricesGroup;

public class ProductsPricesGroupGetDtoMappings : ILSCoreDtoMapper<ProductPriceGroupEntity, ProductPriceGroupGetDto>
{
    public List<ProductPriceGroupGetDto> ToListDto(List<ProductPriceGroupEntity> sender)
    {
        var list = new List<ProductPriceGroupGetDto>();
        foreach (var unit in sender)
            list.Add(ToDto(unit));
        return list;
    }
    
    public ProductPriceGroupGetDto ToDto(ProductPriceGroupEntity sender)
    {
        var dto = new ProductPriceGroupGetDto();
        dto.InjectFrom(sender);
        return dto;
    }
}