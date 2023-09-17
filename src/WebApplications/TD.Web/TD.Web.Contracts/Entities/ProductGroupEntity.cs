using TD.Core.Contracts.Entities;

namespace TD.Web.Contracts.Entities
{
    public class ProductGroupEntity : Entity
    {
        public string Name { get; set; }
        public ProductGroupEntity? ParentGroup { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
