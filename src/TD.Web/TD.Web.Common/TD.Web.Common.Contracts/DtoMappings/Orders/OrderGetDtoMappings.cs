using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Orders
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
