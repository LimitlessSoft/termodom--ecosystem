using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.ProductPricesGroup
{
    public static class ProductsPricesGroupGetDtoMappings
    {
        public static List<ProductPriceGroupGetDto> ToListDto(this List<ProductPriceGroupEntity> sender)
        {
            var list = new List<ProductPriceGroupGetDto>();
            foreach (var unit in sender)
                list.Add(unit.ToDto());
            return list;
        }
        public static ProductPriceGroupGetDto ToDto(this ProductPriceGroupEntity sender)
        {
            var dto = new ProductPriceGroupGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
