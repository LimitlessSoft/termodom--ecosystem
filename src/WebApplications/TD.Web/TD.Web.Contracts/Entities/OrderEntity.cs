using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Entities
{
    public class OrderEntity : Entity
    {
        public int UserId { get; set; }
        public int? Referent {  get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Date { get; set; }
        public int StoreId { get; set; }
        public int PaymentType { get; set; }

        //[NotMapped]
        //public UserEntity UserEntity { get; set; }
    }
}
