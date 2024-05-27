using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Products
{
    public class ProductsGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public int UnitId { get; set; }
        public int Classification { get; set; }
        public decimal VAT { get; set; }
        public List<int> Groups { get; set; }
        public int ProductPriceGroupId { get; set; }
        public int? AlternateUnitId { get; set; }
        public decimal? OneAlternatePackageEquals { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public decimal MinWebBase { get; set; }
        public decimal MaxWebBase { get; set; }
        public int PriorityIndex { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
