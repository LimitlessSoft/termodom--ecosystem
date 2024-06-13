using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class OrderOneTimeInformationEntity : LSCoreEntity
{
    public string Name { get; set; }
    public string Mobile { get; set; }
    public long OrderId { get; set; }
    [NotMapped]
    public OrderEntity Order;
}