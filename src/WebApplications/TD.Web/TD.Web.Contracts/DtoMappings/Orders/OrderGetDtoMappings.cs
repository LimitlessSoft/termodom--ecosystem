using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.Orders;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Orders
{
    public static class OrderGetDtoMappings
    {
        public static OrderGetDto toDto(this OrderEntity orderEntity)
        {
            var dto = new OrderGetDto();
            dto.InjectFrom(orderEntity);
            return dto;
        }
    }
}
