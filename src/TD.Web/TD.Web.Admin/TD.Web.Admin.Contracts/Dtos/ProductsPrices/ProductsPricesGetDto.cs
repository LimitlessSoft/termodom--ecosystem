namespace TD.Web.Admin.Contracts.Dtos.ProductPrices
{
    public class ProductsPricesGetDto
    {
        public long Id {  get; set; }
        public long ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
