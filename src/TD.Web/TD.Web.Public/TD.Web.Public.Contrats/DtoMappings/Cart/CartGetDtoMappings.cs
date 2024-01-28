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
