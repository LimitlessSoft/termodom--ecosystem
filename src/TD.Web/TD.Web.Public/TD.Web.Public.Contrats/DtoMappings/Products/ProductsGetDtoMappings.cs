using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Products;

namespace TD.Web.Public.Contracts.DtoMappings.Products
{
    public class ProductsGetDtoMappings : ILSCoreDtoMapper<ProductsGetDto, ProductEntity>
    {
        public ProductsGetDto ToDto(ProductEntity sender) =>
            new ProductsGetDto()
            {
                Id = sender.Id,
                Title = sender.Name,
                VAT = sender.VAT,
                Unit = sender.Unit.Name,
                Src = sender.Src,
                Classification = sender.Classification
            };
    }
}
