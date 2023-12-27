using LSCore.Contracts.Dtos;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Products
{
    public class ProductsGetSingleDto
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string? CatalogId { get; set; }
        public string? FullDescription { get; set; }
        public string? ShortDescription { get; set; }
        public string Unit { get; set; }
        public string? AlternateUnit { get; set; }
        public decimal? OneAlternatePackageEquals { get; set; }
        public OneTimePricesDto? OneTimePrice { get; set; }
        public UserPricesDto? UserPrice { get; set; }
        public LSCoreFileDto? ImageData { get; set; }
        public ProductClassification Classification { get; set; }
    }
}
