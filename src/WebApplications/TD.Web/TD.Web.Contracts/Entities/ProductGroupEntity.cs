using TD.Core.Contracts;

namespace TD.Web.Contracts.Entities
{
    public class ProductGroupEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductGroupEntity? ParentGroup { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
