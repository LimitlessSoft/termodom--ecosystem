using LSCore.Common.Extensions;
using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Orders;

namespace TD.Web.Public.Contracts.DtoMappings.Orders;

public class OrdersGetDtoMappings : ILSCoreMapper<OrderEntity, OrdersGetDto>
{
	public OrdersGetDto ToMapped(OrderEntity sender) =>
		new()
		{
			OneTimeHash = sender.OneTimeHash,
			Date = sender.CheckedOutAt,
			Status = sender.Status.GetDescription()!,
			ValueWithVAT = sender.Items.Sum(x =>
				(x.Price * x.Quantity * ((x.Product.VAT + 100) / 100))
			),
			DiscountValue = sender.Items.Sum(x => (x.PriceWithoutDiscount - x.Price) * x.Quantity)
		};
}
