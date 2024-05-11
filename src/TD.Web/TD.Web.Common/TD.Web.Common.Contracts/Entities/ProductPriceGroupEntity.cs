using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceGroupEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public bool TrackUserLevel { get; set; }

        [NotMapped]
        public List<ProductEntity>? Products { get; set; }

        [NotMapped]
        public List<ProductPriceGroupLevelEntity> ProductPriceGroupLevels { get; set; }
    }
}
