using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.Orders;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Orders
{
    public static class OrderGetDtoMappings
    {
        public static OrdersGetDto toDto(this OrderEntity orderEntity)
        {
            var dto = new OrdersGetDto();
            dto.InjectFrom(orderEntity);
            return dto;
        }
    }
}
