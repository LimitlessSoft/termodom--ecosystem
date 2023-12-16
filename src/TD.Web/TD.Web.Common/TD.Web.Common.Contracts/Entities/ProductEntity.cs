using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public int UnitId { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public int PriceId { get; set; }
        public int ProductPriceGroupId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        [NotMapped]
        public ProductPriceEntity Price { get; set; }

        [NotMapped]
        public UnitEntity Unit { get; set; }

        [NotMapped]
        public ProductPriceGroupEntity ProductPriceGroup { get; set; }

        [NotMapped]
        public List<ProductGroupEntity> Groups { get; set; }
    }
}
