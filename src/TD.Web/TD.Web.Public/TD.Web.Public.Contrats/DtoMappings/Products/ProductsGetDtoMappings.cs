using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contrats.Dtos.Products;

namespace TD.Web.Public.Contrats.DtoMappings.Products
{
    public class ProductsGetDtoMappings : ILSCoreDtoMapper<ProductsGetDto, ProductEntity>
    {
        public ProductsGetDto ToDto(ProductEntity sender)
        {
            var dto = new ProductsGetDto();
            dto.Title = sender.Name;
            dto.Src = sender.Src;
            dto.ImageSrc = sender.Image;
            dto.Classification = sender.Classification;
            return dto;
        }
    }
}
