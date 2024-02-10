using LSCore.Contracts.Extensions;
using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Orders
{
    public class OrdersGetDtoMappings : ILSCoreDtoMapper<OrdersGetDto, OrderEntity>
    {
        public OrdersGetDto ToDto(OrderEntity sender) => 
            new OrdersGetDto
            {
                OneTimeHash = sender.OneTimeHash,
                CreatedAt = sender.CreatedAt,
                Status = sender.Status.GetDescription()!,
                User = (sender.OrderOneTimeInformation != null) ? sender.OrderOneTimeInformation.Name : sender.User.Nickname,
                ValueWithVAT = sender.Items.Sum(x => (x.Price * x.Quantity * ((x.Product.VAT + 100) / 100))),
                DiscountValue = sender.Items.Sum(x => ((x.PriceWithoutDiscount - x.Price) * x.Quantity))
            };
    }
}
