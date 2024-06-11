using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsGroups;

public class ProductsGroupsGetDtoMappings : ILSCoreDtoMapper<ProductGroupEntity, ProductsGroupsGetDto>
{
    public ProductsGroupsGetDto ToDto(ProductGroupEntity sender)
    {
        var dto = new ProductsGroupsGetDto();
        dto.InjectFrom(sender);
        dto.TypeId = (int)sender.Type;
        return dto;
    }
}