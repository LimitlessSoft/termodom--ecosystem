using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Products;

public class ProductsGetMultipleRequest
{
    public ProductClassification[]? Classification { get; set; }
    public long[]? Groups { get; set; }
    public string? SearchFilter { get; set; }
    public long[]? Id { get; set; }
    public ProductStatus[]? Status { get; set; }
}