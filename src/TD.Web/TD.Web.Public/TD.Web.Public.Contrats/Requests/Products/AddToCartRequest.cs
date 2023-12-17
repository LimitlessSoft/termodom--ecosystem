namespace TD.Web.Public.Contracts.Requests.Products
{
    public class AddToCartRequest
    {
        public int Id {  get; set; }
        public decimal Quantity { get; set; }
        public string? OneTimeHash {  get; set; }
    }
}
