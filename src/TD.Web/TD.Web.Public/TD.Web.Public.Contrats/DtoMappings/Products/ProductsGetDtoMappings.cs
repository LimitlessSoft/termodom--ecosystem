using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contrats.Dtos.Products;

namespace TD.Web.Public.Contrats.DtoMappings.Products
{
    public class ProductsGetDtoMappings : ILSCoreDtoMapper<ProductsGetDto, ProductEntity>
    {
        public ProductsGetDto ToDto(ProductEntity sender) =>
            new ProductsGetDto()
            {
                Title = sender.Name,
                Src = sender.Src,
                ImageSrc = sender.Image,
                Classification = sender.Classification
            };
    }
}
