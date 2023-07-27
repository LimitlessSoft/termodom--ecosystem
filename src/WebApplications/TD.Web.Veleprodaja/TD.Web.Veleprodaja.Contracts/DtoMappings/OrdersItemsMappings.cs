using Omu.ValueInjecter;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Contracts.DtoMappings
{
    public static class OrdersItemsMappings
    {
        public static List<OrdersItemDto> ToOrdersItemDtoList(this List<OrderItem> items)
        {
            var dto = new List<OrdersItemDto>();
            foreach(var item in items)
            {
                var d = new OrdersItemDto();
                d.InjectFrom(item);
                dto.Add(d);
            }
            return dto;
        }
    }
}
