using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Admin.Contracts.Dtos.Orders;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        LSCoreResponse<OrderGetDto> GetCurrentUserOrder();
    }
}
