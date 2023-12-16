using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceGroupLevelEntity : LSCoreEntity
    {
        public int UserId { get; set; }
        public int Level {  get; set; }
        public int ProductPriceGroupId { get; set; }

        [NotMapped]
        public UserEntity User { get; set; }

        [NotMapped]
        public ProductPriceGroupEntity ProductPriceGroup { get; set; }
    }
}
