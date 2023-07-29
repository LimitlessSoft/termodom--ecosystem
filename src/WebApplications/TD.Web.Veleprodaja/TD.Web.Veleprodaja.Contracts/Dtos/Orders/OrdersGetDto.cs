
namespace TD.Web.Veleprodaja.Contracts.Dtos.Orders
{
    public class OrdersGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrdersItemDto> Items { get; set; } = new List<OrdersItemDto>();
    }
}
