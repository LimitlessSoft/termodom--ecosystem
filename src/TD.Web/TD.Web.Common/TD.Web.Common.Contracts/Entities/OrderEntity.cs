using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities
{
    public class OrderEntity : LSCoreEntity
    {
        public int? UserId { get; set; }
        public string? OneTimeHash { get; set; }
        public int? StoreId { get; set; }
        public DateTime Date { get; set; }
        public int? Referent {  get; set; }
        public int? PaymentType { get; set; }
        public OrderStatus Status { get; set; }

        [NotMapped]
        public UserEntity UserEntity { get; set; }
    }
}
