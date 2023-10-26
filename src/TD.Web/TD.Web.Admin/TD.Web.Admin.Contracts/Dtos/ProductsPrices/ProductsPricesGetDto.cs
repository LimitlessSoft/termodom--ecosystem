namespace TD.Web.Admin.Contracts.Dtos.ProductPrices
{
    public class ProductsPricesGetDto
    {
        public int Id {  get; set; }
        public int ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
