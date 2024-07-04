using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Public.Contracts.DtoMappings.ProductsGroups;

public class ProductsGroupsGetDtoMappings : ILSCoreDtoMapper<ProductGroupEntity, ProductsGroupsGetDto>
{
    public ProductsGroupsGetDto ToDto(ProductGroupEntity sender)
    {
        var dto = new ProductsGroupsGetDto();
        dto.InjectFrom(sender);
        dto.ParentName = sender.ParentGroup?.Name;
        dto.WelcomeMessage = sender.WelcomeMessage;
        dto.SalesMobile = sender.SalesMobile;
        dto.Type = sender.Type;
        return dto;
    }
}