using Omu.ValueInjecter;
using TD.Core.Contracts.Interfaces;
using TD.Web.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.ProductPricesGroup
{
    public class ProductsPricesGroupGetDtoMappings : IDtoMapper<ProductPriceGroupGetDto, ProductPriceGroupEntity>
    {
        public List<ProductPriceGroupGetDto> ToListDto(List<ProductPriceGroupEntity> sender)
        {
            var list = new List<ProductPriceGroupGetDto>();
            foreach (var unit in sender)
                list.Add(ToDto(unit));
            return list;
        }
        public ProductPriceGroupGetDto ToDto(ProductPriceGroupEntity sender)
        {
            var dto = new ProductPriceGroupGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
