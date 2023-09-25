namespace TD.Web.Contracts.Dtos.ProductPrices
{
    public class ProductsPricesGetDto
    {
        public int Id {  get; set; }
        public long ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
