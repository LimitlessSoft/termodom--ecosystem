using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Products
{
    public class ProductsGetMultipleRequest
    {
        public ProductClassification[]? Classification { get; set; }
        public int[]? Groups { get; set; }
        public string? SearchFilter { get; set; }
    }
}
