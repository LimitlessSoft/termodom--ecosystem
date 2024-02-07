using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities
{
    public class OrderEntity : LSCoreEntity
    {
        public string? OneTimeHash { get; set; }
        public int? StoreId { get; set; }
        public int? Referent { get; set; }
        public int? BrDok { get; set; }
        public int? VrDok { get; set; }
        public int? PaymentTypeId { get; set; }
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime? CheckedOutAt { get; set; }

        [NotMapped]
        public List<OrderItemEntity> Items { get; set; }

        [NotMapped]
        public OrderOneTimeInformationEntity? OrderOneTimeInformation { get; set; }
    }
}
