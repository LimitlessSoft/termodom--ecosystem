using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Helpers.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Common.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        private IHttpContextAccessor _httpContextAccessor;
        
        public CartController(ICartManager cartManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _cartManager = cartManager;
            _cartManager.SetContext(_httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/cart")]
        public LSCoreResponse<CartGetDto> Get([FromQuery]CartGetRequest request) =>
            _cartManager.Get(request);

        [HttpPost]
        [Route("/checkout")]
        public LSCoreResponse Checkout([FromBody]CheckoutRequestBase request) =>
            _cartManager.Checkout(request.ToCheckoutRequest(_httpContextAccessor));

        [HttpPost]
        [Route("/cart-current-level-information")]
        public LSCoreResponse<CartGetCurrentLevelInformationDto> GetCurrentLevelInformation([FromBody]CartCurrentLevelInformationRequest request) =>
            _cartManager.GetCurrentLevelInformation(request);
    }
}
