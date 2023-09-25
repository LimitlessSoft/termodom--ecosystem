using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.ProductPrices;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.ProductsPrices
{
    public static class ProductsPricesGetDtoMapping
    {
        public static List<ProductsPricesGetDto> ToListDto(this List<ProductPriceEntity> sender)
        {
            var list = new List<ProductsPricesGetDto>();
            foreach (var productPrices in sender)
                list.Add(productPrices.ToDto());
            return list;
        }
        public static ProductsPricesGetDto ToDto(this ProductPriceEntity sender)
        {
            var dto = new ProductsPricesGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
