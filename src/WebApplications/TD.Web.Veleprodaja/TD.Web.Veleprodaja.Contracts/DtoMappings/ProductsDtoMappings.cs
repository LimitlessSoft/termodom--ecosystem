using Omu.ValueInjecter;
using TD.Web.Veleprodaja.Contracts.Dtos.Products;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Contracts.DtoMappings
{
    public static class ProductsDtoMappings
    {
        public static List<ProductsGetDto> ToProductsGetDtoList(this List<Product> products)
        {
            var dto = new List<ProductsGetDto>();
            products.ForEach(x => dto.Add(x.ToProductsGetDto()));
            return dto;
        }

        public static ProductsGetDto ToProductsGetDto(this Product product)
        {
            var dto = new ProductsGetDto();
            dto.InjectFrom(product);
            return dto;
        }
    }
}
