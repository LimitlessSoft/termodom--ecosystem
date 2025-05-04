using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Helpers.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class CartController(ICartManager cartManager, IHttpContextAccessor httpContextAccessor)
	: ControllerBase
{
	[HttpGet]
	[Route("/cart")]
	public CartGetDto Get([FromQuery] CartGetRequest request) => cartManager.Get(request);

	[HttpGet]
	[Route("/checkout")]
	public IActionResult GetCheckout([FromQuery] string oneTimeHash) =>
		Ok(cartManager.GetCheckout(oneTimeHash));

	[HttpPost]
	[Route("/checkout")]
	public void Checkout([FromBody] CheckoutRequestBase request) =>
		cartManager.Checkout(request.ToCheckoutRequest(httpContextAccessor));

	[HttpGet]
	[Route("/cart-current-level-information")]
	public CartGetCurrentLevelInformationDto GetCurrentLevelInformation(
		[FromQuery] CartCurrentLevelInformationRequest request
	) => cartManager.GetCurrentLevelInformation(request);
}
