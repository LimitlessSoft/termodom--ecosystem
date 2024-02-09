using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Orders
{
    public class OrderGetSingleDto
    {
        public int OrderId { get; set; }
        public int? KomercijalnoBrDok { get; set; }
        public int? KomercijalnoVrDok { get; set; }
        public DateTime? CreatedDate { get; set; }
        public OrderUserInformationDto UserInformation { get; set; }
        public int? StoreId { get; set; }
        public OrderStatus Status { get; set; }
        public int? PaymentTypeId { get; set; }
        public string? ReferentName { get; set; }
        public string? Note { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public OrderSummaryDto PriceSummary { get; set; }
    }
}
