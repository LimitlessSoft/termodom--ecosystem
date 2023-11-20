using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class ProductPriceEntity : LSCoreEntity
    {
        public int ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        [NotMapped]
        public ProductEntity Product { get; set; }
    }
}
