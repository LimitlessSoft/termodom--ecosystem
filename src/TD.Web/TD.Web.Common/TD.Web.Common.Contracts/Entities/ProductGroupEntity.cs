using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductGroupEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }

        [NotMapped]
        public List<ProductEntity> Products { get; set; }

        [NotMapped]
        public ProductGroupEntity? ParentGroup { get; set; }
    }
}
