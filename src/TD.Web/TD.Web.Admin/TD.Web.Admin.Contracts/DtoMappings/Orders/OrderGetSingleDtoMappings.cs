using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Orders
{
    public class OrderGetSingleDtoMappings : ILSCoreDtoMapper<OrderGetSingleDto, OrderEntity>
    {
        public OrderGetSingleDto ToDto(OrderEntity sender)
        {
            var valueWithVAT = sender.Items.Sum(x => x.Price * (x.VAT / 100 + 1) * x.Quantity);
            var valueWithoutVAT = sender.Items.Sum(x => x.Price * x.Quantity);

            var priceSummary = new OrderSummaryDto()
            {
                VATValue = valueWithVAT - valueWithoutVAT,
                ValueWithoutVAT = valueWithoutVAT,
                ValueWithVAT = valueWithVAT,
                DiscountValue = sender.Items.Sum(x => x.PriceWithoutDiscount * x.Quantity * (x.VAT / 100 + 1)) - valueWithVAT,
            };

            var dto = new OrderGetSingleDto()
            {
                OrderId = sender.Id,
                CreatedDate = sender.CheckedOutAt,
                StoreId = sender.StoreId,
                Status = sender.Status,
                PaymentTypeId = sender.PaymentTypeId,
                Note = sender.Note,
                PriceSummary = priceSummary
            };
            dto.Items = new List<OrderItemDto>();

            sender.Items.ForEach(x =>
            {
                var item = new OrderItemDto()
                {
                    ProductId = x.Product.Id,
                    Name = x.Product.Name,
                    Quantity = x.Quantity,
                    PriceWithVAT = x.Price * (x.VAT / 100 + 1),
                    ValueWithVAT = x.Price * (x.VAT / 100 + 1) * x.Quantity,
                    Discount = 1 - x.Price / x.PriceWithoutDiscount
                };
                dto.Items.Add(item);
            });


            return dto;
        }
    }
}
