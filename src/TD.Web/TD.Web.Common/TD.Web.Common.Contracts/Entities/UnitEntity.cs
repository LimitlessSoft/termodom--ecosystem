using TD.Core.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class UnitEntity : Entity
    {
        public string Name { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
