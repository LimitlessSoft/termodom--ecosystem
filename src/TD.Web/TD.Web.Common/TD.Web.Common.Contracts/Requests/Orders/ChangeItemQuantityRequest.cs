namespace TD.Web.Common.Contracts.Requests.Orders
{
    public class ChangeItemQuantityRequest
    {
        public string? OneTimeHash { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity {  get; set; }
    }
}
