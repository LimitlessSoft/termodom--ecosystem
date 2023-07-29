using Omu.ValueInjecter;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Contracts.DtoMappings
{
    public static class OrdersDtoMappings
    {
        public static OrdersGetDto ToOrderGetDto(this Order order )
        {
            var dto = new OrdersGetDto();
            dto.InjectFrom(order);
            dto.Items = order.Items.ToOrdersItemDtoList();
            return dto;
        }
    }
}
