using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Requests.OrderItems
{
    public class DeleteOrderItemRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
