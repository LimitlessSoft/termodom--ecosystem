using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.ProductPrices;

namespace TD.Web.Public.Contracts.DtoMappings.ProductPrice
{
    public class GetProductPriceDtoMappings : ILSCoreDtoMapper<GetProductPricesDto, ProductPriceEntity>
    {
        public GetProductPricesDto ToDto(ProductPriceEntity sender) =>
            new GetProductPricesDto()
            {
                MaxPrice = sender.Max,
                MinPrice = sender.Min
            };
    }
}
