using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface ICartManager : ILSCoreBaseManager
    {
        LSCoreResponse<CartGetDto> Get(CartGetRequest request);
        LSCoreResponse Checkout(CheckoutRequest request);
        LSCoreResponse<CartGetCurrentLevelInformationDto> GetCurrentLevelInformation(CartCurrentLevelInformationRequest request);
    }
}
