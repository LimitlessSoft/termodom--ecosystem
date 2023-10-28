using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceEntity : Entity
    {
        public int ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        [NotMapped]
        public ProductEntity Product { get; set; }
    }
}
