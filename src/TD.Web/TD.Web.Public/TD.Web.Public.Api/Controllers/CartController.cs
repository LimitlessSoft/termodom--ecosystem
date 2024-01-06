using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Common.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        
        public CartController(ICartManager cartManager, IHttpContextAccessor httpContextAccessor)
        {
            _cartManager = cartManager;
            _cartManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/cart")]
        public LSCoreResponse<CartGetDto> Get([FromQuery]CartGetRequest request) =>
            _cartManager.Get(request);
    }
}
