using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Dtos.Products
{
    public class ProductsGetMultipleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public int? CatalogId { get; set; }
        public int? UnitId { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public List<ProductsGetMultipleGroupItemDto> Groups { get; set; }
    }
}
