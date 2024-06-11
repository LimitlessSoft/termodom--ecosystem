using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Public.Contracts.Dtos.Cart;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface ICartManager
{
    CartGetDto Get(CartGetRequest request);
    void Checkout(CheckoutRequest request);
    CartGetCurrentLevelInformationDto GetCurrentLevelInformation(CartCurrentLevelInformationRequest request);
}