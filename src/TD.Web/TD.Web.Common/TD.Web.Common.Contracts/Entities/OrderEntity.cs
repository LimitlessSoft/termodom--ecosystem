using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class OrderEntity : LSCoreEntity
{
	public string OneTimeHash { get; set; }
	public long StoreId { get; set; }
	public long? ReferentId { get; set; }
	public int? KomercijalnoBrDok { get; set; }
	public int? KomercijalnoVrDok { get; set; }
	public long PaymentTypeId { get; set; }
	public OrderStatus Status { get; set; }
	public string? Note { get; set; }
	public DateTime? CheckedOutAt { get; set; }
	public string? DeliveryAddress { get; set; }
	public string? AdminComment { get; set; }
	public string? PublicComment { get; set; }

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

	[NotMapped]
	public StoreEntity? Store { get; set; }
}
