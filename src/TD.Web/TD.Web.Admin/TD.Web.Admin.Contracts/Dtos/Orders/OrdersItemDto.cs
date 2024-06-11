namespace TD.Web.Admin.Contracts.Dtos.Orders;

public class OrdersItemDto
{
    public long ProductId { get; set; }
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public required decimal PriceWithVAT { get; set; }
    public decimal ValueWithVAT { get => PriceWithVAT * Quantity; }
    public decimal Discount { get; set; }
}