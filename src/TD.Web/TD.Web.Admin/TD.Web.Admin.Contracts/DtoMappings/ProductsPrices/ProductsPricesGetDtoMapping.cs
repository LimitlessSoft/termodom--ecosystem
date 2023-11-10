using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsPrices
{
    public class ProductsPricesGetDtoMapping : ILSCoreDtoMapper<ProductsPricesGetDto, ProductPriceEntity>
    {
        public List<ProductsPricesGetDto> ToListDto(List<ProductPriceEntity> sender)
        {
            var list = new List<ProductsPricesGetDto>();
            foreach (var productPrices in sender)
                list.Add(ToDto(productPrices));
            return list;
        }

        public ProductsPricesGetDto ToDto(ProductPriceEntity sender)
        {
            var dto = new ProductsPricesGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
