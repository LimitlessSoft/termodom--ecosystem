using Microsoft.AspNetCore.Http;
using Omu.ValueInjecter;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Contracts.Helpers.Cart
{
    public static class CartHelper
    {
        public static CheckoutRequest ToCheckoutRequest(this CheckoutRequestBase request, IHttpContextAccessor httpContextAccessor)
        {
            var checkoutRequest = new CheckoutRequest();
            checkoutRequest.InjectFrom(request);

            checkoutRequest.IsCurrentUserAuthenticated = (httpContextAccessor.HttpContext.User.Identity != null && 
                httpContextAccessor.HttpContext.User.Identity.IsAuthenticated);

            return checkoutRequest;
        }
    }
}
