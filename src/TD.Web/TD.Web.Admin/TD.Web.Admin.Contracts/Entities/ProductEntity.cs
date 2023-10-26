using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;
using TD.Web.Admin.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Entities
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
        public int PriceId { get; set; }
        public int ProductPriceGroupId { get; set; }

        [NotMapped]
        public ProductPriceEntity Price { get; set; }

        [NotMapped]
        public UnitEntity Unit { get; set; }

        [NotMapped]
        public ProductPriceGroupEntity ProductPriceGroup { get; set; }

    }
}
