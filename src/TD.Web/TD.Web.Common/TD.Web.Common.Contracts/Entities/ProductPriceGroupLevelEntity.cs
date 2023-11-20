using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceGroupLevelEntity : LSCoreEntity
    {
        public int ProductPriceGroupId { get; set; }
        public int UserId { get; set; }
        public int Level {  get; set; }

        [NotMapped]
        public ProductPriceGroupEntity ProductPriceGroup { get; set; }
        [NotMapped]
        public UserEntity User { get; set; }
    }
}
