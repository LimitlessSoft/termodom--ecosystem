using LSCore.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Products
{
    public class ProductsGetSingleDto
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string BaseUnit { get; set; }
        public string? CatalogId { get; set; }
        public string? AlternateUnit { get; set; }
        public string FullDescription { get; set; }
        public string ShortDescription { get; set; }
        public LSCoreFileDto? ImageData { get; set; }
        public decimal? AlternateUnitMultiplicator { get; set; }
        public ProductClassification Classification { get; set; }
    }
}
