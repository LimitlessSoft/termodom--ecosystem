using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Orders
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
