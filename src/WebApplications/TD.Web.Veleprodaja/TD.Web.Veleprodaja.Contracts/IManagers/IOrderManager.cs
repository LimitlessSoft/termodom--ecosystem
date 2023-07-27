using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;

namespace TD.Web.Veleprodaja.Contracts.IManagers
{
    public interface IOrderManager
    {
        Response<OrdersGetDto> Get();
    }
}
