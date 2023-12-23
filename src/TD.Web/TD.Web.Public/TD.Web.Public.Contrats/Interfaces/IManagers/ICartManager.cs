using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface ICartManager : ILSCoreBaseManager
    {
        public LSCoreResponse<CartGetDto> Get(CartGetRequest request);
    }
}
