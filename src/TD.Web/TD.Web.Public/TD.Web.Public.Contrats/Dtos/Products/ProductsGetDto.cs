using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Products;

public class ProductsGetDto
{
    public long Id { get; set; }
    public string Src { get; set; }
    public string Title { get; set; }
    public decimal VAT { get; set; }
    public string Unit { get; set; }
    public string ImageContentType { get; set; }
    public string ImageData { get; set; }
    public ProductClassification Classification { get; set; }
    public ProductsGetUserPricesDto? UserPrice { get; set; }
    public ProductsGetOneTimePricesDto? OneTimePrice { get; set; }
    public int PriorityIndex { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsWholesale { get; set; }
}