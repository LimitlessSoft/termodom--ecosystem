namespace TD.Web.Public.Contracts.Requests.Cart
{
    public class CheckoutRequest : CheckoutRequestBase
    {
        public bool CurrentUser { get; set; } = false;
    }
}
