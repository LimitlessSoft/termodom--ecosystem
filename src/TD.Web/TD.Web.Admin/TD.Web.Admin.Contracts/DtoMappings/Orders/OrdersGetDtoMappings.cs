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
                Id = sender.Id,
                OneTimeHash = sender.OneTimeHash,
                CheckedOutAt = sender.CheckedOutAt,
                Status = sender.Status.GetDescription()!,
                Referent = sender.Referent == null ? null : new OrdersReferentDto()
                {
                    Id = sender.Referent.Id,
                    Name = sender.Referent.Nickname
                },
                UserInformation = sender.OrderOneTimeInformation == null ?
                    new OrdersUserInformationDto()
                    {
                        Id = sender.User.Id,
                        Name = sender.User.Nickname,
                        Mobile = sender.User.Mobile,
                        Username = sender.User.Username
                    } :
                    new OrdersUserInformationDto()
                    {
                        Name = sender.OrderOneTimeInformation!.Name,
                        Mobile = sender.OrderOneTimeInformation!.Mobile
                    },
                Summary = new OrdersSummaryDto()
                {
                    ValueWithoutVAT = sender.Items.Sum(x => (x.Price * x.Quantity)),
                    VATValue = sender.Items.Sum(x => (x.Price * x.Quantity * (x.Product.VAT / 100))),
                    DiscountValue = sender.Items.Sum(x => (x.PriceWithoutDiscount - x.Price) * (1 + (x.Product.VAT / 100)) * x.Quantity)
                },
                StatusId = (int)sender.Status,
                KomercijalnoVrDok = sender.KomercijalnoVrDok,
                KomercijalnoBrDok = sender.KomercijalnoBrDok,
                StoreId = sender.StoreId,
                Note = sender.Note,
                PaymentTypeId = sender.PaymentTypeId,
                Items = sender.Items.Select(x =>new OrdersItemDto
                    {
                        ProductId = x.ProductId,
                        Name = x.Product.Name,
                        Quantity = x.Quantity,
                        PriceWithVAT = x.Price * (1 + (x.Product.VAT / 100)),
                        Discount = (x.PriceWithoutDiscount - x.Price) * (1 + (x.Product.VAT / 100))
                    }).ToList()
            };
    }
}
