using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Contracts.Entities
{
    public class ProductPriceEntity : Entity
    {
        public long ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        [NotMapped]
        public ProductEntity Product { get; set; }
    }
}
