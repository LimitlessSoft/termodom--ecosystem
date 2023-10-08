using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Contracts.Entities
{
    public class ProductPriceGroupEntity: Entity
    {
        public string Name { get; set; }
        public List<ProductEntity>? Products { get; set; }
    }
}
