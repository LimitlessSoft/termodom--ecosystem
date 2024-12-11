using LSCore.Contracts.Extensions;
using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Orders;

public class OrdersGetDtoMappings : ILSCoreDtoMapper<OrderEntity, OrdersGetDto>
{
    public OrdersGetDto ToDto(OrderEntity sender) =>
        new()
        {
            HasAtLeastOneMaxPriceLevel = sender.User.ProductPriceGroupLevels.Any(x =>
                x.ProductPriceGroup.TrackUserLevel
                && x.Level >= (Common.Contracts.Constants.NumberOfProductPriceGroupLevels - 1)
            ),
            Username = sender.User.Username,
            Id = sender.Id,
            OneTimeHash = sender.OneTimeHash,
            CheckedOutAt = sender.CheckedOutAt,
            Status = sender.Status.GetDescription()!,
            DeliveryAddress = sender.DeliveryAddress,
            Referent =
                sender.Referent == null
                    ? null
                    : new OrdersReferentDto()
                    {
                        Id = sender.Referent.Id,
                        Name = sender.Referent.Nickname
                    },
            UserInformation =
                sender.OrderOneTimeInformation == null
                    ? new OrdersUserInformationDto()
                    {
                        Id = sender.User.Id,
                        Name = sender.User.Nickname,
                        Mobile = sender.User.Mobile,
                    }
                    : new OrdersUserInformationDto()
                    {
                        Name = sender.OrderOneTimeInformation!.Name,
                        Mobile = sender.OrderOneTimeInformation!.Mobile
                    },
            Summary = new OrdersSummaryDto()
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
            AdminComment = sender.AdminComment,
            PublicComment = sender.PublicComment,
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
                    Discount =
                        x.PriceWithoutDiscount == 0
                            ? 0
                            : (x.Price / x.PriceWithoutDiscount - 1) * -100
                })
                .ToList()
        };
}
