using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Requests.Products
{
    public class ProductsGetMultipleRequest
    {
        public ProductClassification[]? Classification { get; set; }
        public int[]? Groups { get; set; }
    }
}
