using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Web.Veleprodaja.Contracts.Entities
{
    public class OrderItem : IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double Quantity { get; set; }
        public double PriceWithoutVat { get; set; }
        public double Vat { get; set; }

        [NotMapped]
        public Order Order { get; set; }
    }
}
