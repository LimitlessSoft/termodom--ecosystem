using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class OrderEntity : LSCoreEntity
{
    public string OneTimeHash { get; set; }
    public short StoreId { get; set; }
    public long? ReferentId { get; set; }
    public int? KomercijalnoBrDok { get; set; }
    public int? KomercijalnoVrDok { get; set; }
    public long PaymentTypeId { get; set; }
    public OrderStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime? CheckedOutAt { get; set; }

    [NotMapped]
    public List<OrderItemEntity> Items { get; set; }

    [NotMapped]
    public OrderOneTimeInformationEntity? OrderOneTimeInformation { get; set; }

    [NotMapped]
    public UserEntity? Referent { get; set; }

    [NotMapped]
    public UserEntity User { get; set; }
        
    [NotMapped]
    public PaymentTypeEntity PaymentType { get; set; }
}