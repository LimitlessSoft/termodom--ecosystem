using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Public.Contracts.DtoMappings.Products;

public class ProductsGetDtoMappings : ILSCoreDtoMapper<ProductEntity, ProductsGetDto>
{
    public ProductsGetDto ToDto(ProductEntity sender) =>
        new ()
        {
            Id = sender.Id,
            Title = sender.Name,
            VAT = sender.VAT,
            Unit = sender.Unit.Name,
            PriorityIndex = sender.PriorityIndex,
            Src = sender.Src,
            Classification = sender.Classification
        };
}