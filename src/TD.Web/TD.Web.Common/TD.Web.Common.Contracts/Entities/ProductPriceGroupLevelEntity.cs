using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceGroupLevelEntity : Entity
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
