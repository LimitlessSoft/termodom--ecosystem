using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;

namespace TD.Web.Veleprodaja.Contracts.IManagers
{
    public interface ICartManager : IBaseManager
    {
        Response<OrdersGetDto> Get();
    }
}
