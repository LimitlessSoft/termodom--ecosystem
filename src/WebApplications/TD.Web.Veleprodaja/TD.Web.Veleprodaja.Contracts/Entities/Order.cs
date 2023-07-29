using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;
using TD.Web.Veleprodaja.Contracts.Enums.Orders;

namespace TD.Web.Veleprodaja.Contracts.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }

        [NotMapped]
        public List<OrderItem> Items { get; set; }
        [NotMapped]
        public User User { get; set; }
    }
}
