using LSCore.Contracts.Requests;

namespace TD.Web.Public.Contracts.Requests.Products
{
    public class RemoveFromCartRequest : LSCoreIdRequest
    {
        public string? OneTimeHash { get; set; }
    }
}
