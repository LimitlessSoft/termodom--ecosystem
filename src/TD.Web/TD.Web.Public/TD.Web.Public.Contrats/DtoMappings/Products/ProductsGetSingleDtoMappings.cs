using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Products;

namespace TD.Web.Public.Contracts.DtoMappings.Products
{
    public class ProductsGetSingleDtoMappings : ILSCoreDtoMapper<ProductsGetSingleDto, ProductEntity>
    {
        public ProductsGetSingleDto ToDto(ProductEntity sender) =>
            new ProductsGetSingleDto()
            {
                FullDescription = "To be implemented",
                AlternateUnitMultiplicator = null,
                AlternateUnit = null,
                Title = sender.Name,
                BaseUnit = sender.Unit.Name,
                Category = "To be implemented", // here implement helper which will get category by concating all parents in sequence
                CatalogId = sender.CatalogId,
                Classification = sender.Classification,
                ShortDescription = "To be implemented"
            };
    }
}
