namespace TD.Web.Common.Contracts.Requests.Orders
{
    public class OrdersAddItemRequest
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? OneTimeHash { get; set; }
    }
}
