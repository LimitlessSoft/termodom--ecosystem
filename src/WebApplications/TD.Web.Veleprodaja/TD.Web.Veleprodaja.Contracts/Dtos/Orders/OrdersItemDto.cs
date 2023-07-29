namespace TD.Web.Veleprodaja.Contracts.Dtos.Orders
{
    public class OrdersItemDto
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public double PriceWithoutVat { get; set; }
        public double Vat { get; set; }
    }
}
