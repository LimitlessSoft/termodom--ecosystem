namespace TD.Web.Common.Contracts.Requests.OrderItems
{
    public class OrderItemExistsRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
