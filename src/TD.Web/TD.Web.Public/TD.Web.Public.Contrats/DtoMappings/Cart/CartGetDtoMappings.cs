using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Cart;

namespace TD.Web.Public.Contracts.DtoMappings.Cart
{
    public class CartGetDtoMappings : ILSCoreDtoMapper<CartGetDto, OrderEntity>
    {
        public CartGetDto ToDto(OrderEntity sender)
        {
            var dto = new CartGetDto();
            dto.OneTimeHash = sender.OneTimeHash;
            dto.Items = new List<CartItemDto>();

            var valueWithoutVAT = sender.Items.Sum(x => x.Price * x.Quantity);
            dto.Summary = new CartSummaryDto()
            {
                VATValue = sender.Items.Sum(x => (x.VAT/100) * x.Price * x.Quantity),
                ValueWithoutVAT = valueWithoutVAT,
                ValueWithVAT = sender.Items.Sum(x => x.Price * (x.VAT/100 + 1) * x.Quantity),
                DiscountValue = sender.Items.Sum(x => x.PriceWithoutDiscount * x.Quantity) - valueWithoutVAT,
            };

            sender.Items.ForEach(x =>
            {
                var item = new CartItemDto()
                {
                    Id = x.Id,
                    ProductId = x.Product.Id,
                    Name = x.Product.Name,
                    Quantity = x.Quantity,
                    Unit = x.Product.Unit.Name,
                    Price = x.Price,
                    VAT = x.VAT
                };
                dto.Items.Add(item);
            });
            return dto;
        }
    }
}
