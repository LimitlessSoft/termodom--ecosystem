using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Contracts.Dtos.Orders;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : IBaseManager
    {
        Response<OrderGetDto> GetCurrentUserOrder();
    }
}
