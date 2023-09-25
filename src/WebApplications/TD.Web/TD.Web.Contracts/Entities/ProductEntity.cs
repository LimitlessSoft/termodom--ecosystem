using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Entities
{
    public class ProductEntity : Entity
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public int UnitId { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public List<ProductGroupEntity> Groups { get; set; }

        [NotMapped]
        public ProductPriceEntity Price { get; set; }

        [NotMapped]
        public UnitEntity Unit { get; set; }
        
    }
}
