using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Dtos.Orders;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        LSCoreResponse<OrderGetDto> GetCurrentUserOrder();
        LSCoreResponse<OrderGetDto> GetOneTimeOrder(string OneTimeHash);
    }
}
