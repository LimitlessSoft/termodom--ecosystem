using LSCore.Common.Extensions;
using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Orders;

namespace TD.Web.Public.Contracts.DtoMappings.Orders;

public class OrderGetSingleDtoMappings : ILSCoreMapper<OrderEntity, OrderGetSingleDto>
{
	public OrderGetSingleDto ToMapped(OrderEntity sender) =>
		new OrderGetSingleDto
		{
			Id = sender.Id,
			OneTimeHash = sender.OneTimeHash,
			CheckedOutAt = sender.CheckedOutAt,
			Status = sender.Status.GetDescription()!,
			UserInformation =
				sender.OrderOneTimeInformation == null
					? new OrdersUserInformationDto()
					{
						Id = sender.User.Id,
						Name = sender.User.Nickname,
						Mobile = sender.User.Mobile
					}
					: new OrdersUserInformationDto()
					{
						Name = sender.OrderOneTimeInformation!.Name,
						Mobile = sender.OrderOneTimeInformation!.Mobile
					},
			Summary = new OrderSummaryDto()
			{
				ValueWithoutVAT = sender.Items.Sum(x => (x.Price * x.Quantity)),
				VATValue = sender.Items.Sum(x => (x.Price * x.Quantity * (x.Product.VAT / 100))),
				DiscountValue = sender.Items.Sum(x =>
					(x.PriceWithoutDiscount - x.Price) * (1 + (x.Product.VAT / 100)) * x.Quantity
				)
			},
			StatusId = (int)sender.Status,
			KomercijalnoVrDok = sender.KomercijalnoVrDok,
			KomercijalnoBrDok = sender.KomercijalnoBrDok,
			StoreId = sender.StoreId,
			Note = sender.Note,
			PaymentTypeId = sender.PaymentTypeId,
			Items = sender
				.Items.Select(x => new OrdersItemDto
				{
					ProductId = x.ProductId,
					Name = x.Product.Name,
					Quantity = x.Quantity,
					PriceWithVAT = x.Price * (1 + (x.Product.VAT / 100)),
					Discount = (x.PriceWithoutDiscount - x.Price) * (1 + (x.Product.VAT / 100)),
					StockType = x.Product.StockType
				})
				.ToList()
		};
}
