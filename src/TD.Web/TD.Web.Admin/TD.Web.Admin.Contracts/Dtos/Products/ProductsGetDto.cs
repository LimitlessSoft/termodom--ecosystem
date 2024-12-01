using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Products;

public class ProductsGetDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Src { get; set; }
    public string Image { get; set; }
    public string? CatalogId { get; set; }
    public long UnitId { get; set; }
    public int Classification { get; set; }
    public decimal VAT { get; set; }
    public List<long> Groups { get; set; }
    public long ProductPriceGroupId { get; set; }
    public long? AlternateUnitId { get; set; }
    public decimal? OneAlternatePackageEquals { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public decimal MinWebBase { get; set; }
    public decimal MaxWebBase { get; set; }
    public int PriorityIndex { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool CanEdit { get; set; }
    public ProductStatus Status { get; set; }
    public ProductStockType StockType { get; set; }
    public List<string>? SearchKeywords { get; set; }
    public decimal PlatinumPriceWithoutVAT { get; set; }
    public decimal IronPriceWithoutVAT { get; set; }
}