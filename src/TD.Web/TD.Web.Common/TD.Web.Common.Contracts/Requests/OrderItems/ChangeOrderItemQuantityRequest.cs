namespace TD.Web.Common.Contracts.Requests.OrderItems
{
    public class ChangeOrderItemQuantityRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
