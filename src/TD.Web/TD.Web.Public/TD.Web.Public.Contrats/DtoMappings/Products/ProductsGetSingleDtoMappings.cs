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
                Id = sender.Id,
                FullDescription = sender.Description,
                OneAlternatePackageEquals = sender.OneAlternatePackageEquals,
                Unit = sender.Unit.Name,
                AlternateUnit = sender.AlternateUnit?.Name,
                Title = sender.Name,
                Vat = sender.VAT,
                CatalogId = sender.CatalogId,
                Classification = sender.Classification,
                ShortDescription = sender.ShortDescription,
                MetaDescription = sender.MetaDescription,
                MetaTitle = sender.MetaTitle
            };
    }
}
