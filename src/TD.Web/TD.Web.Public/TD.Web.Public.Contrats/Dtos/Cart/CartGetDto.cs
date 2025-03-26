namespace TD.Web.Public.Contracts.Dtos.Cart;

public class CartGetDto
{
    public List<CartItemDto> Items { get; set; }
    public CartSummaryDto Summary { get; set; }
}
