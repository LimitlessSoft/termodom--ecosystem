using LSCore.Contracts.Dtos;
using TD.Web.Common.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Products
{
    public class ProductsGetSingleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<GetProductGroupSequentialDto> Category { get; set; }
        public string? CatalogId { get; set; }
        public decimal Vat { get; set; }
        public string? FullDescription { get; set; }
        public string? ShortDescription { get; set; }
        public string Unit { get; set; }
        public string? AlternateUnit { get; set; }
        public decimal? OneAlternatePackageEquals { get; set; }
        public ProductsGetOneTimePricesDto? OneTimePrice { get; set; }
        public ProductsGetUserPricesDto? UserPrice { get; set; }
        public LSCoreFileDto? ImageData { get; set; }
        public ProductClassification Classification { get; set; }
    }
}
