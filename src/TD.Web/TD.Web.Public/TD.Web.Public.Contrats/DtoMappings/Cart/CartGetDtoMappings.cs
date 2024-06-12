using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Public.Contracts.DtoMappings.Cart;

public class CartGetDtoMappings : ILSCoreDtoMapper<OrderEntity, CartGetDto>
{
    public CartGetDto ToDto(OrderEntity sender)
    {
        var dto = new CartGetDto();
        dto.OneTimeHash = sender.OneTimeHash;
        dto.Items = new List<CartItemDto>();

        var valueWithVAT = sender.Items.Sum(x => x.Price * (x.VAT / 100 + 1) * x.Quantity);
        dto.Summary = new CartSummaryDto()
        {
            VATValue = sender.Items.Sum(x => (x.VAT/100) * x.Price * x.Quantity),
            ValueWithoutVAT = sender.Items.Sum(x => x.Price * x.Quantity),
            ValueWithVAT = valueWithVAT,
            DiscountValue = sender.Items.Sum(x => x.PriceWithoutDiscount * x.Quantity * (x.VAT / 100 + 1)) - valueWithVAT,
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