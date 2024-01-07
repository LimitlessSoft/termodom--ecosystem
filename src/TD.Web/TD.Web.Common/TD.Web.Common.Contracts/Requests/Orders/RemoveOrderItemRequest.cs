namespace TD.Web.Common.Contracts.Requests.Orders
{
    public class RemoveOrderItemRequest
    {
        public string? OneTimeHash { get; set; }
        public int ProductId { get; set; }
    }
}
