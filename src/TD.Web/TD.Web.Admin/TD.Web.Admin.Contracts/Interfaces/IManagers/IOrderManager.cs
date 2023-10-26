using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Admin.Contracts.Dtos.Orders;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : IBaseManager
    {
        Response<OrderGetDto> GetCurrentUserOrder();
    }
}
