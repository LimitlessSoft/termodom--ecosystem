using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Products;

public class ProductsGetDtoMappings : ILSCoreDtoMapper<ProductEntity, ProductsGetDto>
{
    public ProductsGetDto ToDto(ProductEntity sender)
    {
        var dto = new ProductsGetDto();
        dto.InjectFrom(sender);
        
        if(sender.Groups != null)
            dto.Groups = sender.Groups.Select(z => z.Id).ToList();
        
        if(sender.Price != null)
        {
            dto.MinWebBase = sender.Price.Min;
            dto.MaxWebBase = sender.Price.Max;
        }

        dto.UnitId = sender.Unit.Id;
        dto.Classification = (int)sender.Classification;
        return dto;
    }
}