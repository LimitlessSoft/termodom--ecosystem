using TD.Core.Contracts;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Entities
{
    public class ProductEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public int? UnitId { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public List<ProductGroupEntity> Groups { get; set; }
        public string Something1 { get; set; }
    }
}
