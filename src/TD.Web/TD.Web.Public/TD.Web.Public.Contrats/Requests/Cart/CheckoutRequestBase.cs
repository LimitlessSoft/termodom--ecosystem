namespace TD.Web.Public.Contracts.Requests.Cart
{
    public class CheckoutRequestBase
    {
        public string? OneTimeHash { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Note { get; set; }
        public int StoreId { get; set; }
        public int PaymentTypeId { get; set; }
    }
}
