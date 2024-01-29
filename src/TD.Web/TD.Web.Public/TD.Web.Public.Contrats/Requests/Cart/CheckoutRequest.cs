namespace TD.Web.Public.Contracts.Requests.Cart
{
    public class CheckoutRequest
    {
        public string? OneTimeHash { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public int StoreId { get; set; }
        public int PaymentTypeId { get; set; }
    }
}
