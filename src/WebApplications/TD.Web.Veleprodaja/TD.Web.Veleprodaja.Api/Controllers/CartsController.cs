using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TD.Core.Contracts.Http;
using TD.Core.Framework.Extensions;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;
using TD.Web.Veleprodaja.Contracts.IManagers;

namespace TD.Web.Veleprodaja.Api.Controllers
{
    [ApiController]
    public class CartsController : Controller
    {
        private readonly ICartManager _cartManager;

        public CartsController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpGet]
        [Authorize]
        [Route("/cart")]
        public Response<OrdersGetDto> Get()
        {
            return _cartManager
                .AttachCurrentContext(this.HttpContext)
                .Get();
        }
    }
}
