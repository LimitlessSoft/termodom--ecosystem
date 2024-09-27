namespace TD.Web.Admin.Contracts.Dtos.Orders;

public class OrdersGetDto
{
    public required long Id { get; set; }
    public string? OneTimeHash { get; set; }
    public int? KomercijalnoVrDok { get; set; }
    public int? KomercijalnoBrDok { get; set; }
    public long? StoreId { get; set; }
    public DateTime? CheckedOutAt { get; set; }
    public OrdersReferentDto? Referent { get; set; }
    public required string Status { get; set; }
    public required int StatusId { get; set; }
    public string? Note { get; set; }
    public long? PaymentTypeId { get; set; }
    public OrdersUserInformationDto? UserInformation { get; set; }
    public OrdersSummaryDto? Summary { get; set; }
    public required List<OrdersItemDto> Items { get; set; } = new List<OrdersItemDto>();
    public string? Username { get; set; }
    public bool HasAtLeastOneMaxPriceLevel { get; set; }
    public string? DeliveryAddress { get; set; }
}