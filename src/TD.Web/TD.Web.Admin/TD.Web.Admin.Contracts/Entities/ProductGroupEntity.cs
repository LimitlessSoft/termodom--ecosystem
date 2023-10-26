using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Entities
{
    public class ProductGroupEntity : Entity
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }

        [NotMapped]
        public List<ProductEntity> Products { get; set; }

        [NotMapped]
        public ProductGroupEntity? ParentGroup { get; set; }
    }
}
