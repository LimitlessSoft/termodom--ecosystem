using TD.Web.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Dtos.Products
{
    public class ProductsGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public string Unit { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public List<ProductsGetGroupItemDto> Groups { get; set; }
        public ProductPriceGroupGetDto ProductsPricesGroup { get; set; }
    }
}
