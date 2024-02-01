using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Web.Common.Contracts.Entities
{
    public class OrderOneTimeInformationEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int OrderId { get; set; }
        [NotMapped]
        public OrderEntity Order;
    }
}
