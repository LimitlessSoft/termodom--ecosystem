namespace TD.Web.Public.Contracts.Dtos.Orders
{
    public class OrderGetSingleDto
    {
        public required int Id { get; set; }
        public string? OneTimeHash { get; set; }
        public int? KomercijalnoVrDok { get; set; }
        public int? KomercijalnoBrDok { get; set; }
        public int? StoreId { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public required string Status { get; set; }
        public required int StatusId { get; set; }
        public string? Note { get; set; }
        public int? PaymentTypeId { get; set; }
        public OrdersUserInformationDto? UserInformation { get; set; }
        public OrderSummaryDto? Summary { get; set; }
        public required List<OrdersItemDto> Items { get; set; } = new List<OrdersItemDto>();
    }
}
