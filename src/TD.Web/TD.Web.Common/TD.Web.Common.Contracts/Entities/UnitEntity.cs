
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class UnitEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
