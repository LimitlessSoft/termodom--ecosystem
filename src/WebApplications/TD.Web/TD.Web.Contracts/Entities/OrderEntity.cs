using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Entities;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Entities
{
    public class OrderEntity : Entity
    {
        public int userId { get; set; }
        public int? referent {  get; set; }
        public OrderStatus status { get; set; }
        public DateTime date { get; set; }
        public int storeId { get; set; }
        public int paymentType { get; set; }

        [NotMapped]
        public UserEntity userEntity { get; set; }
    }
}
