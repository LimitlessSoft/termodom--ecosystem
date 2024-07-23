using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Products;

public class ProductsSaveRequest : LSCoreSaveRequest
{
    public string Name { get; set; }
    public string? Src { get; set; }
    public string Image { get; set; }
    public string? CatalogId { get; set; }
    public int UnitId { get; set; }
    public ProductClassification Classification { get; set; }
    public decimal VAT { get; set; }
    public List<long> Groups { get; set; } = [];
    public int ProductPriceGroupId { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public int? AlternateUnitId { get; set; }
    public decimal? OneAlternatePackageEquals { get; set; }
    public int PriorityIndex { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public ProductStatus Status { get; set; }
}