namespace TD.Web.Admin.Contracts.Dtos.Orders
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceWithVAT { get; set; }
        public decimal ValueWithVAT { get; set; }
        public decimal Discount { get; set; }
    }
}
