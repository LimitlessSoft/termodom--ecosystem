using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceGroupEntity : Entity
    {
        public string Name { get; set; }

        [NotMapped]
        public List<ProductEntity>? Products { get; set; }

        [NotMapped]
        public List<ProductPriceGroupLevelEntity> ProductPriceGroupLevels { get; set; }
    }
}
