using LSCore.Contracts.Requests;

namespace TD.Web.Public.Contracts.Requests.Products
{
    public class AddToCartRequest : LSCoreIdRequest
    {
        public decimal Quantity { get; set; }
        public string? OneTimeHash { get; set; }
    }
}
