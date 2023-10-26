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
        public string Unit { get; set; }
        public string Classification { get; set; }
        public decimal VAT { get; set; }
        public List<ProductsGetGroupItemDto> Groups { get; set; }
        public ProductPriceGroupGetDto ProductsPricesGroup { get; set; }
    }
}
