using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface ICartManager
{
	CartGetDto Get(CartGetRequest request);
	void Checkout(CheckoutRequest request);
	CartGetCurrentLevelInformationDto GetCurrentLevelInformation(
		CartCurrentLevelInformationRequest request
	);
	CheckoutGetDto GetCheckout(string oneTimeHash);
}
