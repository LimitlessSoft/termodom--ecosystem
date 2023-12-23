namespace TD.Web.Public.Contracts.Dtos.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public decimal PriceWithoutDiscount { get; set; }
    }
}
