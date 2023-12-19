using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IOrderItemManager : ILSCoreBaseManager
    {
        void AddProductToCart(OrderItemEntity request);
        bool ItemExists(int productId, int userId, string oneTimeHash);
    }
}
